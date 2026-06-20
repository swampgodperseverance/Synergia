using System.ComponentModel;
using Synergia.Common.GlobalNPCs.AI;
using Synergia.Content.GlobalNPCs.AI;
using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs
{
    public class MechanicsConfig : BaseConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;


        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.NewRecipe")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.NewRecipeTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool NewRecipe { get; set; } = true;

        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.DynamicalPrices")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.DynamicalPricesTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool DynimicalPrices { get; set; } = true;

        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.StaminaHooks")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.StaminaHooksTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool StaminaHooks { get; set; } = true;

        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.HPScaling")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.HPScalingTooltip")]
        [DefaultValue(true)]
        public bool NpcHpScaling { get; set; } = true;

        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.WeaponsScaling")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.WeaponsScalingTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool WeaponsScaling { get; set; } = true;


        public override void OnLoaded() => UpdateAIStates();
        public override void OnChanged() => UpdateAIStates();

        private void UpdateAIStates()
        {
            Core.PriceSystem.DynamicPriceManager.Disabled = !this.DynimicalPrices;
            Common.GlobalNPCs.Changes.NPCBalanceEditor.Disabled = !this.NpcHpScaling;
            Common.GlobalItems.Changes.BalanceEditor.Disabled = !this.WeaponsScaling;
            Common.GlobalProjectiles.HookGP.Disabled = !this.StaminaHooks;
        }
    }
}