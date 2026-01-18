using Synergia.Content.Projectiles.Reworks.Reworks2;
using System;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Content.Global
{
    public class SunstormGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "Avalon", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "Sunstorm", StringComparison.OrdinalIgnoreCase);
        }


        

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                ModContent.ProjectileType<SunRework>(), 
                damage,
                knockback,
                player.whoAmI
            );

            return false;
        }

    }
}
