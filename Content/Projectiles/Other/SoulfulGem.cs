using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.NPCs.Dungeon;
using Synergia.Content.NPCs.Miniboss;
using Synergia.Content.Dusts;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.NPCs.Dungeon;

namespace Synergia.Content.Projectiles.Other
{
    public class SoulfulGem : ModProjectile
    {
        private Vector2 orbitOffset;
        private float angle;
        private float speed;
        private bool hasArrived = false;
        private float arrivalTimer = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60 * 60 * 5;
            Projectile.ignoreWater = true;
            Projectile.oldPos = new Vector2[5];
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1];
            }
            Projectile.oldPos[0] = Projectile.Center;

            if (!hasArrived)
            {
                arrivalTimer++;

                if (arrivalTimer < 30)
                {
                    Vector2 toPlayer = player.Center - Projectile.Center;
                    float distance = toPlayer.Length();

                    if (distance < 100f)
                    {
                        hasArrived = true;
                        arrivalTimer = 0;

                        for (int i = 0; i < 15; i++)
                        {
                            Dust.NewDustPerfect(
                                Projectile.Center,
                                ModContent.DustType<CruorDust>(),
                                Main.rand.NextVector2Circular(3f, 3f),
                                0,
                                default,
                                1.2f
                            );
                        }
                    }
                    else
                    {
                        Vector2 move = toPlayer.SafeNormalize(Vector2.Zero) * 12f;
                        Projectile.Center += move;
                        Projectile.rotation += 0.2f;

                        if (Main.rand.NextBool(3))
                        {
                            Dust.NewDustPerfect(
                                Projectile.Center,
                                ModContent.DustType<CruorDust>(),
                                Main.rand.NextVector2Circular(1f, 1f),
                                0,
                                default,
                                0.8f
                            );
                        }
                        return;
                    }
                }
                else
                {
                    hasArrived = true;
                }
            }

            angle += 0.05f + Main.rand.NextFloat(0.01f, 0.03f);
            speed = 60f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f + Projectile.whoAmI) * 10f;
            orbitOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle * 1.3f)) * speed;
            Vector2 targetPos = player.Center + orbitOffset;
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, 0.12f);

            Projectile.rotation += 0.08f;
            Lighting.AddLight(Projectile.Center, 0.9f, 0.3f, 0.6f);

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center - new Vector2(4, 4),
                    8, 8,
                    ModContent.DustType<CruorDust>(),
                    0, 0,
                    0,
                    default,
                    0.6f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(0.5f, 0.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;
            Color glowColor = new Color(255, 120, 200);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                Vector2 pos = Projectile.oldPos[i] + origin - Main.screenPosition;
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                float scale = Projectile.scale * (1f - i * 0.05f);

                Main.spriteBatch.Draw(
                    texture,
                    pos,
                    null,
                    glowColor * alpha * 0.3f,
                    Projectile.rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }

            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi * i / 8f + Main.GlobalTimeWrappedHourly * 2f;
                Vector2 offset = angle.ToRotationVector2() * (2f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 10f) * 0.5f);

                Main.spriteBatch.Draw(
                    texture,
                    Projectile.Center - Main.screenPosition + offset,
                    null,
                    glowColor * (0.3f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8f + i) * 0.1f),
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.05f,
                    SpriteEffects.None,
                    0f
                );
            }

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<CruorDust>(),
                    Main.rand.NextVector2Circular(2f, 2f),
                    0,
                    default,
                    1.2f
                );
                dust.noGravity = true;
            }
        }
    }

    public class SoulGemPlayer : ModPlayer
    {
        private bool hasSpawned = false;
        private int summonTimer = 0;
        private List<Projectile> gemsToSummon = new List<Projectile>();

        public override void PostUpdate()
        {
            int count = 0;
            List<Projectile> gems = new List<Projectile>();

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.owner == Player.whoAmI &&
                    proj.type == ModContent.ProjectileType<SoulfulGem>())
                {
                    count++;
                    gems.Add(proj);
                }
            }

            if (count >= 2 && !hasSpawned)
            {
                hasSpawned = true;
                gemsToSummon = gems;
                summonTimer = 120;
            }

            if (summonTimer > 0)
            {
                summonTimer--;

                if (summonTimer <= 0)
                {
                    SpawnCruor(gemsToSummon);
                    gemsToSummon.Clear();
                }
            }
        }

        private void SpawnCruor(List<Projectile> gems)
        {
            foreach (var proj in gems)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Dust.NewDustPerfect(
                        proj.Center,
                        ModContent.DustType<CruorDust>(),
                        Main.rand.NextVector2Circular(3f, 3f),
                        0,
                        default,
                        1.2f
                    );
                    dust.noGravity = true;
                }
                proj.Kill();
            }

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Player.Center,
                    ModContent.DustType<CruorDust>(),
                    Main.rand.NextVector2Circular(4f, 4f),
                    0,
                    default,
                    1.5f
                );
                dust.noGravity = true;
            }

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Player.Center);

            int npcID = ModContent.NPCType<Cruor>();

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                int npcIndex = NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), (int)Player.Center.X, (int)Player.Center.Y - 100, npcID);
                if (npcIndex >= 0 && npcIndex < Main.maxNPCs)
                {
                    Main.npc[npcIndex].target = Player.whoAmI;
                }
            }
            else
            {
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, Player.whoAmI, npcID, Player.Center.X, Player.Center.Y - 100);
            }
        }

        public override void ResetEffects()
        {
            if (summonTimer <= 0)
            {
                hasSpawned = false;
            }
        }
    }

    public class SoulfulGemDropSystem : GlobalNPC
    {
        public static List<int> RareDropNPCs = new()
        {
            NPCID.RustyArmoredBonesSwordNoArmor,
            ModContent.NPCType<RadiantBones>(),
            ModContent.NPCType<RadiantBones2>(),
            ModContent.NPCType<RadiantBones3>(),
            ModContent.NPCType<RadiantBones4>(),
            ModContent.NPCType<Sparky>(),
            NPCID.RustyArmoredBonesFlail,
            NPCID.RustyArmoredBonesSword,
            NPCID.RustyArmoredBonesAxe,
            NPCID.BlueArmoredBones,
            NPCID.BlueArmoredBonesMace,
            NPCID.BlueArmoredBonesNoPants,
            NPCID.BlueArmoredBonesSword,
            NPCID.HellArmoredBones,
            NPCID.HellArmoredBonesSpikeShield,
            NPCID.HellArmoredBonesMace,
            NPCID.HellArmoredBonesSword,
            NPCID.Necromancer,
            NPCID.RaggedCaster
        };

        public static List<int> CommonDropNPCs = new()
        {
            NPCID.Paladin,
            NPCID.SkeletonSniper,
            NPCID.NecromancerArmored,
            NPCID.RaggedCasterOpenCoat,
            NPCID.TacticalSkeleton,
            NPCID.SkeletonCommando,
            ModContent.NPCType<ValhallaMod.NPCs.Dungeon.Radiator2>(),
            ModContent.NPCType<ValhallaMod.NPCs.Dungeon.Radiator>(),
            NPCID.BoneLee,
        };

        public static List<int> ModRareNPCs = new()
        {
            ModContent.NPCType<Cruor>()
        };

        public override void OnKill(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Player closest = Main.player[Player.FindClosest(npc.Center, 16, 16)];
            if (closest == null || !closest.active)
                return;

            bool shouldDrop = false;
            int dropCount = 1;

            if (RareDropNPCs.Contains(npc.type) || ModRareNPCs.Contains(npc.type))
            {
                if (Main.rand.NextFloat() < 0.05f)
                {
                    shouldDrop = true;
                    dropCount = Main.rand.Next(1, 3);
                }
            }

            if (CommonDropNPCs.Contains(npc.type))
            {
                if (Main.rand.NextFloat() < 0.20f)
                {
                    shouldDrop = true;
                    dropCount = Main.rand.Next(1, 3);
                }
            }

            if (shouldDrop)
            {
                SpawnGem(closest, npc.Center, dropCount);
            }
        }

        private void SpawnGem(Player player, Vector2 position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);

                Projectile.NewProjectile(
                    player.GetSource_FromThis(),
                    position,
                    velocity,
                    ModContent.ProjectileType<SoulfulGem>(),
                    0,
                    0f,
                    player.whoAmI
                );

                for (int d = 0; d < 6; d++)
                {
                    Dust dust = Dust.NewDustPerfect(
                        position,
                        ModContent.DustType<CruorDust>(),
                        Main.rand.NextVector2Circular(2f, 2f) + velocity * 0.5f,
                        0,
                        default,
                        1f
                    );
                    dust.noGravity = true;
                }
            }
        }
    }
}