// Code by 𝒜𝑒𝓇𝒾𝓈
using Bismuth.Content.Items.Other;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.GlobalTiles;

public class StructureBlock : GlobalTile {
    public override bool CanPlace(int i, int j, int type) {
        if (HellStructBlock(i, j)) { return Main.LocalPlayer.HasItem(ItemType<MasterToolBox>()) || type == 4; }
        else { return BaseLogic(i, j); }
    }
    public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) => BaseLogic(i, j);
    public override bool CanExplode(int i, int j, int type) => BaseLogic(i, j);
    public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced) => BaseLogic(i, j);
    static bool HellStructBlock(int i, int j) => WorldHelper.CheckBiomeTile(i, j, 237 + SynergiaGenVars.HellArenaPositionX - SynergiaGenVars.HellLakeX, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 119);
    static bool SnowVillages(int i, int j) => SynergiaGenVars.VillageTiles.Contains(new Vector2(i, j));
    static bool BaseLogic(int i, int j) {
        if (HellStructBlock(i, j) || SnowVillages(i, j)) {
            if (Main.LocalPlayer.HasItem(ItemType<MasterToolBox>())) { return true; }
            else { return false; }
        }
        else { return true; }
    }
}