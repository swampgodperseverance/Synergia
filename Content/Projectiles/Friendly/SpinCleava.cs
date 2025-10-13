using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Friendly
{
    public class SpinCleava : ModProjectile
    {
        private bool launched = false;
        private int timer = 0;
        private float rotationSpeed = 0f;
        private float pulse = 0f;
        private float launchProgress = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 400;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.scale = 0.5f; 
        }

        public override void AI()
        {
            timer++;
            if (Projectile.alpha > 0)
                Projectile.alpha -= 4;

            pulse = (float)Math.Sin(Main.GameUpdateCount * 0.25f) * 0.5f + 0.5f;

            if (!launched)
            {

                rotationSpeed = MathHelper.Clamp(rotationSpeed + 0.008f, 0f, 0.5f);
                Projectile.rotation += rotationSpeed;

                if (Projectile.scale < 1f)
                    Projectile.scale += 0.008f;

                Projectile.velocity *= 0.94f;
                Lighting.AddLight(Projectile.Center, 1.1f * pulse, 0.4f * pulse, 0f);

                if (timer >= 80)
                {
                    launched = true;
                    SoundEngine.PlaySound(SoundID.Item74 with { Pitch = -0.5f }, Projectile.Center);
                }
            }
            else
            {

                if (launchProgress < 1f)
                    launchProgress += 0.02f;

                float velocityStrength = MathHelper.SmoothStep(0f, 40f, launchProgress);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, new Vector2(0f, -velocityStrength), 0.1f);

                rotationSpeed = MathHelper.Lerp(rotationSpeed, 0f, 0.05f);
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

                Lighting.AddLight(Projectile.Center, 1.3f * pulse, 0.4f * pulse, 0f);
                CreateLavaTrail();


                if (Projectile.position.Y < Main.screenPosition.Y - 200f)
                {
                    SpawnBoneRain();
                    Projectile.Kill();
                }
            }
        }

        private void CreateLavaTrail()
        {
            Vector2 backPos = Projectile.Center + Projectile.velocity * -0.5f;
            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustPerfect(backPos + Main.rand.NextVector2Circular(8, 8), DustID.Lava,
                    new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(2, 4)));
                d.scale = Main.rand.NextFloat(1f, 1.4f);
                d.fadeIn = 0.7f;
                d.noGravity = true;
                d.alpha = 120;
                d.velocity *= 0.3f;
            }
        }

        private void SpawnBoneRain()
        {
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<ScreenShakePlayer>().TriggerShake(15, 1.2f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < Main.rand.Next(12, 16); i++)
            {
                float xOffset = Main.rand.NextFloat(-500, 500);
                float spawnY = Main.screenPosition.Y - 200f - Main.rand.NextFloat(100, 300);
                Vector2 spawnPos = new Vector2(Main.screenPosition.X + Main.screenWidth / 2 + xOffset, spawnY);
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(8f, 14f));

                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, velocity,
                    ModContent.ProjectileType<SpinBone>(), Projectile.damage / 2, 2f, Projectile.owner);

                if (p >= 0)
                {
                    Main.projectile[p].alpha = 255;
                    Main.projectile[p].rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                }
            }

            for (int i = 0; i < 40; i++)
            {
                Dust.NewDust(Projectile.Center, 50, 50, DustID.Lava,
                    Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 150, default, 1.8f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D trailTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 trailOrigin = trailTexture.Size() / 2;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;
                float trailScale = Projectile.scale * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Color trailColor = Color.Lerp(Color.OrangeRed, Color.Yellow, ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)) * 0.5f;
                Main.spriteBatch.Draw(trailTexture, trailPos, null, trailColor, Projectile.rotation, trailOrigin, trailScale, SpriteEffects.None, 0f);
            }

            Vector2 position = Projectile.Center - Main.screenPosition;
            Color glow = Color.Lerp(Color.OrangeRed, Color.Yellow, pulse) * (1f - Projectile.alpha / 255f);
            Main.spriteBatch.Draw(trailTexture, position, null, glow, Projectile.rotation, trailOrigin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

    }
}
