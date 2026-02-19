using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Items.Weapons.Throwing;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Common.GlobalItems
{
    public class EverwoodJGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<EverwoodJavelin>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType <EverwoodJavelin2> ();
            velocity *= 1.35f;
            damage = (int)(damage * 1.2f);
            knockback *= 1.2f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}