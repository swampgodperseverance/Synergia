using Terraria.Localization;

namespace Synergia.Common.BloodBuffSeting.Core {
    public abstract class AbstractBestiaryInfo : BestiaryInfo, ILoadable {
        public static string Localization(string tir) => Language.GetTextValue($"Mods.Synergia.Buffs.BloodBuff.{tir}");
        public void Load(Mod mod) {
            BestiaryManger manager = BestiaryManger.Instance;
            manager.Info.Add(this);
            manager.CurrentLevel.Add(Leveled, this);
        }
        public void Unload() { }
    }
}