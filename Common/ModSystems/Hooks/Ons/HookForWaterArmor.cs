using Bismuth.Content.Items.Armor;
using Synergia.Content.Projectiles.Armor;
using System;
using Terraria;
using Terraria.Audio;
using Synergia.Reassures;

namespace Synergia.Common.ModSystems.Hooks.Ons
{
    public class WaterArmorHook : HookForArmorSetBonus
    {
        int cooldownTimer = 600;
        public override Type Armor => typeof(WatersHelmet);
        public override int ArmorType => ItemType<WatersHelmet>();
        public override void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player)
        {
            orig(item, player);
            string keyname = VanillaKeybinds.ArmorSetBonusActivation.GetAssignedKeys().Count > 0 ? VanillaKeybinds.ArmorSetBonusActivation.GetAssignedKeys()[0] : "UNBOUND";
            player.setBonus += "\n" + string.Format(ItemTooltip(ARM, "ActiveBonus"), keyname) + "\n" + ItemTooltip(ARM, "WaterBonus");

            cooldownTimer++;
            if (cooldownTimer > 600){
                cooldownTimer = 600;
            }
            if (cooldownTimer == 600){
                SoundEngine.PlaySound(RSounds.ArmorReady, player.position);
            }

            if (VanillaKeybinds.ArmorSetBonusActivation.JustPressed && cooldownTimer >= 600){
                SoundEngine.PlaySound(RSounds.Watersound, player.position);

                Projectile.NewProjectile(player.GetSource_Misc("ArmorSetBonus"), player.Center, Vector2.Zero, ProjectileType<AquaRework>(), 0, 0f, player.whoAmI);
                cooldownTimer = 0;
            }
        }
    }
}