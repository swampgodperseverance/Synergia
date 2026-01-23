using Microsoft.Xna.Framework;
using Synergia.Common.GlobalItems.ThrowingWeapons;
using Terraria;
using Terraria.DataStructures;
using Consolaria.Content.Items.Weapons.Melee;
using Terraria.ID;

namespace Synergia.Common.GlobalItems
{
    public class AlbinoMandibleGlobalItem : ThrowingGI {
        public override int ItemType => ModContent.ItemType<AlbinoMandible>(); 
        public override bool NewBehavior(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            velocity *= 1f;
            damage = (int)(damage * 1.1f);
            knockback *= 1.1f;

            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10f)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-10f)), type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}