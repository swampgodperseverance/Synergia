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
            Projectile.timeLeft = 2;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Vector2 spawnPos = Projectile.Center;

            for (int d = 0; d < 8; d++)
            {
                Dust dust = Dust.NewDustDirect(
                    spawnPos - new Vector2(4, 4),
                    8, 8,
                    DustID.PinkTorch,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    100,
                    default,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.8f;
            }

            Projectile.NewProjectile(
                Projectile.GetSource_FromAI(),
                spawnPos,
                Vector2.Zero,
                ModContent.ProjectileType<NebulaPike>(),
                64,
                1f,
                Projectile.owner
            );

            Projectile.Kill();
        }
    }
}