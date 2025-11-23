using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Common.GlobalNPCs.AI;
using Synergia.Content.Projectiles.Hostile.Bosses;

namespace Synergia.Common.GlobalNPCs
{
    public class RunicElementalTweaks : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int dustTimer = 0;
        private int projectileTimer = 0;

        private bool initialized = false;

        private const string TARGET_NAME = "RunicElemental";

        private bool IsTarget(NPC npc)
        {
            if (npc.ModNPC != null)
            {
                if (npc.ModNPC.Name == TARGET_NAME) return true;
                if ((npc.ModNPC.GetType().FullName ?? "").EndsWith(TARGET_NAME)) return true;
            }
            return false;
        }

        public override void AI(NPC npc)
        {
            if (!IsTarget(npc)) return;

            if (!PapuanWizardUpgrades.HardAIEnabled)
                return;

            Player player = Main.player[npc.target];
            if (player == null || !player.active)
                player = Main.LocalPlayer;

            dustTimer++;
            projectileTimer++;

            if (npc.aiStyle == -1 && !npc.friendly)
            {
                float baseSpeed = 1.8f;
                npc.velocity *= 1.22f; 
            }

            if (dustTimer >= 120) 
            {
                dustTimer = 0;
                npc.velocity = Vector2.Zero;

                for (int i = 0; i < 15; i++)
                {
                    Vector2 pos = npc.Center + new Vector2(Main.rand.Next(-24, 24), Main.rand.Next(-24, 24));
                    int dust = Dust.NewDust(pos, 6, 6, 169);
                    Main.dust[dust].velocity *= 0.2f;
                    Main.dust[dust].noGravity = true;
                }

                Vector2 spawnPos;
                if (Main.rand.NextBool()) 
                    spawnPos = npc.Center + new Vector2(-48, 0);
                else
                    spawnPos = npc.Center + new Vector2(48, 0);

                Vector2 direction = (player.Center - spawnPos).SafeNormalize(Vector2.Zero) * 10f;

                Projectile.NewProjectile(npc.GetSource_FromThis(), spawnPos, direction,
                    ModContent.ProjectileType<SpiritualDisc>(),
                    25, 4f, Main.myPlayer);
            }
        }
    }
}
