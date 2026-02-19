using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Thrower;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Thrown;

namespace Synergia.Common.GlobalItems
{
    public class cACTUSsTARgi : ThrowingGI {
        public override int ItemType => ModContent.ItemType<CactusStar>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<CactusStar2>();
            velocity *= 1.16f;
            damage = (int)(damage * 1.1f);
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);

            return false;
        }
    }
}