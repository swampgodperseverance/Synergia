using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
   public class OrcishSwordRework : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem != null
                && entity.ModItem.Mod.Name == "Bismuth"
                && entity.ModItem.Name == "OrcishSword";
        }

        public override bool CanShoot(Item item, Player player)
        {
            return true; 
        }

        public override bool Shoot(Item item, Player player,
            Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projType = ModContent.ProjectileType<Content.Projectiles.Reworks.OrcishRework>();

            Projectile.NewProjectile(
                source,
                position,
                velocity * 1.2f,
                projType,
                (int)(damage * 0.9f),
                knockback,
                player.whoAmI
            );

            return false;   
        }
    }
}
