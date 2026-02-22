// Code by SerNik
using Synergia.Common.GlobalPlayer;
using Synergia.Content.Projectiles.Thrower;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Common.GlobalItems {
    public class SynergiaGI : GlobalItem {
        public bool isGrapplingHooks;

        public override bool InstancePerEntity => true;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (isGrapplingHooks) {
                tooltips.Add(new TooltipLine(Mod, "hook", "Hooks"));
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.GetModPlayer<BloodPlayer>().Tir7Buffs && Main.rand.NextBool(2)) {
                Projectile.NewProjectile(player.GetSource_ItemUse(item), position, velocity *= 2f, ProjectileType<BloodProjectile>(), damage + Main.rand.Next(25, 50), knockback);
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            else { return base.Shoot(item, player, source, position, velocity, type, damage, knockback); }
        }
    }
}