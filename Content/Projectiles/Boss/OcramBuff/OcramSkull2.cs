using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.OcramBuff {
    public class OcramSkull2 : ModProjectile {
        private float waveTimer = 0f;
        private float spawnTimer = 0f;
        private Vector2 dashDirection = Vector2.Zero;
        private bool hasDashed = false;
        // this one is better, you can make that boss spawns 10 of them like a spread, will be cool
        public override string Texture => "Synergia/Content/Projectiles/Boss/OcramBuff/OcramSkull";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            spawnTimer++;
            waveTimer += 0.09f;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Player target = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];

            if (!hasDashed)
            {
                if (spawnTimer < 60) 
                {
                    Projectile.velocity *= 0.94f;
                    Projectile.velocity.Y -= 0.35f; 
                }
                else if (spawnTimer < 180) 
                {
                    Projectile.velocity.X *= 0.96f;
                    Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -18f, 0.08f); 
                }
                else 
                {
                    hasDashed = true;

                    if (target.active && !target.dead)
                    {
                        dashDirection = target.Center - Projectile.Center;
                        if (dashDirection != Vector2.Zero)
                            dashDirection.Normalize();
                    }
                    else
                    {
                        dashDirection = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                    }

                    Projectile.velocity = dashDirection * 12f;
                }
            }
            else 
            {
                if (target.active && !target.dead)
                {
                    Vector2 toTarget = target.Center - Projectile.Center;
                    toTarget.Normalize();
                    dashDirection = Vector2.Lerp(dashDirection, toTarget, 0.045f);
                }

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, dashDirection * 26f, 0.135f);
            }

            // same as 1
            float wave = (float)Math.Sin(waveTimer * 1.8f) * 1.35f;
            Projectile.velocity.Y += wave * 0.055f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;
            if (Projectile.timeLeft < 45)
            {
                Projectile.alpha += 7;
                Projectile.velocity *= 0.93f;
            }

            Lighting.AddLight(Projectile.Center, 0.65f, 0.25f, 0.95f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Boss/OcramBuff/OcramSkull_Glow").Value;

            Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / Main.projFrames[Projectile.type] / 2);
            int textureHeight = texture.Height / Main.projFrames[Projectile.type];

            Vector2 drawPosMain = Projectile.Center - Main.screenPosition;

            // Trail
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length * 0.9f);
                if (i > 0) color *= 0.45f;

                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight),
                    color, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }

            float pulse = 1f + (float)Math.Sin(waveTimer * 2.8f) * 0.09f;

            for (int i = 0; i < 4; i++)
            {
                float offset = (i + 1) * 2.2f;
                Color glowColor = new Color(125, 35, 200) * (0.32f - i * 0.07f) * (1f - Projectile.alpha / 255f);

                Main.EntitySpriteDraw(glowTex, drawPosMain + new Vector2(offset, 0),
                    new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                    glowColor, Projectile.rotation, origin, Projectile.scale * (1.18f + i * 0.06f), effects, 0);

                Main.EntitySpriteDraw(glowTex, drawPosMain - new Vector2(offset, 0),
                    new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                    glowColor, Projectile.rotation, origin, Projectile.scale * (1.18f + i * 0.06f), effects, 0);

                Main.EntitySpriteDraw(glowTex, drawPosMain + new Vector2(0, offset * 0.75f),
                    new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                    glowColor * 0.75f, Projectile.rotation, origin, Projectile.scale * (1.12f + i * 0.05f), effects, 0);
            }

            Main.EntitySpriteDraw(glowTex, drawPosMain, new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                new Color(175, 85, 245) * 0.72f * (1f - Projectile.alpha / 255f),
                Projectile.rotation, origin, Projectile.scale * 1.26f * pulse, effects, 0);

            Main.EntitySpriteDraw(texture, drawPosMain, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight),
                Color.White * (1f - Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch,
                    Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 120, default, 1.4f);
            }
        }
    }
}