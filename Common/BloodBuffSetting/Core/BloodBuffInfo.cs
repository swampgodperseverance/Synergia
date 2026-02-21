// Code by 𝒜𝑒𝓇𝒾𝓈
using Terraria;
using Terraria.Localization;

namespace Synergia.Common.BloodBuffSeting.Core {
    public class BloodBuffInfo {
        public static string Localization(string tir) => Language.GetTextValue($"Mods.Synergia.Buffs.BloodBuff.{tir}");
        public virtual int Leveled => -1;
        public virtual string Tooltips => "";
        // List<String> ?
        public virtual string AdditionalTooltips => "";
        public virtual void Buff(Player player) { }
    }
}