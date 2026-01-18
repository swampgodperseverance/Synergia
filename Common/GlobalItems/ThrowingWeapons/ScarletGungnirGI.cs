using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using NewHorizons.Content.Items.Weapons.Throwing;
using NewHorizons.Content.Projectiles.Throwing;

namespace Synergia.Common.GlobalItems
{
    public class sgGlobalItem : ThrowingGI
    {
        public override int ItemType => ModContent.ItemType<ScarletGungnir>();

        public override bool NewBehavior(
            Item item,
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            int blackType = ModContent.ProjectileType<BlackScarletGungnir>();
            int centerType = ModContent.ProjectileType<ScarletGungnir2>();

            velocity *= 1.5f;
            damage = (int)(damage * 0.8f);

            Projectile.NewProjectile(
                source,
                position,
                velocity.RotatedBy(MathHelper.ToRadians(15f)),
                blackType,
                damage,
                knockback,
                player.whoAmI
            );

            Projectile.NewProjectile(
                source,
                position,
                velocity,
                centerType,
                damage,
                knockback,
                player.whoAmI
            );

            Projectile.NewProjectile(
                source,
                position,
                velocity.RotatedBy(MathHelper.ToRadians(-15f)),
                blackType,
                damage,
                knockback,
                player.whoAmI
            );

            return false;
        }
    }
}
