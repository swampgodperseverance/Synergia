using Bismuth.Content.Items.Accessories;
using Bismuth.Content.Items.Weapons.Magical;
using Bismuth.Content.Items.Weapons.Melee;
using Bismuth.Content.Items.Weapons.Ranged;
using Bismuth.Content.Items.Weapons.Throwing;
using Synergia.Common.Rarities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;
using ValhallaMod.Items.Weapons.Summon.Whips;

namespace Synergia.Common.GlobalPlayer {
    public class StartWeapons : ModPlayer {
        public int start = 0;
        public int questProgress = 0;

        public bool QuestComplite = false;

        public override void Initialize() {
            start = 0;
            questProgress = 0;
            QuestComplite = false;
        }
        public override void SaveData(TagCompound tag) {
            tag["StartW"] = start;
            tag["questProgress"] = questProgress;
            tag["QuestComplite"] = QuestComplite;
        }
        public override void LoadData(TagCompound tag) {
            start = tag.GetInt("StartW");
            questProgress = tag.GetInt("questProgress");
            QuestComplite = tag.GetBool("QuestComplite");
        }
    }
    public class MicroQuest : ModSystem {
        static bool npcChatFocusCustom;
        readonly string[] playerClassName = new string[6];
        readonly Color[] playerClassColor = new Color[6];

        int QuestProgress {
            get { return Main.LocalPlayer is null ? 0 : Main.LocalPlayer.GetModPlayer<StartWeapons>().questProgress; }
            set { Main.LocalPlayer.GetModPlayer<StartWeapons>().questProgress = value; }
        }

        readonly Dictionary<string, bool> buttonHoverPrev = [];
        readonly Dictionary<string, bool> buttonHoverNow = [];

        public override void Load() {
            On_Main.DrawNPCChatButtons += On_Main_DrawNPCChatButtons;
        }

        void On_Main_DrawNPCChatButtons(On_Main.orig_DrawNPCChatButtons orig, int superColor, Color chatColor, int numLines, string focusText, string focusText3) {
            Player player = Main.LocalPlayer;
            NPC npc = Main.npc[player.talkNPC];

            if (!Main.LocalPlayer.GetModPlayer<StartWeapons>().QuestComplite && npc.type == NPCType<Bismuth.Content.NPCs.ImperianCommander>()) {
                orig(superColor, chatColor, numLines, "", "");
                if (npc == null || !npc.active) { return; }

                buttonHoverNow.Clear();

                playerClassName[0] = LocKey(CategoryName.NPC, "ImperianCommander.Class.Name.Class0"); playerClassColor[0] = BaseRarity.AnimatedColor([Color.DarkRed, Color.Red], 90);
                playerClassName[1] = LocKey(CategoryName.NPC, "ImperianCommander.Class.Name.Class1"); playerClassColor[1] = BaseRarity.AnimatedColor([Color.DarkGreen, Color.Green], 90);
                playerClassName[2] = LocKey(CategoryName.NPC, "ImperianCommander.Class.Name.Class2"); playerClassColor[2] = BaseRarity.AnimatedColor([Color.MediumPurple, Color.Purple], 90);
                playerClassName[3] = LocKey(CategoryName.NPC, "ImperianCommander.Class.Name.Class3"); playerClassColor[3] = BaseRarity.AnimatedColor([Color.DarkBlue, Color.Blue], 90);
                playerClassName[4] = LocKey(CategoryName.NPC, "ImperianCommander.Class.Name.Class4"); playerClassColor[4] = BaseRarity.AnimatedColor([new(96, 225, 161), new(20, 179, 107)], 90);
                playerClassName[5] = LocKey(CategoryName.NPC, "ImperianCommander.Class.Name.Class5"); playerClassColor[5] = BaseRarity.AnimatedColor([Color.DarkOrange, Color.Orange], 90);

                int num = 180 + (Main.screenWidth - 800) / 2;
                float y = 130 + numLines * 30;
                float x = num + ChatManager.GetStringSize(FontAssets.MouseText.Value, focusText, new(0.9f)).X + 30f;
                x += ChatManager.GetStringSize(FontAssets.MouseText.Value, Lang.inter[52].Value, new(0.9f)).X + 30f;

                string text = "";

                Vector2 pos = new(x, y);
                Vector2 vector4 = new(1f);
                Vector2 mousePos = new(Main.mouseX, Main.mouseY);

                string chatText = "";
                int index = 0;
                switch (QuestProgress) {
                    case 0: DrawButton(LocKey(CategoryName.NPC, "ImperianCommander.Buttons.PreEp"), hovers: out _); break;
                    case 1: {
                        text = LocKey(CategoryName.NPC, "ImperianCommander.Buttons.EpText") + ": " + playerClassName[1];
                        DrawButton(text.Replace(playerClassName[1], ""), hovers: out _, action: NextWeapon);
                        index = player.GetModPlayer<StartWeapons>().start;
                        chatText = LocKey(CategoryName.NPC, "ImperianCommander.Say.PreEp");
                        string chatText2 = "";
                        switch (index) {
                            case 0: chatText2 = LocKey(CategoryName.NPC, "ImperianCommander.Class.Display.Class0"); break;
                            case 1: chatText2 = LocKey(CategoryName.NPC, "ImperianCommander.Class.Display.Class1"); break;
                            case 2: chatText2 = LocKey(CategoryName.NPC, "ImperianCommander.Class.Display.Class2"); break;
                            case 3: chatText2 = LocKey(CategoryName.NPC, "ImperianCommander.Class.Display.Class3"); break;
                            case 4: chatText2 = LocKey(CategoryName.NPC, "ImperianCommander.Class.Display.Class4"); break;
                            case 5: chatText2 = LocKey(CategoryName.NPC, "ImperianCommander.Class.Display.Class5"); break;
                        }
                        chatText += " " + chatText2;
                        x += ChatManager.GetStringSize(FontAssets.MouseText.Value, text, new(0.9f)).X;
                        x += -20;
                        pos = new(x, y);
                        DrawButton(playerClassName[index], action: () => { QuestProgress = 2; }, start: playerClassColor[index], end: Color.Black, hovers: out _);
                        break;
                    }
                    case 2: chatText = LocKey(CategoryName.NPC, "ImperianCommander.Say.PostEp"); ; break;
                }
                if (chatText != "") { Main.npcChatText = chatText; }
                void DrawButton(string text, out bool hovers, Action action = null, Color? start = null, Color? end = null) {
                    Vector2 vector3 = new(0.9f);
                    Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, vector3);
                    if (stringSize.X > 260f) { vector4.X *= 260f / stringSize.X; }
                    bool hover = mousePos.Between(pos, pos + stringSize * vector3 * vector4.X);
                    buttonHoverNow[text] = hover;
                    bool wasHover = buttonHoverPrev.TryGetValue(text, out bool v) && v;
                    hovers = false;
                    if (hover && !PlayerInput.IgnoreMouseInterface) {
                        player.mouseInterface = true;
                        player.releaseUseItem = false;
                        vector3 *= 1.2f;
                        if (Main.mouseLeft && Main.mouseLeftRelease) {
                            QuestProgress = 1;
                            QuestProgress.ToString();
                            action?.Invoke();
                        }
                        npcChatFocusCustom = true;
                    }
                    if (hover && !wasHover && npcChatFocusCustom) {
                        SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
                        npcChatFocusCustom = true;
                    }
                    if (!hover && wasHover && npcChatFocusCustom) {
                        SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
                        npcChatFocusCustom = false;
                    }
                    if (!hover) { npcChatFocusCustom = false; }
                    hovers = npcChatFocusCustom;
                    start ??= new Color(Main.mouseTextColor, (int)(Main.mouseTextColor / 1.1), Main.mouseTextColor / 2, Main.mouseTextColor);
                    end ??= npcChatFocusCustom ? Color.Brown : Color.Black;
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, text, new Vector2(pos.X, pos.Y) + stringSize * vector4 * 0.5f, (Color)start, (Color)end, 0f, stringSize * 0.5f, vector3 * vector4); 
                }
                void NextWeapon() {
                    if (player.GetModPlayer<StartWeapons>().start + 1 < 6) { player.GetModPlayer<StartWeapons>().start++; }
                    else { player.GetModPlayer<StartWeapons>().start = 0; }
                }
                buttonHoverPrev.Clear();
                foreach (KeyValuePair<string, bool> kv in buttonHoverNow) { buttonHoverPrev[kv.Key] = kv.Value; }
            }
            else { orig(superColor, chatColor, numLines, focusText, focusText3); }
        }
        public override void PostUpdatePlayers() {
            int type = 0;

            if (QuestProgress < 2 && Main.LocalPlayer.talkNPC == -1) { QuestProgress = 0; }
            if (QuestProgress == 2) {
                switch (Main.LocalPlayer.GetModPlayer<StartWeapons>().start) {
                    case 0: type = ItemType<Gladius>(); break;
                    case 1: type = ItemType<WoodenCrossbow>(); break;
                    case 2: type = ItemType<WoodenStaff>(); break;
                    case 3: type = ItemType<BambooWhip>(); break;
                    case 4: type = ItemType<Lancea>(); break;
                    case 5: type = ModList.Roa.Find<ModItem>("PastoralRod").Type; break;
                }
            }
            if (Main.LocalPlayer.talkNPC == -1 && type != 0 && !Main.LocalPlayer.GetModPlayer<StartWeapons>().QuestComplite) {
                SpawItem();
                if (type == ItemType<Gladius>()) { type = ItemType<Scutum>(); SpawItem(); }
                Main.LocalPlayer.GetModPlayer<StartWeapons>().QuestComplite = true;
            }
            void SpawItem() => Item.NewItem(Main.LocalPlayer.GetSource_FromThis(), Main.LocalPlayer.position, type, 1, false, Main.rand.Next(0, 10));
        }
    }
}