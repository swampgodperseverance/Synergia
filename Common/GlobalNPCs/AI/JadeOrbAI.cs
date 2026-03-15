using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Hostile;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class JadeOrbGlobalNPC : GlobalNPC
    {

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC?.Mod.Name == "ValhallaMod" && npc.ModNPC?.Name == "JadeOrb";

        public override Color? GetAlpha(NPC npc, Color lightColor) => Color.Lerp(Color.Turquoise with {A = 0}, Color.White, npc.Opacity) * npc.Opacity;

        public override void SetDefaults(NPC npc) => npc.alpha = 255;

        public override void AI(NPC npc) {
            npc.localAI[0] = 0f;
            if(npc.alpha > 0) npc.alpha -= 15;
        }

        public override void OnKill(NPC npc)  {
            if(Main.netMode != 1) for(int i = 0; i < 3; i++) Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Main.rand.NextVector2Circular(3f, 3f), ModContent.ProjectileType<MicroOrb>(), npc.damage / 4, 0f, Main.myPlayer);
        }
    }
}
