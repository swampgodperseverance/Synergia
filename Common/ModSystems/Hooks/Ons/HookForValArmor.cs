using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Armor;
using System;
using Terraria;
using ValhallaMod.Items.Armor;

namespace Synergia.Common.ModSystems.Hooks.Ons
{
    public class ArmorSetKeybindHook : HookForArmorSetBonus
    {
        int cooldownTimer = 1800;

        public override Type Armor => typeof(ValhalliteHead);
        public override int ArmorType => ItemType<ValhalliteHead>();
        public override void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player) {
            orig(item, player);
            cooldownTimer++;
            if (cooldownTimer > 1800) {
                cooldownTimer = 1800;
            }
            if (VanillaKeybinds.ArmorSetBonusActivation.JustPressed && cooldownTimer >= 1800) {
                Projectile.NewProjectile(player.GetSource_Misc("ArmorSetBonus"), player.Center, Vector2.Zero, ProjectileType<ValhalliteKnight>(), 20, 0f, player.whoAmI);
                cooldownTimer = 0;
            }
        }
    }
}
