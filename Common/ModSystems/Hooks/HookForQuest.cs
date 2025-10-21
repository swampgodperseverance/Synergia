using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Common.ModSystems.Hooks
{
    public abstract class HookForQuest : ModSystem
    {
        private Hook _hookSetChatButtons;
        private Hook _hookGetChat;

        public abstract Type CompileTime();

        public override void Load()
        {
            Type type = CompileTime();

            MethodInfo setChatButtonsMethod = type.GetMethod("SetChatButtons", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getChatMethod = type.GetMethod("GetChat", BindingFlags.Instance | BindingFlags.Public);

            if (setChatButtonsMethod != null) _hookSetChatButtons = new Hook(setChatButtonsMethod, (GetSetChatButtons)NewSetChatButtons);
            if (getChatMethod != null) _hookGetChat = new Hook(getChatMethod, (GetChat)NewGetChat);
        }

        public override void Unload()
        {
            _hookSetChatButtons?.Dispose();
            _hookSetChatButtons = null;

            _hookGetChat?.Dispose();
            _hookGetChat = null;
        }

        public delegate void Orig_SetChatButtons(Dwarf npc, ref string button, ref string button2);
        public delegate void GetSetChatButtons(Orig_SetChatButtons orig, Dwarf npc, ref string button, ref string button2);

        public delegate string Orig_GetChat(Dwarf npc);
        public delegate string GetChat(Orig_GetChat orig, Dwarf npc);

        public virtual void NewSetChatButtons(Orig_SetChatButtons orig, Dwarf npc, ref string button, ref string button2)
        {
            orig(npc, ref button, ref button2);
        }
        public virtual string NewGetChat(Orig_GetChat orig, Dwarf npc)
        {
            return orig(npc);
        }
    }
}