using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Helpers;

namespace Synergia.Content.Projectiles.Reworks
{
    public class FlamingBladeRework : ModProjectile
    {
        private static readonly SoundStyle swordSound = new SoundStyle("Synergia/Assets/Sounds/swordSound")
        {
            Volume = 0.9f,
            PitchVariance = 0.15f
        };

        private Player Player => Main.player[Projectile.owner];
        private ref float Timer => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 60;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new(6);
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
            Projectile.noEnchantmentVisuals = true;
        }

        public override void AI()
        {
            Timer++;

            Projectile.Center = Player.Center;
            Projectile.spriteDirection = Projectile.direction;

            float progress = MathHelper.Clamp(Timer / 90f, 0f, 1f);
            float eased = EaseInOutQuad(progress);
            float rotationSpeed = MathHelper.Lerp(0f, 0.25f * Player.direction, eased);
            Projectile.rotation += rotationSpeed;

         
            float pulse = 0.8f + 0.2f * (float)Math.Sin(Timer * 0.15f);
            Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, pulse, 0.1f);

            if (Timer % 5 == 0)
            {
                int soundCooldown = 20;
                if (Projectile.localAI[1] <= 0)
                {
                    SoundStyle swing = swordSound with
                    {
                        Volume = 0.9f + Main.rand.NextFloat(-0.05f, 0.05f),
                        Pitch = Main.rand.NextFloat(-0.1f, 0.1f)
                    };
                    SoundEngine.PlaySound(swing, Projectile.Center);
                    Projectile.localAI[1] = soundCooldown;
                }
                else
                    Projectile.localAI[1]--;
            }

            if (!Player.channel || Player.noItems || Player.CCed || Timer > 160f)
                Player.reuseDelay = 3;

            SpawnSwordDust();

            if (Main.myPlayer == Projectile.owner && Timer % 45 == 0 && Main.rand.NextBool(4))
                ShootFireball();

            if (!Player.channel)
            {
                Projectile.localAI[0]++;
                float fadeProgress = Projectile.localAI[0] / 45f;
                fadeProgress = MathHelper.Clamp(fadeProgress, 0f, 1f);

                Projectile.rotation *= MathHelper.Lerp(1f, 0.92f, fadeProgress);
                Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 0f, fadeProgress); 
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.85f, fadeProgress);
                Lighting.AddLight(Projectile.Center, 0.8f * Projectile.Opacity, 0.25f * Projectile.Opacity, 0f);

                if (fadeProgress >= 1f)
                {
                    Projectile.Kill();
                    return;
                }
            }
            else
            {
                Projectile.localAI[0] = 0;
            }

            UpdatePlayerVisuals();
        }

        private void SpawnSwordDust()
        {
            float rotation = Projectile.rotation - (Projectile.direction == -1 ? 0f : MathHelper.PiOver2) - MathHelper.PiOver4;

            int dustCount = 10;

            for (int i = 0; i < dustCount; i++)
            {

                float alongBlade = Main.rand.NextFloat(0.2f, 1f);
                Vector2 basePos = Projectile.Center + rotation.ToRotationVector2() * 85f * alongBlade;

                Vector2 offset = Main.rand.NextVector2Circular(10f, 10f);
                Vector2 dustPos = basePos + offset;

                Vector2 dustVel = rotation.ToRotationVector2().RotatedByRandom(0.7f) * Main.rand.NextFloat(0.3f, 1.2f);

                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Torch, dustVel);
                dust.noGravity = true;

                dust.scale = Main.rand.NextFloat(0.8f, 1.5f);

                dust.color = Color.Lerp(Color.White, new Color(255, 150, 50), Main.rand.NextFloat(0.3f, 1f));
            }
        }

                

        private void ShootFireball()
        {
            float rotation = Projectile.rotation - (Projectile.direction == -1 ? 0f : MathHelper.PiOver2) - MathHelper.PiOver4;
            Vector2 shootDir = rotation.ToRotationVector2();
            Vector2 spawnPos = Projectile.Center + shootDir * 60f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                spawnPos,
                shootDir * 10f,
                ModContent.ProjectileType<FireballProjectile>(),
                Projectile.damage / 2,
                Projectile.knockBack,
                Projectile.owner
            );

            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.7f }, spawnPos);
        }

        private void UpdatePlayerVisuals()
        {
            if (Player.channel)
                Projectile.timeLeft = 20;

            Player.ChangeDir(Projectile.direction);
            Player.SetDummyItemTime(2);
            Player.heldProj = Projectile.whoAmI;

            float rotation = Projectile.rotation - MathHelper.Pi - MathHelper.PiOver4;
            if (Player.direction == -1)
                rotation += MathHelper.PiOver4 * 2;

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            float rotation = Projectile.rotation - (Projectile.direction == -1 ? 0f : MathHelper.PiOver2) - MathHelper.PiOver4;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center + rotation.ToRotationVector2() * 85f,
                4f,
                ref collisionPoint
            );
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.active || target.CountsAsACritter || target.immortal || target.dontTakeDamage)
                return;

            target.AddBuff(BuffID.OnFire, 600);
        }

       public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new(Projectile.spriteDirection == 1 ? texture.Width : 0f, texture.Height);
            Color color = Color.White * Projectile.Opacity;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float fade = (1f - i / (float)Projectile.oldPos.Length);
                Color trail = color * 0.3f * fade;
                Main.spriteBatch.Draw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition,
                    null,
                    trail,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale,
                    effects,
                    0f
                );
            }

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                color,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0f
            );

            return false;
        }

        public static float EaseInOutQuad(float t) => t < 0.5f ? 2 * t * t : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2;
    }
}
