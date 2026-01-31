using Bismuth.Content.Items.Weapons.Throwing;
using Microsoft.Xna.Framework;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Common.GlobalItems
{
    public class MythJavGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<NewHorizons.Content.Items.Weapons.Throwing.MythrilJavelin>();
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int newType = ModContent.ProjectileType<MythrilJavelin2>();
            velocity *= 1.2f;
            damage = (int)(damage * 1.2f);
            knockback *= 1.2f;
            Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
            return false;
        }
    }
}