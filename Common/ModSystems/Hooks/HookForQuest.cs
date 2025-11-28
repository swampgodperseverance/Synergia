using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using ReLogic.Graphics;
using Synergia.Dataset;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static Synergia.Common.QuestSystem;
using Carrot = StramsSurvival.Items.Foods.Carrot;

namespace Synergia.Common.ModSystems.Hooks
{
    public class HookForQuest : ModSystem {
        static bool npcChatFocusCustom;
        static bool npcNewTextButton;

        Hook hook;

        int lastTalkNPC = -1;

        readonly UIHelper helper = new();

        public static readonly Dictionary<int, QuestData> NpcQuestKeys = [];

        public override void Load() {
            MethodInfo methodInfo = typeof(Main).GetMethod("DrawNPCChatButtons", BindingFlags.Static | BindingFlags.NonPublic, null, [typeof(int), typeof(Color), typeof(int), typeof(string), typeof(string)], null);
            hook = new Hook(methodInfo, DrawNPCChatButtonsPatch);
        }
        public override void Unload() {
            hook?.Undo();
            hook = null;
        }
        void DrawNPCChatButtonsPatch(Action<int, Color, int, string, string> orig,int superColor, Color chatColor, int numLines, string focusText, string focusText3)          
        {
            orig(superColor, chatColor, numLines, focusText, focusText3);

            Player player = Main.LocalPlayer;
            NPC npc = Main.npc[player.talkNPC];

            NpcQuestKeys.TryGetValue(npc.type, out var questData);
            IQuest quest = QuestRegistry.GetAvailableQuests(player, questData.QuestKey).FirstOrDefault();

            if (npc == null || !npc.active || quest == null) {
                return;
            }

            float y = 130 + numLines * 30;
            int num = 180 + (Main.screenWidth - 800) / 2;

            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 scale = new(0.9f);

            Vector2 size1 = ChatManager.GetStringSize(font, focusText, scale);
            float x = num + size1.X + 30f;

            string text2 = Lang.inter[52].Value;
            Vector2 size2 = ChatManager.GetStringSize(font, text2, scale);
            x += size2.X + 30f;

            if (!string.IsNullOrWhiteSpace(focusText3)) {
                Vector2 size3 = ChatManager.GetStringSize(font, focusText3, scale);
                x += size3.X + 30f;
            }
            if (!Main.remixWorld || !NPCID.Sets.NoTownNPCHappiness[npc.type]) {
                string happy = Language.GetTextValue("UI.NPCCheckHappiness");
                Vector2 size4 = ChatManager.GetStringSize(font, happy, scale);
                x += size4.X + 30f;
            }

            string text;

            if (npcNewTextButton) {
                text = quest?.GetButtonText(player, ref questData.IsFirstClicked);
            }
            else {
                text = Language.GetTextValue($"Mods.Synergia.Quests.BaseButton");
            }

            Vector2 vector3 = new(0.9f);
            Vector2 pos = new(x - questData.X, y);
            Vector2 vanillaRightPos = new(Main.screenWidth / 2 + TextureAssets.ChatBack.Width() / 2, 100 + (numLines + 1) * 30 + 30);
            Vector2 posIcon = vanillaRightPos - new Vector2(57f, 25f);
            Vector2 stringSize = ChatManager.GetStringSize(font, text, vector3);
            Vector2 vector4 = new(1f);
            Vector2 mousePos = new(Main.mouseX, Main.mouseY);

            if (stringSize.X > 260f) vector4.X *= 260f / stringSize.X;

            bool hover = mousePos.Between(pos, pos + stringSize * vector3 * vector4.X);
            float num2 = 1.2f;

            if (hover && !PlayerInput.IgnoreMouseInterface) {
                player.mouseInterface = true;
                player.releaseUseItem = false;
                vector3 *= num2;
                if (!npcChatFocusCustom) SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
                npcChatFocusCustom = true;
            }
            else {
                if (npcChatFocusCustom) SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
                npcChatFocusCustom = false;
            }

            Color shadowColor = npcChatFocusCustom ? Color.Brown : Color.Black;

            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, pos + stringSize * vector4 * 0.5f, new Color(Main.mouseTextColor, (int)(Main.mouseTextColor / 1.1), Main.mouseTextColor / 2, Main.mouseTextColor), shadowColor, 0f, stringSize * 0.5f, vector3 * vector4);

            Vector2 fullPos = posIcon + stringSize * vector4 * 0.5f;

            if (questData.ItemList != null) {
                int itemId = helper.NoStaticGetNextItemType(questData.ItemList);
                Item item = new();
                item.SetDefaults(itemId);
                float num4 = 1f;
                Main.GetItemDrawFrame(item.type, out Texture2D itemTexture, out Rectangle rectangle2);
                itemTexture = TextureAssets.Item[item.type].Value;
                if (rectangle2.Width > 32 || rectangle2.Height > 32) {
                    num4 = ((rectangle2.Width <= rectangle2.Height) ? (32f / (float)rectangle2.Height) : (32f / (float)rectangle2.Width));
                }
                Main.spriteBatch.Draw(itemTexture, posIcon + stringSize * vector4 * 0.5f, rectangle2, Color.White, 0f, rectangle2.Size(), num4, SpriteEffects.None, 0f);
                if (mousePos.Between(new Vector2(fullPos.X - 35f, fullPos.Y - 30f), new Vector2(fullPos.X + 10f, fullPos.Y + 10f))) {
                    if (Main.mouseLeftRelease && Main.mouseLeft) {
                        if (!Main.drawingPlayerChat) {
                            Main.OpenPlayerChat();
                        }
                        if (ChatManager.AddChatText(FontAssets.MouseText.Value, ItemTagHandler.GenerateTag(item), Vector2.One)) {
                            SoundEngine.PlaySound(SoundID.MenuOpen);
                        }
                    }
                    Main.cursorOverride = 2;
                    Main.instance.MouseText(item.Name, -11, 0);
                }
            }
            if (hover && Main.mouseLeft && Main.mouseLeftRelease) {
                if (questData.Progress < questData.MaxProgress) {
                    Main.npcChatText = quest?.GetChat(npc, player) ?? "";
                    questData.Progress++;
                    NpcQuestKeys[npc.type] = questData;
                    npcNewTextButton = true;
                }
                else if (questData.Progress == questData.MaxProgress) {
                    questData.IsFirstClicked = false;
                    NpcQuestKeys[npc.type] = questData;
                    quest?.OnChatButtonClicked(player);
                }
                SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
            }
        }
        public override void PostUpdatePlayers()
        {
            Player player = Main.LocalPlayer;

            if (lastTalkNPC != -1 && player.talkNPC == -1)
            {
                if (Main.npc.IndexInRange(lastTalkNPC) && NpcQuestKeys.TryGetValue(Main.npc[lastTalkNPC].type, out var questData))
                {
                    questData.Progress = 0;
                    NpcQuestKeys[Main.npc[lastTalkNPC].type] = questData;
                    npcNewTextButton = false;
                }
            }

            lastTalkNPC = player.talkNPC;
        }
    }
}