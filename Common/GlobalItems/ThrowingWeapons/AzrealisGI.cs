using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Thrower;
using Terraria.ID;
using Bismuth.Content.Items.Weapons.Throwing;
using ValhallaMod.Items.Weapons.Blood;

namespace Synergia.Common.GlobalItems
{
    public class aZREALISGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<AzraelsHeartstopper>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<AzraelsHeartstopper2>();
            velocity *= 1.3f;
            damage = (int)(damage * 1.1f);
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);

            return false;
        }
    }
}