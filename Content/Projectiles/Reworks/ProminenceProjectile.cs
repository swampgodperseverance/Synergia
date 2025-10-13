using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class ProminenceProjectile : ModProjectile
    {
        private bool isDashing;
        private int dashTimer;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prominence");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1800;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            int index = GetProjectileIndex();
            float offsetY = 24f + index * 28f;

            Vector2 idlePos = player.Center + new Vector2(0, offsetY);
            float lerpSpeed = isDashing ? 0.02f : 0.12f;
            Projectile.Center = Vector2.Lerp(Projectile.Center, idlePos, lerpSpeed);

            Projectile.rotation += isDashing ? 0.4f : 0.05f;

            Lighting.AddLight(Projectile.Center, 1.4f, 1.2f, 0.3f);
            if (Main.rand.NextBool(10))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 120, default, 1.3f);
                Main.dust[dust].noGravity = true;
            }

            NPC target = FindClosestNPC(600f);
            if (target != null && !isDashing)
            {
                
                isDashing = true;
                dashTimer = 0;

                Vector2 dir = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = dir * 22f; 
            }

            if (isDashing)
            {
                dashTimer++;
                CreateTrailEffect();

                if (dashTimer > 30)
                {
                    isDashing = false;
                    Projectile.velocity *= 0.4f;
                }
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }
        }

        private int GetProjectileIndex()
        {
            int index = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.owner == Projectile.owner && other.type == Projectile.type)
                {
                    if (other.whoAmI == Projectile.whoAmI)
                        return index;
                    index++;
                }
            }
            return index;
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }
            return closestNPC;
        }

        private void CreateTrailEffect()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 trailPos = Projectile.position - Projectile.velocity * i * 0.3f;
                int dust = Dust.NewDust(trailPos, Projectile.width, Projectile.height, DustID.GoldFlame, 0, 0, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 25; i++)
            {
                int dust = Dust.NewDust(target.position, target.width, target.height, DustID.GoldFlame,
                    Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 100, default, 1.9f);
                Main.dust[dust].noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
    