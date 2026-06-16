using System;
using Microsoft.Xna.Framework;
using NewHorizons;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class granitburGI : GlobalItem
    {
        public override bool InstancePerEntity => true;
        private int swingDirection;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "Granitbur", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;
            item.scale = 1.2f;
            item.shoot = ModContent.ProjectileType<GranitburRework>();
        }

        public int swingCounter = 0;

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = Vector2.Zero;
            swingDirection = swingDirection == 1 ? -1 : 1;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 handOffset = new Vector2(player.direction * 12, -4 * player.gravDir);
            Vector2 handPos = player.RotatedRelativePoint(player.MountedCenter + handOffset);

            Projectile.NewProjectile(
                source,
                handPos,
                Vector2.Zero,
                type,
                damage,
                knockback,
                player.whoAmI,
                ai0: swingDirection,
                ai1: player.MountedCenter.AngleTo(Main.MouseWorld)
            );

            return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
}