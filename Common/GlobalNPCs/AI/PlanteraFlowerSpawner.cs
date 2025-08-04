using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles.Hostile;

namespace Vanilla.Common.GlobalNPCs.AI
{
    public class PlanteraFlowerSpawner : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool spawnedFlower = false;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Plantera)
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
}