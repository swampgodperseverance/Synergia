using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class UnderwaterAuraProjectile : AuraAI
    {
        private int damageTimer = 0;
        private const int DamageInterval = 60; // Damage every 60 ticks (1 second)

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            buffTypes[AuraEffectTarget.Team].Add(BuffID.WaterWalking);

            auraColor = new Color(100, 200, 255, 160); // голубая аура
            auraColor2 = new Color(80, 180, 230, 120);

            distanceMax = 160f;
            orbCount = 6;
            orbSpeed = 1.0f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;
            spectreCut = false;
        }

        public override void PostAI()
        {
            float radius = distanceMax;

            // Увеличиваем радиус на 16% в воде
            if (Collision.DrownCollision(Projectile.Center, Projectile.width, Projectile.height))
            {
                radius *= 1.16f;
            }

            // Пузырьки вокруг — визуальный эффект
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(radius, radius),
                    DustID.Water,
                    Main.rand.NextVector2Circular(0.5f, 0.5f),
                    100,
                    default,
                    1.1f
                ).noGravity = true;
            }

            // Эффекты на игроков (постоянные)
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.Distance(player.Center, Projectile.Center) < radius)
                {
                    player.statDefense += 6;
                    player.accFlipper = true; // Подвижность в воде
                    player.fishingSkill += 20; // Рыбалка
                }
            }

            // Урон наносится только по таймеру
            damageTimer++;
            if (damageTimer >= DamageInterval)
            {
                damageTimer = 0;
                
                // Эффекты на врагов (только когда таймер срабатывает)
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Projectile.Center) < radius)
                    {
                        var hitInfo = new NPC.HitInfo()
                        {
                            Damage = 10,
                            Knockback = 0f,
                            HitDirection = 0
                        };
                        npc.StrikeNPC(hitInfo);
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, 0f, 0f, 150, default, 1.2f);

                    }
                }
            }
        }
    }
}