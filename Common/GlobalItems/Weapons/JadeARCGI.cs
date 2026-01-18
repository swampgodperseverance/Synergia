using System;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Content.Global
{
    public class JadEARcanumGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "JadeArcanum", StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Vector2 spawnPosition = Main.MouseWorld;


            Projectile.NewProjectile(
                source,
                spawnPosition,
                velocity, 
                ModContent.ProjectileType<JadeGreatJavelin>(),
                damage,
                knockback,
                player.whoAmI
            );

            return false; 
        }
    }
}
