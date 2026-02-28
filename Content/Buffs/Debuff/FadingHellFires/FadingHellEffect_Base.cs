using Synergia.Common.GlobalPlayer.Armor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using static Synergia.Common.GlobalItems.FadingHellData;

namespace Synergia.Content.Buffs.Debuff.FadingHellFires
{
    public class FadingHellEffect_Base : ModBuff
    {
        protected FireType fireType = FireType.None;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            fireType = GetFireType();
        }
        protected virtual FireType GetFireType() => FireType.None;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            npc.DelBuff(buffIndex);
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.wet && !player.lavaWet)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    player.DelBuff(buffIndex);
                return;
            }

            player.buffTime[buffIndex] = 18000;
            FadingHellPlayer fadingHellPlayer = player.GetModPlayer<FadingHellPlayer>();
            fadingHellPlayer.isOnFire = true;
            fadingHellPlayer.currentFireType = fireType;
        }
    }
}
