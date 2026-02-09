using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class AzraelsHeartstopper2 : ModProjectile
    {
        private PrimDrawer visualTrail;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 106;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            DrawOffsetX = -17;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0f;
        }

        public override void AI()
        {
            if (visualTrail == null)
            {
                MiscShaderData shader = null;
                string[] possibleShaders = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
                foreach (var key in possibleShaders)
                {
                    if (GameShaders.Misc.TryGetValue(key, out shader))
                        break;
                }
                if (shader != null)
                {
                    shader.UseImage1("Images/Misc/noise");
                    shader.UseOpacity(0.75f);
                    shader.UseColor(new Color(100, 220, 255));
                    shader.UseSecondaryColor(new Color(40, 140, 220));
                }
                visualTrail = new PrimDrawer(
                    widthFunc: t => MathHelper.Lerp(7f, 0.5f, t),
                    colorFunc: t =>
                    {
                        Color start = new Color(140, 230, 255);
                        Color end = new Color(60, 160, 255);
                        float blend = MathHelper.Clamp(t * 1.6f, 0f, 1f);
                        Color mix = Color.Lerp(start, end, blend);
                        mix *= (1f - t * 0.9f);
                        return mix;
                    },
                    shader: shader
                );
            }

            Lighting.AddLight(Projectile.Center, 0.2f, 0.4f, 0.7f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (visualTrail != null)
            {
                var points = new System.Collections.Generic.List<Vector2>();
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero) continue;
                    points.Add(Projectile.oldPos[i] + Projectile.Size / 2f);
                }
                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
                    visualTrail.DrawPrims(points, offset, totalTrailPoints: 48);
                }
            }
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                int type = ModContent.ProjectileType<CupidJavelin>();
                for (int i = 0; i < 4; i++)
                {
                    float angle = MathHelper.TwoPi * i / 4f + Main.rand.NextFloat(-0.25f, 0.25f);
                    Vector2 vel = angle.ToRotationVector2() * 5.5f;
                    int p = Projectile.NewProjectile(
                        Projectile.GetSource_OnHit(target),
                        Projectile.Center,
                        vel,
                        type,
                        0,
                        0f,
                        Projectile.owner
                    );
                    Main.projectile[p].rotation = vel.ToRotation() + MathHelper.PiOver4;
                }
            }
            SoundEngine.PlaySound(SoundID.Item117, Projectile.position);
        }
    }

    public class CupidJavelin : ModProjectile
    {
        private const int LIFETIME = 300;
        private const float MAX_SPEED = 16f;
        private const float ACCELERATION = 0.18f;
        private const int HEAL_AMOUNT = 25;
        private float appearTimer = 0f;
        private const float APPEAR_TIME = 20f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = LIFETIME;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            DrawOffsetX = -4;
            DrawOriginOffsetY = -4;
        }

        public override void AI()
        {
            if (appearTimer < APPEAR_TIME)
            {
                appearTimer++;
                Projectile.alpha = (int)(255 * (1f - appearTimer / APPEAR_TIME));
            }
            else
            {
                Projectile.alpha = 0;
            }

            if (Projectile.velocity.Length() < MAX_SPEED)
            {
                Projectile.velocity *= (1f + ACCELERATION);
                if (Projectile.velocity.Length() > MAX_SPEED)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= MAX_SPEED;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            Lighting.AddLight(Projectile.Center, 0.4f, 0.15f, 0.25f);
        }

        public override bool? CanHitNPC(NPC target) => false;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (target.statLife < target.statLifeMax2 && !target.immune)
            {
                int heal = HEAL_AMOUNT;
                target.statLife += heal;
                if (target.statLife > target.statLifeMax2) target.statLife = target.statLifeMax2;
                target.HealEffect(heal, true);
                CombatText.NewText(target.Hitbox, CombatText.HealLife, heal, dramatic: true);

                for (int i = 0; i < 12; i++)
                {
                    Vector2 dir = Main.rand.NextVector2Circular(1f, 1f);
                    int d = Dust.NewDust(Projectile.Center + dir * 20f, 0, 0, DustID.MagicMirror, dir.X * 1.2f, dir.Y * 1.2f, 140, default, 1.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].fadeIn = 0.9f;
                    Main.dust[d].velocity *= 0.8f;
                }

                SoundEngine.PlaySound(SoundID.Item4 with { PitchVariance = 0.3f, Volume = 0.6f }, Projectile.position);
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;
            Color drawColor = Color.White * (1f - Projectile.alpha / 255f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;
                float progress = (float)i / Projectile.oldPos.Length;
                float alphaMult = MathHelper.Lerp(0.7f, 0.05f, progress);
                Color afterimageColor = drawColor * alphaMult * 0.8f;
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float rot = Projectile.oldRot[i];

                Main.spriteBatch.Draw(
                    texture,
                    pos,
                    null,
                    afterimageColor,
                    rot,
                    origin,
                    Projectile.scale * MathHelper.Lerp(1f, 0.4f, progress),
                    SpriteEffects.None,
                    0f
                );
            }

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 255 - Projectile.alpha);

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.HeartCrystal, 0f, 0f, 100, default, 1.2f);
            }
        }
    }
}