using Synergia.Content.NPCs;
using Terraria.Achievements;

namespace Synergia.Content.Achievements {
    public class CogwormAchieve : BaseAchieve {
        public override AchievementCategory BaseCategory => AchievementCategory.Slayer;
        public override Position GetDefaultPosition() => new After(PositionName(BossAchievementPosition.Plantera));
        public override AchievementCondition Target => AddNPCKilledCondition(NPCType<Cogworm>());
    }
}