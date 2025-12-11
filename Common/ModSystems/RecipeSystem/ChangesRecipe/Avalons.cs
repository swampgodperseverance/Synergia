using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Armor.Hardmode;
using Avalon.Items.Material.Bars;
using Avalon.Items.Placeable.Crafting;
using Avalon.Items.Tomes.Hardmode;
using Avalon.Items.Tomes.PreHardmode;
using Consolaria.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria.ID;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using ValhallaMod.Items.Placeable.Blocks;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons {
        // Item public static int Item              { get; private set; } =                         ItemType<T>();
        public static int AlchemicalSkull           { get; private set; } =           ItemType<AlchemicalSkull>();
        public static int AFlowerlessPlant          { get; private set; } =          ItemType<AFlowerlessPlant>();
        public static int CloakofAssists            { get; private set; } =            ItemType<CloakofAssists>();
        public static int CreatorsTome              { get; private set; } =              ItemType<CreatorsTome>();
        public static int DamagedBook               { get; private set; } =               ItemType<DamagedBook>();
        public static int SoulofBlight              { get; private set; } =              ItemType<SoulofBlight>();
        public static int NecroBuckler              { get; private set; } =              ItemType<NecroBuckler>();
        public static int DullingTotem              { get; private set; } =              ItemType<DullingTotem>();
        public static int ReflexShield              { get; private set; } =              ItemType<ReflexShield>();
        public static int GiftofStarpower           { get; private set; } =           ItemType<GiftofStarpower>();
        public static int Windshield                { get; private set; } =                ItemType<Windshield>();
        public static int GoblinArmyKnife           { get; private set; } =           ItemType<GoblinArmyKnife>();
        public static int Sinstone                  { get; private set; } =                  ItemType<Sinstone>();
        public static int LoveUpandDown             { get; private set; } =             ItemType<LoveUpandDown>();
        public static int MistyPeachBlossoms        { get; private set; } =        ItemType<MistyPeachBlossoms>();
        public static int TaleoftheRedLotus         { get; private set; } =         ItemType<TaleoftheRedLotus>();
        public static int TomorrowsPhoenix          { get; private set; } =          ItemType<TomorrowsPhoenix>();
        public static int CaesiumBar                { get; private set; } =                ItemType<CaesiumBar>();
        public static int CaesiumPlateMail          { get; private set; } =          ItemType<CaesiumPlateMail>();
        public static int CaesiumGreaves            { get; private set; } =            ItemType<CaesiumGreaves>();
        public static int CaesiumGalea              { get; private set; } =              ItemType<CaesiumGalea>();
        public static int CorrodeBar                { get; private set; } =                ItemType<CorrodeBar>();
        public static int EarthsplitterChestpiece   { get; private set; } =   ItemType<EarthsplitterChestpiece>();
        public static int EarthsplitterHelm         { get; private set; } =         ItemType<EarthsplitterHelm>();
        public static int EarthsplitterLeggings     { get; private set; } =     ItemType<EarthsplitterLeggings>();
        public static int NaquadahAnvil             { get; private set; } =             ItemType<NaquadahAnvil>();
        public static int TroxiniumForge            { get; private set; } =            ItemType<TroxiniumForge>();
        public static int CaesiumForge              { get; private set; } =              ItemType<CaesiumForge>();
        // Tile public static int Tile              { get; private set; } =                         TileType<T>();
        public static int LCS                       { get; private set; } =           TileID.LunarCraftingStation;
        public static int CaesiumForgeTile          { get; private set; } = TileType<Avalon.Tiles.CaesiumForge>();
	    public static int CaesiumHeavyAnvilTile     { get; private set; } =     TileType<CaesiumHeavyAnvilTile>();
    }
}