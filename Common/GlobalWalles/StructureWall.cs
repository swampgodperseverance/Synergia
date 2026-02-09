// Code by 𝒜𝑒𝓇𝒾𝓈
using Bismuth.Content.Items.Other;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;

namespace Synergia.Common.GlobalWalles {
    public class StructureWall : GlobalWall {
        public override void KillWall(int i, int j, int type, ref bool fail) {
            if (BaseLogic(i, j)) {
                fail = true;
                Main.tile[i, j].WallType = (ushort)type;
            }
        }
        public override bool Drop(int i, int j, int type, ref int dropType) => BaseLogic(i, j);
        public override bool CanExplode(int i, int j, int type) => BaseLogic(i, j);
        static bool HellStructBlock(int i, int j) => WorldHelper.CheckBiomeTile(i, j, 237 + SynergiaGenVars.HellArenaPositionX - SynergiaGenVars.HellLakeX, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 119);
        static bool SnowVillages(int i, int j) => SynergiaGenVars.VillageTiles.Contains(new Vector2(i, j));
        static bool BaseLogic(int i, int j) {
            if (HellStructBlock(i, j) || SnowVillages(i, j)) {
                if (Main.LocalPlayer.HasItem(ItemType<MasterToolBox>())) { return true; }
                return false;
            }
            return true;
        }
    }
}