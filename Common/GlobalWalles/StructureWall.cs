// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.ModSystems.WorldGens;
using Terraria;

namespace Synergia.Common.GlobalWalles {
    public class StructureWall : GlobalWall {
        public override void KillWall(int i, int j, int type, ref bool fail) {
            if (EnableVector(i, j)) {
                fail = true;
                Main.tile[i, j].WallType = (ushort)type;
            }
        }
        public override bool Drop(int i, int j, int type, ref int dropType) {
            if (EnableVector(i, j)) {
                return false;
            }
            return base.Drop(i, j, type, ref dropType);
        }
        public override bool CanExplode(int i, int j, int type) {
            if (EnableVector(i, j)) {
                return false;
            }
            else { return true; }
        }
        static bool EnableVector(int i, int j) {
            bool snowVilage = BaseWorldGens.VilageWalles.Contains(new Vector2(i, j));
            bool arena = BaseWorldGens.ArenaWalles.Contains(new Vector2(i, j));
            bool hellVilage = BaseWorldGens.HellVillageWallesVector.Contains(new Vector2(i, j));
            bool lake = BaseWorldGens.HellLakeWallesVector.Contains(new Vector2(i, j));
            if (snowVilage || arena || hellVilage || lake) { return true; }
            else { return false; }
        }
    }
}
