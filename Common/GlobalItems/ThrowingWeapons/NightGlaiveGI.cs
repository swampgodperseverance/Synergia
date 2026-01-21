using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Thrower;
using NewHorizons.Content.Items.Weapons.Throwing;
using Terraria.ID;

namespace Synergia.Common.GlobalItems
{
    public class NightGlaiveGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<NightGlaive>(); 
                public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<NightGlaive2>();
            velocity *= 1f;
            damage = (int)(damage * 1.1f);
            knockback *= 1f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}