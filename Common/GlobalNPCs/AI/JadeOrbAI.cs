using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Hostile;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class JadeOrbGlobalNPC : GlobalNPC
    {

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC?.Mod.Name == "ValhallaMod" && npc.ModNPC?.Name == "JadeOrb";

        public override void OnKill(NPC npc)
        {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);

                        Projectile.NewProjectile(
                            npc.GetSource_Death(),
                            npc.Center,
                            velocity,
                            ModContent.ProjectileType<MicroOrb>(),
                            npc.damage / 4,
                            0f,
                            Main.myPlayer
                        );
                    }
                }
        }
    }
}
