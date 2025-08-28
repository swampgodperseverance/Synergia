using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageChest22 : ModProjectile
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

        private int shotsPerFan = 2;
        private float fanSpreadAngle = 0.8f; 
        private float timeBetweenShots = 12f; 
        private float projectileSpeed = 7f; 

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
            
                    if (state == ChestState.Fading)
                    {
                        state = ChestState.ClosedIdle;
                        stateTimer = 0;
                        Projectile.alpha = 0;
                        Projectile.frame = 0;
                    }
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
                    HandleAppearingState();
                    break;

                case ChestState.ClosedIdle:
                    HandleClosedIdleState();
                    break;

                case ChestState.Opening:
                    HandleOpeningState();
                    break;

                case ChestState.Shooting:
                    HandleShootingState();
                    break;

                case ChestState.Closing:
                    HandleClosingState();
                    break;

                case ChestState.Fading:
                    HandleFadingState();
                    break;
            }
        }


        private void HandleAppearingState()
        {
            Projectile.alpha = Math.Max(Projectile.alpha - 10, 0);
            if (Projectile.alpha <= 0)
            {
                state = ChestState.ClosedIdle;
                stateTimer = 0;
                Projectile.frame = 0;
            }
        }

        private void HandleClosedIdleState()
        {
            if (stateTimer >= 120) 
            {
                state = ChestState.Opening;
                stateTimer = 0;
            }
        }

        private void HandleOpeningState()
        {
            if (stateTimer % 10 == 0 && Projectile.frame < 2)
                Projectile.frame++;

            if (Projectile.frame >= 2)
            {
                state = ChestState.Shooting;
                stateTimer = 0;
                currentFan = 0;
                fanShotCount = 0;
            }
        }

        private void HandleShootingState()
        {
            if (stateTimer % timeBetweenShots == 0)
            {
                ShootInFanPattern(currentFan, fanShotCount);
                fanShotCount++;

                if (fanShotCount >= shotsPerFan)
                {
                    fanShotCount = 0;
                    currentFan++;

                    stateTimer = -30; 
                    
                    if (currentFan > 2) 
                    {
                        state = ChestState.Closing;
                        stateTimer = 0;
                    }
                }
            }
        }

        private void HandleClosingState()
        {
            if (stateTimer % 10 == 0 && Projectile.frame > 0)
                Projectile.frame--;

            if (Projectile.frame <= 0)
            {
                state = ChestState.Fading;
                stateTimer = 0;
            }
        }

        private void HandleFadingState()
        {
            Projectile.alpha = Math.Min(Projectile.alpha + 10, 255);
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }

        private void ShootInFanPattern(int fanDirection, int shotIndex)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Vector2 baseDirection = fanDirection switch
            {
                0 => new Vector2(-1f, 0f), 
                1 => new Vector2(1f, 0f), 
                2 => new Vector2(0f, -1f), 
                _ => Vector2.Zero
            };

            float angleStep = fanSpreadAngle / (shotsPerFan - 1);
            float angle = -fanSpreadAngle/2 + angleStep * shotIndex;
            
            Vector2 velocity = baseDirection.RotatedBy(angle) * projectileSpeed;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ModContent.ProjectileType<CarnageAxe2>(),
                Projectile.damage,
                0f,
                Projectile.owner
            );

            if (shotIndex == shotsPerFan/2) 
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.Center,
                        0, 0,
                        DustID.Blood,
                        Main.rand.NextFloat(-1f, 1f),
                        Main.rand.NextFloat(-1f, 1f),
                        100,
                        default,
                        1.5f
                    );
                    dust.noGravity = true;
                }
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