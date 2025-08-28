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
    public class CarnageChest333 : ModProjectile
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
        private int shotsFired = 0;

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
            Projectile.damage = 108;
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
                        shotsFired = 0;
                    }
                    break;

                case ChestState.Shooting:
                    if (shotsFired < 4 && stateTimer % 10 == 0)
                    {
                        ShootAtPlayer();
                        shotsFired++;
                    }
                    if (shotsFired >= 4 && stateTimer >= 60)
                    {
                        state = ChestState.Closing;
                        stateTimer = 0;
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

        private void ShootAtPlayer()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            Player target = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];
            if (!target.active || target.dead) return;

            Vector2 direction = Vector2.Normalize(target.Center - Projectile.Center) * Main.rand.NextFloat(8f, 10f);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                direction,
                ModContent.ProjectileType<CarnageThing>(),
                Projectile.damage * 2,
                0f,
                Projectile.owner
            );
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

