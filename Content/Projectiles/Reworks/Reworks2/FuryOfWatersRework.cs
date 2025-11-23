using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Bismuth.Content.Projectiles;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class FuryOfWatersRework : ModProjectile
    {
        private int trailLength = 28; 
        private Vector2[] oldPositions;

        public override void SetDefaults()
        {
            Projectile.tileCollide = true;
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.scale = 1f;
            Projectile.extraUpdates = 3;
            Projectile.aiStyle = 1;

            oldPositions = new Vector2[trailLength];
        }

        public override void AI()
        {

            for (int i = 0; i < 1; i++)
            {
                int index = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.BlueCrystalShard, Projectile.oldVelocity.X * 0.4f, Projectile.oldVelocity.Y * 0.4f, 0, default(Color), 1f);
                Main.dust[index].scale = 0.6f;
                Main.dust[index].noGravity = true;
            }

            for (int i = trailLength - 1; i > 0; i--)
            {
                oldPositions[i] = oldPositions[i - 1];
            }
            oldPositions[0] = Projectile.Center;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < 40; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].velocity *= 6f;
                Main.dust[num].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D trailTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Trails/FuryWaters_Trail").Value;

            for (int i = 0; i < trailLength - 1; i++)
            {
                if (oldPositions[i] == Vector2.Zero) continue;

                float scale = Projectile.scale * ((trailLength - i) / (float)trailLength);
                float alpha = (trailLength - i) / (float)trailLength * 0.5f;

                Main.spriteBatch.Draw(trailTexture, oldPositions[i] - Main.screenPosition,
                    null, new Color(0, 200, 255) * alpha, Projectile.rotation,
                    trailTexture.Size() / 2, scale, SpriteEffects.None, 0f);
            }

            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            fallThrough = true;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int numberOfProjectiles = 3; 
            float spread = MathHelper.ToRadians(20f); 
            Vector2 baseVelocity = Projectile.velocity * 0.65f;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float rotation = MathHelper.Lerp(-spread, spread, i / (float)(numberOfProjectiles - 1));
                Vector2 newVelocity = baseVelocity.RotatedBy(rotation);

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    newVelocity,
                    ModContent.ProjectileType<FuryOfWatersP2>(),
                    60,
                    4f, 
                    Projectile.owner
                );
            }
        }
    }
}
