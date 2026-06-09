using Synergia.Content.Items.Armor.Magic.FadingHell;
using Synergia.Content.Items.Armor.Melee.Coreburned;
using Synergia.Content.Items.Armor.Summon;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.QuestItem;
using Synergia.Content.Items.Tools;
using Synergia.Content.Items.Weapons.Mage;
using Synergia.Content.Items.Weapons.Melee;
using Synergia.Content.Items.Weapons.Ranged;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria.Localization;

namespace Synergia.Common.ModSystems.ModSupports {
    public abstract class MoreTooltipsSupport : ModSystem {
        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("MoreObtainingTooltips", out Mod moreObtainingTooltips))
            {
                moreObtainingTooltips.Call(
                    "AddCustomizedSource",
                    Language.GetTextValue($"Mods.Synergia.Tooltips.ObtainingTooltip"),
                    new int[] {
                ModContent.ItemType<Rhabdomyolysis>(),
                ModContent.ItemType<FadingHellChestplate>(),
                ModContent.ItemType<FadingHellHat>(),
                ModContent.ItemType<FadingHellPants>(),
                ModContent.ItemType<Ghalihieri>(),
                ModContent.ItemType<CoreburnedAxe>(),
                ModContent.ItemType<PhoenixDownfall>(),
                ModContent.ItemType<Enfer>(),
                ModContent.ItemType<Gutshelmet>(),
                ModContent.ItemType<Gutslegs>(),
                ModContent.ItemType<Gutsplate>(),
                ModContent.ItemType<CoreburnedBreastplate>(),
                ModContent.ItemType<CoreburnedHelmet>(),
                ModContent.ItemType<CoreburnedLeggings>(),
                ModContent.ItemType<HellLuceat>(),
                ModContent.ItemType<Flarion>(),
                ModContent.ItemType<FeneathsBrush>(),
                ModContent.ItemType<Malebolge>(),
                ModContent.ItemType<Echelonis>(),
                ModContent.ItemType<ScorcherRequiem>(),
                ModContent.ItemType<CoreburnedPickaxe>()
                    }
                );
            }
        }
    }
}