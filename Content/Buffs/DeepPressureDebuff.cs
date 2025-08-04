using Terraria;
using Terraria.ModLoader;

namespace Vanilla.Content.Buffs
{
    public class DeepPressureDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.active && npc.buffTime[buffIndex] > 0)
            {
                int damage = Main.rand.Next(80, 121);
                npc.SimpleStrikeNPC(damage, 0); // Updated to SimpleStrikeNPC for reliability
            }
        }
    }
}