using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.OcramBuff {
    public class OcramSkull1 : ModProjectile {
        private float waveTimer = 0f;
        //I HAVE NOT TESTED THIS, CAN BE ASS
        // should be just projectile with some wavy movements
        public override string Texture => "Synergia/Content/Projectiles/Boss/OcramBuff/OcramSkull";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            waveTimer += 0.085f;

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Player target = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];
            if (target.active && !target.dead)
            {
                Vector2 toTarget = target.Center - Projectile.Center;
                toTarget.Normalize();

                float acceleration = 0.1f;
                Vector2 targetVel = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), toTarget, 0.05f * Projectile.timeLeft / 300f))
                                  * (Projectile.velocity.Length() + acceleration);

                // here i tried to make wavy movement
                float waveOffset = (float)Math.Sin(waveTimer) * 1.8f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVel, 0.12f);
                Projectile.velocity.Y += waveOffset * 0.045f; 
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.timeLeft < 40)
            {
                Projectile.alpha += 6;
                Projectile.velocity *= 0.95f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Boss/OcramBuff/OcramSkull_Glow").Value;
            Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / Main.projFrames[Projectile.type] / 2);
            int textureHeight = texture.Height / Main.projFrames[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                if (i > 0) color *= 0.5f;
                Main.EntitySpriteDraw(texture, drawPos, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight),
                    color, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }

            Vector2 drawPosMain = Projectile.Center - Main.screenPosition;
            float pulse = 1f + (float)Math.Sin(waveTimer * 2.5f) * 0.08f;

            for (int i = 0; i < 4; i++)
            {
                float offset = (i + 1) * 1.8f;
                Color glowColor = new Color(130, 40, 210) * (0.28f - i * 0.06f) * (1f - Projectile.alpha / 255f);
                Main.EntitySpriteDraw(glowTex, drawPosMain + new Vector2(offset, 0),
                    new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                    glowColor, Projectile.rotation, origin, Projectile.scale * (1.15f + i * 0.05f), effects, 0);
                Main.EntitySpriteDraw(glowTex, drawPosMain - new Vector2(offset, 0),
                    new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                    glowColor, Projectile.rotation, origin, Projectile.scale * (1.15f + i * 0.05f), effects, 0);
                Main.EntitySpriteDraw(glowTex, drawPosMain + new Vector2(0, offset * 0.7f),
                    new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                    glowColor * 0.7f, Projectile.rotation, origin, Projectile.scale * (1.1f + i * 0.04f), effects, 0);
            }
            Main.EntitySpriteDraw(glowTex, drawPosMain, new Rectangle(0, Projectile.frame * textureHeight, glowTex.Width, textureHeight),
                new Color(180, 90, 255) * 0.65f * (1f - Projectile.alpha / 255f),
                Projectile.rotation, origin, Projectile.scale * 1.22f * pulse, effects, 0);
            Main.EntitySpriteDraw(texture, drawPosMain, new Rectangle(0, Projectile.frame * textureHeight, texture.Width, textureHeight),
                Color.White * (1f - Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, effects, 0);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch,
                    Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 100, default, 1.3f);
            }
        }
    }
}