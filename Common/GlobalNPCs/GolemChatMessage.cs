using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace Synergia.GlobalNPCs
{
    public class GolemChatMessage : GlobalNPC
    {
        private static bool hellishMessageShown = false;

        public override void OnKill(NPC npc)
        {

            if (npc.type == NPCID.Golem && !hellishMessageShown)
            {
                hellishMessageShown = true;

                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText("Hellish creatures have grown stronger!", Color.OrangeRed);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(
                        NetworkText.FromLiteral("Hellish creatures have grown stronger!"),
                        Color.OrangeRed
                    );
                }
            }
        }
    }
}
