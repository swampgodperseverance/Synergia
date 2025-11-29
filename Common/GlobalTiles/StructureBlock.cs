using Bismuth.Content.Items.Other;
using Microsoft.Xna.Framework;
using Synergia.Common.WorldGenSystem;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalTiles;

public class StructureBlock : GlobalTile {
    public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) => BaseLogic(i, j);
    public override bool CanExplode(int i, int j, int type) => BaseLogic(i, j);
    public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced) => BaseLogic(i, j);
    static bool BaseLogic(int i, int j) {
        bool vectorActive = EnableVector(i, j);
        if (vectorActive && !Main.LocalPlayer.HasItem(ModContent.ItemType<MasterToolBox>())) {
            return false;
        }
        else {
            return true;
        }
    }
    static bool EnableVector(int i, int j) {
        bool snowVilage = GenerationSnowVillage.VilageTiles.Contains(new Vector2(i, j));
        bool arena = GenerationArenasInHell.ArenaTiles.Contains(new Vector2(i, j));
        bool hellVilage = GenerationVillageInHell.hellVillageTilesVector.Contains(new Vector2(i, j));
        if (snowVilage || arena || hellVilage) {
            return true;
        }
        else {
            return false;
        }
    }
}