using System.Collections.Generic;
using Terraria;
using Terraria.Localization;

namespace Synergia.Helpers {
    public class EventHelper {
        public static void GetProgress(NPC npc, List<int> necessaryNPC, ref int eventProgress, int eventPoint) {
            if (necessaryNPC.Contains(npc.type)) {
                eventProgress += eventPoint;
            }
        }
        public static void SpawnNPC(ref IDictionary<int, float> pool, int npcType, float chance) {
            pool.Add(npcType, chance);
        }
        public static void TextWave(int wave, params int[] npcID) {
            NetworkText text = GetInvasionWaveText(wave, npcID);
            Main.NewText(text);
        }
        public static NetworkText GetInvasionWaveText(int wave, params int[] npcIds) {
            NetworkText[] array = new NetworkText[npcIds.Length + 1];
            for (int i = 0; i < npcIds.Length; i++) {
                array[i + 1] = NetworkText.FromKey(Lang.GetNPCName(npcIds[i]).Key);
            }

            array[0] = wave switch {
                -1 => NetworkText.FromKey("Game.FinalWave"),
                0 => NetworkText.FromKey("Game.FirstWave"),
                _ => NetworkText.FromKey("Game.Wave", wave),
            };
            string key = "Game.InvasionWave_Type" + npcIds.Length;
            object[] substitutions = array;
            return NetworkText.FromKey(key, substitutions);
        }
    }
}
