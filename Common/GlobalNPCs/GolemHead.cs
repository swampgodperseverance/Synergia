using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Vanilla.Content.Projectiles;

namespace Vanilla.Common.GlobalNPCs
{
    public class GolemHead : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool phaseTwoStarted = false;
        private int spawnTimer = -1;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Golem && npc.life < npc.lifeMax / 2)
            {
                if (!phaseTwoStarted)
                {
                    phaseTwoStarted = true;
                    spawnTimer = 0;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center + new Vector2(0f, -60f),
                            Vector2.Zero,
                            ModContent.ProjectileType<StoneMark>(),
                            0,
                            0f,
                            Main.myPlayer
                        );
                    }
                }

                if (spawnTimer >= 0)
                {
                    spawnTimer++;
                    if (spawnTimer == 120)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 spawnPos = npc.Center + new Vector2(Main.rand.Next(-100, 100), -50);
                                NPC.NewNPC(
                                    npc.GetSource_FromAI(),
                                    (int)spawnPos.X,
                                    (int)spawnPos.Y,
                                    ModContent.NPCType<ValhallaMod.NPCs.Jungle.TempleGolem>()
                                );
                            }
                        }

                        spawnTimer = -1; // disable timer
                    }
                }
            }
        }
    }
}