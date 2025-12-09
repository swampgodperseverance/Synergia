using Synergia.Common.GlobalNPCs.AI;
using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems.Hooks.Ons;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs
{
    public class BossConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [LabelKey("$Mods.Synergia.Config.OcramHardModeAI")]
        [TooltipKey("$Mods.Synergia.Config.MakesOcramAI")]
        [DefaultValue(true)]
        public bool HardModeEnabled { get; set; }

        [LabelKey("$Mods.Synergia.Config.OcramWildAI")]
        [TooltipKey("$Mods.Synergia.Config.MakesOcramAI2")]
        [DefaultValue(true)]
        public bool BacteriumPrimeBuffEnabled { get; set; }

        [LabelKey("$Mods.Synergia.Config.PapuanHardAI")]
        [TooltipKey("$Mods.Synergia.Config.MakesPapuanAI")]
        [DefaultValue(true)]
        public bool PapuanWizardHardAIEnabled { get; set; }

        [LabelKey("$Mods.Synergia.Config.NewUI")]
        [TooltipKey("$Mods.Synergia.Config.NewUITooltip")]
        [DefaultValue(true)]
        public bool ActiveNewUI { get; set; }

        [Header("$Mods.Synergia.Config.SummonUI")]
        [LabelKey("$Mods.Synergia.Config.ActiveSummonUI")]
        [TooltipKey("$Mods.Synergia.Config.ActiveSummonUITooltip")]
        [DefaultValue(true)]
        public bool ActiveSummonUI { get; set; }

        [LabelKey("$Mods.Synergia.Config.HoverUI")]
        [TooltipKey("$Mods.Synergia.Config.HoverUITooltip")]
        [DefaultValue(true)]
        public bool HoverUI { get; set; }

        [LabelKey("$Mods.Synergia.Config.ResetUI")]
        [TooltipKey("$Mods.Synergia.Config.ResetUITooltip")]
        [DefaultValue(true)]
        public bool ResetUI { get; set; }


        public override void OnLoaded() {
            OcramUpgrades.HardModeEnabled = HardModeEnabled;
            BacteriumPrimeBuff.Enabled = BacteriumPrimeBuffEnabled;
            PapuanWizardUpgrades.HardAIEnabled = PapuanWizardHardAIEnabled;
            HookForNewProgressBar.NewUI = ActiveNewUI;
            SummonUI.IsActiveSummonUI = ActiveSummonUI;
            SummonUI.HoverUIElement = HoverUI;
            SummonUI.ResetUIPositions = ResetUI;
        }
        public override void OnChanged()
        {
            OcramUpgrades.HardModeEnabled = HardModeEnabled;
            BacteriumPrimeBuff.Enabled = BacteriumPrimeBuffEnabled;
            PapuanWizardUpgrades.HardAIEnabled = PapuanWizardHardAIEnabled;
            HookForNewProgressBar.NewUI = ActiveNewUI;
            SummonUI.IsActiveSummonUI = ActiveSummonUI;
            SummonUI.HoverUIElement = HoverUI;
            SummonUI.ResetUIPositions = ResetUI;
        }
    }
}
