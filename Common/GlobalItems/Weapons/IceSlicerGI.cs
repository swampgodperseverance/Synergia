using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Reworks;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class IceSlicerGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null) return false;
            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", System.StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "BlueSlice", System.StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int shotCount = 3;
            const int delayBetweenShots = 8;
            const float spread = 0.15f;

            for (int i = 0; i < shotCount; i++)
            {
                Vector2 perturbedVelocity = velocity.RotatedBy(Main.rand.NextFloat(-spread, spread));

                int proj = Projectile.NewProjectile(
                    source,
                    position,
                    perturbedVelocity,
                    ModContent.ProjectileType<GiantSnow>(),
                    damage,
                    knockback,
                    player.whoAmI
                );

                Main.projectile[proj].ai[0] = i * delayBetweenShots;
            }

            return false;
        }
    }
}
