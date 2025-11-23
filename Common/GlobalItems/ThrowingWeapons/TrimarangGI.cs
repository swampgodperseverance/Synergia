using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.Players;

namespace Synergia.Common.GlobalItems
{
    public class TrimarangGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Trimarang;
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<TrimarangPlayer>();
            if (modPlayer.doubleMode)
            {
                velocity *= 1.5f; 
                damage = (int)(damage * 1.2f); 
                knockback *= 1.2f; 

                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10f)), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-10f)), type, damage, knockback, player.whoAmI);
                return false;
            }
            return true;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<TrimarangPlayer>().AddCombo();
        }

        public override void HoldItem(Item item, Player player)
        {
            var modPlayer = player.GetModPlayer<TrimarangPlayer>();

            if (modPlayer.interfaceProj != null && modPlayer.interfaceProj.active &&
                modPlayer.interfaceProj.ModProjectile is Content.Projectiles.Thrower.ThrowerInterface1 ui)
            {
                ui.SetFrame(modPlayer.comboCount);
            }
        }
    }
}