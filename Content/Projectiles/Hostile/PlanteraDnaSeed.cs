using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Hostile
{
    public class PoisonDnaSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("DNA Seed");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
        }

        private Vector2 baseVelocity;
        //private float speed = 20f;
        private float frequency = 0.30f;
        private float waveMagnitude = 120f;
        //private float phase;

         public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {

                Player target = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                baseVelocity = direction * 5f;
                Projectile.ai[1] = 1; 
            }

            float time = Projectile.localAI[0] + Projectile.ai[0] * MathF.PI;
            float waveOffset = (float)Math.Sin(time * frequency) * waveMagnitude;
            Vector2 normal = baseVelocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero);
            Vector2 offset = normal * waveOffset;

            Projectile.velocity = baseVelocity;
            Projectile.position += offset * 0.05f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.localAI[0] += 1f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;
                Color trailColor = Color.LimeGreen * progress;
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Main.spriteBatch.Draw(texture, drawPos, null, trailColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Vector2 pos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, pos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}