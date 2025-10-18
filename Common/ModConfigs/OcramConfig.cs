using Synergia.Common.GlobalNPCs.AI;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs
{
    public class BossConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [LabelKey("$Mods.Synergia.Config.OcramHardModeAI")]
        [TooltipKey("$Mods.Synergia.Config.MakesOcramAI")]
        [DefaultValue(false)]
        public bool HardModeEnabled;

        [LabelKey("$Mods.Synergia.Config.OcramWildAI")]
        [TooltipKey("$Mods.Synergia.Config.MakesOcramAI2")]
        [DefaultValue(false)]
        public bool BacteriumPrimeBuffEnabled;


        [LabelKey("$Mods.Synergia.Config.PapuanHardAI")]
        [TooltipKey("$Mods.Synergia.Config.MakesPapuanAI")]
        [DefaultValue(true)]
        public bool PapuanWizardHardAIEnabled;

        public override void OnChanged()
        {
            OcramUpgrades.HardModeEnabled = HardModeEnabled;
            BacteriumPrimeBuff.Enabled = BacteriumPrimeBuffEnabled;
            PapuanWizardUpgrades.HardAIEnabled = PapuanWizardHardAIEnabled;
        }
    }
}
