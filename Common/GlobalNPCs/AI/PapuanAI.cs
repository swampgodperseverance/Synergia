using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Hostile.Bosses;

namespace Synergia.Common.GlobalNPCs
{
    public class PapuanWizardGlobal : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int magnetTimer = 0;
        private int nextMagnetTime = 0;

        public override void AI(NPC npc)
        {
            if (npc.ModNPC == null || npc.ModNPC.Mod.Name != "Bismuth" || npc.ModNPC.Name != "PapuanWizard")
                return;

            if (nextMagnetTime == 0)
                nextMagnetTime = Main.rand.Next(960, 1320); 

            magnetTimer++;

            if (magnetTimer >= nextMagnetTime)
            {
                magnetTimer = 0;
                nextMagnetTime = Main.rand.Next(960, 1320); 
                SpawnSandMagnet(npc);
            }
        }

        private void SpawnSandMagnet(NPC npc)
        {
            var source = npc.GetSource_FromAI();
            Vector2 spawnPos = npc.Center + new Vector2(0f, -180f);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int proj = Projectile.NewProjectile(
                    source,
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<SandMagnet>(),
                    0, 
                    0f,
                    Main.myPlayer
                );

                if (proj != Main.maxProjectiles && proj >= 0)
                {
                    Main.projectile[proj].timeLeft = 300; 
                    Main.projectile[proj].netUpdate = true;
                }
            }


            for (int i = 0; i < 25; i++)
            {
                int dust = Dust.NewDust(spawnPos, 20, 20, DustID.Sandnado, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
                Main.dust[dust].noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20, npc.Center);
        }
    }
}
