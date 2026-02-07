global using static Synergia.Common.QuestSystem;
global using static Synergia.Common.QuestSystem.QuestConst;
global using static Synergia.Common.SUtils.LocUtil;
using static Bismuth.Utilities.ModSupport.QuestRegistry;
using Bismuth.Utilities.ModSupport;
using Terraria;
using System.Collections.Generic;

namespace Synergia.Content.Quests;

public abstract class BaseQuestLogic : BaseQuest, ILoadable, IPostSetup {
    public virtual bool IsEndQuest => false;
    public virtual int QuestNPC => 0;
    public abstract string Key { get; }
    public static Dictionary<int, List<BaseQuestLogic>> AllQuest { get; private set; } = [];
    public sealed override string UniqueKey => Key;
    public string BaseGetChat(Player player, string npcName, string npcKey, string npcKey2, string npcKey3) {
        Main.npcChatCornerItem = CornerItem;
        QuestPlayer q = player.GetModPlayer<QuestPlayer>();
        bool defeated = HasDefeated(PostBossRequirement);
        if (q.CompletedQuests.Contains(UniqueKey) && defeated) {
            Progress = 0;
            return LocQuestKey(npcName, npcKey);
        }
        if (q.ActiveQuests.Contains(UniqueKey) && defeated) {
            Progress = 2;
            return LocQuestKey(npcName, npcKey2);
        }
        Progress = 1;
        return LocQuestKey(npcName, npcKey3);
    }
    public string BaseGetButtonText(Player player, ref bool Isfristclicked, string npcName, string npcKey, string npcKey2) {
        bool defeated = HasDefeated(PostBossRequirement);
        QuestPlayer q = player.GetModPlayer<QuestPlayer>();
        if (q.CompletedQuests.Contains(UniqueKey) && defeated) return "";
        if (Isfristclicked) {
            return LocQuestKey(npcName, npcKey);
        }
        else return LocQuestKey(npcName, npcKey2);
    }
    public bool BaseIsCompleted(Player player) {
        QuestPlayer q = player.GetModPlayer<QuestPlayer>();
        return q.CompletedQuests.Contains(UniqueKey);
    }
    public void BaseOnChatButtonClicked(Player player) {
        QuestPlayer q = player.GetModPlayer<QuestPlayer>();

        if (q.CompletedQuests.Contains(UniqueKey)) return;

        if (!q.ActiveQuests.Contains(UniqueKey)) {
            Notification(player, false, true);
            q.ActiveQuests.Add(UniqueKey);
            Progress = 2;
        }
    }
    public bool BaseIsAvailable(Player player) {
        QuestPlayer q = player.GetModPlayer<QuestPlayer>();
        bool isAvailable = !q.CompletedQuests.Contains(UniqueKey);
        bool isDefeated = HasDefeated(PostBossRequirement);
        if (isAvailable && isDefeated) Progress = 1;
        else Progress = 0;
        if (isDefeated) { return isAvailable; }
        else { return isDefeated; }
    }
    public bool BaseIsActive(Player player) {
        QuestPlayer q = player.GetModPlayer<QuestPlayer>();
        bool isActive = q.ActiveQuests.Contains(UniqueKey);
        if (isActive) Progress = 2;
        return isActive;
    }   
    public virtual void PostSetup(Mod mod) {
        if (QuestNPC <= 0) { return; }

        if (!AllQuest.TryGetValue(QuestNPC, out var list)) {
            list = [];
            AllQuest.Add(QuestNPC, list);
        }

        list.Add(this); 
    }

    public void Load(Mod mod) {
        Register(this);
    }

    public void Unload() { }
}