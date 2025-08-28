using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Friendly
{
    public class Rapire1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46; 
            Projectile.height = 46; 
            Projectile.aiStyle = 1; 
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 120, default, 1.3f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava,
                    Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 100, default, 1.1f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
        {

            CreateExplosionEffect();

            ApplyExplosionDamage();
        }

        private void CreateExplosionEffect()
        {

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Lava, 
                    velocity.X, velocity.Y, 120, default, 2f);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }

            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch, 
                    velocity.X, velocity.Y, 100, default, 1.5f);
                dust.noGravity = false;
            }

            for (int i = 0; i < 15; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(2f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GoldFlame, 
                    velocity.X, velocity.Y, 80, default, 1.2f);
                dust.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 2f, 1f, 0.5f);
        }

        private void ApplyExplosionDamage()
        {
            int explosionRadius = 100;
            int explosionDamage = (int)(Projectile.damage * 0.8f);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy() && 
                    Vector2.Distance(npc.Center, Projectile.Center) < explosionRadius)
                {
                    NPC.HitInfo hit = new NPC.HitInfo()
                    {
                        Damage = explosionDamage,
                        Knockback = 3f,
                        HitDirection = npc.Center.X > Projectile.Center.X ? 1 : -1,
                        Crit = false
                    };
                    
                    npc.StrikeNPC(hit);
                    
                    npc.AddBuff(BuffID.OnFire3, 180);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (texture == null || texture.IsDisposed)
                return false;

            Vector2 origin = texture.Size() / 2f;


            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) 
                    continue;

                float progress = i / (float)Projectile.oldPos.Length;
                float fade = (1f - progress) * 0.8f;
                Color color = new Color(255, 120, 40, 150) * fade;
                float scale = Projectile.scale * (0.7f + progress * 0.3f);

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, 
                lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            
            return false;
        }
    }
}