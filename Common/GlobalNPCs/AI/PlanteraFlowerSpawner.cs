using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Synergia.Content.Projectiles.Hostile;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class PlanteraFlowerSpawner : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool spawnedFlower = false;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.Plantera;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode > 0) bitWriter.WriteBit(spawnedFlower);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode > 0) spawnedFlower = bitReader.ReadBit();
        }
        public override void AI(NPC npc)
        {
                if (!spawnedFlower && npc.life < npc.lifeMax / 2)
                {
                    spawnedFlower = true;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<PlanteraFlower>(),
                            0,
                            0f,
                            Main.myPlayer);
                    }
                }
        }
    }
}
