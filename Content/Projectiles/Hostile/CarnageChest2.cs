using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageChest2 : ModProjectile
    {
        private enum ChestState
        {
            Appearing,
            ClosedIdle,
            Opening,
            Shooting,
            Closing,
            Fading
        }

        private ChestState state = ChestState.Appearing;
        private int stateTimer = 0;

        private int currentFan = 0; 
        private int fanShotCount = 0; 

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.damage = 100;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255; 
        }

        public override void AI()
        {
            if (Projectile.ai[0] >= 0 && Projectile.ai[0] < Main.maxNPCs)
            {
                NPC boss = Main.npc[(int)Projectile.ai[0]];
                if (boss.active && boss.life > 0)
                {
                    Projectile.Center = boss.Center + new Vector2(0, 190f);
                }
                else
                {
                    if (state != ChestState.Fading)
                    {
                        state = ChestState.Fading;
                        stateTimer = 0;
                    }
                }
            }

            stateTimer++;

            switch (state)
            {
                case ChestState.Appearing:
                    Projectile.alpha = Math.Max(Projectile.alpha - 10, 0);
                    if (Projectile.alpha <= 0)
                    {
                        state = ChestState.ClosedIdle;
                        stateTimer = 0;
                        Projectile.frame = 0;
                    }
                    break;

                case ChestState.ClosedIdle:
                    if (stateTimer >= 120) 
                    {
                        state = ChestState.Opening;
                        stateTimer = 0;
                    }
                    break;

                case ChestState.Opening:
                    if (stateTimer % 10 == 0 && Projectile.frame < 2)
                        Projectile.frame++;

                    if (Projectile.frame >= 2)
                    {
                        state = ChestState.Shooting;
                        stateTimer = 0;
                        currentFan = 0;
                        fanShotCount = 0;
                    }
                    break;

                case ChestState.Shooting:
                    if (stateTimer % 18 == 0)
                    {
                        ShootFanProjectile(currentFan);
                        fanShotCount++;
                        if (fanShotCount >= 3)
                        {
                            fanShotCount = 0;
                            currentFan++;
                            if (currentFan > 2)
                            {
                                state = ChestState.Closing;
                                stateTimer = 0;
                            }
                        }
                    }
                    break;

                case ChestState.Closing:
                    if (stateTimer % 10 == 0 && Projectile.frame > 0)
                        Projectile.frame--;

                    if (Projectile.frame <= 0)
                    {
                        state = ChestState.Fading;
                        stateTimer = 0;
                    }
                    break;

                case ChestState.Fading:
                    Projectile.alpha = Math.Min(Projectile.alpha + 10, 255);
                    if (Projectile.alpha >= 255)
                    {
                        Projectile.Kill();
                    }
                    break;
            }
        }

        private void ShootFanProjectile(int fanIndex)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Vector2 baseVelocity = fanIndex switch
            {
                0 => new Vector2(-1f, 0f), 
                1 => new Vector2(1f, 0f),  
                2 => new Vector2(0f, -1f), 
                _ => Vector2.Zero
            };

            float spread = 0.3f;
            Vector2 velocity = baseVelocity.RotatedBy(Main.rand.NextFloat(-spread, spread));
            velocity *= Main.rand.NextFloat(6f, 9f);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ModContent.ProjectileType<CarnageAxe1>(),
                Projectile.damage,
                0f,
                Projectile.owner
            );


            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center,
                    0, 0,
                    DustID.Water,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    100,
                    default,
                    1.3f
                );
                dust.noGravity = true;
                dust.velocity *= 0.7f;
            }

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center,
                    0, 0,
                    DustID.Smoke,
                    Main.rand.NextFloat(-1.5f, 1.5f),
                    Main.rand.NextFloat(-1.5f, 1.5f),
                    150,
                    default,
                    1f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle frameRect = texture.Frame(1, Main.projFrames[Type], 0, Projectile.frame);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                frameRect,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                frameRect.Size() / 2,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
