using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Common.GlobalItems
{
    public class TrimarangGlobalItem : ThrowingGI {
        public override int ItemType => ItemID.Trimarang;
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            velocity *= 1.5f;
            damage = (int)(damage * 1.2f);
            knockback *= 1.2f;

            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10f)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-10f)), type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}