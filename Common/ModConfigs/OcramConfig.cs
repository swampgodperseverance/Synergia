using Synergia.Common.GlobalNPCs.AI;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs
{
    public class BossConfig : BaseConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [LabelKey("$Mods.Synergia.Configs.Config.SynegiaConfig.OcramHardModeAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynegiaConfig.MakesOcramAI")]
        [DefaultValue(true)]
        public bool HardModeEnabled { get; set; }

        [LabelKey("$Mods.Synergia.Configs.Config.SynegiaConfig.OcramWildAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynegiaConfig.MakesOcramAI2")]
        [DefaultValue(true)]
        public bool BacteriumPrimeBuffEnabled { get; set; }

        [LabelKey("$Mods.Synergia.Configs.Config.SynegiaConfig.PapuanHardAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynegiaConfig.MakesPapuanAI")]
        [DefaultValue(true)]
        public bool PapuanWizardHardAIEnabled { get; set; }

        [ReloadRequiredAttribute()]
        [LabelKey("$Mods.Synergia.Configs.Config.SynegiaConfig.NewRecipe")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynegiaConfig.NewRecipeTooltip")]
        [DefaultValue(true)]
        public bool NewRecipe { get; set; }

        public override void OnLoaded() {
            OcramUpgrades.HardModeEnabled = HardModeEnabled;
            BacteriumPrimeBuff.Enabled = BacteriumPrimeBuffEnabled;
            PapuanWizardUpgrades.HardAIEnabled = PapuanWizardHardAIEnabled;
        }
        public override void OnChanged()
        {
            OcramUpgrades.HardModeEnabled = HardModeEnabled;
            BacteriumPrimeBuff.Enabled = BacteriumPrimeBuffEnabled;
            PapuanWizardUpgrades.HardAIEnabled = PapuanWizardHardAIEnabled;
        }
    }
}