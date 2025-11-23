using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using Synergia.Content.Projectiles.Reworks;

namespace Synergia.Content.Projectiles.Reworks
{
    public class NebulaPikeSpawner : ModProjectile
    {
        public static readonly SoundStyle NebulaHarp = new("Synergia/Assets/Sounds/NebulaHarp");

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 0;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2; // мгновенно спавнит и исчезает
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {

            int count = Main.rand.Next(3,4);

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi * i / count; 
                float radius = Main.rand.NextFloat(40f, 60f); 
                Vector2 spawnPos = Projectile.Center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<NebulaPike>(),
                    64, // урон, можно изменить
                    1f, // knockback
                    Projectile.owner
                );
            }


            Projectile.Kill(); // уничтожаем спавнер
        }
    }
}
