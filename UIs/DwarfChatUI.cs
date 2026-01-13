using ReLogic.Content;
using ReLogic.Graphics;
using Synergia.Content.NPCs;
using System;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Synergia.UIs {
    public class DwarfGUIChat : UIState {
        DrawChat drawChat;

        const byte maxChar = 45;
        bool showUI = false;
        bool tickPlayed;
        bool npcChatFocusCustom;
        bool hover;

        static Asset<Texture2D> GetAsset(string name) => Request<Texture2D>(Reassures.Reassures.GetUIElementName(name));
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

            if (Main.LocalPlayer.talkNPC == -1 || Main.npc[Main.LocalPlayer.talkNPC].type != NPCType<HellDwarf>()) {
                GetInstance<Synergia>().DwarfChatInterface.SetState(null);
                return;
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            drawChat.Height.Set(Pos(Main.npcChatText), 0f);
            float a;
            if (Language.ActiveCulture.Name == "ru-RU") {
                a = 40;
            }
            else {
                a = 0;
            }
            drawChat.Width.Set(500f + a, 0f);
            drawChat.Recalculate();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            if (!showUI) { base.Draw(spriteBatch); }
            else { return; }

            Player player = Main.LocalPlayer;

            // NPC say
            StringBuilder sb = new();
            int visibleCount = 0;

            foreach (char c in Main.npcChatText) {
                sb.Append(c);

                if (c != ' ' || c != ',' || c != '.') 
                    visibleCount++;

                if (visibleCount >= maxChar) {
                    sb.AppendLine();
                    visibleCount = 0;
                }
            }

            //for (int i = 0; i < Main.npcChatText.Length; i += maxChar) {
            //    int len = Math.Min(maxChar, Main.npcChatText.Length - i);
            //    sb.AppendLine(Main.npcChatText.Substring(i, len));
            //}

            string result = sb.ToString().TrimEnd();
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 basePos = new(170 + (Main.screenWidth - 800) / 2, 125f);
            Vector2 vector3 = new(1f);

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, result, basePos, Color.AliceBlue, Color.Black, 0f, Vector2.Zero, vector3);

            Color baseButtonColor = new(Main.mouseTextColor, (int)(Main.mouseTextColor / 1.1), Main.mouseTextColor / 2, Main.mouseTextColor);

            // Shop
            string shopButton = Language.GetTextValue("LegacyInterface.28");
            Vector2 stringSize = ChatManager.GetStringSize(font, shopButton, vector3);
            float scale = stringSize.X + ScaleForLanguage(Language.ActiveCulture.Name);
            Vector2 buttonPos = new(basePos.X + 12, basePos.Y + 40 + Pos(Main.npcChatText, -25));
            DrawButton(spriteBatch, font, shopButton, buttonPos, vector3, player, Shop);

            // Reforge
            string reforgeButton = Language.GetTextValue("LegacyInterface.19");
            Vector2 reforgeButtonPos = new(buttonPos.X + scale, buttonPos.Y);
            DrawButton(spriteBatch, font, reforgeButton, reforgeButtonPos, vector3, player, Reforge);

            // Quest
            string qustButton = Language.GetTextValue($"Mods.Synergia.Quests.BaseButton");
            float scale2 = ScaleForLanguage2(Language.ActiveCulture.Name);
            Vector2 qustButtonPos = new(reforgeButtonPos.X + scale2, reforgeButtonPos.Y);
            DrawButton(spriteBatch, font, qustButton, qustButtonPos, vector3, player, Quest);

            // Close
            string closeButton = Language.GetTextValue("LegacyInterface.52");
            float s = Language.ActiveCulture.Name switch { "de-DE" => -20, "fr-FR" => -20, "ru-RU" => -20, _ => 0, };
            Vector2 closeButtonPos = new(qustButtonPos.X + scale + s, qustButtonPos.Y);
            DrawButton(spriteBatch, font, closeButton, closeButtonPos, vector3, player, CloseWindow);

            // Happiness
            string happinessButton = Language.GetTextValue("UI.NPCCheckHappiness");
            Vector2 happinessButtonPos = new(closeButtonPos.X + scale, closeButtonPos.Y);
            DrawButton(spriteBatch, font, happinessButton, happinessButtonPos, vector3, player, Happiness);
        }
        void BaseLogicForHover(bool hover, Player player) {
            this.hover = hover;
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
        bool SpecialEvent(bool hover, bool realis, bool showUI = true) {
            if (hover && realis) {
                this.showUI = showUI;
                return true;
            }
            else {
                return false;
            }
        }
        static void Shop() {
            NPC npc = Main.LocalPlayer.TalkNPC;
            Main.playerInventory = true;
            Main.stackSplit = 9999;
            Main.npcChatText = "";
            Main.SetNPCShopIndex(1);
            Main.instance.shop[Main.npcShop].SetupShop(NPCShopDatabase.GetShopName(npc.type, nameof(HellDwarf)), npc);
        }
        static void Reforge() {
            Main.playerInventory = true;
            Main.npcChatText = "";
            GetInstance<Synergia>().DwarfUserInterface.SetState(new DwarfUI());
        }
        static void Quest() {
            Main.npcChatText = "I not quest for you";
        }
        static void CloseWindow() {
            Main.npcChatText = "";
            Main.playerInventory = false;
            Main.LocalPlayer.SetTalkNPC(-1);
        }
        static void Happiness() {
            ShoppingSettings settings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, Main.npc[Main.LocalPlayer.talkNPC]);
            Main.npcChatText = settings.HappinessReport;
        }
        static float Pos(string chat, float scale = 70) {
            int lineCount = (chat.Length + maxChar - 1) / maxChar;

            float lineHeight = FontAssets.MouseText.Value.LineSpacing;
            float padding = scale;

            float newHeight = lineCount * lineHeight + padding;
            return newHeight;
        }
        void DrawButton(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, Vector2 vector3, Player player, Action onClick) {
            Color baseButtonColor = new(Main.mouseTextColor, (int)(Main.mouseTextColor / 1.1), Main.mouseTextColor / 2, Main.mouseTextColor);
            Vector2 stringSize = ChatManager.GetStringSize(font, text, vector3);
            Vector2 vector4 = new(1f);
            Vector2 mousePos = new(Main.mouseX, Main.mouseY);
            if (stringSize.X > 260f) vector4.X *= 260f / stringSize.X;
            bool hover = mousePos.Between(pos, pos + stringSize * vector3 * vector4.X);
            bool release = Main.mouseLeft && Main.mouseLeftRelease;
            BaseLogicForHover(hover, player);
            Color shadowColor = npcChatFocusCustom ? Color.Brown : Color.Black;
            ChatManager.DrawColorCodedStringWithShadow(sb, font, text, pos, baseButtonColor, shadowColor, 0f, Vector2.Zero, vector3);
            if (SpecialEvent(hover, release)) {
                onClick?.Invoke();
            }
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
    public class DrawChat(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4) : UIPanel(customBackground, customborder, customCornerSize, customBarSize) {
        public override void OnInitialize() {
            BackgroundColor = Color.White;
            BorderColor = Color.White;
        }
    }
}