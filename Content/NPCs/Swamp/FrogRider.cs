using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs.Swamp
{
    public class FrogRider : ModNPC
    {
        private double frameCounter;
        private int frame;
        private bool jumpSoundPlayed;
        private bool spawnOffsetApplied;

        public override string LocalizationCategory => Category(CategoryName.NPC);

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 13;
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 42;
            NPC.damage = 22;
            NPC.defense = 10;
            NPC.lifeMax = 150;
            NPC.aiStyle = 1;
            AIType = NPCID.Frog;
            AnimationType = -1;
            NPC.knockBackResist = 0.6f;
        }

        public override void AI()
        {
            if (!spawnOffsetApplied)
            {
                NPC.position.Y += 2f;
                spawnOffsetApplied = true;
            }

            NPC.TargetClosest(true);

            if (NPC.velocity.X != 0f)
            {
                NPC.direction = Math.Sign(NPC.velocity.X);
                NPC.spriteDirection = NPC.direction;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y == 0f)
            {
                jumpSoundPlayed = false;

                if (Math.Abs(NPC.velocity.X) < 0.1f)
                {
                    frame = 0;
                }
                else
                {
                    frameCounter++;
                    if (frameCounter >= 5)
                    {
                        frame++;
                        frameCounter = 0;
                    }

                    if (frame > 8)
                        frame = 0;
                }
            }
            else if (NPC.velocity.Y < 0f)
            {
                frame = 9;

                if (!jumpSoundPlayed)
                {
                    SoundEngine.PlaySound(
                        Reassures.Reassures.RSounds.SwamplingRoar with
                        {
                            Volume = 0.8f,
                            PitchVariance = 0.12f
                        },
                        NPC.Center
                    );

                    jumpSoundPlayed = true;
                }
            }
            else
            {
                frame = 10;
            }

            if (frame >= 13)
                frame = 0;

            NPC.frame.Y = frame * frameHeight;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath27, NPC.Center);

                for (int k = 1; k < 4; k++)
                {
                    if (Mod.Find<ModGore>($"SwampGore{k}") != null)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity * 0.4f, Mod.Find<ModGore>($"SwampGore{k}").Type);
                    }
                }

                for (int i = 0; i < 24; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        NPC.Center,
                        DustID.Blood,
                        Main.rand.NextVector2Circular(5f, 5f),
                        50
                    );
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(0.9f, 1.6f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            SoundEngine.PlaySound(SoundID.NPCHit13, NPC.Center);
        }
    }
}