using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Hostile;
using System.Linq;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class JadeOrbGlobalNPC : GlobalNPC
    {

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC?.Mod.Name == "ValhallaMod" && npc.ModNPC?.Name == "JadeOrb";

        public override Color? GetAlpha(NPC npc, Color lightColor) => Color.Lerp(Color.Turquoise with { A = 0 }, Color.White, npc.Opacity) * npc.Opacity;

        public override void SetDefaults(NPC npc) => npc.alpha = 255;

        public override void AI(NPC npc)
        {
            npc.localAI[0] = 0f;
            if (npc.alpha > 0) npc.alpha -= 15;

            // Added chheck if Emperor  exists in the world
            bool hasEmperor = Main.npc.Any(n => n.active && n.ModNPC?.Mod.Name == "ValhallaMod" && n.ModNPC?.Name == "Emperor");

            if (!hasEmperor)
            {
                npc.alpha += 10;
                if (npc.alpha >= 255)
                {
                    npc.active = false;
                    npc.life = 0;
                    npc.netUpdate = true;
                }
                return;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (Main.netMode != 1) for (int i = 0; i < 3; i++) Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Main.rand.NextVector2Circular(3f, 3f), ModContent.ProjectileType<MicroOrb>(), npc.damage / 4, 0f, Main.myPlayer);
        }
    }
}