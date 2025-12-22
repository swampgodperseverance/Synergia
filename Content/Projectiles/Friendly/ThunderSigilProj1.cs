using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public sealed class ThunderSigilProj1 : ModProjectile 
    {
        public override void SetStaticDefaults() 
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
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
            Projectile.width = 28;
            Projectile.height = 10;
            Projectile.damage = 76;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;
        }

        public override void AI()
        {
   
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

            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                float homingSpeed = 0.15f; 
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * Projectile.velocity.Length(), homingSpeed);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, 1.2f, 1.1f, 0.3f);

            if (Main.rand.NextBool(4))
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
                    DustID.GemTopaz,
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

