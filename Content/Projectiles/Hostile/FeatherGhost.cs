using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Hostile
{
    public class DesertBeakFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 46;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(
                new SoundStyle("Synergia/Assets/Sounds/FeatherFlow") 
                { 
                    Volume = 0.8f, 
                    Pitch = Main.rand.NextFloat(-0.2f, 0.2f),
                    MaxInstances = 3
                }, 
                Projectile.Center
            );
        }

            public override void AI()
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.tileCollide = true;
                    Projectile.ai[2]++;
                }

                // Первоначальное замедление вертикальной скорости
                if (Projectile.ai[1] == 0)
                {
                    Projectile.velocity.Y *= 0.5f;
                    Projectile.ai[1]++;
                }

                // Плавное появление снаряда
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 10;
                }

                // Управление вертикальной скоростью для подъема
                if (Projectile.ai[2] > 40)
                {
                    Projectile.velocity.Y -= 0.03f; // Снаряд поднимается
                }
                else
                {
                    Projectile.velocity *= 0.995f; // Легкое замедление горизонтальной скорости
                }
            }
            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                return true;
            }
            public override void OnKill(int timeLeft)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Sand);
                }
            }
        
    }
}