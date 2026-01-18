using MonoMod.Cil;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using DownedBossSystemConsolaria = Consolaria.Common.ModSystems.DownedBossSystem;

namespace Synergia.Common.ModSystems.Hooks.ILs {
    public class HookForVanillaBossProgress : ModSystem {
        public override void Load() {
            IL_CultistRitual.CheckRitual += EditRitual;
            IL_Player.ItemCheck_UseEventItems += EditEventItemIfUse;
            IL_Player.ItemCheck_CheckCanUse += CanUseEventItem;
        }
        void EditRitual(ILContext il) => EditGolemBoss(il);
        void EditEventItemIfUse(ILContext il) => EditGolemBoss(il);
        void CanUseEventItem(ILContext il) => EditGolemBoss(il);
        static void EditGolemBoss(ILContext il) {
            ILCursor c = new(il);
            c.GotoNext(i => i.MatchLdsfld(typeof(NPC).GetField("downedGolemBoss", BindingFlags.Static | BindingFlags.Public)));
            c.Remove();
            c.EmitDelegate(() => DownedBossSystemConsolaria.downedOcram);
        }
        public override void PostUpdateWorld() {
            foreach (NPC npc in Main.npc) {
                if (npc.type == NPCID.EmpressButterfly) {
                    if (npc.active && !NPC.downedGolemBoss) {
                        npc.active = false;
                    }
                }
            }
        }
        public override void Unload() {
            IL_CultistRitual.CheckRitual -= EditRitual;
            IL_Player.ItemCheck_UseEventItems -= EditEventItemIfUse;
            IL_Player.ItemCheck_CheckCanUse -= CanUseEventItem;
        }
    }
}