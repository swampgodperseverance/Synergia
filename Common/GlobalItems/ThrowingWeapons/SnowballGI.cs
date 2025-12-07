using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class SnowballGlobalItem : ThrowingGI {
        public override int ItemType => ItemID.Snowball;
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<SnowballMega>();
            velocity *= 1.2f;
            damage = (int)(damage * 0.8f);
            knockback *= 1.2f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}