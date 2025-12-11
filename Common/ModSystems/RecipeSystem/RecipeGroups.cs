using Avalon.Items.Material.Herbs;
using Avalon.Items.Material.Ores;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges.Avalons;
using static Terraria.ID.ItemID;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem {
    public class RecipeGroups : ModSystem {
        public const string FRAGMENT = "FragmentSolarOrVortex";
        public const string FRAGMENT2 = "FragmentNebulaOrStardust";
        public const string HARDMODEANVIL = "HardModeAnvil";
        public const string HARDMODEFORGE = "HardModeforge";
        public const string FLOWER = "AlchemicFlower";
        public const string IRON = "AlchemicIron";


        public override void AddRecipeGroups() {
            RecipeGroup group = new(() => Language.GetTextValue("LegacyMisc.37") + " " + "Solar or Vortex fragment", [ItemType<NULLItem.FragmentSolar>(), FragmentSolar, FragmentVortex]);
            RecipeGroup.RegisterGroup(FRAGMENT, group);

            AddRecipeGroups(group, "Nebula or Stardust fragment", FRAGMENT2, ItemType<NULLItem.FragmentStardust>(), FragmentNebula, FragmentStardust);
            AddRecipeGroups(group, "Hard Mode anvil", HARDMODEANVIL, ItemType<NULLItem.HardModeAnvil>(), NaquadahAnvil, OrichalcumAnvil, MythrilAnvil);
            AddRecipeGroups(group, "Hard Mode forge", HARDMODEFORGE, ItemType<NULLItem.HardModeForge>(), TitaniumForge, AdamantiteForge, TroxiniumForge);
            AddRecipeGroups(group, "Alchemic Flower", FLOWER, ItemType<NULLItem.AlchemicFlower>(), Deathweed, ItemType<Bloodberry>(), ItemType<Barfbush>());
            AddRecipeGroups(group, "Alchemic Iron", IRON, ItemType<NULLItem.AlchemicOre>(), IronOre, LeadOre, ItemType<NickelOre>());
        }
        static void AddRecipeGroups(RecipeGroup group, string groupName, string groupKey, params int[] groupItem) {
            group = new(() => Language.GetTextValue("LegacyMisc.37") + " " + groupName, groupItem);
            RecipeGroup.RegisterGroup(groupKey, group);
        }
    }
}