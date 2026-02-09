using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems.Hooks.Ons;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs {
    public class UIConfig : BaseConfig {
        [LabelKey("$Mods.Synergia.Configs.Config.UI.NewUI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.UI.NewUITooltip")]
        [DefaultValue(true)]
        public bool ActiveNewUI { get; set; }

        [Header("$Mods.Synergia.Configs.Config.UI.SummonUI")]
        [LabelKey("$Mods.Synergia.Configs.Config.UI.ActiveSummonUI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.UI.ActiveSummonUITooltip")]
        [DefaultValue(true)]
        public bool ActiveSummonUI { get; set; }

        [LabelKey("$Mods.Synergia.Configs.Config.UI.HoverUI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.UI.HoverUITooltip")]
        [DefaultValue(true)]
        public bool HoverUI { get; set; }

        [LabelKey("$Mods.Synergia.Configs.Config.UI.ResetUI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.UI.ResetUITooltip")]
        [DefaultValue(true)]
        public bool ResetUI { get; set; }

        public override void OnLoaded() {
            HookForNewProgressBar.NewUI = ActiveNewUI;
            SummonUI.IsActiveSummonUI = ActiveSummonUI;
            SummonUI.HoverUIElement = HoverUI;
            SummonUI.ResetUIPositions = ResetUI;
        }
        public override void OnChanged() {
            HookForNewProgressBar.NewUI = ActiveNewUI;
            SummonUI.IsActiveSummonUI = ActiveSummonUI;
            SummonUI.HoverUIElement = HoverUI;
            SummonUI.ResetUIPositions = ResetUI;
        }
    }
}