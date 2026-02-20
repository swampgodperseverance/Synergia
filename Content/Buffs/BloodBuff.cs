using Synergia.Common;
using Synergia.Common.BloodBuffSeting.Core;
using System;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Content.Buffs {
    public class BloodBuff : ModBuff {
        public override void SetStaticDefaults() {
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams) {
            /// Draw Blood if Mouse in buff icon
            /// This method is not suitable for this purpose.
            /// <see cref="Common.ModSystems.Hooks.Ons.HookForBloodBuff.On_Main_MouseText_DrawBuffTooltip">
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
            //tip = string.Format(Language.GetTextValue("Mods.Synergia.Tooltips.Buff", DamageBonuses()));
            AbstractBestiaryInfo info = GetLevel();
            tip = string.Format(info.Tooltips, info.Leveled);
        }
        public override void Update(Player player, ref int buffIndex) {
           // Main.NewText(GetBestiaryLevel());
            player.GetDamage(DamageClass.Throwing).Flat += DamageBonuses();
            AbstractBestiaryInfo info = GetLevel();
            info.Buff(player);
        }
        internal static int GetBestiaryLevel() {
            int level = (int)(Main.GetBestiaryProgressReport().CompletionAmountTotal / 10f);

            return Math.Min(level, 1);
        }
        internal static AbstractBestiaryInfo GetLevel() {
            AbstractBestiaryInfo info = BestiaryManger.Instance.GetLevelBloodBuff(GetBestiaryLevel());
            return info;
        }
        internal static int DamageBonuses() => (int)Main.GetBestiaryProgressReport().CompletionAmountTotal == 1 ? (int)Main.GetBestiaryProgressReport().CompletionAmountTotal : (int)Main.GetBestiaryProgressReport().CompletionAmountTotal / 2;
    }
}