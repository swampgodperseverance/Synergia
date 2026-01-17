using Bismuth.Utilities.ModSupport;
using ReLogic.Content;
using ReLogic.Graphics;
using Synergia.Common;
using Synergia.Content.NPCs;
using Synergia.Dataset;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using static Synergia.Common.ModSystems.Hooks.Ons.HookForQuest;
using static Synergia.Common.SUtils.LocUtil;
using static Synergia.Reassures.Reassures;
using static Terraria.Main;
using static Synergia.Helpers.SynegiaHelper;
namespace Synergia.UIs {
    public class DwarfGUIChat : UIState {
        DrawChat drawChat;
        HellQuest hellQuest;

        const byte maxChar = 60;

        bool showUI = false;
        bool tickPlayed;
        bool npcChatFocusCustom;
        bool blockClickThisFrame;

        string text;
        string button;

        static bool isQuestButton;

        static Texture2D itemIcon;
        static Texture2D chain;
        readonly TextDisplayCache2 _cache = new();


        static Asset<Texture2D> GetAsset(string name) => Request<Texture2D>(GetUIElementName(name));
        public override void OnInitialize() {
            drawChat = new DrawChat(GetAsset("UIDwarfPanelBackground"), GetAsset("DwarfUIPanelBorder"));
            drawChat.Width.Set(500f, 0f);
            drawChat.Height.Set(120f, 0f);
            drawChat.Left.Set(-250f, 0.5f);
            drawChat.Top.Set(100f, 0f);

            Append(drawChat);
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            blockClickThisFrame = false;

            if (LocalPlayer.talkNPC == -1 ||
                npc[LocalPlayer.talkNPC].type != NPCType<HellDwarf>()) {
                TryGetQuest(LocalPlayer, out NPC npc, out QuestData questData, out IQuest _);
                isQuestButton = false;
                if (questData.Progress >= 1) {
                    questData.Progress--;
                    NpcQuestKeys[npc.type] = questData;
                }
                GetInstance<Synergia>().DwarfChatInterface.SetState(null);
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            RegisterTexture();
            drawChat.Height.Set(Pos(npcChatText), 0f);
            float a = Language.ActiveCulture.Name == "ru-RU" ? 80 : 0;
            drawChat.Width.Set(500f + a, 0f);
            drawChat.Recalculate();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            if (!showUI) { base.Draw(spriteBatch); }
            else { return; }

            Player player = LocalPlayer;

            string text = npcChatText;
            Color color = Color.AliceBlue;

            _cache.PrepareCache(text, color);

            List<List<TextSnippet>> lines = _cache.TextLines;
            int lineCount = _cache.AmountOfLines;

            string result = npcChatText;
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 basePos = new(170 + (screenWidth - 800) / 2, 125f);
            Vector2 vector3 = new(1f);

            Vector2 startPos = new(170 + (screenWidth - 800) / 2, 125f);

            for (int i = 0; i < lineCount; i++) {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, [.. lines[i]], startPos + new Vector2(0, i * 30), 0f, Color.AliceBlue, Color.Black, Vector2.Zero, Vector2.One, out _);
            }

            Color baseButtonColor = new(mouseTextColor, (int)(mouseTextColor / 1.1), mouseTextColor / 2, mouseTextColor);

            // Shop
            string shopButton = Language.GetTextValue("LegacyInterface.28");
            Vector2 stringSize = ChatManager.GetStringSize(font, shopButton, vector3);
            float scale = stringSize.X + ScaleForLanguage(Language.ActiveCulture.Name);
            Vector2 buttonPos = new(basePos.X + 12, basePos.Y + 40 + Pos(npcChatText, -25));

            // Reforge
            string reforgeButton = Language.GetTextValue("LegacyInterface.19");
            Vector2 reforgeButtonPos = new(buttonPos.X + scale, buttonPos.Y);

            // Quest
            string qustButton = Language.GetTextValue("Mods.Synergia.Quests.BaseButton");
            float scale2 = ScaleForLanguage2(Language.ActiveCulture.Name);
            Vector2 qustButtonPos = new(reforgeButtonPos.X + scale2, reforgeButtonPos.Y);
            Vector2 posIfQuest = isQuestButton ? buttonPos : qustButtonPos;
            DrawQuestButton(spriteBatch, font, posIfQuest, vector3, qustButtonPos, scale2, player);

            // Close
            string closeButton = Language.GetTextValue("LegacyInterface.52");
            float s = Language.ActiveCulture.Name switch { "de-DE" => -20, "fr-FR" => -20, "ru-RU" => -20, _ => 0, };
            float w = isQuestButton ? 25 : 0;
            Vector2 closeButtonPos = new(posIfQuest.X + scale + s + w, posIfQuest.Y);
            DrawButton(spriteBatch, font, closeButton, closeButtonPos, vector3, player, CloseWindow);

            // Happiness
            string happinessButton = Language.GetTextValue("UI.NPCCheckHappiness");
            Vector2 happinessButtonPos = new(closeButtonPos.X + scale, closeButtonPos.Y);
            if (!isQuestButton) {
                DrawButton(spriteBatch, font, shopButton, buttonPos, vector3, player, Shop);
                DrawButton(spriteBatch, font, reforgeButton, reforgeButtonPos, vector3, player, Reforge);
                DrawButton(spriteBatch, font, happinessButton, happinessButtonPos, vector3, player, Happiness);
            }
        }
        // TODO: FIX SOUNG
        void BaseLogicForHover(bool hover, Player player) {
            if (hover && !PlayerInput.IgnoreMouseInterface) {
                player.mouseInterface = true;
                player.releaseUseItem = false;
                if (tickPlayed) {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }
                tickPlayed = false;
                npcChatFocusCustom = true;
            }
            else {
                npcChatFocusCustom = false;
                tickPlayed = true;
            }
        }
        bool SpecialEvent(bool hover, bool release) {
            if (hover && release && !blockClickThisFrame) {
                blockClickThisFrame = true;
                return true;
            }
            return false;
        }
        void Shop() {
            NPC npc = LocalPlayer.TalkNPC;
            playerInventory = true;
            stackSplit = 9999;
            npcChatText = "";
            SetNPCShopIndex(1);
            instance.shop[npcShop].SetupShop(NPCShopDatabase.GetShopName(npc.type, nameof(HellDwarf)), npc);
            showUI = true;
            isQuestButton = false;
        }
        void Reforge() {
            playerInventory = true;
            npcChatText = "";
            GetInstance<Synergia>().DwarfUserInterface.SetState(new DwarfUI());
            showUI = true;
            isQuestButton = false;
        }
        void DrawQuestButton(SpriteBatch sb, DynamicSpriteFont font, Vector2 pos, Vector2 vector3, Vector2 qustButtonPos, float scale2, Player player) {
            if (isQuestButton) {
                DrawQuestItemIcon(sb, new Vector2(qustButtonPos.X - scale2 + 320, qustButtonPos.Y + 30));
                QuestButton(sb, font, pos, vector3, qustButtonPos, player);
            }
            else {
                DrawButton(sb, font, Language.GetTextValue("Mods.Synergia.Quests.BaseButton"), pos, vector3, player, Quest);
            }
        }
        void Quest() {
            Player player = LocalPlayer;
            TryGetQuest(player, out NPC npc, out QuestData questData, out IQuest quest);

            if (quest != null) {
                isQuestButton = true;
            }
            if (quest == null && !isQuestButton) {
                npcChatText = LocUIKey("DwarfChat", "NoQuest");
            }
        }
        static void DrawQuestItemIcon(SpriteBatch sb, Vector2 pos) {
            if (itemIcon != null && chain != null) {
                sb.Draw(chain, pos, Color.White);
                sb.Draw(itemIcon, new Vector2(pos.X, pos.Y + 37.9f), Color.White);
                int itemType = npcChatCornerItem;
                Item iItemType = new(itemType);
                Asset<Texture2D> item = TextureAssets.Item[npcChatCornerItem];
                Texture2D tex = item.Value;
                if (item != null) {
                    Vector2 mousePos = new(mouseX, mouseY);
                    Vector2 itemSlot = pos + new Vector2(28f, 60f);
                    Vector2 origin = tex.Size() / 2f;
                    float scale = 1f;
                    if (tex.Width > 32 || tex.Height > 32) {
                        scale = 32f / Math.Max(tex.Width, tex.Height);
                    }
                    sb.Draw(item.Value, itemSlot, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
                    if (mousePos.Between(new Vector2(itemSlot.X - 20, itemSlot.Y - 20), new Vector2(itemSlot.X + 20, itemSlot.Y + 10))) {
                        if (mouseLeftRelease && mouseLeft) {
                            if (!drawingPlayerChat) {
                                OpenPlayerChat();
                            }
                            if (ChatManager.AddChatText(FontAssets.MouseText.Value, ItemTagHandler.GenerateTag(iItemType), Vector2.One)) {
                                SoundEngine.PlaySound(SoundID.MenuOpen);
                            }
                        }
                        cursorOverride = 2;
                        instance.MouseText(iItemType.Name, -11, 0);
                    }
                }
            }
        }
        // TODO: FINAL TEXT NOT WORK
        void QuestButton(SpriteBatch sb, DynamicSpriteFont font, Vector2 pos, Vector2 vector3, Vector2 qustButtonPos, Player player) {
            string baseSay = LocUIKey("DwarfChat", "NoQuest");
            TryGetQuest(player, out NPC npc, out QuestData questData, out IQuest quest);
            hellQuest = new HellQuest(quest, player);
            string size = button ?? Language.GetTextValue("Mods.Synergia.Quests.BaseButton");
            Vector2 stringSize = ChatManager.GetStringSize(font, size, vector3);
            Vector2 vector4 = new(1f);
            Vector2 mousePos = new(mouseX, mouseY);
            if (stringSize.X > 260f) vector4.X *= 260f / stringSize.X;
            bool hover = mousePos.Between(pos, pos + stringSize * vector3 * vector4.X);
            bool release = mouseLeft && mouseLeftRelease;
            if (quest != null) {
                isQuestButton = true;
                int item = npcChatCornerItem;
                if (!quest.IsCompleted(player)) {
                    if (questData.IsFirstClicked) {
                        text = quest.GetChat(npc, player);
                        button = quest.GetButtonText(player, ref questData.IsFirstClicked);
                        if (SpecialEvent(hover, release)) {
                            quest.OnChatButtonClicked(player);
                            questData.IsFirstClicked = false;
                            NpcQuestKeys[npc.type] = questData;
                        }
                    }
                    else {
                        if (questData.Progress <= 0) {
                            text = quest.GetChat(npc, player);
                            button = quest.GetButtonText(player, ref questData.IsFirstClicked);
                            if (SpecialEvent(hover, release)) {
                                if (item != 0) {
                                    quest.OnChatButtonClicked(player);
                                    questData.Progress++;
                                    NpcQuestKeys[npc.type] = questData;
                                }
                            }
                        }
                        else {
                            text = hellQuest.QuestChat;
                            button = quest.GetButtonText(player, ref questData.IsFirstClicked);
                            if (SpecialEvent(hover, release)) {
                                quest.OnChatButtonClicked(player);
                            }
                        }
                    }
                }
                if (text != null && button != null) {
                    npcChatText = text;
                    DrawButton(sb, font, button, pos, vector3, player, Quest);
                }
            }
            else {
                npcChatText = baseSay;
                DrawButton(sb, font, Language.GetTextValue("Mods.Synergia.Quests.BaseButton"), pos, vector3, player, Quest);
            }
        }
        void CloseWindow() {
            npcChatText = "";
            playerInventory = false;
            LocalPlayer.SetTalkNPC(-1);
            showUI = false;
            isQuestButton = false;
        }
        void Happiness() {
            ShoppingSettings settings = Main.ShopHelper.GetShoppingSettings(LocalPlayer, npc[LocalPlayer.talkNPC]);
            npcChatText = settings.HappinessReport;
            isQuestButton = false;
        }
        static float Pos(string chat, float scale = 70) {
            int lineCount = (chat.Length + maxChar - 1) / maxChar;

            float lineHeight = FontAssets.MouseText.Value.LineSpacing;
            float padding = scale;

            float newHeight = lineCount * lineHeight + padding;
            return newHeight;
        }
        void DrawButton(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, Vector2 vector3, Player player, Action onClick) {
            Color baseButtonColor = new(mouseTextColor, (int)(mouseTextColor / 1.1), mouseTextColor / 2, mouseTextColor);
            Vector2 stringSize = ChatManager.GetStringSize(font, text, vector3);
            Vector2 vector4 = new(1f);
            Vector2 mousePos = new(mouseX, mouseY);
            if (stringSize.X > 260f) vector4.X *= 260f / stringSize.X;
            bool hover = mousePos.Between(pos, pos + stringSize * vector3 * vector4.X);
            bool release = mouseLeft && mouseLeftRelease;
            BaseLogicForHover(hover, player);
            Color shadowColor = npcChatFocusCustom ? Color.Brown : Color.Black;
            ChatManager.DrawColorCodedStringWithShadow(sb, font, text, pos, baseButtonColor, shadowColor, 0f, Vector2.Zero, vector3);
            if (SpecialEvent(hover, release)) {
                onClick?.Invoke();
            }
        }
        static void RegisterTexture() {
            itemIcon = Request<Texture2D>(GetUIElementName("HellsmithItemBg")).Value;
            chain = Request<Texture2D>(GetUIElementName("HellsmithChain")).Value;
        }
        static float ScaleForLanguage(string name) {
            float scale = name switch {
                "ru-RU" => 10f,
                "en-US" => 30f,
                _ => 10f,
            };
            return scale;
        }
        static float ScaleForLanguage2(string name) {
            float scale = name switch {
                "ru-RU" => 140f,
                "de-DE" => 130f,
                "en-US" => 80f,
                "it-IT" => 100f,
                "fr-FR" => 80f,
                "es-ES" => 80f,
                _ => 40
            };
            return scale;
        }
    }
}