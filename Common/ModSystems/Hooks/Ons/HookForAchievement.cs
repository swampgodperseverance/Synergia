using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Synergia.Content.Achievements;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.Achievements;
using Terraria.GameContent.UI.Elements;
using Terraria.Initializers;
using Terraria.ModLoader;
using Terraria.UI;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    internal class HookForAchievement : ModSystem {
        Hook completeAchieve;
        static FieldInfo ConditionsField;

        delegate void Orig_OnCompleted(ModAchievement modAchievement, Achievement achievement);
        delegate void Get_OnCompletedDetour(Orig_OnCompleted orig, ModAchievement modAchievement, Achievement achievement);

        public override void Load() {
            ConditionsField = typeof(Achievement).GetField("_conditions", BindingFlags.Instance | BindingFlags.NonPublic);

            MethodInfo achieve = typeof(ModAchievement).GetMethod(nameof(ModAchievement.OnCompleted), BindingFlags.Public | BindingFlags.Instance);
            completeAchieve = new Hook(achieve, (Get_OnCompletedDetour)SetCompleteAchieve);

            On_AchievementInitializer.OnAchievementCompleted += ChatMsgIfCompleted;
            On_InGameNotificationsTracker.AddCompleted += NotificationIfCompleted;
            On_AchievementManager.ClearAll += On_AchievementManager_ClearAll;

            IL_UIAchievementListItem.DrawSelf += IL_UIAchievementListItem_DrawSelf;
        }
        void SetCompleteAchieve(Orig_OnCompleted orig, ModAchievement modAchievement, Achievement achievement) {
            orig(modAchievement, achievement);

            Dictionary<string, AchievementCondition> dict = (Dictionary<string, AchievementCondition>)ConditionsField.GetValue(achievement);

            if (dict == null) {
                return;
            }

            foreach (KeyValuePair<string, AchievementCondition> pair in dict) {
                AchievementCondition condition = pair.Value;
                SaveAchieveIfCompleted.MarkCompleted(achievement.Name, condition.Name, true);
            }
        }
        void ChatMsgIfCompleted(On_AchievementInitializer.orig_OnAchievementCompleted orig, Achievement achievement) {
            if (CheckAchieve(achievement)) { return; }
            else { orig(achievement); }
        }
        void NotificationIfCompleted(On_InGameNotificationsTracker.orig_AddCompleted orig, Achievement achievement) {
            if (CheckAchieve(achievement)) { return; }
            else { orig(achievement); }
        }
        void On_AchievementManager_ClearAll(On_AchievementManager.orig_ClearAll orig, AchievementManager self) {
            orig(self);
            foreach (Achievement achievement in self.CreateAchievementsList()) {
                ClearValue(achievement);
            }
        }
        void IL_UIAchievementListItem_DrawSelf(ILContext il) {
            ILCursor iLCursor = new(il);
            iLCursor.GotoNext(i => i.MatchCeq());
            iLCursor.Index--;
            iLCursor.RemoveRange(2);
            iLCursor.Emit(OpCodes.Ldarg_0);
            iLCursor.Emit(OpCodes.Ldfld, typeof(UIAchievementListItem).GetField("_achievement", BindingFlags.Instance | BindingFlags.NonPublic));
            iLCursor.EmitDelegate<Func<bool, Achievement, bool>>((isCompleted, achievement) => {
                return !(isCompleted || CheckAchieve(achievement));
            });
        }
        static bool CheckAchieve(Achievement achievement) {
            foreach (CompletedEntry entry in SaveAchieveIfCompleted.SaveSystem.Completed) {
                if (entry.Achievement == achievement.Name && entry.Value) {
                    return true;
                }
            }
            return false;
        }
        static void ClearValue(Achievement achievement) {
            foreach (CompletedEntry entry in SaveAchieveIfCompleted.SaveSystem.Completed) {
                if (entry.Achievement == achievement.Name && entry.Value) {
                    entry.Value = false;
                }
            }
            SaveAchieveIfCompleted.Save();
        }
        public override void Unload() {
            completeAchieve?.Dispose();
            completeAchieve = null;
            ConditionsField = null;

            On_AchievementInitializer.OnAchievementCompleted -= ChatMsgIfCompleted;
            On_InGameNotificationsTracker.AddCompleted -= NotificationIfCompleted;
            On_AchievementManager.ClearAll -= On_AchievementManager_ClearAll;
            IL_UIAchievementListItem.DrawSelf -= IL_UIAchievementListItem_DrawSelf;
        }
    }
}