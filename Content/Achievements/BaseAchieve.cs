using Terraria.Achievements;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.Content.Achievements
{
    public abstract class BaseAchieve : ModAchievement
    {
        public abstract string BaseTexture { get; }
        public abstract string BaseName { get; }
        public abstract AchievementCategory BaseCategory { get; }
        public override string TextureName => $"Synergia/Assets/Textures/Achievements/{BaseTexture}";
        public override LocalizedText FriendlyName => Language.GetText($"Mods.Synergia.Achieve.Name.{BaseName}");
        public override LocalizedText Description => Language.GetText($"Mods.Synergia.Achieve.Description.{BaseName}");
        public override void SetStaticDefaults() { Achievement.SetCategory(BaseCategory); }
        public enum BossAchivePositon
        {
            Slime,
            EoC,
            Goblin,
            BoC,
            Bea,
            Skeletron,
            Deerclops,
            WoF,
            Pirat,
            QueenSlime,
            AllMechanic,
            Plantera,
            PumpKin,
            Golem,
            EoL,
            Fishron,
            Marcian,
            Culthist,
            MoonLord,
            AllBoss
        }
        public static string PositionName(BossAchivePositon achivePositon)
        {
            return achivePositon switch
            {
                BossAchivePositon.Slime => "SLIPPERY_SHINOBI",
                BossAchivePositon.EoC => "EYE_ON_YOU",
                BossAchivePositon.Goblin => "GOBLIN_PUNTER",
                BossAchivePositon.BoC => "MASTERMIND",
                BossAchivePositon.Bea => "STING_OPERATION",
                BossAchivePositon.Skeletron => "BONED",
                BossAchivePositon.Deerclops => "DEFEAT_DEERCLOPS",
                BossAchivePositon.WoF => "STILL_HUNGRY",
                BossAchivePositon.Pirat => "DAVY_JONES_LOCKER",
                BossAchivePositon.QueenSlime => "DEFEAT_QUEEN_SLIME",
                BossAchivePositon.AllMechanic => "BUCKETS_OF_BOLTS",
                BossAchivePositon.Plantera => "THE_GREAT_SOUTHERN_PLANTKILL",
                BossAchivePositon.PumpKin => "PUMPKIN_SMASHER",
                BossAchivePositon.Golem => "LIHZAHRDIAN_IDOL",
                BossAchivePositon.EoL => "DEFEAT_EMPRESS_OF_LIGHT",
                BossAchivePositon.Marcian => "INDEPENDENCE_DAY",
                BossAchivePositon.Fishron => "FISH_OUT_OF_WATER",
                BossAchivePositon.Culthist => "OBSESSIVE_DEVOTION",
                BossAchivePositon.MoonLord => "CHAMPION_OF_TERRARIA",
                BossAchivePositon.AllBoss => "SLAYER_OF_WORLDS",
                _ => "THE_GREAT_SOUTHERN_PLANTKILL",
            };
        }
    }
}