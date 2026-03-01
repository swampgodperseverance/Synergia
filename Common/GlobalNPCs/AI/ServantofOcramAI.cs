using Consolaria.Content.NPCs.Bosses.Ocram;
using Synergia.Content.Projectiles.Hostile;
using Terraria;
using Terraria.ModLoader.IO;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class ServantofOcramAIEdit : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        
        private int attackTimer = 0;
        private bool initialized = false;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == ModContent.NPCType<ServantofOcram>();

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode == 0) return;
            bitWriter.WriteBit(initialized);
            binaryWriter.Write(attackTimer);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode == 0) return;
            initialized = bitReader.ReadBit();
            attackTimer = binaryReader.ReadInt32();
        }

        public override void PostAI(NPC npc)
        {
            if (!initialized)
            {
                attackTimer = 0;
                initialized = true;
            }
                
            attackTimer++;

            if (attackTimer == 90)
            {
                LaunchScythe(npc);
                attackTimer = 0;
            }
        }

        private void LaunchScythe(NPC npc)
        {
            int targetIndex = npc.FindClosestPlayer();
            if (targetIndex == -1) return;
            
            Player target = Main.player[targetIndex];
            if (target == null || !target.active || target.dead) return;

            Vector2 direction = target.Center - npc.Center;
            direction.Normalize();

            float speed = 8f; 
            float scale = 0.6f;

            int proj = Projectile.NewProjectile(
                npc.GetSource_FromAI(),
                npc.Center,
                direction * speed,
                ModContent.ProjectileType<ServantScythe>(),
                npc.damage / 2, 
                0f,
                Main.myPlayer,
                ai0: scale); 

            if (Main.projectile[proj].ModProjectile is ServantScythe scythe)
            {

            }
        }
    }
}
