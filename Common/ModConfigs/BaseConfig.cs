using Terraria.ModLoader.Config;

namespace Synergia.Common.ModConfigs {
    public abstract class BaseConfig : ModConfig {
        public override string LocalizationCategory => "Configs";
        public override ConfigScope Mode => ConfigScope.ServerSide;
    }   
}