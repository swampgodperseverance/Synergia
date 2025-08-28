using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Synergia.Content.Buffs;

public class Hellborn : ModBuff {
    public override void SetStaticDefaults() {
        // DisplayName.SetDefault("Hellborn");
        // Description.SetDefault("Your attacks are more intense");

        Main.buffNoSave[Type] = false;
        Main.debuff[Type] = false;
        BuffID.Sets.IsATagBuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
    }
}
