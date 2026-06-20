using Bismuth.Content.Buffs;
using Bismuth.Content.NPCs;
using Synergia.Common.Biome;
using Synergia.Content.Buffs;
using Synergia.Content.NPCs.Boss.SinlordWyrm;
using Synergia.Content.NPCs.Miniboss;
using Terraria;
using ValhallaMod.NPCs.Emperor;

namespace Synergia.Common {
    public class EditSound : ModSceneEffect {
        string song;
        public override int Music => MusicLoader.GetMusicSlot(GetSongByName2(song));
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override bool IsSceneEffectActive(Player player) {
            bool resolute = false;
            if (player == null || !player.active) { return false; }
            if (player.HasBuff<AuraOfEmpire>() || player.HasBuff<SnowVillageBuff>()) { song = "PeacefulTownV2"; resolute = true; }
            if (player.InModBiome<NewHell>()) { song = "InfernoFrontierSoundtrack"; resolute = true; }
            if (NPC.AnyNPCs(ModContent.NPCType<Sinlord>()))
            {
                song = "SinlordPresence";
                resolute = true;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<EvilNecromancer>()))
            {
                song = "Morrow";
                resolute = true;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<Minotaur>()))
            {
                song = "Morrow";
                resolute = true;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<EvilBabaYaga>()))
            {
                song = "Morrow";
                resolute = true;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<Cruor>()))
            {
                song = "Morrow";
                resolute = true;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<Emperor>()))
            {
                song = "JadePeace";
                resolute = true;
            }
            return resolute;
        }
    }
}