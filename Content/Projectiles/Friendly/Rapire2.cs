using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Friendly
{
    public class Rapire2 : ModProjectile
    {
        private float fadeProgress = 0f;
        private bool isFading = false;
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 60;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 15 && !isFading)
            {
                isFading = true;
                fadeProgress = 0f;
            }

            if (isFading)
            {
                fadeProgress += 1f / 15f;
                Projectile.scale = MathHelper.Lerp(1f, 0.5f, fadeProgress);
                Projectile.alpha = (int)(fadeProgress * 200);
            }

            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.1f * (1f - fadeProgress));

            if (Main.rand.NextBool(isFading ? 4 : 2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 120, default, 1.2f * (1f - fadeProgress));
                dust.noGravity = true;
                dust.velocity *= (1f - fadeProgress);
            }
            
            if (Main.rand.NextBool(isFading ? 6 : 3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava,
                    Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 100, default, 1.1f * (1f - fadeProgress));
                dust.velocity *= 0.3f * (1f - fadeProgress);
            }

            if (isFading)
            {
                Projectile.velocity *= 0.92f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (isFading)
            {
                CreateDisappearEffect();
            }
            else
            {
                CreateExplosionEffect();
            }
        }

        private void CreateDisappearEffect()
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GoldFlame, 
                    velocity.X, velocity.Y, 100, new Color(255, 200, 100), 1.8f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                dust.velocity *= 1.5f;
            }

            for (int i = 0; i < 15; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(2f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Lava, 
                    velocity.X, velocity.Y, 80, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.8f;
            }

            Lighting.AddLight(Projectile.Center, 1.5f, 1f, 0.3f);
        }

        private void CreateExplosionEffect()
        {
            int explosionRadius = 80;
            int explosionDamage = (int)(Projectile.damage * 0.6f);
            
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy() && 
                    Vector2.Distance(npc.Center, Projectile.Center) < explosionRadius)
                {
                    int direction = npc.direction;
                    NPC.HitInfo hit = new NPC.HitInfo()
                    {
                        Damage = explosionDamage,
                        Knockback = 0f,
                        HitDirection = direction,
                        Crit = false
                    };
                    
                    npc.StrikeNPC(hit);
                    npc.AddBuff(BuffID.OnFire3, 180);
                }
            }

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Lava, 
                    velocity.X, velocity.Y, 120, default, 2f);
                dust.noGravity = true;
            }
            
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch, 
                    velocity.X, velocity.Y, 100, default, 1.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) 
                    continue;

                float progress = i / (float)Projectile.oldPos.Length;
                float fade = (1f - progress) * (1f - fadeProgress);
                Color color = new Color(255, 120, 40, 100) * fade * 0.7f;
                float scale = Projectile.scale * (0.8f + progress * 0.2f);

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0);
            }

            Color drawColor = lightColor * (1f - fadeProgress);
            drawColor.A = (byte)(255 * (1f - fadeProgress));
            
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, 
                drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            
            return false;
        }
    }
}