// Code by SerNik
using Synergia.Common.BloodBuffSeting.Core;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Content.Buffs {
    public class BloodBuff : ModBuff {
        public override void SetStaticDefaults() {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
            /// Draw Blood if Mouse in buff icon
            /// This method is not suitable for this purpose.
            /// <see cref="Common.ModSystems.Hooks.Ons.HookForBloodBuff.On_Main_MouseText_DrawBuffTooltip">
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
            AbstractBloodBuffInfo info = GetLevel();
            if (info.AdditionalTooltips != "") { tip = string.Format(info.Tooltips, info.AdditionalTooltips); }
            else { tip = info.Tooltips; }
        }
        public override void Update(Player player, ref int buffIndex) {
            int level = GetBestiaryLevel();
            for (int i = 0; i <= level; i++) { BloodBuffManger.Instance.GetLevelBloodBuff(i)?.Buff(player); }
        }
        internal static AbstractBloodBuffInfo GetLevel() => BloodBuffManger.Instance.GetLevelBloodBuff(GetBestiaryLevel());
        internal static int GetBestiaryLevel() {
            for (int i = 100; i > 0;) {
                if (GetBestiaryProgress(i)) { return i / 10; }
                i -= 10;
            }
            return 0;
        }
        static bool GetBestiaryProgress(int value) => Main.GetBestiaryProgressReport().CompletionPercent >= value / 100f;
    }
}