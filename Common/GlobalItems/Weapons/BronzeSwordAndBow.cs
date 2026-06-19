using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.RangedProjectiles;
using Synergia.Content.Projectiles.Reworks.AltUse;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Global
{
    public class BronzeSwordGI : GlobalItem
    {
        private static Dictionary<int, int> playerShotCounters = new Dictionary<int, int>();
        private int swingDirection;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "Bismuth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "BronzeSword", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<BronzeSwordRework>();
            item.shootSpeed = 6f;
            item.UseSound = null;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = Vector2.Zero;
            swingDirection = swingDirection == 1 ? -1 : 1;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 handOffset = new Vector2(player.direction * 3, -1 * player.gravDir);
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

    public class BronzeBowGI : GlobalItem
    {
        private static Dictionary<int, int> playerShotCounters = new Dictionary<int, int>();

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem != null &&
                   entity.ModItem.Mod.Name == "Bismuth" &&
                   entity.ModItem.Name == "BronzeBow";
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int playerIndex = player.whoAmI;

            if (!playerShotCounters.ContainsKey(playerIndex))
            {
                playerShotCounters[playerIndex] = 0;
            }

            playerShotCounters[playerIndex]++;

            if (playerShotCounters[playerIndex] % 3 == 0)
            {
                float spread = MathHelper.ToRadians(8f);
                Vector2 velocity1 = velocity.RotatedBy(-spread);
                Vector2 velocity2 = velocity.RotatedBy(spread);

                Projectile.NewProjectile(source, position, velocity1, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI);

                return false;
            }

            return true;
        }
    }
}