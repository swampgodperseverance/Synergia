using Terraria.ModLoader;

namespace Synergia.Common.GlobalPlayer
{
    public class BiomePlayer : ModPlayer
    {
        public bool InSnowVillage = false;

        public override void ResetEffects()
        {
            InSnowVillage = false;
        }
    }
}