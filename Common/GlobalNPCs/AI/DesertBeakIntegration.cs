using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles.Hostile;

namespace Vanilla.Common.GlobalNPCs.AI
{
    public class DesertBeakIntegration : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int attackCounter = 0;
        private int sandstormCooldown = 0;
        private bool isPreparingSandstorm = false;
        private float originalScale = 1f;
        private Vector2 originalSize;

        public override void AI(NPC npc)
        {
            if (npc.type != ModLoader.GetMod("Avalon").Find<ModNPC>("DesertBeak").Type)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead) return;

            if (sandstormCooldown > 0) sandstormCooldown--;

            if (npc.life < npc.lifeMax * 0.4f && sandstormCooldown <= 0 && !isPreparingSandstorm)
            {
                StartSandstormPreparation(npc);
            }

            if (isPreparingSandstorm)
            {
                ExecuteSandstormAttack(npc, target);
            }
        }

        private void StartSandstormPreparation(NPC npc)
        {
            isPreparingSandstorm = true;
            originalScale = npc.scale;
            originalSize = npc.Size;

            npc.velocity *= 0.5f;
        }

        private void ExecuteSandstormAttack(NPC npc, Player target)
        {
            float riseSpeed = MathHelper.Lerp(0.5f, 2f, npc.ai[3] / 120f);
            npc.velocity.Y = -riseSpeed;
            npc.ai[3]++; 

            npc.scale = originalScale * (1 + npc.ai[3] / 240f);

            if (npc.ai[3] >= 120)
            {
                SpawnGiantSandstorm(npc, target);
                isPreparingSandstorm = false;
                npc.ai[3] = 0;
                npc.scale = originalScale;
                sandstormCooldown = 1600; 
            }
        }

        private void SpawnGiantSandstorm(NPC npc, Player target)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Vector2 spawnPosition = npc.Center + new Vector2(0, -100);

            int proj = Projectile.NewProjectile(
                npc.GetSource_FromAI(),
                spawnPosition,
                Vector2.Zero,
                ModContent.ProjectileType<GiantSandstorm>(),
                npc.damage,
                5f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.Item45 with { Pitch = -0.3f, Volume = 1.5f }, npc.position);

            for (int i = 0; i < 50; i++)
            {
                Dust.NewDustPerfect(
                    spawnPosition,
                    DustID.Sandstorm,
                    new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-10f, -5f)),
                    100,
                    default,
                    2.5f
                ).noGravity = true;
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (isPreparingSandstorm && npc.ai[3] > 0)
            {
                float progress = npc.ai[3] / 120f;
                Color energyColor = Color.Lerp(Color.SandyBrown, Color.Gold, progress);

                for (int i = 0; i < 3; i++)
                {
                    Vector2 offset = new Vector2(Main.rand.NextFloat(-20f, 20f) * npc.scale);
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.Sandnado,
                        Vector2.Zero,
                        0,
                        energyColor,
                        1.5f * progress
                    ).noGravity = true;
                }
            }
        }
    }
}
