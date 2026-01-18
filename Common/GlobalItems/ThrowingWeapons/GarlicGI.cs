using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Thrower;
using Terraria.ID;
using ValhallaMod.Items.Garden;

namespace Synergia.Common.GlobalItems
{
    public class GarlicGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<Garlic>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<Garlic2>();
            velocity *= 1.3f;
            damage = (int)(damage * 1.1f);
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);

            return false;
        }
    }
}