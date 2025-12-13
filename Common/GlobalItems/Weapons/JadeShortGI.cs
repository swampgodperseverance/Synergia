using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Synergia.Content.Global
{
    public class JadEShortGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "JadeShortsword", StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
            Vector2 shootVel = velocity.SafeNormalize(Vector2.UnitX) * 12f;

            Projectile.NewProjectile(
                source,
                position,
                shootVel,
                ModContent.ProjectileType<JadeShard>(),
                (int)(damage * 0.8f), 
                knockback,
                player.whoAmI
            );


            return true;
        }
    }
}
