using Consolaria.Content.NPCs.Bosses.Ocram;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Hostile;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class ServantofOcramAIEdit : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        
        private int attackTimer = 0;
        private bool initialized = false;

        public override void PostAI(NPC npc)
        {
            if (npc.type == ModContent.NPCType<ServantofOcram>())
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
            else
            {
                initialized = false;
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