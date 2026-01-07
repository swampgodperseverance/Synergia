using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Reworks;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class FeroziumBladeGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int swingDirection;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "Avalon", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "FeroziumIceSword", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;

            item.DamageType = DamageClass.Melee;
            item.knockBack = 5f;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTurn = false;
            item.autoReuse = true;
            item.ArmorPenetration = 5;
            item.noMelee = true;
            item.UseSound = null;
            item.damage = 108;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<FeroziumBladeRework>();
            item.shootSpeed = 0f;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!AppliesToEntity(item, false)) return;

            velocity = Vector2.Zero;
            swingDirection = swingDirection == 1 ? -1 : 1;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!AppliesToEntity(item, false)) return base.Shoot(item, player, source, position, velocity, type, damage, knockback);

            Vector2 handPos = player.MountedCenter;
            Vector2 offset = new Vector2(4f * player.direction, -6f * player.gravDir);

            Projectile.NewProjectile(
                source,
                handPos + offset,
                Vector2.Zero,
                ModContent.ProjectileType<FeroziumBladeRework>(),
                damage,
                knockback,
                player.whoAmI,
                ai0: swingDirection,
                ai1: player.MountedCenter.AngleTo(Main.MouseWorld),
                ai2: 0
            );

            return false;
        }
    }
}