using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Friendly
{
    public class PyroclastShoot : ModProjectile
    {
        private bool hasExploded = false;
        private float timer = 0f;
        private const float ExplosionTime = 90f; // 1.5 секунды при 60 FPS

        public override void SetStaticDefaults()
        {
    
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            timer++;

            // Вращение стрелы по направлению движения
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            // Визуальные эффекты полета
            CreateFlightDust();

            // Проверка на взрыв по времени
            if (timer >= ExplosionTime && !hasExploded)
            {
                Explode();
                return;
            }

            // Медленное замедление для эффекта "тяжелой" стрелы
            Projectile.velocity *= 0.995f;

            // Световой эффект
            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.1f);
        }

        private void CreateFlightDust()
        {
            // Основные лавовые частицы
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(10f, 10f),
                    DustID.Torch,
                    Projectile.velocity * -0.2f + Main.rand.NextVector2Circular(1f, 1f),
                    0,
                    new Color(255, 150, 50),
                    1.5f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }

            // Дополнительные оранжевые частицы
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(8f, 8f),
                    DustID.OrangeTorch,
                    Projectile.velocity * -0.3f + Main.rand.NextVector2Circular(1.5f, 1.5f),
                    0,
                    Color.Orange,
                    1.2f
                );
                dust.noGravity = true;
            }

            // Искры
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.FlameBurst,
                    Projectile.velocity * -0.1f + Main.rand.NextVector2Circular(2f, 2f),
                    0,
                    Color.Yellow,
                    0.8f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.4f;
            }

            // Дымный эффект
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Smoke,
                    Projectile.velocity * -0.2f + Main.rand.NextVector2Circular(1f, 1f),
                    100,
                    new Color(80, 80, 80, 100),
                    0.8f
                );
                dust.noGravity = true;
            }
        }

        private void Explode()
        {
            if (hasExploded) return;
            
            hasExploded = true;

            // Звук взрыва
            SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -0.2f }, Projectile.Center);

            // Создание взрывных частиц
            CreateExplosionDust();

            // Нанесение урона в области
            float explosionRadius = 120f;
            Vector2 explosionCenter = Projectile.Center;

            // Урон по NPC в радиусе взрыва
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.life > 0 && npc.type != NPCID.TargetDummy)
                {
                    float distance = Vector2.Distance(npc.Center, explosionCenter);
                    if (distance <= explosionRadius)
                    {
                        // Уменьшение урона с расстоянием
                        float damageMultiplier = 1f - (distance / explosionRadius);
                        int explosionDamage = (int)(Projectile.damage * damageMultiplier * 0.8f);
                        
                        if (explosionDamage > 0)
                        {
                            NPC.HitInfo hitInfo = new NPC.HitInfo()
                            {
                                Damage = explosionDamage,
                                Knockback = 5f,
                                HitDirection = Math.Sign(npc.Center.X - explosionCenter.X),
                                Crit = false
                            };
                            npc.StrikeNPC(hitInfo);
                        }

                        // Эффект горения
                        npc.AddBuff(BuffID.OnFire, 180);
                    }
                }
            }

            // Эффект отталкивания игроков
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    float distance = Vector2.Distance(player.Center, explosionCenter);
                    if (distance <= explosionRadius * 0.8f)
                    {
                        Vector2 knockback = Vector2.Normalize(player.Center - explosionCenter) * 4f;
                        player.velocity += knockback;
                        
                        // Нанесение урона игроку (если PvP включено)
                        if (Main.netMode != NetmodeID.SinglePlayer && player.hostile && player.active && !player.dead)
                        {
                            Player.HurtInfo hurtInfo = new Player.HurtInfo()
                            {
                                Damage = (int)(Projectile.damage * 0.3f),
                                Knockback = 4f,
                                HitDirection = Math.Sign(player.Center.X - explosionCenter.X)
                            };
                            player.Hurt(hurtInfo);
                        }
                    }
                }
            }

            // Удаление проекции
            Projectile.Kill();
        }

        private void CreateExplosionDust()
        {
            // Основной взрыв
            for (int i = 0; i < 25; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(8f, 8f),
                    0,
                    new Color(255, 100, 0),
                    2.5f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.4f;
            }

            // Огненные частицы
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.FlameBurst,
                    Main.rand.NextVector2Circular(12f, 12f),
                    0,
                    Color.Orange,
                    2f
                );
                dust.noGravity = true;
            }

            // Дым
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Smoke,
                    Main.rand.NextVector2Circular(6f, 6f),
                    100,
                    new Color(60, 60, 60, 100),
                    1.5f
                );
                dust.noGravity = true;
            }

            // Искры
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Firework_Red,
                    Main.rand.NextVector2Circular(10f, 10f),
                    0,
                    Color.Yellow,
                    1.2f
                );
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Эффект при попадании в NPC
            target.AddBuff(BuffID.OnFire, 120);

            // Дополнительные частицы при попадании
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    target.Center,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(4f, 4f),
                    0,
                    new Color(255, 150, 50),
                    1.5f
                );
                dust.noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Эффект при попадании в игрока (PvP)
            target.AddBuff(BuffID.OnFire, 60);
        }

        public override void OnKill(int timeLeft)
        {
            // Взрыв при столкновении с тайлами
            if (!hasExploded && Projectile.timeLeft > 0)
            {
                Explode();
            }
        }

       

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Стандартная коллизия для стрелы
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}