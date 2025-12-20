using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Common.GlobalItems
{
    public class WoodenBoomerangGlobalItem : ThrowingGI {
        public override int ItemType => ItemID.WoodenBoomerang;
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            velocity *= 1.3f;
            damage = (int)(damage * 1.1f);
            knockback *= 1.1f;

            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-10f)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10f)), type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}