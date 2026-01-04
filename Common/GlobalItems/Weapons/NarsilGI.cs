using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.AltUse;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class NarsilGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int swingDirection;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "Bismuth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "Narsil", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            item.damage = 50;
            item.DamageType = DamageClass.Melee;
            item.knockBack = 12f;
            item.useTime = 42;
            item.useAnimation = 42;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTurn = false;
            item.autoReuse = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(0, 5);
            item.ArmorPenetration = 5;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<NarsilRework>();
            item.shootSpeed = 6f;
            item.width = 56;
            item.height = 62;
            item.UseSound = null;
        }

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