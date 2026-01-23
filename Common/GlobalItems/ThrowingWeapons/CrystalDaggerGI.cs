using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Synergia.Content.Projectiles.Thrower;
using Terraria.DataStructures;
using Terraria.ID;
using NewHorizons.Content.Items.Weapons.Throwing;

namespace Synergia.Common.GlobalItems
{
    public class DaggerGIGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<CrystalDagger>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int miniType = ModContent.ProjectileType<CrystalDagger2>(); 

            velocity *= 1.5f;
            damage = (int)(damage * 0.7f);

            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(5f)),  miniType, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity,                           miniType, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-5f)), miniType, damage, knockback, player.whoAmI);

            return false;
        }
    }
}