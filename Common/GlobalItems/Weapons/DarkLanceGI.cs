using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class DarkLanceGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int hitCounter = 0;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DarkLance;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCounter++;

            if (hitCounter >= 4)
            {
                hitCounter = 0;

                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 velocity = player.DirectionTo(target.Center) * 15f;

                    Projectile.NewProjectile(
                        player.GetSource_OnHit(target),
                        player.Center,
                        velocity,
                        1,//ModContent.ProjectileType<DarkRework2>(),
                        (int)(item.damage * 1.5f),
                        item.knockBack * 2f,
                        player.whoAmI,
                        ai0: player.itemAnimationMax // Передаем время использования как ai[0]
                    );
                }

                // Визуальный эффект
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(target.Center, 10, 10, DustID.Shadowflame);
                    dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
                    dust.scale = 1.5f;
                    dust.noGravity = true;
                }
            }
        }


        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            // Можно добавить небольшой бонус к урону на 4-м ударе
            if (hitCounter == 3) // Следующий удар будет 4-м
            {
                damage += 0.25f; // +25% урона на заряженном ударе
            }
        }

    }
}