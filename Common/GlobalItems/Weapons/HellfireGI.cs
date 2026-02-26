using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks;

namespace Synergia.Content.Global
{
    public class HellfireGlobal : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "Avalon", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "Hellrazer", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item entity)
        {
            if (AppliesToEntity(entity, false))
            {
                entity.useTime /= 1;
                entity.useAnimation /= 1;

                if (entity.useTime < 1)
                    entity.useTime = 1;

                if (entity.useAnimation < 1)
                    entity.useAnimation = 1;
            }
        }

        public override bool Shoot(
            Item item,
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            if (item.useAmmo == AmmoID.Bullet)
            {
                Vector2 playerPos = player.Center;

                Projectile.NewProjectile(
                    source,
                    playerPos,
                    velocity,
                    ModContent.ProjectileType<Hellbullet>(),
                    damage,
                    knockback,
                    player.whoAmI
                );

                return false;
            }

            return true;
        }
    }
}
