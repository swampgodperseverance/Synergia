using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using Synergia.Helpers;

namespace Synergia.Content.Projectiles.Reworks
{
    public class NebulaPike : ModProjectile
    {
        private float appearProgress = 0f;
        private bool hasAppeared = false;
        private const int appearDuration = 20;
        private const int fadeOutDuration = 30;
        private int timer = 0;

        public static readonly SoundStyle NecroSword = new("Synergia/Assets/Sounds/NecroSword");

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Type] = 4; // 4 фрейма
        }

        public override void SetDefaults()
        {
            Projectile.damage = 64;
            Projectile.width = 18;   // ширина спрайта
            Projectile.height = 176; // высота спрайта
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            timer++;

            // Анимация 4 фрейма всегда работает
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            // Появление
            if (!hasAppeared)
            {
                appearProgress = MathHelper.Clamp(timer / (float)appearDuration, 0f, 1f);
                float eased = EaseFunctions.EaseOutBack(appearProgress);

                Projectile.alpha = (int)(255 * (1f - appearProgress));
                Projectile.scale = 0.5f + 0.5f * eased;

                // Красивые частицы появления
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(Projectile.Center - new Vector2(8), 16, 16,
                        DustID.PinkTorch, Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.7f, 0.7f),
                        150, new Color(255, 140, 255), 1.4f);
                    Main.dust[dust].velocity *= 0.5f;
                    Main.dust[dust].noGravity = true;
                }

                if (appearProgress >= 1f)
                {
                    hasAppeared = true;
                    timer = 0;

                    // Случайная скорость
                    Projectile.localAI[1] = Main.rand.NextFloat(0.8f, 1.2f);
                }
            }
            else
            {
                // Антиципация: сначала немного опускается
                float anticipationDuration = 10f;
                float ascendDuration = Projectile.timeLeft - fadeOutDuration - anticipationDuration;

                if (timer <= anticipationDuration)
                {
                    float t = timer / anticipationDuration;
                    Projectile.position.Y += 4f * (1f - EaseFunctions.EaseOutQuad(t)); // мягкое опускание вниз
                }
                else
                {
                    float moveT = MathHelper.Clamp((timer - anticipationDuration) / ascendDuration, 0f, 1f);
                    float easedMove = EaseFunctions.EaseOutQuint(moveT);

                    float speedModifier = Projectile.localAI[1];
                    Projectile.velocity.Y = -22f * easedMove * speedModifier;
                }

                Projectile.rotation = -MathHelper.PiOver2; // всегда вверх

                Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 1.0f);

                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.PinkTorch, 0f, -1.2f, 150, new Color(255, 160, 255), 1.0f);
                    Main.dust[dust].velocity *= 0.3f;
                    Main.dust[dust].noGravity = true;
                }

                if (Projectile.timeLeft < fadeOutDuration)
                {
                    float fade = Projectile.timeLeft / (float)fadeOutDuration;
                    Projectile.alpha = (int)(255 * (1f - fade));
                    Projectile.scale = 1f * fade;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            NebulaTrail.Draw(Projectile);

            int frameHeight = TextureAssets.Projectile[Type].Value.Height / Main.projFrames[Type];
            Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, TextureAssets.Projectile[Type].Value.Width, frameHeight);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float rot = 0f; // повернули на 90° вправо


            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos, frame,
                Color.White, rot,
                new Vector2(TextureAssets.Projectile[Type].Value.Width, frameHeight) / 2,
                Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

    }

    public struct NebulaTrail
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        public static void Draw(Projectile proj)
        {
            float transit = Utils.GetLerpValue(0f, 6f, proj.localAI[0], clamped: true);
            MiscShaderData shader = GameShaders.Misc["FlameLash"];
            shader.UseSaturation(2f);
            shader.UseOpacity(MathHelper.Lerp(3f, 5f, transit));
            shader.Apply();

            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        private static Color StripColors(float progress)
        {
            float lerpVal = Utils.GetLerpValue(0f, 0.7f, progress, clamped: true);
            Color brightPink = new Color(255, 140, 255);
            Color deepPurple = new Color(160, 60, 200);
            Color midGlow = new Color(255, 90, 220);
            Color result = Color.Lerp(Color.Lerp(brightPink, midGlow, 0.5f), deepPurple, lerpVal)
                           * (1f - Utils.GetLerpValue(0f, 0.98f, progress));
            result.A /= 8;
            return result;
        }

        private static float StripWidth(float progress)
        {
            float lerpVal = Utils.GetLerpValue(0f, 0.06f + 0.01f, progress, clamped: true);
            lerpVal = 1f - (1f - lerpVal) * (1f - lerpVal);
            return MathHelper.Lerp(16f, 6f, Utils.GetLerpValue(0f, 1f, progress, clamped: true)) * lerpVal;
        }
    }
}
