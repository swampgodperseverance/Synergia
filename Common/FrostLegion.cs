using Synergia.Common.ModSystems;
using Synergia.Content.NPCs;
using Synergia.Content.Projectiles.Other;
using Synergia.GraphicsSetting;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.Localization;
using ValhallaMod.NPCs.Snowman;
using static Synergia.Helpers.EventHelper;

namespace Synergia.Common;

public class FrostLegion : ModEvent {
    readonly List<int> vanilla = [NPCID.SnowmanGangsta, NPCID.MisterStabby, NPCID.SnowBalla, NPCType<Coldmando>(), NPCType<MisterShotty>()];
    bool preSpawn = false;

    public override void SettingEvent() {
        EventName = Lang.inter[87].Value;
        EventPoint = 1;
        MaxWave = 2;
        EventEnemies = [NPCID.SnowmanGangsta, NPCID.MisterStabby, NPCID.SnowBalla, NPCType<Coldmando>(), NPCType<MisterShotty>(), NPCType<SnowmanTrasher>(), NPCType<Snowykaze>(), NPCType<ElderSnowman>()];
        if (FistText) {
            DoWave(CurrentWave);
        }
    }
    public override void PostUpdateWorld(int currentWave) {
        foreach (NPC npc in Main.npc) {
            if (npc.active) {
                switch (currentWave) {
                    case 0: DeActive(npc, NPCType<Coldmando>(), NPCType<MisterShotty>(), NPCType<SnowmanTrasher>(), NPCType<ColdFather>()); EventSize = 40; break;
                    case 1: DeActive(npc, NPCType<ColdFather>()); EventSize = 30; break;
                    case 2: EventEnemies.Add(NPCType<ColdFather>()); EventSize = 1; break;
                }
            }
        }
        if (FistText) {
            ActivePresentInSky(IsActive);
        }
    }
    public override void OnKillNPC(NPC npc, int currentWave) {
        if (currentWave != 2) {
            GetProgress(npc, EventEnemies, ref EventProgress, EventPoint);
            GetProgress(npc, NPCType<ElderSnowman>(), ref EventProgress, 4);
        }
        if (currentWave == 2) {
            if (npc.type == NPCType<ColdFather>()) {
                EventProgress = 1;
            }
        }
    }
    public override void SpawnNPC(ref IDictionary<int, float> pool, int currentWave) {
        if (currentWave != 2) {
            GetVanillaNPC(ref pool, currentWave);
        }
        switch (currentWave) {
            case 0: EventHelper.SpawnNPC(ref pool, NPCType<Snowykaze>(), 0.45f); break;
            case 1: Spawn(ref pool, NPCType<ElderSnowman>(), 0.30f); break;
            case 2: Spawn2(ref pool, NPCType<ColdFather>(), 1f); pool.Remove(NPCType<Snowykaze>()); pool.Remove(NPCType<ElderSnowman>()); break;
        }
    }
    public override void DoWave(int currentWave) {
        if (!FistText) {
            TextWave(currentWave, 143, 144, 145);
        }
        Main.LocalPlayer.ManageSpecialBiomeVisuals(SynegiyGraphics.PRESENTSKY, IsActive);
    }
    public override void OnNextWave(int currentWave) {
        switch (currentWave) {
            case 1: TextWave(currentWave + 1, [.. EventEnemies]); break;
            case 2: TextWave(currentWave + 1, NPCType<ColdFather>()); break;
        }
    }
    public override void OnEnd() {
        DownedBossSystem.CompleteNewFrostEvent = true;
        preSpawn = false;
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

        NPC.SetEventFlagCleared(ref NPC.downedFrost, 1);
        AchievementsHelper.NotifyProgressionEvent(12);

        Main.LocalPlayer.ManageSpecialBiomeVisuals(SynegiyGraphics.PRESENTSKY, IsActive);
    }
    static void DeActive(NPC npc, params int[] npcType) {
        if (npcType.Contains(npc.type)) {
            npc.active = false;
        }
    }
    static void Spawn(ref IDictionary<int, float> pool, int npcType, float chance) {
        if (!pool.ContainsKey(npcType)) {
            EventHelper.SpawnNPC(ref pool, npcType, chance);
        }
        if (NPC.AnyNPCs(npcType)) {
            pool.Clear();
        }
    }
    static void Spawn2(ref IDictionary<int, float> pool, int npcType, float chance) {
        pool.Clear();
        if (!pool.ContainsKey(npcType)) {
            EventHelper.SpawnNPC(ref pool, npcType, chance);
        }
        if (NPC.AnyNPCs(npcType)) {
            pool.Clear();
        }
    }
    static void ActivePresentInSky(bool active) {
        if (!active) {
            return;
        }
        List<int> projType = [ProjectileType<GreenPresentProj>(), ProjectileType<WhitePresentProj>(), ProjectileType<YellowPresentProj>()];
        if (Main.rand.NextBool(180)) {
            for (int i = 0; i < Main.maxPlayers; i++) {
                Player player = Main.player[i];
                if (player.active) {
                    Vector2 pos = new(player.Center.X - Main.rand.Next(-1000, 1000), player.Center.Y - 1000);
                    Projectile proj = Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), pos, Vector2.Zero, Main.rand.Next(projType), 0, 0f, Main.myPlayer, 0f, 0f)];
                }
            }
        }
    }
    void GetVanillaNPC(ref IDictionary<int, float> pool, int currentWave) {
        if (currentWave != 2) {
            for (int i = 0; i < 4; i++) {
                EventHelper.SpawnNPC(ref pool, vanilla[i], 1f);
            }
        }
    }
}