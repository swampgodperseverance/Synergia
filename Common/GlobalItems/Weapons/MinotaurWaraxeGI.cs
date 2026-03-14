using System;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class MinotaauraGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "Bismuth", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "MinotaursWaraxe", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;
            item.autoReuse = true;
            item.ArmorPenetration = 5;
            item.noMelee = true;
            item.UseSound = null;
            item.noUseGraphic = true;
            item.scale = 1.1f;
            item.shoot = ModContent.ProjectileType<MinotaursWaraxeRework>();
            item.shootSpeed = 1f;
            item.channel = true;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer != player.whoAmI) return false;

            Vector2 shootDirection = Vector2.Normalize(Main.MouseWorld - player.Center);
            if (shootDirection == Vector2.Zero) shootDirection = Vector2.UnitX * player.direction;

            float initialTime = 900f;

            int proj = Projectile.NewProjectile(
                source,
                player.Center,
                shootDirection,
                type,
                damage,
                knockback,
                player.whoAmI,
                0f,
                initialTime
            );

            Main.projectile[proj].direction = player.direction;

            return false;
        }
    }
}