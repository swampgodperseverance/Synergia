using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class CultistAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        
        private int attackTimer;

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
            if (npc.type != NPCID.CultistBoss)
                return;

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
            }
        }
    }
}