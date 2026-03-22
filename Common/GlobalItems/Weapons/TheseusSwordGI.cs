using System;
using Microsoft.Xna.Framework;
using NewHorizons.Content.Items.Accessories;
using NewHorizons.Content.Projectiles.Melee;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class TheseuaSwordGI : GlobalItem
    {
        public override bool InstancePerEntity => true;
        private int swingDirection;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "Bismuth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "TheseusSword", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = true;
            item.useStyle = 5;
            item.shoot = ModContent.ProjectileType<TheseusSword>();
            item.useTurn = false;
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            {
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<TheseusSword>(), damage, knockback, player.whoAmI, 0f, this.XZ ? 1f : -1f, 0f).ai[1] = (this.XZ ? 1f : -1f);
                this.XZ = !this.XZ;
                return false;
            }
        }
        private bool XZ;
    }
}