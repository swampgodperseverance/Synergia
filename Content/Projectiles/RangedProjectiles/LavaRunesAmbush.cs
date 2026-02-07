using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public abstract class LavaRuneBase : ModProjectile
    {
        protected virtual Vector2 BaseOffset => Vector2.Zero;
        private VertexStrip _trail = new VertexStrip();

        private const float ORBIT_DISTANCE = 90f;
        private const float PULSE_BASE = 0.018f;
        private const int APPEAR_DURATION = 20;
        private const int FADE_DURATION = 30;

        public override string Texture => "Synergia/Content/Projectiles/RangedProjectiles/" + Name;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = false;       
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2400;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 255;
            Projectile.scale = 0.1f;
            Projectile.damage = 0;               
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Projectile.oldPos[i] = Projectile.Center;
                Projectile.oldRot[i] = Projectile.rotation;
            }
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (owner == null || !owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            Vector2 mousePos = Main.MouseWorld;

            var activeRunes = Main.projectile.Where(p => p.active && p.owner == Projectile.owner &&
                (p.type == ModContent.ProjectileType<LavaRune1>() ||
                 p.type == ModContent.ProjectileType<LavaRune2>() ||
                 p.type == ModContent.ProjectileType<LavaRune3>() ||
                 p.type == ModContent.ProjectileType<LavaRune4>())).ToList();

            int runeCount = activeRunes.Count;

            float pulseMultiplier = runeCount switch
            {
                1 => 0f,
                2 => 0.6f,
                3 => 1f,
                4 => 1.4f,
                _ => 1.4f
            };

            int timeAlive = 2400 - Projectile.timeLeft;

            if (timeAlive < APPEAR_DURATION)
            {
                float progress = (float)timeAlive / APPEAR_DURATION;
                float eased = progress * progress * (3f - 2f * progress);
                Projectile.alpha = (int)(255 * (1f - eased));
                Projectile.scale = MathHelper.Lerp(0.1f, 1.15f, eased);
                Projectile.rotation = MathHelper.Lerp(0f, MathHelper.PiOver4, eased) * (Projectile.whoAmI % 2 == 0 ? 1 : -1);
            }
            else if (Projectile.timeLeft > FADE_DURATION)
            {
                Projectile.alpha = 0;
                Projectile.rotation = 0f;

                float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f * (1f + pulseMultiplier * 0.3f) + Projectile.whoAmI * 1.4f)
                            * PULSE_BASE * pulseMultiplier;

                Projectile.scale = 1f + pulse * 0.12f;
            }
            else
            {
                float progress = Projectile.timeLeft / (float)FADE_DURATION;
                float eased = 1f - (1f - progress) * (1f - progress);
                Projectile.alpha = (int)(255 * (1f - eased));
                Projectile.scale = MathHelper.Lerp(1f, 0.35f, 1f - progress);
                Projectile.rotation += 0.18f * (1f - progress);
            }

            Vector2 desiredPos = mousePos + BaseOffset * ORBIT_DISTANCE;
            Projectile.Center = Vector2.Lerp(Projectile.Center, desiredPos, 0.16f);

            if (owner.controlUseItem && owner.itemAnimation == 0)
            {
                if (Projectile.ai[1] == 0)
                {
                    var mainRune = activeRunes.OrderBy(p => p.whoAmI).FirstOrDefault();
                    if (mainRune != null && mainRune.whoAmI == Projectile.whoAmI)
                    {
                        Projectile.ai[1] = 1;
                        Projectile.ai[0] = 0;
                        Projectile.netUpdate = true;

                        foreach (var rune in activeRunes)
                        {
                            if (rune.whoAmI != Projectile.whoAmI && rune.ai[1] == 0)
                            {
                                rune.ai[1] = 2;
                                rune.ai[0] = 0;
                                rune.netUpdate = true;
                            }
                        }
                    }
                }
            }

            if (Projectile.ai[1] > 0)
            {
                Projectile.velocity *= 0f;
                Projectile.ai[0]++;

                Vector2 currentMouse = Main.MouseWorld;

                if (Projectile.ai[0] <= 8)
                {
                    Vector2 dirBack = Vector2.Normalize(Projectile.Center - currentMouse);
                    float backDist = 4f * (8f - Projectile.ai[0]) / 8f;
                    Projectile.Center += dirBack * backDist;
                }
                else if (Projectile.ai[0] <= 22)
                {
                    float progress = (Projectile.ai[0] - 8f) / 14f;
                    progress *= progress;
                    float dashSpeed = MathHelper.Lerp(10f, 48f, progress);
                    Vector2 dirToMouse = Vector2.Normalize(currentMouse - Projectile.Center);
                    Projectile.Center += dirToMouse * dashSpeed;
                }
                else
                {
                    if (Projectile.ai[1] == 1)
                    {
                        SpawnExplosion();
                    }
                    Projectile.Kill();
                }
            }

            if (Vector2.Distance(Projectile.Center, Projectile.oldPos[0]) > 0.5f)
            {
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
                {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                    Projectile.oldRot[i] = Projectile.oldRot[i - 1];
                }
                Projectile.oldPos[0] = Projectile.Center;
                Projectile.oldRot[0] = Projectile.rotation;
            }
        }

        private void SpawnExplosion()
        {
            int fixedDamage = Main.rand.Next(900, 1100);
            int boomType = ModContent.ProjectileType<EnferBoom>();
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero, 
                boomType,
                fixedDamage, 
                8f,        
                Projectile.owner
            );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            float opacity = 1f - Projectile.alpha / 255f;

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            GameShaders.Misc["MagicMissile"].Apply();

            float trailOpacity = opacity * (Projectile.timeLeft < FADE_DURATION ? 1.5f : 1f);

            _trail ??= new VertexStrip();

            _trail.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                p => Color.Lerp(
                    new Color(255, 180, 60) * (trailOpacity * (p <= 0.25f ? p / 0.25f : 1f)),
                    new Color(255, 240, 100) * (trailOpacity * 0.7f),
                    p
                ),
                p => 58f * Projectile.scale * (1f - p) * (Projectile.timeLeft < FADE_DURATION ? 1.4f : 1f),
                -Main.screenPosition + Projectile.Size / 2f,
                true
            );

            _trail.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

            float glowPulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 10f + Projectile.whoAmI) * 0.5f + 0.5f;
            Color glowColor = Color.Lerp(new Color(255, 180, 60), new Color(255, 240, 120), glowPulse) * opacity * 0.9f;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float baseScale = Projectile.scale * (1f + glowPulse * 0.3f);
            sb.Draw(tex, Projectile.Center - Main.screenPosition, null, glowColor * 0.8f, Projectile.rotation, tex.Size() / 2f, baseScale * 1.5f, SpriteEffects.None, 0f);
            sb.Draw(tex, Projectile.Center - Main.screenPosition, null, glowColor * 0.45f, Projectile.rotation, tex.Size() / 2f, baseScale * 2f, SpriteEffects.None, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Color mainColor = new Color(1f, 0.88f, 0.6f, 0.95f) * opacity;
            sb.Draw(tex, Projectile.Center - Main.screenPosition, null, mainColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White * (1f - Projectile.alpha / 255f);
    }

    public class LavaRune1 : LavaRuneBase { protected override Vector2 BaseOffset => new Vector2(0, -1); }
    public class LavaRune2 : LavaRuneBase { protected override Vector2 BaseOffset => new Vector2(0, 1); }
    public class LavaRune3 : LavaRuneBase { protected override Vector2 BaseOffset => new Vector2(-1, 0); }
    public class LavaRune4 : LavaRuneBase { protected override Vector2 BaseOffset => new Vector2(1, 0); }
}   