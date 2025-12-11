using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Tomes.PreHardmode;
using Consolaria.Content.Items.Materials;
using NewHorizons.Content.Items.Materials;
using Starforgedclassic.Content.Accessories.SkyShield;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Accessory;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Material.Bar;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons {
        public class Avalon_Accessories : BaseRecipe {
            public override void DisableRecipe(Recipe recipe) {
                DisableRecipe(recipe, AlchemicalSkull);
                DisableRecipe(recipe, CloakofAssists, ItemID.BeeCloak);
                DisableRecipe(recipe, CloakofAssists, ItemID.PanicNecklace);
                DisableRecipe(recipe, GiftofStarpower);
            }
            public override void Ingredient(Recipe recipe) {
                AddIngredient(recipe, ItemType<AdventuresandMishaps>(), 0, new Item(RoAItem("AnimalLeather")));
                AddIngredient(recipe, AFlowerlessPlant, 0, new Item(DamagedBook));
                AddIngredient(recipe, AFlowerlessPlant, 2, new Item(ItemType<WhiteThread>(), 10));
                AddIngredient2(recipe, ItemType<BacchusBoots>(), ItemType<NutfulNecklace>(), ItemID.FragmentStardust, ingredientCount2: 10);
                AddIngredient(recipe, ItemType<BurningDesire>(), 2, new Item(RoAItem("FlamingFabric"), 3));
                AddIngredient(recipe, CloakofAssists, 1, new Item(ItemType<BeeNecklace>()));
                AddIngredient(recipe, CreatorsTome, 3, new Item(ItemType<EvilIngot>()));
                AddIngredient(recipe, CreatorsTome, 4, new Item(SoulofBlight, 10));
                AddIngredient(recipe, ItemType<DionysusAmulet>(), 3, new Item(SoulofBlight, 5));
                AddIngredient(recipe, DullingTotem, indexItem: 0, new Item(NecroBuckler));
                AddIngredientNotIndex(recipe, DullingTotem, ItemType<DiamondAmulet>());
                AddIngredientNotIndex(recipe, ReflexShield, ItemType<SkyShield>());
                AddIngredientNotIndex(recipe, ReflexShield, ItemType<RiotShield>());
                AddIngredientNotIndex(recipe, ReflexShield, ItemID.LunarBar, 10);
                AddIngredient(recipe, ItemType<FlankersTome>(), 1, new Item(ItemType<HardenedGlass>(), 10));
                AddIngredient(recipe, GoblinArmyKnife, indexItem: 4, new Item(ItemType<MagnetHorseshoe>()));
                AddIngredient(recipe, GoblinArmyKnife, indexItem: 5, new Item(NecroBuckler));
                AddIngredient(recipe, ItemType<HadesCross>(), 2, new Item(Sinstone, 10));
                AddIngredient(recipe, LoveUpandDown, 3, new Item(RoAItem("NaturesHeart")));
                AddIngredient(recipe, LoveUpandDown, 4, new Item(SoulofBlight));
                AddIngredient(recipe, ItemType<MediationsFlame>(), 2, new Item(ItemType<Beetroot>()));
                AddIngredient(recipe, MistyPeachBlossoms, 0, new Item(DamagedBook));
                AddIngredient(recipe, MistyPeachBlossoms, 1, new Item(RoAItem("MiracleMint")));
                AddIngredient(recipe, ItemType<SoutheasternPeacock>(), 2, new Item(DamagedBook));
                AddIngredient(recipe, TaleoftheRedLotus, 2, new Item(RoAItem("MiracleMint")));
                AddIngredient(recipe, TaleoftheRedLotus, 3, new Item(DamagedBook));
                AddIngredient(recipe, ItemType<ReflexCharm>(), 1, new Item(ItemType<BigLens>()));
                AddIngredientNotIndex(recipe, ItemType<TaleoftheDolt>(), DamagedBook);
                ForeachIngredient(recipe, ItemType<TheVoidlands>(), new Item(RoAItem("SphereOfAspiration")));
                AddIngredient(recipe, ItemType<TomeofDistance>(), 2, new Item(DamagedBook));
                AddIngredient(recipe, ItemType<TomeoftheRiverSpirits>(), 3, new Item(DamagedBook));
                AddIngredient(recipe, TomorrowsPhoenix, 2, new Item(RoAItem("Galipot")));
                AddIngredient(recipe, TomorrowsPhoenix, 0, new Item(ItemType<Sap>(), 5));
                AddIngredient(recipe, ItemType<UnderestimatedResolve>(), 0, new Item(RoAItem("MiracleMint")));
            }
            public override void RemoveIngredient(Recipe recipe) {
                RemoveIngredient(recipe, CreatorsTome, 6);
                RemoveIngredient(recipe, CreatorsTome, 5);
                RemoveIngredient(recipe, GoblinArmyKnife, 6);
                RemoveIngredient(recipe, LoveUpandDown, 6);
                RemoveIngredient(recipe, LoveUpandDown, 5);
                RemoveIngredient(recipe, MistyPeachBlossoms, 3);
            }
            public override void Tiles(Recipe recipe) {
                Tiles(recipe, ItemType<BacchusBoots>(), LCS);
            }
            public override void Recipes() {
                Recipes4(AlchemicalSkull, RoAItem("AlchemicalSkull"), NecroBuckler, Sinstone, ItemType<MysticalClaw>(), TileID.Hellforge, count3: 10);
                Recipes4(GiftofStarpower, ItemType<NaturesEndowment>(), ItemID.SorcererEmblem, ItemType<CondensedKnowledge>(), ItemID.FragmentNebula, LCS, count4: 10);
                Recipes3(Windshield, ItemType<GlassShield>(), ItemType<BreezeShard>(), ItemID.CrystalShard, tileType: TileID.MythrilAnvil, count2: 5, count3: 10);
            }
        }
    }
}