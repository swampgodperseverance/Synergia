using Synergia.Content.NPCs;
using Synergia.Helpers;
using Synergia.UIs;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForDwarfChat : ModSystem {
        DwarfGUIChat chat;

        public override void Load() {
            On_Main.GUIChatDrawInner += NewDwarfChat;
        }
        void NewDwarfChat(On_Main.orig_GUIChatDrawInner orig, Main self) {
            Player player = Main.LocalPlayer;
            SynegiaHelper.TryGetTalkNPC(player, out NPC npc);
            if (npc == null) {
                return;
            }
            if (npc.type == NPCType<HellDwarf>()) {
                if (chat == null) {
                    chat = new DwarfGUIChat();
                    GetInstance<Synergia>().DwarfChatInterface.SetState(chat);
                }
            }
            else {
                orig(self);
            }
        }
        public override void PostUpdatePlayers() {
            Player player = Main.LocalPlayer;
            if (player.talkNPC == -1 || Main.npc[player.talkNPC].type != NPCType<HellDwarf>()) {
                chat = null;
            }
        }
    }
}