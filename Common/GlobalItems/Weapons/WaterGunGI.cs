using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.Reworks2; 

namespace Synergia.Content.GlobalItems.Weapons
{
    public class WaterGunGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            return string.Equals(entity.ModItem.Mod?.Name, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(entity.ModItem.Name, "WaterFighter", StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            type = ModContent.ProjectileType<WaterGunBubble>();

            float spread = 0.22f; 
            velocity = velocity.RotatedBy(Main.rand.NextFloat(-spread, spread));


            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}
