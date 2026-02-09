using Bismuth.Content.Items.Weapons.Throwing;
using Consolaria.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using NewHorizons.Content.Items.Weapons.Throwing;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Synergia.Common.GlobalItems
{
    public class PulsarlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<Pulsar>();
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int pulsar2 = ModContent.ProjectileType<Pulsar2>();
            velocity *= 1f;
            damage = (int)(damage * 1.1f);
            knockback *= 1.1f;
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(50f)), pulsar2, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(20f)), pulsar2, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-20f)), pulsar2, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-50f)), pulsar2, damage, knockback, player.whoAmI);
            return false;
        }
    }
}