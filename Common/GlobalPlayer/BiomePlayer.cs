// Code by SerNik
using Terraria;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;

namespace Synergia.Common.GlobalPlayer
{
    public class BiomePlayer : ModPlayer
    {
        public bool InSnowVillage;
        public bool lakeBiome;
        public bool villageBiome;
        public bool arenaBiome;

        public override void Initialize() {
            InSnowVillage = false;
            lakeBiome = false;
            villageBiome = false;
            arenaBiome = false;
        }
        public override void PostUpdate() {
            EditsArena(HellArenaPositionX - 198, HellArenaPositionY);
            EditsVillage(HellVillageX - 280, HellVillageY);
            EditsLake(HellLakeX - 236, HellLakeY);
        }
        static void EditsArena(int x, int y) {
            if (Main.LocalPlayer.GetModPlayer<BiomePlayer>().arenaBiome) {
                //SynegiaHelper.SpawnNPC((x + 110) * 16, (y - 28) * 16, NPCType<HellheartMonolith>());
            }
        }
        static void EditsVillage(int x, int y) { }
        static void EditsLake(int x, int y) { }
    }                                                                                  
}