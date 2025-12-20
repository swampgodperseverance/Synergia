using Terraria;

namespace Synergia.Content.Buffs.Debuff {
    public abstract class DebuffBase : ModBuff {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}
