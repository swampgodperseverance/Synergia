using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Armor
{
    public class AshenCopy : ModProjectile
    {
        private int timer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 800;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            timer++;
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            if (timer == 1)
            {
                Vector2 spawnPos = owner.Center;
                spawnPos.Y += 16f;

                int tileX = (int)(spawnPos.X / 16f);
                int tileY = (int)(spawnPos.Y / 16f);

                while (tileY < Main.maxTilesY && Main.tile[tileX, tileY] == null || !Main.tile[tileX, tileY].HasTile)
                {
                    tileY++;
                }

                spawnPos.Y = tileY * 16f - Projectile.height / 2;
                Projectile.Center = spawnPos;
                Projectile.spriteDirection = owner.direction;
            }


            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = 0f;

            float pulse = 1f + (float)System.Math.Sin(timer * 0.04f) * 0.08f;
            Projectile.scale = pulse;

            if (timer < 30)
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, timer / 30f);
            else if (Projectile.timeLeft < 60)
            {
                float t = Projectile.timeLeft / 60f;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, t);
                Projectile.scale *= t;
            }

            SpawnAshDust();
            TauntEnemies();
        }

        private void SpawnAshDust()
        {
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(18, 24),
                    DustID.Ash,
                    new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(-1.2f, -0.2f)),
                    120,
                    new Color(200, 190, 170),
                    Main.rand.NextFloat(1.0f, 1.5f)
                );
                d.noGravity = true;
            }
        }

        private void TauntEnemies()
        {
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy())
                    continue;

                float dist = Vector2.Distance(npc.Center, Projectile.Center);
                if (dist > 900f)
                    continue;

                npc.target = Projectile.owner;
                Vector2 dir = Projectile.Center - npc.Center;
                npc.velocity += dir.SafeNormalize(Vector2.Zero) * 0.05f;
            }
        }

        public override bool? CanDamage() => false;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 210, 200, 255 - Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(24, 32),
                    DustID.Ash,
                    Main.rand.NextVector2Circular(2f, 2f),
                    100,
                    new Color(210, 180, 150),
                    1.4f
                );
                d.noGravity = true;
            }
        }
    }
}
