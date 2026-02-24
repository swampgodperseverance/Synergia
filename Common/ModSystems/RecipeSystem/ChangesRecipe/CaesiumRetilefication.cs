using Bismuth.Content.Items.Armor;
using Bismuth.Content.Items.Placeable;
using Bismuth.Content.Items.Tools;
using Bismuth.Content.Items.Weapons.Ranged;
using Bismuth.Content.Items.Weapons.Magical;
using Bismuth.Content.Items.Weapons.Melee;
using Synergia.Content.Items.Materials;
using Synergia.Content.Items.Misc;
using Terraria;
using Consolaria.Content.Items.Armor.Melee;
using Consolaria.Content.Items.Armor.Ranged;
using Consolaria.Content.Items.Armor.Magic;
using Consolaria.Content.Items.Armor.Summon;
using Consolaria.Content.Items.Weapons.Ammo;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Accessories;
using Consolaria.Content.Items.Summons;
using Avalon.Items.Weapons.Ranged.Hardmode.CaesiumCrossbow;
using Avalon.Items.Weapons.Melee.Hardmode.CaesiumScimitar;
using Avalon.Items.Weapons.Melee.Hardmode.CaesiumMace;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe {
    public class CaesiumHeavyAnvilTileChanger : BaseRecipe {
        public override void Ingredient(Recipe recipe)
        {
           
        }

        public override void Tiles(Recipe recipe)
        {
            RemoveTiles(recipe,
                134,
                TileType<CaesiumHeavyAnvilTile>(),

                ItemType<BismuthumBar>(),
                ItemType<BismuthumBreastplate>(),
                ItemType<BismuthumHat>(),
                ItemType<BismuthumHeadgear>(),
                ItemType<BismuthumHelmet>(),
                ItemType<BismuthumLeggings>(),
                ItemType<BismuthumCrossbow>(),
                ItemType<BismuthumDrill>(),
                ItemType<BismuthumPickaxe>(),
                ItemType<BismuthumSaw>(),
                ItemType<BismuthumGlove>(),
                ItemType<BismuthumSword >(),
                ItemType<DragonBreastplate>(),
                ItemType<DragonGreaves>(),
                ItemType<DragonMask>(),
                ItemType<TitanHelmet>(),
                ItemType<TitanMail>(),
                ItemType<TitanLeggings>(),
                ItemType<PhantasmalHeadgear>(),
                ItemType<PhantasmalRobe>(),
                ItemType<PhantasmalSubligar>(),
                ItemType<WarlockHood>(),
                ItemType<WarlockLeggings>(),
                ItemType<WarlockRobe>(),
                ItemType<VolcanicRepeater>(),
                ItemType<Tonbogiri>(),
                ItemType<SparklyWings>(),
                ItemType<SuspiciousLookingSkull>(),
                ItemType<CaesiumCrossbow>(),
                ItemType<CaesiumScimitar>(),
                ItemType<CaesiumMace>()

            );
        }
    }
}
