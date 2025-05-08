using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles;

namespace Vanilla.Common.GlobalNPCs
{
    public class PlanteraFlowerSpawner : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool spawnedFlower = false;

        public override void AI(NPC npc)
        {
            // Только для ванильной Плантеры
            if (npc.type == NPCID.Plantera)
            {
                // Проверка здоровья и флага
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