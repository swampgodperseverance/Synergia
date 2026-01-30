using Synergia.Content.NPCs;
using static Synergia.Common.ModSystems.WorldGens.BaseWorldGens;
using static Synergia.Helpers.SynegiaHelper;

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
        public override void ResetEffects() {
            InSnowVillage = false;
        }
        public void Arena(bool active) {
            // тут твой код если персонаж в арене
            SpawnNPC((HellArenaPositionX - 198 + 110) * 16, (HellArenaPositionY - 28) * 16, NPCType<HellheartMonolith>());
            arenaBiome = active;
        }
        public void Village(bool active) {
            villageBiome = active;
            // тут твой код если персонаж в деревни
        }        
        public void Lake(bool active) {
            lakeBiome = active;
        }
    }                                                                                                                       
}