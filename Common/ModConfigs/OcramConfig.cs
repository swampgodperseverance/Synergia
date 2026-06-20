using System.ComponentModel;
using Synergia.Common.GlobalNPCs.AI;
using Synergia.Content.GlobalNPCs.AI;
using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs
{
    public class BossConfig : BaseConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;


        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.NewRecipe")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.NewRecipeTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool NewRecipe { get; set; } = true;
        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.PapuanWizardHardAIEnabled")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.PapuanWizardHardAIEnabledTooltip")]
        public bool PapuanWizardHardAIEnabled { get; set; } = true;


        // --- Настройки Нового ИИ для Боссов ---

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.KingSlimeAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.KingSlimeAITooltip")]
        public bool KingSlimeAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.BrainOfCthulhuAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.BrainOfCthulhuAITooltip")]
        public bool BrainOfCthulhuAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.WallOfFleshAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.WallOfFleshAITooltip")]
        public bool WallOfFleshAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.TwinsAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.TwinsAITooltip")]
        public bool TwinsAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.DestroyerAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.DestroyerAITooltip")]
        public bool DestroyerAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.SkeletronPrimeAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.SkeletronPrimeAITooltip")]
        public bool SkeletronPrimeAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.PlanteraAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.PlanteraAITooltip")]
        public bool PlanteraAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.GolemAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.GolemAITooltip")]
        public bool GolemAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.CultistAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.CultistAITooltip")]
        public bool CultistAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.OcramAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.OcramAITooltip")]
        public bool OcramAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.TurkorAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.TurkorAITooltip")]
        public bool TurkorAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.ColdFatherAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.ColdFatherAITooltip")]
        public bool ColdFatherAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.JadeEmperorAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.JadeEmperorAITooltip")]
        public bool JadeEmperorAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.NecromancerAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.NecromancerAITooltip")]
        public bool NecromancerAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.MinotaurAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.MinotaurAITooltip")]
        public bool MinotaurAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.LothorAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.LothorAITooltip")]
        public bool LothorAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.DesertBeakAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.DesertBeakAITooltip")]
        public bool DesertBeakAI { get; set; } = true;

        [DefaultValue(true)]
        [LabelKey("$Mods.Synergia.Configs.Config.SynergiaConfig.ColossalCarnageAI")]
        [TooltipKey("$Mods.Synergia.Configs.Config.SynergiaConfig.ColossalCarnageAITooltip")]
        public bool ColossalCarnageAI { get; set; } = true;

        public override void OnLoaded() => UpdateAIStates();
        public override void OnChanged() => UpdateAIStates();

        private void UpdateAIStates()
        {
            PapuanWizardUpgrades.HardAIEnabled = PapuanWizardHardAIEnabled;



            Common.GlobalNPCs.AI.KingSlimeAI.Disabled = !this.KingSlimeAI;
            BrainDashAI.Disabled = !BrainOfCthulhuAI;
            Common.GlobalNPCs.AI.WallOfFleshAI.Disabled = !this.WallOfFleshAI;
            Common.GlobalNPCs.AI.TwinsAI.Disabled = !this.TwinsAI;
            Common.GlobalNPCs.AI.DestroyerAI.Disabled = !this.DestroyerAI;
            Common.GlobalNPCs.AI.SkeletronPrimeAI.Disabled = !this.SkeletronPrimeAI;
            Common.GlobalNPCs.AI.PlanteraFlowerSpawner.Disabled = !this.PlanteraAI;
            Common.GlobalNPCs.AI.GolemExtraAttack.Disabled = !this.GolemAI;
            Common.GlobalNPCs.AI.CultistAI.Disabled = !this.CultistAI;
            Common.GlobalNPCs.AI.OcramAI.Disabled = !this.OcramAI;
            Common.GlobalNPCs.TurkorFeatherAI.Disabled = !this.TurkorAI;
            ColdFatherExtraAttack.Disabled = !ColdFatherAI;
            Common.GlobalNPCs.AI.JadeEmperorRework.Disabled = !this.JadeEmperorAI;
            Common.GlobalNPCs.AIs.EvilNecromancerAI.Disabled = !this.NecromancerAI;
            Content.Compat.MinotaurAI.Disabled = !this.MinotaurAI;
            Common.GlobalNPCs.AI.LothorAttackSystem.Disabled = !this.LothorAI;
            Common.GlobalNPCs.AI.DesertBeakIntegration.Disabled = !this.DesertBeakAI;
            CarnageAI.Disabled = !ColossalCarnageAI;

        }
    }
}