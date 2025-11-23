using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Synergia.Common.Players;
using Synergia.Content.Projectiles.Thrower;

namespace Synergia.Common.GlobalItems
{
    public class SnowballGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            // ✅ ПРОВЕРЯЕМ SNOWBALL (ванильный + модовые)
            if (entity.type == ItemID.Snowball)
                return true;

            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            // ✅ Ищем модовые снежки по имени
            return modName != null && itemName != null &&
                   itemName.Contains("Snowball", StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<SnowballPlayer>();
            
            if (modPlayer.doubleMode)
            {
                // ✅ Твой SnowballMega
                int newType = ModContent.ProjectileType<SnowballMega>();
                velocity *= 1.2f;
                damage = (int)(damage * 0.8f);
                knockback *= 1.2f;

                Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
                return false; // НЕ стреляем оригинал
            }

            return true; // Обычный снежок
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<SnowballPlayer>().AddCombo();
        }

        public override void HoldItem(Item item, Player player)
        {
            var modPlayer = player.GetModPlayer<SnowballPlayer>();
            if (modPlayer.interfaceProj != null && modPlayer.interfaceProj.active &&
                modPlayer.interfaceProj.ModProjectile is ThrowerInterface1 ui)
            {
                ui.SetFrame(modPlayer.comboCount);
            }
        }
    }
}