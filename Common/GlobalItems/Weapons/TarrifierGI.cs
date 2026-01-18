using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class TarrifierGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", System.StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "Tarrifier", System.StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, 
                                   Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            int[] possibleProjectiles = new int[]
            {
                ModContent.ProjectileType<TarSpike>(),
                ModContent.ProjectileType<TarSpike1>(),
                ModContent.ProjectileType<TarSpike2>()
            };

            int chosenType = Main.rand.Next(possibleProjectiles.Length);

            Projectile.NewProjectile(source, position, velocity, possibleProjectiles[chosenType], damage, knockback, player.whoAmI);

            return false;
        }
    }
}
