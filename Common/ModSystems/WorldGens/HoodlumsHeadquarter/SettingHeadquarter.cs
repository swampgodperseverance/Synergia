// Code by SerNik
using Bismuth.Utilities;
using Terraria.WorldBuilding;

namespace Synergia.Common.ModSystems.WorldGens.HoodlumsHeadquarter {
    public class SettingHeadquarter : BaseWorldGens {
        bool swampGen;

        public override string NameGen => "[Synergia] Setting struct in swamp";
        public override int Index => 1;
        public override bool GensBool { get => swampGen; set => swampGen = value; }
        public override bool Do_MakeGen(GenerationProgress progress) => HeadquarterLayerOne.GenHeadquarter(progress); // BismuthWorld.WorldSize == 1 ? HeadquarterLayerOne.GenHeadquarter(progress) : SwampCave.GenCave(progress);
        public override void PostWorldGen() {
            //HeadquarterLayerOne.SpawnLava();
        }
    }
}