using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Synergia.Content.Buffs;

public class GloomDebuff : ModBuff {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

    public override void Update(Player player, ref int buffIndex) {
    }
}
