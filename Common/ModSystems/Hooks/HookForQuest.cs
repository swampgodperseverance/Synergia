using Bismuth.Utilities.ModSupport;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Synergia.Common.ModSystems.Hooks
{
    public class HookForQuest : ModSystem
    {
        private Hook hook;
        private static bool npcChatFocusCustom;

        public static readonly Dictionary<int, QuestData> NpcQuestKeys = [];

        public override void Load()
        {
            var methodInfo = typeof(Main).GetMethod("DrawNPCChatButtons", BindingFlags.Static | BindingFlags.NonPublic, null, [typeof(int), typeof(Color), typeof(int), typeof(string), typeof(string)], null);
            hook = new Hook(methodInfo, DrawNPCChatButtonsPatch);
        }

        public override void Unload()
        {
            hook?.Undo();
            hook = null;
            NpcQuestKeys.Clear();
        }

        private void DrawNPCChatButtonsPatch(Action<int, Color, int, string, string> orig,int superColor, Color chatColor, int numLines, string focusText, string focusText3)          
        {
            orig(superColor, chatColor, numLines, focusText, focusText3);

            Player player = Main.LocalPlayer;
            NPC npc = Main.npc[player.talkNPC];

            if (npc == null || !npc.active) return;

            if (!NpcQuestKeys.TryGetValue(npc.type, out var questData))
                return;

            var quest = QuestRegistry.GetAvailableQuests(player, questData.QuestKey).FirstOrDefault();
            if (quest == null) { questData.Progres = 0; return; }

            float y = 130 + numLines * 30;
            int num = 180 + (Main.screenWidth - questData.X) / 2;
            string text = "DEBAG";
            if (questData.Progres < questData.MaxProgres) text = Language.GetTextValue("Mods.Synergia.Quest.BaseButton");
            else if (questData.Progres == questData.MaxProgres) text = quest?.GetButtonText(player);
            Vector2 vector3 = new(0.9f);
            Vector2 pos = new(num - 90, y);
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            Vector2 stringSize = ChatManager.GetStringSize(font, text, vector3);
            Vector2 vector4 = new(1f);
            if (stringSize.X > 260f) vector4.X *= 260f / stringSize.X;
            Vector2 mousePos = new(Main.mouseX, Main.mouseY);
            bool hover = mousePos.Between(pos, pos + stringSize * vector3 * vector4.X);
            float num2 = 1.2f;

            if (hover && !PlayerInput.IgnoreMouseInterface)
            {
                player.mouseInterface = true;
                player.releaseUseItem = false;
                vector3 *= num2;
                if (!npcChatFocusCustom) SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
                npcChatFocusCustom = true;
            }
            else
            {
                if (npcChatFocusCustom) SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
                npcChatFocusCustom = false;
            }

            Color baseColor = chatColor;
            Color shadowColor = npcChatFocusCustom ? Color.Brown : Color.Black;
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, pos + stringSize * vector4 * 0.5f, baseColor, shadowColor, 0f, stringSize * 0.5f, vector3 * vector4);

            if (hover && Main.mouseLeft && Main.mouseLeftRelease)
            {
                if (questData.Progres < questData.MaxProgres)
                {
                    Main.npcChatText = quest?.GetChat(npc, player) ?? "";
                    questData.Progres++;
                    NpcQuestKeys[npc.type] = questData;
                }
                else if (questData.Progres == questData.MaxProgres)
                {
                    quest?.OnChatButtonClicked(player);
                }

                SoundEngine.PlaySound(SoundID.MenuTick, npc.position);
            }
        }
    }
}