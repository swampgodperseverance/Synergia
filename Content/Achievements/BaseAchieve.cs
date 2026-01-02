using Terraria.Achievements;
using Terraria.Localization;

namespace Synergia.Content.Achievements {
    public abstract class BaseAchieve : ModAchievement {
        public abstract AchievementCategory BaseCategory { get; }
        public abstract AchievementCondition Target { get; }
        public override string TextureName => $"Synergia/Assets/Textures/Achievements/{GetType().Name}";
        public override LocalizedText FriendlyName => Language.GetText($"Mods.Synergia.Achievements.Name.{GetType().Name}");
        public override LocalizedText Description => Language.GetText($"Mods.Synergia.Achievements.Description.{GetType().Name}");
        public override void SetStaticDefaults() { 
            Achievement.SetCategory(BaseCategory);
            _ = Target.IsCompleted;
        }
        public enum BossAchievementPosition {
            Slime, EoC, Goblin, BoC, Bea, Skeletron, Deerclops, WoF, Pirat, QueenSlime, AllMechanic, Plantera, PumpKin, Golem, EoL, Fishron, Marcian, Culthist, MoonLord, AllBoss
        }
        public static string PositionName(BossAchievementPosition achievePosition) {
            return achievePosition switch {
                BossAchievementPosition.Slime => "SLIPPERY_SHINOBI",
                BossAchievementPosition.EoC => "EYE_ON_YOU",
                BossAchievementPosition.Goblin => "GOBLIN_PUNTER",
                BossAchievementPosition.BoC => "MASTERMIND",
                BossAchievementPosition.Bea => "STING_OPERATION",
                BossAchievementPosition.Skeletron => "BONED",
                BossAchievementPosition.Deerclops => "DEFEAT_DEERCLOPS",
                BossAchievementPosition.WoF => "STILL_HUNGRY",
                BossAchievementPosition.Pirat => "DAVY_JONES_LOCKER",
                BossAchievementPosition.QueenSlime => "DEFEAT_QUEEN_SLIME",
                BossAchievementPosition.AllMechanic => "BUCKETS_OF_BOLTS",
                BossAchievementPosition.Plantera => "THE_GREAT_SOUTHERN_PLANTKILL",
                BossAchievementPosition.PumpKin => "PUMPKIN_SMASHER",
                BossAchievementPosition.Golem => "LIHZAHRDIAN_IDOL",
                BossAchievementPosition.EoL => "DEFEAT_EMPRESS_OF_LIGHT",
                BossAchievementPosition.Marcian => "INDEPENDENCE_DAY",
                BossAchievementPosition.Fishron => "FISH_OUT_OF_WATER",
                BossAchievementPosition.Culthist => "OBSESSIVE_DEVOTION",
                BossAchievementPosition.MoonLord => "CHAMPION_OF_TERRARIA",
                BossAchievementPosition.AllBoss => "SLAYER_OF_WORLDS",
                _ => "THE_GREAT_SOUTHERN_PLANTKILL",
            };
        }
    }
}