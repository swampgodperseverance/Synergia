using System; using Microsoft.Xna.Framework;using Microsoft.Xna.Framework.Graphics;using Terraria;using Terraria.ID;using Terraria.ModLoader;using Terraria.GameContent;using Terraria.Audio;

namespace Synergia.Content.Projectiles.Thrower
{
    public class BloodProjectile : ModProjectile
    {
        private bool hasHit = false;
        private const float HomingStrength = 0.12f;
        private const float MaxDetectRadius = 600f;

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            Projectile.light = 0.25f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 20, 20, 0);
        }

        public override void AI()
        {
            NPC target = FindClosestNPC(MaxDetectRadius);

            if (target != null)
            {
                Vector2 direction = Projectile.DirectionTo(target.Center);
                Vector2 desiredVelocity = direction * Projectile.velocity.Length();

                Projectile.velocity = Vector2.Lerp(
                    Projectile.velocity,
                    desiredVelocity,
                    HomingStrength
                );
            }

            Projectile.rotation =
                Projectile.velocity.ToRotation() + MathHelper.PiOver2;

          
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Blood,
                    Projectile.velocity * -0.2f
                );

                dust.scale = 1.3f;
                dust.noGravity = true;
            }
        }

     
        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closest = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float sqrDistance = Vector2.DistanceSquared(
                        npc.Center,
                        Projectile.Center
                    );

                    if (sqrDistance < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistance;
                        closest = npc;
                    }
                }
            }

            return closest;
        }

       
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasHit)
                return;

            hasHit = true;
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
         
            for (int i = 0; i < 25; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Blood,
                    velocity
                );

                dust.scale = 1.5f;
                dust.noGravity = false;
            }

            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
        }

       
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Color color = Color.Red * progress * 0.6f;

                Vector2 drawPos =
                    Projectile.oldPos[i] -
                    Main.screenPosition +
                    origin;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }
    }
}