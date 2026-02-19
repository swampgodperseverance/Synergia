using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Boomerang;
using ValhallaMod.Visual;

namespace Synergia.Common.GlobalItems
{
    public class OmegaDiscGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<ValhallaMod.Items.Weapons.Boomerang.OmegaDisc>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<OmegaDiscRework>();
            velocity *= 1.2f;
            damage = (int)(damage * 1f);
            knockback *= 1.2f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}