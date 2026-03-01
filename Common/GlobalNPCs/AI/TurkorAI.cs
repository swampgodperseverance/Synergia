using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Synergia.Common.GlobalNPCs
{
    public class TurkorFeatherAI : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == ModContent.NPCType<Consolaria.Content.NPCs.Bosses.Turkor.TurkortheUngrateful>();

        public override void AI(NPC npc)
        {
            Player target = Main.player[npc.target];
            npc.localAI[0] += 1f;
            if (npc.localAI[1] > 0)
                npc.localAI[1]--;
            bool wasInAir = npc.localAI[2] == 1f;
            bool isInAir = npc.velocity.Y != 0 || !npc.collideY;
            if (npc.localAI[0] > 180 && npc.localAI[1] == 0)
            {
                if (wasInAir && !isInAir)
                {
                    npc.localAI[1] = 120; 
                    FireFeathersUnderPlayer(target);
                }
            }

            npc.localAI[2] = isInAir ? 1f : 0f;
        }

        private void FireFeathersUnderPlayer(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 screenBottom = player.Center + new Vector2(0f, Main.screenHeight / 2f + 60f);

            for (int i = 0; i < 8; i++)
            {
                Vector2 spawnPos = screenBottom + new Vector2(Main.rand.Next(-800, 800), 0); 
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-29f, -12f)); 

                Projectile.NewProjectile(
                    player.GetSource_FromThis(),
                    spawnPos,
                    velocity,
                    ModContent.ProjectileType<Consolaria.Content.Projectiles.Enemies.TurkorFeather>(),
                    25,
                    0f,
                    Main.myPlayer);
            }

            SoundEngine.PlaySound(SoundID.Item42, player.Center);
        }
    }
}
