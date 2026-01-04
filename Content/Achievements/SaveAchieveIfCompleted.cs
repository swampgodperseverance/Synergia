using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Achievements;
using Synergia.Common;

namespace Synergia.Content.Achievements {
    public class SaveAchieveIfCompleted {
        static string FilePath => Path.Combine(Main.SavePath, "SynergiaUtil", "Achievements.json");
        public static SaveAchieveIfCompleted SaveSystem { get; private set; } = new();
        [JsonProperty]
        public List<CompletedEntry> Completed = [];

        public static void Load() {
            if (File.Exists(FilePath)) {
                SaveSystem = JsonConvert.DeserializeObject<SaveAchieveIfCompleted>(File.ReadAllText(FilePath));
            }
            else { SaveSystem = new SaveAchieveIfCompleted(); Save(); }
        }
        public static void Save() {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(SaveSystem, Formatting.Indented));
        }
        public static void MarkCompleted(string achievementName, string conditionName, bool isCompleted) {
            CompletedEntry entry = SaveSystem.Completed.Find(e => e.Achievement == achievementName && e.Condition == conditionName);

            if (entry == null) {
                SaveSystem.Completed.Add(new CompletedEntry { Achievement = achievementName, Condition = conditionName, Value = isCompleted });
            }
            else { 
                entry.Value = isCompleted; 
            }

            Save();
        }
        public static void Restore() {
            foreach (CompletedEntry completed in SaveSystem.Completed.ToArray()) {
                if (!completed.Value) {
                    continue;
                }
                AchievementCondition cond = Main.Achievements.GetCondition(completed.Achievement, completed.Condition);
                cond.Complete();
            }
        }
    }
}