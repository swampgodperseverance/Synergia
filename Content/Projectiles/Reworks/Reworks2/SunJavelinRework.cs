using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class SunJavelinRework : JavelinAI
    {
        private const int MaxStickingJavelins = 5;
        private const int ExplosionDamage = 40; 

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // При обычном состоянии — белый
            if (Projectile.localAI[0] == 1f)
                return new Color(255, 220, 80);
            return Color.White;
        }

        public override void AI()
        {
            // Добавляем красивый короткий желтый трейл
            if (Main.rand.NextBool(2)) // не слишком часто
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            // Проверяем количество копий в цели
            int javelinCount = CountStickingJavelins(target);

            // Если достигли 5 копий, меняем цвет и взрываем
            if (javelinCount >= MaxStickingJavelins)
            {
                Projectile.localAI[0] = 1f; // Пометка, что он в "золотом" состоянии

                // Взрывной эффект
                Explode(target);
            }
        }

        private int CountStickingJavelins(NPC target)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.owner == Projectile.owner && 
                    other.type == Projectile.type && 
                    other.ai[0] == 1f && // Снаряд застрял
                    other.ai[1] == target.whoAmI)
                {
                    count++;
                }
            }
            return count;
        }

        private void Explode(NPC target)
        {
            // Звук взрыва
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Визуальные частицы
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GoldFlame, 
                    Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
            }

            // Дополнительный урон: константа + 25% от текущего урона снаряда
            int extraDamage = ExplosionDamage + (int)(Projectile.damage * 0.25f);

            int radius = 80; // радиус взрыва (в пикселях)
            
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.CanBeChasedBy() && 
                    Vector2.Distance(npc.Center, Projectile.Center) < radius)
                {
                    // Используем правильный метод для нанесения урона
                    int hitDirection = npc.Center.X < Projectile.Center.X ? -1 : 1;
                    
                    // Используем StrikeNPC с правильными параметрами
                    npc.SimpleStrikeNPC(extraDamage, hitDirection);
                }
            }

            // Удаляем снаряд (он уже "взорвался")
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            // Воспроизводим звук только если это не взрыв
            if (Projectile.localAI[0] != 1f)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }

            Vector2 usePos = Projectile.position;
            Vector2 rotVector = Utils.ToRotationVector2(Projectile.rotation - MathHelper.ToRadians(90f));
            usePos += rotVector * 16f;

            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(usePos, Projectile.width, Projectile.height, 
                    DustID.GoldFlame, 0f, 0f, 0, default, 1.2f);
                Dust dust = Main.dust[dustIndex];
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.velocity += rotVector * 2f;
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                usePos -= rotVector * 8f;
            }
        }
    }
}