using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Thrower;
using Terraria.ID;
using Bismuth.Content.Items.Weapons.Throwing;

namespace Synergia.Common.GlobalItems
{
    public class aEglosGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<Aeglos>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<Aeglos2>();
            velocity *= 1.7f;
            damage = (int)(damage * 1.1f);
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);

            return false;
        }
    }
}