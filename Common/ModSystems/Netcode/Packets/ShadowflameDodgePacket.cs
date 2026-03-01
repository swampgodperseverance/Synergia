using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.Netcode.Packets
{
    sealed class ShadowflameDodgePacket : NetPacket
    {
        public ShadowflameDodgePacket(Player player) {
            Writer.TryWriteSenderPlayer(player);
        }
        public override void Read(BinaryReader reader, int sender)
        {
            if (!reader.TryReadSenderPlayer(sender, out Player player))
                return;

            player.GetModPlayer<FadingHellShadowflameDodge>().DodgeEffect();

            if (Main.netMode == NetmodeID.Server)
                MultiplayerSystem.SendPacket(new ShadowflameDodgePacket(player), ignoreClient: sender);
        }
    }
}
