using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace Synergia.Common.GlobalNPCs
{
    public class GolemChatMessage : GlobalNPC
    {
        private static bool hellishMessageShown = false;
        
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.Golem;

        public override void OnKill(NPC npc)
        {

            if (!hellishMessageShown)
            {
                hellishMessageShown = true;

                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(Language.GetTextValue("Mods.Synergia.GolemDefeat"), Color.OrangeRed);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(
                        NetworkText.FromLiteral(Language.GetTextValue("Mods.Synergia.GolemDefeat")),
                        Color.OrangeRed
                    );
                }
            }
        }
    }
}
