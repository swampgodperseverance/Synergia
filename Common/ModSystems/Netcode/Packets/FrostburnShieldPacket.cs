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
    sealed class FrostburnShieldSyncDataPacket : NetPacket
    {
        public FrostburnShieldSyncDataPacket(Player player, bool isActive, int shieldProgression, int dashDelay, int dashTimer)
        {
            Writer.TryWriteSenderPlayer(player);
            Writer.Write(isActive);
            Writer.Write(shieldProgression);
            Writer.Write(dashDelay);
            Writer.Write(dashTimer);
        }
        public override void Read(BinaryReader reader, int sender)
        {
            if (!reader.TryReadSenderPlayer(sender, out Player player))
                return;

            bool isActive = reader.ReadBoolean();
            int shieldProgression = reader.ReadInt32();
            int dashDelay = reader.ReadInt32();
            int dashTimer = reader.ReadInt32();
            player.GetModPlayer<FadingHellFrostburnShield>().ReceivePacket(isActive, shieldProgression, dashDelay, dashTimer);

            if (Main.netMode == NetmodeID.Server)
                MultiplayerSystem.SendPacket(new FrostburnShieldSyncDataPacket(player, isActive, shieldProgression, dashDelay, dashTimer), ignoreClient: sender);
        }
    }
    sealed class FrostburnShieldDashPacket : NetPacket
    {
        public FrostburnShieldDashPacket(Player player, Vector2 newVelocity)
        {
            Writer.TryWriteSenderPlayer(player);
            Writer.WriteVector2(newVelocity);
        }
        public override void Read(BinaryReader reader, int sender)
        {
            if (!reader.TryReadSenderPlayer(sender, out Player player))
                return;

            Vector2 newVelocity = reader.ReadVector2();
            player.GetModPlayer<FadingHellFrostburnShield>().Dash(newVelocity);

            if (Main.netMode == NetmodeID.Server)
                MultiplayerSystem.SendPacket(new FrostburnShieldDashPacket(player, newVelocity), ignoreClient: sender);
        }
    }
}
