using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class CultistAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        
        private int attackTimer;


        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode > 0) binaryWriter.Write(attackTimer);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode > 0) attackTimer = binaryReader.ReadInt32();
        }

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return npc.type == NPCID.CultistBoss;
        }

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.CultistBoss)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                npc.damage = (int)(npc.damage * 1.1f);
                npc.defense += 20;
            }
        }

        public override void AI(NPC npc)
        {
            attackTimer++;

            if (attackTimer >= 600)
            {
                attackTimer = 0;

                int numberProjectiles = 16;
                float rotationStep = MathHelper.TwoPi / numberProjectiles;

                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 velocity = rotationStep.ToRotationVector2() * 8f;
                    velocity = velocity.RotatedBy(i * rotationStep);

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        velocity,
                        ProjectileID.Fireball,  
                        40,                   
                        2f,
                        Main.myPlayer
                    );
                }
                npc.netUpdate = true;
            }
        }
    }
}
