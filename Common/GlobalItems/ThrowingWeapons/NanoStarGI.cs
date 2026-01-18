using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using NewHorizons.Content.Items.Weapons.Throwing;

namespace Synergia.Common.GlobalItems
{
    public class nANOGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<NanoStar>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<NanoStar1>();
            velocity *= 1.2f;
            damage = (int)(damage * 1f);
            knockback *= 1.1f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}