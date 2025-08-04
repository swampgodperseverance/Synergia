using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Dusts;
using Microsoft.Xna.Framework;

namespace Vanilla.Content.Projectiles.Hostile
{
    public class DefensiveRing : ModProjectile
    {
        private const float RotationSpeed = 0.05f;
        private const float HeightOffset = -0f;
        private int? _lothorTypeCache = null;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 296;
            Projectile.height = 296;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.Opacity = 1f;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (Projectile.ai[0] >= 0 && Projectile.ai[0] < Main.maxNPCs)
            {
                NPC boss = Main.npc[(int)Projectile.ai[0]];
                if (boss.active && IsLothor(boss))
                {
                    Projectile.Center = boss.Center + new Vector2(0, HeightOffset);
                    
                    Projectile.rotation += RotationSpeed;

                    if (Main.rand.NextBool(3))
                    {
                        SpawnRedLineDust();
                    }
                    return;
                }
            }
            Projectile.Kill();
        }

        private void SpawnRedLineDust()
        {
            Vector2 spawnPosition = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width/2, Projectile.height/2);
            Vector2 velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 2f;
            
            Dust dust = Dust.NewDustPerfect(
                spawnPosition,
                ModContent.DustType<RedLineDust>(),
                velocity,
                0,
                Color.Red,
                1.5f
            );
            dust.noGravity = true;
            dust.noLight = false;
            dust.fadeIn = 1.2f;
        }

        private bool IsLothor(NPC npc)
        {
            if (_lothorTypeCache == null)
            {
                Mod roaMod = ModLoader.GetMod("RoA");
                if (roaMod != null)
                {
                    _lothorTypeCache = roaMod.Find<ModNPC>("Lothor")?.Type;
                }
                _lothorTypeCache ??= -1;
            }
            return npc.type == _lothorTypeCache;
        }
    }
}