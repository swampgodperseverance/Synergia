using System;
using NewHorizons.Content.Items.Armor.AshenArmor;
using Synergia.Content.Projectiles.Armor;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using ValhallaMod.Items.Armor;
using Synergia.Reassures;

namespace Synergia.Common.ModSystems.Hooks.Ons
{
    public class AshenArmorHook : HookForArmorSetBonus
    {
        int cooldownTimer = 1800;
        bool isInvisible = false;
        int invisibilityTimer = 0;

        public override Type Armor => typeof(AshenShroud);
        public override int ArmorType => ItemType<AshenShroud>();

        public override void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player)
        {
            orig(item, player);
            string keyname = VanillaKeybinds.ArmorSetBonusActivation.GetAssignedKeys().Count > 0 ? VanillaKeybinds.ArmorSetBonusActivation.GetAssignedKeys()[0] : "UNBOUND";
            player.setBonus += "\n" + string.Format(ItemTooltip(ARM, "ActiveBonus"), keyname) + "\n" + ItemTooltip(ARM, "AshenBonus");
            cooldownTimer++;
            if (cooldownTimer > 1800)
            {
                cooldownTimer = 1800;
            }
            if (cooldownTimer == 1800)
            {
                SoundEngine.PlaySound(RSounds.ArmorReady, player.position);
            }

            if (invisibilityTimer > 0)
            {
                invisibilityTimer--;
                player.stealth = 1f; 
                player.aggro -= 1000; 
                player.AddBuff(BuffID.Invisibility, 2);
            }
            else
            {
                if (isInvisible)
                {
                    isInvisible = false;
                    player.stealth = 0f;
                }
            }

            if (VanillaKeybinds.ArmorSetBonusActivation.JustPressed && cooldownTimer >= 1800)
            {
                isInvisible = true;
                invisibilityTimer = 600; 
                player.stealth = 1f;
                player.AddBuff(BuffID.Invisibility, 600);
                Projectile.NewProjectile(player.GetSource_Misc("ArmorSetBonus"), player.Center, Vector2.Zero, ProjectileType<AshenCopy>(), 20, 0f, player.whoAmI);
                cooldownTimer = 0;
            }
        }
    }
}