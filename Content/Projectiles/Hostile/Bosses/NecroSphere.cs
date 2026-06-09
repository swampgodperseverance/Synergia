using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroSphere : ModProjectile
    {
        private float rotationSpeed = 0.08f;
        private float maxRotationSpeed = 0.48f;
        private float rotationAcceleration = 0.008f;

        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.scale = 0.8f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void AI()
        {
            rotationSpeed += rotationAcceleration;
            if (rotationSpeed > maxRotationSpeed)
                rotationSpeed = maxRotationSpeed;

            Projectile.rotation += rotationSpeed;

            float appearTime = 20f;
            float disappearTime = 20f;
            if (Projectile.timeLeft > 60)
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, (90f - Projectile.timeLeft) / appearTime);
            else if (Projectile.timeLeft < disappearTime)
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, (disappearTime - Projectile.timeLeft) / disappearTime);
            else
                Projectile.alpha = 0;

            float lightIntensity = 0.4f + (rotationSpeed / maxRotationSpeed) * 0.6f;
            Lighting.AddLight(Projectile.Center, new Vector3(0.4f * lightIntensity, 0f, 0.6f * lightIntensity));

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0, 0, 150, Color.Purple, 1.3f + rotationSpeed * 1.5f);
                d.noGravity = true;
                d.velocity *= 0.5f;
                d.velocity += Projectile.velocity.RotatedByRandom(rotationSpeed);
            }

            if (rotationSpeed > maxRotationSpeed * 0.7f && Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, 0, 0, 100, Color.Magenta, 1f);
                d.noGravity = true;
                d.velocity = Projectile.velocity.RotatedByRandom(rotationSpeed) * 0.3f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            float glowIntensity = 0.7f + (rotationSpeed / maxRotationSpeed) * 0.8f;
            return new Color(180, 100, 255, 200) * ((255 - Projectile.alpha) / 255f) * glowIntensity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            float rotationProgress = rotationSpeed / maxRotationSpeed;

            Color trailColor = new Color(160, 60, 255, 100);
            Color outlineColor = new Color(120, 20, 200, 180);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float scale = Projectile.scale * (1f - i / (float)Projectile.oldPos.Length);
                float alpha = (1f - i / (float)Projectile.oldPos.Length) * (0.5f + rotationProgress * 0.5f);
                float trailRotation = Projectile.oldRot[i];

                Main.spriteBatch.Draw(texture, drawPos, null, trailColor * alpha, trailRotation, origin, scale, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < 3; i++)
            {
                float outlineOffset = 2f + rotationProgress * 3f;
                Vector2 outlinePos = Projectile.Center - Main.screenPosition + new Vector2(
                    (i == 0 ? outlineOffset : (i == 1 ? -outlineOffset : 0)),
                    (i == 0 ? -outlineOffset : (i == 1 ? 0 : outlineOffset))
                );

                float outlineAlpha = 0.4f + rotationProgress * 0.3f;
                Main.spriteBatch.Draw(texture, outlinePos, null, outlineColor * outlineAlpha, Projectile.rotation, origin, Projectile.scale * 1.05f, SpriteEffects.None, 0f);
            }

            float glowScale = 1f + rotationProgress * 0.3f;
            Color glowColor = new Color(200, 100, 255, 80) * (0.5f + rotationProgress * 0.5f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, origin, Projectile.scale * glowScale, SpriteEffects.None, 0f);

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null,
                Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            float rotationProgress = rotationSpeed / maxRotationSpeed;
            float spreadSpeed = 14f + rotationProgress * 8f;

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, spreadSpeed, 0f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -spreadSpeed, 0f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, spreadSpeed, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -spreadSpeed, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, spreadSpeed * 0.7f, spreadSpeed * 0.7f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -spreadSpeed * 0.7f, spreadSpeed * 0.7f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, spreadSpeed * 0.7f, -spreadSpeed * 0.7f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -spreadSpeed * 0.7f, -spreadSpeed * 0.7f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.8f, Pitch = -0.3f }, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 150, Color.Purple, 1.8f + rotationProgress);
                d.noGravity = true;
                d.velocity *= 1.2f;
            }

            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, Color.Magenta, 1.3f);
                d.noGravity = true;
            }
        }
    }
}