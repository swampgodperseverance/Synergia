using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using System.ComponentModel;
using Synergia.Common.GlobalNPCs.AI;

namespace Synergia.Common.ModConfigs
{
    public class BossConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Enable Ocram Hard Mode AI")]
        [Tooltip("Makes Ocram's AI more challenging")]
        [DefaultValue(false)]
        public bool HardModeEnabled;

        [Label("Enable Ocram Wild AI")]
        [Tooltip("Makes Bacterium Prime completely broken. Default Bacterium Prime without ai get 3 more new attacks. Why do you need that?")]
        [DefaultValue(false)]
        public bool BacteriumPrimeBuffEnabled;


        [Label("Enable Papuan Wizard Hard AI")]
        [Tooltip("Makes Papuan Wizard significantly stronger with teleport and projectile attacks.")]
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
