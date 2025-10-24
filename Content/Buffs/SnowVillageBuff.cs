using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Buffs
{
    public class SnowVillageBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex) => player.GetModPlayer<BiomePlayer>().InSnowVillage = true;

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }
}