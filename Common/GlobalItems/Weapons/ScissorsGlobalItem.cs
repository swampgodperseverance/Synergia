using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class ScissorsGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public int rightClickCooldown = 0;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", System.StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "ScissorsKnives", System.StringComparison.OrdinalIgnoreCase);
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (rightClickCooldown > 0)
                rightClickCooldown--;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (player.altFunctionUse == 2) 
            {
             
                item.shoot = 0;
                item.noMelee = true;
                item.noUseGraphic = true;
            }
            else 
            {
                item.shoot = ModContent.ProjectileType<ScissorsRework>();
                item.shootSpeed = 0f;
                item.noMelee = true;
                item.noUseGraphic = true;
            }

            return base.CanUseItem(item, player);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            return true; 
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (player.altFunctionUse == 2 && rightClickCooldown <= 0 && Main.myPlayer == player.whoAmI)
            {
        
                Projectile.NewProjectile(
                    item.GetSource_FromThis(),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<ScissorsRework2>(),
                    item.damage,
                    item.knockBack,
                    player.whoAmI
                );

                rightClickCooldown = 60 * 15; 

                return true;
            }

            return base.UseItem(item, player);
        }
    }
}
