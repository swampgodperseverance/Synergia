using System.IO;
using Terraria;
using Terraria.ModLoader.IO;
using static Synergia.Helpers.SynegiaHelper;

namespace Synergia.Common.ModSystems;

public class DownedBossSystem : ModSystem {
    public static bool DownedSinlordBoss = false;
    public static bool CompleteNewFrostEvent = false;

    public override void ClearWorld() {
        DownedSinlordBoss = false;
        CompleteNewFrostEvent = false;
    }
    public override void SaveWorldData(TagCompound tag) {
        if (DownedSinlordBoss) {
            EzSave(tag, "downedSinlordBoss", ref DownedSinlordBoss);
        }
        if (CompleteNewFrostEvent) {
            EzSave(tag, "CompleteNewFrostEvent", ref CompleteNewFrostEvent);
        }
    }
    public override void LoadWorldData(TagCompound tag) {
        EzLoad(tag, "downedSinlordBoss", ref DownedSinlordBoss);
        EzLoad(tag, "CompleteNewFrostEvent", ref CompleteNewFrostEvent);
    }
    public override void NetSend(BinaryWriter writer) {
        BitsByte flags = new();
        flags[0] = DownedSinlordBoss;
        flags[1] = CompleteNewFrostEvent;
        writer.Write(flags);
    }
    public override void NetReceive(BinaryReader reader) {
        BitsByte flags = reader.ReadByte();
        DownedSinlordBoss = flags[0];
        CompleteNewFrostEvent = flags[1];
    }
}