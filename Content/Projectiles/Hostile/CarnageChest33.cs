using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageChest33 : ModProjectile
    {
        private enum ChestState
        {
            Appearing,
            ClosedIdle,
            Opening,
            ShootingToilets,
            Closing,
            Fading
        }

        private ChestState state = ChestState.Appearing;
        private int stateTimer = 0;
        private const int ToiletsCount = 5; 
        private const float FanSpreadAngle = MathHelper.Pi / 3; 
        private const float ToiletSpeed = 8f; 

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
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

                case ChestState.ShootingToilets:
                    HandleShootingToiletsState();
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
            Projectile.alpha -= 10;
            if (Projectile.alpha < 0) 
                Projectile.alpha = 0;
    
            if (Projectile.alpha == 0)
            {
                state = ChestState.ClosedIdle;
                stateTimer = 0;
                Projectile.frame = 0;
            }
        }

        private void HandleClosedIdleState()
        {
            if (stateTimer >= 60) 
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
                state = ChestState.ShootingToilets;
                stateTimer = 0;
                ShootToiletsFan();
            }
        }

        private void ShootToiletsFan()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center); 

            float baseAngle = -FanSpreadAngle / 2; 
            float angleStep = FanSpreadAngle / (ToiletsCount - 1); 

            for (int i = 0; i < ToiletsCount; i++)
            {
                float angle = baseAngle + angleStep * i;
                Vector2 velocity = Vector2.UnitY.RotatedBy(angle) * -ToiletSpeed; 

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<CarnageToilet>(),
                    Projectile.damage,
                    0f,
                    Projectile.owner
                );
            }

            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GoldFlame,
                    Main.rand.NextVector2Circular(3f, 3f),
                    100,
                    default,
                    1.5f
                ).noGravity = true;
            }
        }

        private void HandleShootingToiletsState()
        {
            if (stateTimer >= 60) 
            {
                state = ChestState.Closing;
                stateTimer = 0;
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frameRect = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

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