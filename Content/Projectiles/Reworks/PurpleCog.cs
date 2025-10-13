using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class PurpleCog : ModProjectile
    {
        private Vector2 startPos;
        private bool initialized = false;
        private bool exploded = false;
         public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2; 
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 400;
        }

        public override void AI()
        {
            if (!initialized)
            {
                startPos = Projectile.Center;
                initialized = true;
            }

            Projectile.rotation += 0.6f + Projectile.velocity.Length() * 0.08f;


            float heightTarget = 160f; 
            float distance = startPos.Y - Projectile.Center.Y;
            if (distance < heightTarget)
            {
                float t = distance / heightTarget;
                Projectile.velocity.Y = MathHelper.Lerp(-10f, -16f, t * t); 
            }
            else
            {
                Projectile.velocity *= 0.9f;

                if (!exploded && Projectile.velocity.Length() < 0.4f)
                {
                    exploded = true;
                    Explode();
                }
            }

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 100, default, 1.3f);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].noGravity = true;
            }
        }

        private void Explode()
        {
            Projectile.timeLeft = 3;

            NPC target = null;
            float maxDist = 1000f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(Projectile.Center, npc.Center) < maxDist)
                {
                    maxDist = Vector2.Distance(Projectile.Center, npc.Center);
                    target = npc;
                }
            }

            if (target != null)
            {
    
                Mod bismuth = ModLoader.GetMod("Bismuth");
                if (bismuth != null)
                {
                    var waveType = bismuth.Find<ModProjectile>("WaveOfForceP").Type;
                    Vector2 dir = Vector2.Normalize(target.Center - Projectile.Center);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        dir * 20f,
                        waveType,
                        Projectile.damage,
                        1f,
                        Projectile.owner
                    );
                }
            }

            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6), 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
            }


        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                1f,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}
