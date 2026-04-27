using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Thrower
{
    public sealed class ThunderSpike2 : ModProjectile
    {
        private bool hasDoneLoop = false;
        private int loopTimer = 0;
        private const int LOOP_DURATION = 45;
        private float loopAngularSpeed;
        private bool isDying = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 5;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = ModContent.Request<Texture2D>(
                "RoA/Resources/Textures/VisualEffects/DefaultSparkle"
            ).Value;

            Vector2 origin = texture.Size() * 0.5f;
            SpriteEffects effects = Projectile.spriteDirection == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;

                Color color = Color.Lerp(
                    new Color(255, 230, 100),
                    new Color(255, 150, 50),
                    k / (float)Projectile.oldPos.Length
                ) * 0.6f;

                spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.oldRot[k] + MathHelper.PiOver2,
                    origin,
                    Projectile.scale * (1f - k * 0.05f),
                    effects,
                    0f
                );
            }

            return true;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
        }

        public override bool PreAI()
        {
            if (isDying)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(Math.PI / 35);
                Projectile.velocity *= 0.98f;
                return true;
            }

            float maxDetectRadius = 300f;
            NPC target = null;
            float closestDistance = maxDetectRadius;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy() && !npc.friendly)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = npc;
                    }
                }
            }

            if (target != null && !hasDoneLoop)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                float homingSpeed = 0.12f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * Projectile.velocity.Length(), homingSpeed);
            }
            else if (!hasDoneLoop)
            {
                hasDoneLoop = true;
                loopTimer = 0;
                loopAngularSpeed = MathHelper.ToRadians(360f / LOOP_DURATION * 2);
            }

            if (hasDoneLoop && !isDying)
            {
                loopTimer++;

                Projectile.velocity = Projectile.velocity.RotatedBy(loopAngularSpeed);

                if (loopTimer < LOOP_DURATION * 0.3f)
                {
                    Projectile.velocity *= 1.01f;
                }
                else if (loopTimer > LOOP_DURATION * 0.6f)
                {
                    Projectile.velocity *= 0.97f;
                }

                if (loopTimer >= LOOP_DURATION)
                {
                    isDying = true;
                }
            }

            return true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, 1.2f, 1.1f, 0.3f);

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldFlame,
                    0f, 0f, 100,
                    default,
                    1.1f
                );

                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].noGravity = true;
            }

            if (isDying)
            {
                for (int k = 0; k < 8; k++)
                {
                    int index2 = Dust.NewDust(Projectile.position, 10, 10, DustID.GoldFlame, 0f, 0f, 0, default, 1.2f);
                    Main.dust[index2].position = Projectile.Center - Projectile.velocity / 5f * (float)k;
                    Main.dust[index2].scale = 0.6f;
                    Main.dust[index2].velocity *= 0f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].noLight = false;
                }
            }

            Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
        }

        public override void PostAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 5)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldFlame,
                    Main.rand.NextFloat(-3, 3),
                    Main.rand.NextFloat(-3, 3),
                    100,
                    default,
                    1.3f
                );
            }
        }
    }
}