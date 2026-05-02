using System;
using Avalon.Items.Weapons.Summon.Hardmode.Gastropod;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Synergia.Content.Global
{
    public class sOLARDISKgi : GlobalItem
    {
        public override bool InstancePerEntity => true;
        private int shootTimer = 0;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "Bismuth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "SolarDisk", StringComparison.OrdinalIgnoreCase);
        }
        public override void SetDefaults(Item item)
        {
            item.autoReuse = true;
            item.useTime += 10;
            item.useAnimation += 10;
            item.shoot = ModContent.ProjectileType<SolarDisc2>();
        }
    }
}