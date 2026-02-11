using Bismuth.Content.Buffs;
using Synergia.Content.Buffs;
using Terraria;

namespace Synergia.Common {
    public partial class EditSound {
        public class PeacefulTown : ModSceneEffect {
            public override int Music => MusicLoader.GetMusicSlot(GetSongByName2("PeacefulTownV2"));
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
            public override bool IsSceneEffectActive(Player player) {
                if (player == null || !player.active) {
                    return false;
                }
                return player.HasBuff<AuraOfEmpire>() || player.HasBuff<SnowVillageBuff>();
            }
        }
    }
}