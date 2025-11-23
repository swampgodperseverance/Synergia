using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks;
using Synergia.Content.Projectiles.Reworks.AltUse;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class GloomScepter : GlobalItem
    {
        public override bool InstancePerEntity => true;

        // Кулдаун в кадрах (60 кадров = 1 секунда)
        public int rightClickCooldown = 0;
        public bool isUsingAlt = false;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "GloomScepter", StringComparison.OrdinalIgnoreCase);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            return AppliesToEntity(item, false) || base.AltFunctionUse(item, player);
        }

        public override void SetDefaults(Item item)
        {
            if (AppliesToEntity(item, false))
            {
                // Уменьшаем скорость стрельбы в 3 раза
                item.useTime *= 3;       // Время между использованиями
                item.useAnimation *= 3;  // Длительность анимации использования
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            if (!AppliesToEntity(item, false))
                return;

            // Сбрасываем состояние использования ПКМ
            isUsingAlt = false;

            // ПКМ способность с кулдауном
            if (player.altFunctionUse == 2 && rightClickCooldown <= 0 && player.whoAmI == Main.myPlayer)
            {
                // Устанавливаем флаг использования ПКМ (скроет предмет)
                isUsingAlt = true;

                Vector2 direction = Main.MouseWorld - player.Center;
                if (direction != Vector2.Zero)
                    direction.Normalize();

                Vector2 spawnPos = player.Center + direction * 60f;
                Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
                spawnPos += perpendicular * 30f * player.direction; // смещение в сторону

                // Создаем снаряд на сервере, чтобы кооп работал
                if (Main.myPlayer == player.whoAmI)
                {
                    Projectile.NewProjectile(
                        player.GetSource_ItemUse(item),
                        spawnPos,
                        Vector2.Zero,
                        ModContent.ProjectileType<GloomR>(),
                        item.damage * 2,
                        2f,
                        player.whoAmI
                    );

                    rightClickCooldown = 60 * 60; // 60 секунд

                    // Визуальная обратная связь
                    CombatText.NewText(player.getRect(), Color.Purple, "Ability Used!", true);

                    // Синхронизация кулдауна на сервере
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, player.whoAmI);
                }
            }

            if (rightClickCooldown > 0)
                rightClickCooldown--;
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if (!AppliesToEntity(item, false))
                return;

            // Скрываем предмет при использовании ПКМ
            if (player.altFunctionUse == 2 && isUsingAlt)
            {
                player.itemLocation = Vector2.Zero;
                player.itemRotation = 0f;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (!AppliesToEntity(item, false))
                return base.CanUseItem(item, player);

            // Блокируем использование при кулдауне ПКМ
            if (player.altFunctionUse == 2 && rightClickCooldown > 0)
            {
                // Визуальная обратная связь о кулдауне
                if (player.whoAmI == Main.myPlayer)
                {
                    int secondsLeft = (int)Math.Ceiling(rightClickCooldown / 60f);
                    CombatText.NewText(player.getRect(), Color.Orange, $"Cooldown: {secondsLeft}s", true);
                }
                return false;
            }

            return base.CanUseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
                                   Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            if (!AppliesToEntity(item, false))
                return true;

            // Блокируем стандартный выстрел для ПКМ
            if (player.altFunctionUse == 2)
                return false;

            // Уменьшаем частоту выстрелов
            if (Main.rand.NextBool(4))
                return false;

            // Увеличиваем урон
            damage = (int)(damage * 1.5f);

            Vector2 direction = Main.MouseWorld - player.Center;
            if (direction != Vector2.Zero)
                direction.Normalize();

            Vector2 spawnPos = player.Center + direction * 60f;

            if (Main.myPlayer == player.whoAmI)
            {
                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<GloomRingAttack>(),
                    damage,
                    knockBack,
                    player.whoAmI,
                    0f,
                    1f
                );
            }

            return false; // блокируем стандартный выстрел
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity,
                                              ref int type, ref int damage, ref float knockBack)
        {
            if (!AppliesToEntity(item, false))
                return;

            velocity *= 0.7f; // замедляем скорость снаряда
        }

    }
}