using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using Synergia.Content.Projectiles.Hostile;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class DesertBeakIntegration : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int sandstormCooldown = 0;
        private bool isPreparingSandstorm = false;

        // Feather Volley
        private bool isPreparingFeatherVolley = false;
        private int featherVolleyTimer = 0;
        private int featherVolleySide = 0;
        //private Vector2 featherVolleyDir;
        private Vector2 featherSpawnPos;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == ModLoader.GetMod("Avalon").Find<ModNPC>("DesertBeak").Type;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode == 0) return;
            bitWriter.WriteBit(isPreparingSandstorm);
            bitWriter.WriteBit(isPreparingFeatherVolley);
            binaryWriter.Write(sandstormCooldown);
            binaryWriter.Write(featherVolleyTimer);
            binaryWriter.Write(featherVolleySide);
            binaryWriter.WriteVector2(featherSpawnPos);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode == 0) return;
            isPreparingSandstorm = bitReader.ReadBit();
            isPreparingFeatherVolley = bitReader.ReadBit();
            sandstormCooldown = binaryReader.ReadInt32();
            featherVolleyTimer = binaryReader.ReadInt32();
            featherVolleySide = binaryReader.ReadInt32();
            featherSpawnPos = binaryReader.ReadVector2();
        }

        public override void AI(NPC npc)
        {

            Player target = Main.player[npc.target];
            if (!target.active || target.dead) return;

            if (sandstormCooldown > 0) sandstormCooldown--;

            if (npc.life < npc.lifeMax * 0.4f && sandstormCooldown <= 0 && !isPreparingSandstorm && !isPreparingFeatherVolley)
            {
                StartSandstormPreparation(npc);
            }

            if (isPreparingSandstorm)
                ExecuteSandstormAttack(npc, target);

            if (!isPreparingSandstorm && !isPreparingFeatherVolley && Main.rand.NextBool(600))
            {
                StartFeatherVolley(npc, target);
            }

            if (isPreparingFeatherVolley)
                ExecuteFeatherVolley(npc, target);
        }

        //Sandstorm

        private void StartSandstormPreparation(NPC npc)
        {
            isPreparingSandstorm = true;
            npc.velocity *= 0.5f;
        }

        private void ExecuteSandstormAttack(NPC npc, Player target)
        {
            npc.ai[3]++;
            float riseSpeed = MathHelper.Lerp(0.5f, 2f, npc.ai[3] / 120f);
            npc.velocity.Y = -riseSpeed;

            if (npc.ai[3] >= 120)
            {
                SpawnGiantSandstorm(npc, target);
                isPreparingSandstorm = false;
                npc.ai[3] = 0;
                sandstormCooldown = 1600;
            }
        }

        private void SpawnGiantSandstorm(NPC npc, Player target)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Vector2 spawnPosition = npc.Center + new Vector2(0, -100);

            Projectile.NewProjectile(
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

   

       
        private void StartFeatherVolley(NPC npc, Player target)
        {
            isPreparingFeatherVolley = true;
            featherVolleyTimer = 0;
            featherVolleySide = Main.rand.NextBool() ? -1 : 1;

            featherSpawnPos = target.Center + new Vector2(
                featherVolleySide * 800f, 
                100f 
            );
        }

        private void ExecuteFeatherVolley(NPC npc, Player target)
        {
            featherVolleyTimer++;

            int interval = 10;
            int shotIndex = (featherVolleyTimer - 1) / interval;

            if (shotIndex < 3 && (featherVolleyTimer - 1) % interval == 0)
            {
                float angleOffset = -10f + shotIndex * 10f;

                Vector2 direction = target.Center - featherSpawnPos;
                direction.Normalize();

                Vector2 velocity = direction * 8f;
                velocity = velocity.RotatedBy(MathHelper.ToRadians(angleOffset));

                // Добавляем случайное вертикальное смещение к позиции появления
                Vector2 spawnPosWithOffset = featherSpawnPos + new Vector2(0, Main.rand.NextFloat(-30f, 30f));

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        spawnPosWithOffset,
                        velocity,
                        ModContent.ProjectileType<DesertBeakFeather>(),
                        Math.Max(npc.damage - 50, 1),
                        2f,
                        Main.myPlayer,
                        0f,
                        0f
                    );
                }

                if (Main.netMode != NetmodeID.Server)
                {
                    SoundEngine.PlaySound(
                        new SoundStyle("Synergia/Assets/Sounds/FeatherFlow") 
                        { 
                            Volume = 0.8f,
                            Pitch = Main.rand.NextFloat(-0.2f, 0.2f)
                        }, 
                        spawnPosWithOffset
                    );
                }
            }

            if (featherVolleyTimer >= 30) 
            {
                isPreparingFeatherVolley = false;
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
