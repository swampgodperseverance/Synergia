using Bismuth.Utilities.ModSupport;
using ReLogic.Content;
using ReLogic.Graphics;
using Synergia.Common;
using Synergia.Content.NPCs;
using Synergia.Dataset;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using ValhallaMod.Items.Placeable;
using static Synergia.Common.ModSystems.Hooks.Ons.HookForQuest;
using static Synergia.Common.QuestSystem;
using static Synergia.Common.SUtils.LocUtil;
using static Synergia.Helpers.SynegiaHelper;
using static Terraria.Main;

namespace Synergia.UIs {
    public class DwarfGUIChat : UIState {
        DrawChat drawChat;
        HellQuest hellQuest;
        readonly TextDisplayCache2 _cache = new();

        const byte maxChar = 60;

        bool showUI = false;
        bool blockClickThisFrame;
        bool anyButtonHovered = false;

        string text;
        string button;

        static bool isQuestButton;

        static Texture2D itemIcon;
        static Texture2D chain;

        readonly Dictionary<string, bool> buttonHoverPrev = [];
        readonly Dictionary<string, bool> buttonHoverNow = [];

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

            if (LocalPlayer.talkNPC == -1 || npc[LocalPlayer.talkNPC].type != NPCType<HellDwarf>()) {
                TryGetQuest(LocalPlayer, out NPC npc, out QuestData questData, out IQuest _);
                isQuestButton = false;
                if (questData.Progress >= 1) {
                    questData.Progress--;
                    NpcQuestKeys[npc.type] = questData;
                }
                buttonHoverPrev.Clear();
                buttonHoverNow.Clear();
                GetInstance<Synergia>().DwarfChatInterface.SetState(null);
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            RegisterTexture();
            drawChat.Height.Set(Pos(), 0f);
            float a = Language.ActiveCulture.Name == "ru-RU" ? 25 : 0;
            drawChat.Width.Set(500f + a, 0f);
            drawChat.Recalculate();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            if (!showUI) { base.Draw(spriteBatch); }
            else { return; }

            buttonHoverNow.Clear();
            anyButtonHovered = false;

            Player player = LocalPlayer;
            _cache.PrepareCache(npcChatText, Color.AliceBlue);

            List<List<TextSnippet>> lines = _cache.TextLines;
            int lineCount = _cache.AmountOfLines;

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
            Vector2 buttonPos = new(basePos.X + 12, basePos.Y + Pos() - 60);

            // Reforge
            string reforgeButton = Language.GetTextValue("LegacyInterface.19");
            Vector2 reforgeButtonPos = new(buttonPos.X + scale, buttonPos.Y);

            // Quest
            string qustButton = Language.GetTextValue("Mods.Synergia.Quests.BaseButton");
            float scale2 = ScaleForLanguage2(Language.ActiveCulture.Name);
            float r = Language.ActiveCulture.Name == "pt-BR" ? 110 : 0;
            Vector2 qustButtonPos = new(reforgeButtonPos.X + scale2, reforgeButtonPos.Y);
            Vector2 posIfQuest = isQuestButton ? buttonPos : qustButtonPos;
            DrawQuestButton(spriteBatch, font, new (posIfQuest.X + r, posIfQuest.Y), vector3, qustButtonPos, scale2, player);

            // Close
            string closeButton = Language.GetTextValue("LegacyInterface.52");
            float s = Language.ActiveCulture.Name switch { "de-DE" => -20, "fr-FR" => -20, "ru-RU" => -20, _ => 0, };
            float w = isQuestButton ? 25 : 0;
            r = Language.ActiveCulture.Name == "pt-BR" ? r += 10 : r = 0;
            Vector2 closeButtonPos = new(posIfQuest.X + scale + s + w + r, posIfQuest.Y);
            DrawButton(spriteBatch, font, "Close", closeButton, closeButtonPos, vector3, player, CloseWindow);

            // Happiness
            string happinessButton = Language.GetTextValue("UI.NPCCheckHappiness");
            r = Language.ActiveCulture.Name == "pt-BR" ? r -= 100 : r = 0;
            Vector2 happinessButtonPos = new(closeButtonPos.X + scale + r, closeButtonPos.Y);
            if (!isQuestButton) {
                DrawButton(spriteBatch, font, "Shop", shopButton, buttonPos, vector3, player, Shop);
                DrawButton(spriteBatch, font, "Reforge", reforgeButton, reforgeButtonPos, vector3, player, Reforge);
                DrawButton(spriteBatch, font, "Happiness", happinessButton, happinessButtonPos, vector3, player, Happiness);
            }

            LocalPlayer.mouseInterface |= anyButtonHovered;

            buttonHoverPrev.Clear();
            foreach (var kv in buttonHoverNow) {
                buttonHoverPrev[kv.Key] = kv.Value;
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
            SoundEngine.PlaySound(SoundID.MenuOpen);
            showUI = true;
            isQuestButton = false;
        }
        void Reforge() {
            playerInventory = true;
            npcChatText = "";
            SoundEngine.PlaySound(SoundID.MenuOpen);
            GetInstance<Synergia>().DwarfReforgeInterface.SetState(new DwarfUI());
            showUI = true;
            isQuestButton = false;
        }
        void DrawQuestButton(SpriteBatch sb, DynamicSpriteFont font, Vector2 pos, Vector2 vector3, Vector2 qustButtonPos, float scale2, Player player) {
            if (isQuestButton) {
                DrawQuestItemIcon(sb, new Vector2(qustButtonPos.X - scale2 + 320, qustButtonPos.Y + 30));
                QuestButton(sb, font, pos, vector3, qustButtonPos, player);
            }
            else {
                DrawButton(sb, font, "BaseQuestButton", Language.GetTextValue("Mods.Synergia.Quests.BaseButton"), pos, vector3, player, Quest);
            }
        }
        void Quest() {
            Player player = LocalPlayer;
            TryGetQuest(player, out NPC npc, out QuestData questData, out IQuest quest);
            SoundEngine.PlaySound(SoundID.MenuTick);

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
                sb.Draw(itemIcon, new Vector2(pos.X, pos.Y + 40f), Color.White);
                Item iItemType = new(npcChatCornerItem);
                Asset<Texture2D> item = TextureAssets.Item[npcChatCornerItem];
                Texture2D tex = item.Value;
                if (item != null) {
                    Vector2 mousePos = new(mouseX, mouseY);
                    Vector2 itemSlot = pos + new Vector2(28f, 60f);
                    float scale = 1f;
                    if (tex.Width > 32 || tex.Height > 32) { scale = 32f / Math.Max(tex.Width, tex.Height); }
                    sb.Draw(item.Value, itemSlot, null, Color.White, 0f, tex.Size() / 2f, scale, SpriteEffects.None, 0);
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
        // TODO: IF QUEST COMPLITE AND CLICK TO BATTON NPC SAY NEXT "NoQuest";
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
            bool questFlag = player.GetModPlayer<QuestBoolean>().HellDwarfQuest;
            if (quest != null) {
                isQuestButton = true;
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
                                quest.OnChatButtonClicked(player);
                                questData.Progress++;
                                NpcQuestKeys[npc.type] = questData;
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
                    DrawButton(sb, font, "BaseQuestButton", button, pos, vector3, player, Quest);
                }
            }
            else {
                if (npcChatCornerItem != 0) {
                    QuestItem(ref npcChatCornerItem, ItemType<DwarvenAnvil>());
                }
                else {
                    text = baseSay;
                }
                npcChatText = text;
                button = Language.GetTextValue("Mods.Synergia.Quests.BaseButton");
                DrawButton(sb, font, "BaseQuestButton", button, pos, vector3, player, Quest);
            }
        }
        void QuestItem(ref int item, int itemType) {
            if (item == itemType) {
                text = hellQuest.GetName(item);
            }
        }
        void CloseWindow() {
            SoundEngine.PlaySound(SoundID.MenuClose);
            npcChatText = "";
            playerInventory = false;
            showUI = false;
            isQuestButton = false;
            LocalPlayer.SetTalkNPC(-1);
        }
        void Happiness() {
            SoundEngine.PlaySound(SoundID.MenuTick);
            ShoppingSettings settings = Main.ShopHelper.GetShoppingSettings(LocalPlayer, npc[LocalPlayer.talkNPC]);
            npcChatText = settings.HappinessReport;
            isQuestButton = false;
        }
        float Pos() {
            string text = npcChatText;
            Color color = Color.AliceBlue;
            _cache.PrepareCache(text, color);
            int lineCount = _cache.AmountOfLines;
            int Y = 40;
            for (int i = 0; i < lineCount; i++) { Y += 39; }
            if (lineCount <= 1) { Y += 20; }
            return Y;
        }
        void DrawButton(SpriteBatch sb, DynamicSpriteFont font, string id, string text, Vector2 pos, Vector2 scale, Player player, Action onClick) {
            Color baseButtonColor = new(mouseTextColor, (int)(mouseTextColor / 1.1), mouseTextColor / 2, mouseTextColor);
            Vector2 size = ChatManager.GetStringSize(font, text, scale);
            Vector2 mousePos = new(mouseX, mouseY);
            bool hover = mousePos.Between(pos, pos + size * scale);
            buttonHoverNow[id] = hover;
            anyButtonHovered |= hover;
            bool wasHover = buttonHoverPrev.TryGetValue(id, out var v) && v;
            if (hover && !wasHover) {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            float hoverScale = hover ? 1.2f : 1f;
            Color shadow = hover ? Color.Brown : Color.Black;
            ChatManager.DrawColorCodedStringWithShadow(sb, font, text, pos + size * 0.5f, baseButtonColor, shadow, 0f, size * 0.5f, scale * hoverScale);
            if (hover && mouseLeft && mouseLeftRelease && !blockClickThisFrame) {
                blockClickThisFrame = true;
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