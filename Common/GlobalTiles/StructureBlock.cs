// Code by 𝒜𝑒𝓇𝒾𝓈
using Bismuth.Content.Items.Other;
using Synergia.Common.Biome;
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Lists;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.GlobalTiles;

public class StructureBlock : GlobalTile {
    public override bool CanPlace(int i, int j, int type) => BaseLogic4(type, Main.LocalPlayer, base.CanPlace(i, j, type));
    public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) => BaseLogic3(i, j, type, Main.LocalPlayer.GetModPlayer<BiomePlayer>(), base.CanKillTile(i, j, type, ref blockDamaged));
    public override bool CanExplode(int i, int j, int type) => BaseLogic3(i, j, type, Main.LocalPlayer.GetModPlayer<BiomePlayer>(), base.CanExplode(i, j, type));
    public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced) => BaseLogic(i, j);
    static bool BaseLogic(int i, int j) {
        bool vectorActive = EnableVector(i, j);
        if (vectorActive && !Main.LocalPlayer.HasItem(ItemType<MasterToolBox>())) { return false; }
        else { return true; }
    }
    static bool BaseLogic2(int i, int j, int type, HashSet<int> multiTiles) {
        if (EnableVector(i, j)) {
            return Main.LocalPlayer.HasItem(ItemType<MasterToolBox>());
        }
        else {
            if (multiTiles.Contains(type)) {
                return Main.LocalPlayer.HasItem(ItemType<MasterToolBox>());
            }
            else { return true; }
        }
    }
    static bool BaseLogic3(int i, int j, int type, BiomePlayer bPlayer, bool returnFlag) {
        if (bPlayer.InSnowVillage) {
            return BaseLogic2(i, j, type, Tiles.SnowVillagesMultiTile);
        }
        else if (bPlayer.arenaBiome) {
            return BaseLogic2(i, j, type, Tiles.HellArenaMultiTile);
        }
        else if (bPlayer.villageBiome) {
            return BaseLogic2(i, j, type, Tiles.HellVillageMultiTile);
        }
        else if (bPlayer.lakeBiome) {
            return BaseLogic2(i, j, type, Tiles.HellLakeMultiTile);
        }
        else {
            return returnFlag;
        }
    }
    static bool BaseLogic4(int type, Player player, bool returnFlag) {
        if (player.InModBiome<SnowVillage>()) {
            return type == 4 || player.HasItem(ItemType<MasterToolBox>());
        }
        else if (player.InModBiome<NewHell>()) {
            return type == 4 || player.HasItem(ItemType<MasterToolBox>());
        }
        return returnFlag;
    }
    static bool EnableVector(int i, int j) {
        bool snowVilage = BaseWorldGens.VilageTiles.Contains(new Vector2(i, j));
        bool arena = BaseWorldGens.ArenaTiles.Contains(new Vector2(i, j));
        bool hellVilage = BaseWorldGens.HellVillageTilesVector.Contains(new Vector2(i, j));
        bool lake = BaseWorldGens.HellLakeTilesVector.Contains(new Vector2(i, j));
        if (snowVilage || arena || hellVilage || lake) { return true; }
        else { return false; }
    }
}