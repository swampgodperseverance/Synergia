using Synergia.Content.NPCs;
using Synergia.Content.NPCs.Boss.SinlordWyrm;
using Terraria.Achievements;

namespace Synergia.Content.Achievements {
    public class CogwormAchieve : BaseAchieve {
        private AchievementCondition _target;
        public override AchievementCategory BaseCategory => AchievementCategory.Slayer;
        public override Position GetDefaultPosition() => new After(PositionName(BossAchievementPosition.Plantera));
        public override AchievementCondition Target => _target ??= AddNPCKilledCondition(NPCType<Sinlord>());
    }
}