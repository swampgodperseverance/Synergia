using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks;

namespace Synergia.Content.Global
{
    public class BatbowProjectileGlobal : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "IluminantBatbow", StringComparison.OrdinalIgnoreCase);
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
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                // Первый выстрел сразу
                Projectile.NewProjectile(
                    source,
                    position,
                    velocity,
                    ModContent.ProjectileType<BatbowProjectile>(),
                    damage,
                    knockback,
                    player.whoAmI
                );

                for (int i = 1; i <= 2; i++)
                {
                    float delay = i * 7; 
                    float spread = MathHelper.ToRadians(5f * i); 
                    Vector2 perturbedSpeed = velocity.RotatedBy(spread * (i - 1.5f));

                    BatbowShotScheduler.AddDelayedShot(
                        player,
                        delay,
                        source,
                        position,
                        perturbedSpeed,
                        ModContent.ProjectileType<BatbowProjectile>(),
                        damage,
                        knockback
                    );
                }

                return false;
            }

            return true;
        }
    }
}
