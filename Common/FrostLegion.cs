using Synergia.Common.ModSystems;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using ValhallaMod.NPCs.Snowman;
using static Synergia.Helpers.EventHelper;

namespace Synergia.Common;

public class FrostLegion : ModEvent {
    public override void SettingEvent() {
        EventName = Lang.inter[87].Value;
        EventPoint = 1;
        MaxWave = 2;
        if (CurrentWave == 0) {
            CurrentWave = 0;
            EventSize = 40;
        }
        EventEnemies = [NPCID.SnowmanGangsta, NPCID.MisterStabby, NPCID.SnowBalla, NPCType<Coldmando>(), NPCType<MisterShotty>(), NPCType<SnowmanTrasher>()];
    }
    public override void PostUpdateWorld(int currentWave) {
        foreach (NPC npc in Main.npc) {
            if (npc.active) {
                switch (currentWave) {
                    case 0: DeActive(npc, NPCType<Coldmando>(), NPCType<MisterShotty>(), NPCType<SnowmanTrasher>(), NPCType<ColdFather>()); break;
                    case 1: DeActive(npc, NPCType<ColdFather>()); EventSize = 30; break;
                    case 2: EventSize = 1; break;
                }
            }
        }
    }
    public override void OnKillNPC(NPC npc, int currentWave) {
        if (currentWave != 2) {
            GetProgress(npc, EventEnemies, ref EventProgress, EventPoint);
        }
        if (currentWave == 2) {
            if (npc.type == NPCType<ColdFather>()) {
                EventProgress = 1;
            }
        }
    }
    public override void SpawnNPC(ref IDictionary<int, float> pool, int currentWave) {
        if (currentWave == 2) {
            //int _143 = NPC.CountNPCS(143);
            //int _144 = NPC.CountNPCS(144);
            //int _145 = NPC.CountNPCS(145);
            //int _Coldmando = NPC.CountNPCS(NPCType<Coldmando>());
            //int _MisterShotty = NPC.CountNPCS(NPCType<MisterShotty>());
            //int _SnowmanTrasher = NPC.CountNPCS(NPCType<SnowmanTrasher>());

            if (NPC.AnyNPCs(NPCType<ColdFather>())) {
                pool.Clear();
            }
        }
    }
    public override void DoWave(int currentWave) {
        TextWave(currentWave, 143, 144, 145);
    }
    public override void OnNextWave(int currentWave) {
        switch (currentWave) {
            case 1: TextWave(currentWave, [.. EventEnemies]); break;
            case 2: TextWave(currentWave, NPCType<ColdFather>()); break;
        }
    }
    public override void OnEnd() {
        DownedBossSystem.CompleteNewFrostEvent = true;
        Main.invasionSize = 0;
        Main.invasionSizeStart = 0;
        Main.invasionProgress = 0;
        Main.invasionProgressMax = 0;

        NPC.downedFrost = true;

        Main.invasionType = 0;

        LocalizedText empty = Lang.misc[4];
        if (Main.netMode == NetmodeID.SinglePlayer) {
            Main.NewText(empty.ToString(), 175, 75);
        }
        if (Main.netMode == NetmodeID.Server) {
            NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon);
            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(empty.Key), new Color(175, 75, 255));
        }
    }
    static void DeActive(NPC npc, params int[] npcType) {
        if (npcType.Contains(npc.type)) {
            npc.active = false;
        }
    }
}