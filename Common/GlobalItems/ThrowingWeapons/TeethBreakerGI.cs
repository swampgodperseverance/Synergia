using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Boomerang;

namespace Synergia.Common.GlobalItems
{
    public class TeethBreakerGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<TeethBreaker>();
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<TeethBreakerMega>();
            velocity *= 1.5f;
            damage = (int)(damage * 2.3f);
            knockback *= 1.2f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}