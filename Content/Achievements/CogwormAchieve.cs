using Synergia.Content.NPCs;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace Synergia.Content.Achievements
{
    public class CogwormAchieve : BaseAchieve
    {
        public override string BaseTexture => "CogwormPNG";
        public override string BaseName => "Cogworm";
        public override AchievementCategory BaseCategory => AchievementCategory.Slayer;
        public override Position GetDefaultPosition() { return new After(PositionName(BossAchivePositon.Plantera)); }
        public override void SetStaticDefaults()
        {
            AddNPCKilledCondition(ModContent.NPCType<Cogworm>());
            base.SetStaticDefaults();
        }
    }
}