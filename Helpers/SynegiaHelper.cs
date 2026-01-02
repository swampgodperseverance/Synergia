using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Synergia.Helpers {
    public partial class SynegiaHelper {
        #pragma warning disable CS8632
        #region Source
        public static IEntitySource GetSource(object? value) {
            if (value is null) {
                return Main.LocalPlayer.GetSource_FromThis();
            }
            return value switch {
                Item item => item.GetSource_FromThis(),
                NPC npc => npc.GetSource_FromThis(),
                Player player => player.GetSource_FromThis(),
                Projectile proj => proj.GetSource_FromThis(),
                _ => Main.LocalPlayer.GetSource_FromThis()
            };
        }
        #endregion
        public static void SpawnNPC(int posX, int posY, int npcType, IEntitySource? source = null, int start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int Target = 255) {
            IEntitySource s;
            if (source != null) { s = source; }
            else { s = Main.LocalPlayer.GetSource_FromThis(); }
	        if(!NPC.AnyNPCs(npcType)){
 		        NPC.NewNPC(s, posX, posY, npcType, start, ai0, ai1, ai2, ai3, Target);
            }
        }
        public static void EzSave<T>(TagCompound tag, string name, ref T type) => tag[name] = type;
        public static void EzLoad<T>(TagCompound tag, string name, ref T type) => type = tag.Get<T>(name);
    }
}