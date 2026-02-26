using Bismuth.Content.Buffs;
using Synergia.Common.Biome;
using Synergia.Content.Buffs;
using Terraria;

namespace Synergia.Common {
    public partial class EditSound {
        public class PeacefulTown : ModSceneEffect {
            string song;
            public override int Music => MusicLoader.GetMusicSlot(GetSongByName2(song));
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
            public override bool IsSceneEffectActive(Player player) {
                bool resolute = false;
                if (player == null || !player.active) { return false; }
                if (player.HasBuff<AuraOfEmpire>() || player.HasBuff<SnowVillageBuff>()) { song = "PeacefulTownV2"; resolute = true; }
                if (player.InModBiome<NewHell>()) { song = "InfernoFrontierSoundtrack"; resolute = true; }
                return resolute;
            }
        }
    }
}