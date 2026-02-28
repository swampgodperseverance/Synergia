using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System;    
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures; 
using Synergia.Content.Projectiles.Hostile;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class IfritGlobalNPC : GlobalNPC
    {
        private int teleportTimer = 0;
        private int teleportCount = 0;
        private bool isTeleporting = false;
        private Player target;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC?.Mod.Name == "ValhallaMod" && npc.ModNPC?.Name == "Ifrit";

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode == 0) return;
            bitWriter.WriteBit(isTeleporting);
            binaryWriter.Write(teleportTimer);
            binaryWriter.Write(teleportCount);
            binaryWriter.Write(target?.whoAmI ?? -1);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode == 0) return;
            isTeleporting = bitReader.ReadBit();
            teleportTimer = binaryReader.ReadInt32();
            teleportCount = binaryReader.ReadInt32();
            int playerId = binaryReader.ReadInt32();
            target = playerId == -1 ? null : Main.player[playerId];
        }

        public override bool InstancePerEntity => true;

        public override void AI(NPC npc)
        {

            target = Main.player[npc.target];

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

                int teleportInterval = NPC.downedGolemBoss ? 60 : 90;

                if (teleportTimer >= teleportInterval)
                {
                    teleportTimer = 0;
                    teleportCount++;

                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava,
                            Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f),
                            100, default, 1.5f);
                    }

                    SoundEngine.PlaySound(SoundID.DD2_BetsySummon, npc.Center);

                    float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                    float distance = 500f;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
                    Vector2 newPos = target.Center + offset;
                    npc.Center = newPos;

                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava,
                            Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f),
                            100, default, 1.5f);
                    }

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 baseDirection = target.Center - npc.Center;
                        baseDirection.Normalize();

                        float speed = 4f;
                        float spread = 15f * (MathHelper.Pi / 180f); 

                        int projectileCount = NPC.downedGolemBoss ? 5 : 3;
                        int start = -(projectileCount / 2);

                        for (int i = start; i < start + projectileCount; i++)
                        {
                            Vector2 perturbedDirection = baseDirection.RotatedBy(i * spread) * speed;

                            int proj = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, perturbedDirection,
                                ModContent.ProjectileType<IfritScythe>(), 40, 1f, Main.myPlayer);

                            Main.projectile[proj].extraUpdates = 1;
                        }
                    }

                    if (teleportCount >= 3)
                    {
                        isTeleporting = false;
                    }

                    npc.netUpdate = true;
                }
            }
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {

            if (NPC.downedGolemBoss)
            {
                npc.lifeMax += 25000;
                npc.life = npc.lifeMax;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            int ifritType = ModContent.NPCType<ValhallaMod.NPCs.Underworld.Ifrit>();
            if (pool.ContainsKey(ifritType))
            {
                pool[ifritType] *= 1.18f;
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            ItemDropRule.ByCondition(new SynergiaCondition.PostGolem(), ModContent.ItemType<ValhallaMod.Items.Material.EvilIngot>(), 1, 1, 1);
        }
    }
}
