using Synergia.UIs;
using System;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;

namespace Synergia.Content.NPCs {
    [AutoloadHead]
    public class HellDwarf : ModNPC {
        static DwarfGUIChat dwarfChatUI;
        static bool draw;
        static bool uiOpened;

        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 5;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 550;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
        }
        public override void SetDefaults() {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 20;
            NPC.height = 35;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 20;
            NPC.lifeMax = 1000;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.GoblinTinkerer;
        }
        public override void SetChatButtons(ref string button, ref string button2) {
            button = Language.GetTextValue("LegacyInterface.28");
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName) {
            if (firstButton) {
                Main.playerInventory = true;
                Main.npcChatText = "dsadasdad";
            }
        }
        public override void AddShops() {
            NPCShop shop = new(Type, Name);
            shop.Add(1);
            shop.Register();
        }
        public override string GetChat() {
            return Language.GetTextValue("tModLoader.DefaultTownNPCChat");
        }
    }
    public class DwarfEmote : ModEmoteBubble {
        public override string Texture => (GetType().Namespace + "." + "Emote").Replace('.', '/');
        public override void SetStaticDefaults() {
            AddToCategory(EmoteID.Category.Town);
        }
    }
    class D : ModSystem {
        public override void Load() {
            On_Main.GUIChatDrawInner += On_Main_GUIChatDraw;
        }
        void On_Main_GUIChatDraw(On_Main.orig_GUIChatDrawInner orig, Main self) {
            Player player = Main.LocalPlayer;
            if (player.talkNPC < 0 || player.talkNPC >= Main.npc.Length)
                return;
            NPC npc = Main.npc[player.talkNPC];
            if (player.talkNPC < 0 && player.sign == -1) {
                Main.npcChatText = "";
                return;
            }
            if (npc.type == NPCType<HellDwarf>()) {
                GetInstance<Synergia>().DwarfChatInterface.SetState(new DwarfGUIChat());
            }
            else {
                orig(self);
            }
        }
    }
}