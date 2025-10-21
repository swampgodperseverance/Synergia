using Synergia.Common.GlobalPlayer;
using Synergia.Content.Items;
using Synergia.Content.Projectiles.Other;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs
{
    public class Transformation : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                Player player;

                if (npc.lastInteraction >= 0 && npc.lastInteraction < Main.maxPlayers) player = Main.player[npc.lastInteraction];
                else player = Main.LocalPlayer;

                PlayerHelpers.StartTransforms(player, npc, ModContent.ItemType<OldTales>(), 1, ref player.GetModPlayer<BookPlayerHardMode>().IsHardModePage, ModContent.ProjectileType<BookProj>());
            }
        }
    }
}