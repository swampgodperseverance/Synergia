using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Synergia.Helpers;
using Avalon.Particles;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class GloomProj1 : ModProjectile
    {
        private Vector2 initialVelocity;
        private float travelTime = 180f; 
        private float elapsed = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (elapsed == 0f)
                initialVelocity = Projectile.velocity;

            elapsed++;
            float t = MathHelper.Clamp(elapsed / travelTime, 0f, 1f);
            Projectile.velocity = initialVelocity * (1f - EaseFunctions.EaseOutCubic(t));

      
            NPC target = FindClosestNPC(300f); 
            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;
                float distanceFactor = MathHelper.Clamp(1f - (toTarget.Length() / 300f), 0f, 1f);
                Vector2 desiredVelocity = Vector2.Normalize(toTarget) * Projectile.velocity.Length();

          
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.05f * distanceFactor);
            }

            if (Projectile.velocity.LengthSquared() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.velocity.Length() < 0.5f)
                Explode();
        }


        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDistance = maxDetectDistance * maxDetectDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this))
                {
                    float sqrDistance = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                    if (sqrDistance < sqrMaxDistance)
                    {
                        sqrMaxDistance = sqrDistance;
                        closestNPC = npc;
                    }
                }
            }
            return closestNPC;
        }


        private void Explode()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
                d.velocity = Main.rand.NextVector2Circular(2f, 2f);
                d.scale = Main.rand.NextFloat(0.9f, 1.3f);
                d.noGravity = true;
                d.fadeIn = 1.2f;
            }


            ParticleSystem.AddParticle(
                new EnergyRevolverParticle(),
                Projectile.Center,
                Vector2.Zero,
                new Color(80, 170, 255), 
                0,
                0.9f,
                20
            );

            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new(texture.Width / 2f, texture.Height / 2f);


            Color drawColor = new Color(100, 200, 255) * Projectile.Opacity; 
            Color trailColor = drawColor * 0.6f;
            Color trailColor2 = new Color(40, 120, 255) * 0.6f;

    
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float fade = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Main.spriteBatch.Draw(
                    texture,
                    pos,
                    null,
                    Color.Lerp(trailColor2, trailColor, fade) * fade,
                    Projectile.oldRot[i],
                    drawOrigin,
                    Projectile.scale,
                    spriteEffects,
                    0f
                );
            }

     
            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                spriteEffects,
                0f
            );

            return false;
        }
    }
}
