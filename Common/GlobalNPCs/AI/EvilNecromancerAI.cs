using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Bismuth.Content.NPCs; 
using Synergia.Content.Projectiles.Hostile.Bosses; 
using Terraria.ID;

namespace Synergia.Common.GlobalNPCs.AIs
{
    public class NecromantAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool spawnedMirror = false; 

        public override void AI(NPC npc)
        {
            //
            if (npc.type != ModContent.NPCType<EvilNecromancer>())
                return;

            // 
            float hpPercent = (float)npc.life / npc.lifeMax;

            // Если хп меньше 35% и зеркала ещё не созданы
            if (hpPercent <= 0.35f && !spawnedMirror)
            {
                SpawnMirrors(npc);
                spawnedMirror = true;
            }

            if (hpPercent > 0.35f)
                spawnedMirror = false;
        }

        private void SpawnMirrors(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            int mirrorCount = 3;
            float radius = 160f;

            for (int i = 0; i < mirrorCount; i++)
            {
                float angle = MathHelper.TwoPi / mirrorCount * i;
                Vector2 offset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * radius;
                Vector2 spawnPos = npc.Center + offset;

                Projectile.NewProjectileDirect(
                    npc.GetSource_FromAI(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<NecroMirror>(),
                    0,
                    0f,
                    Main.myPlayer
                );
            }

            SoundEngine.PlaySound(SoundID.Item29, npc.Center);
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(npc.Center, DustID.ShadowbeamStaff, Main.rand.NextVector2Circular(5f, 5f), 150, default, 1.3f).noGravity = true;
            }
        }
    }
}
