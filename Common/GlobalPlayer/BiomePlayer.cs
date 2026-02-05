// Code by 𝒜𝑒𝓇𝒾𝓈
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
    }                                                                                  
}