using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;    
using Terraria.ID;
using Terraria.Audio;
using Vanilla.Content.Projectiles.Hostile;

namespace Vanilla.Common.GlobalNPCs.AI
{
    public class IfritGlobalNPC : GlobalNPC
    {
        private int teleportTimer = 0;
        private int teleportCount = 0;
        private bool isTeleporting = false;
        private Player target;

        public override bool InstancePerEntity => true;

        public override void AI(NPC npc)
        {
            // Check if this is the correct Ifrit from ValhallaMod
            if (npc.ModNPC == null || npc.ModNPC.Mod.Name != "ValhallaMod" || npc.ModNPC.Name != "Ifrit")
                return;

            target = Main.player[npc.target];

            // Trigger new attack phase randomly (~once every 13 seconds)
            if (!isTeleporting && Main.rand.NextBool(800))
            {
                isTeleporting = true;
                teleportTimer = 0;
                teleportCount = 0;
                npc.netUpdate = true;
            }

            if (isTeleporting)
            {
                teleportTimer++;
                if (teleportTimer >= 90) // 1.5 seconds between teleports
                {
                    teleportTimer = 0;
                    teleportCount++;

                    // Lava dust before teleport
                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava,
                            Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f),
                            100, default, 1.5f);
                    }

                    // Teleport sound
                    SoundEngine.PlaySound(SoundID.DD2_BetsySummon, npc.Center);

                    // Teleport to random point at fixed distance from player
                    float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                    float distance = 500f; // Distance from player
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
                    Vector2 newPos = target.Center + offset;
                    npc.Center = newPos;

                    // Lava dust after teleport
                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava,
                            Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f),
                            100, default, 1.5f);
                    }

                    // Shoot three IfritScythes in a spread pattern, slowly at first
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 baseDirection = target.Center - npc.Center;
                        baseDirection.Normalize();

                        float speed = 4f;
                        float spread = 15f * (MathHelper.Pi / 180f); // 15 degrees

                        for (int i = -1; i <= 1; i++)
                        {
                            Vector2 perturbedDirection = baseDirection.RotatedBy(i * spread) * speed;

                            int proj = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, perturbedDirection,
                                ModContent.ProjectileType<IfritScythe>(), 40, 1f, Main.myPlayer);

                            Main.projectile[proj].extraUpdates = 1; // Smooth motion, can help simulate acceleration
                        }
                    }

                    // End attack after 3 teleports
                    if (teleportCount >= 3)
                    {
                        isTeleporting = false;
                    }

                    npc.netUpdate = true;
                }
            }
        }
    }
}