using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Thrower
{
    public class Anchor2 : ModProjectile
    {
        private const int TrailLength = 6;
        private const float VelocityDecay = 0.985f;
        private const float UpwardBoost = -0.04f;
        private const float DownwardAccel = 1.01f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = TrailLength;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 1;         
            Projectile.DamageType = DamageClass.Throwing;
            AIType = ProjectileID.BoneJavelin;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
            Projectile.velocity *= VelocityDecay;

            if (Projectile.velocity.Y < 0f)
                Projectile.velocity.Y += UpwardBoost;     
            else
                Projectile.velocity.Y *= DownwardAccel;   
            if (Projectile.ai[1] == 0f)
                Projectile.ai[1] = Main.rand.Next(10, 18);

            Projectile.ai[0]++;

            if (Projectile.ai[0] >= Projectile.ai[1])
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 junkVelocity = new Vector2(
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-3f, -1.2f)
                    );

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromAI(),
                        Projectile.Center,
                        junkVelocity,
                        ModContent.ProjectileType<AnchorJunk>(),
                        Projectile.damage / 2,
                        0f,                      
                        Projectile.owner
                    );
                }

                Projectile.ai[0] = 0f;
                Projectile.ai[1] = Main.rand.Next(16, 25);
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] % 8 == 0)
                Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    lightColor * alpha * 0.6f,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0
                );
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 dustVel = Projectile.velocity * Main.rand.NextFloat(0.2f, 1.4f);
                Dust.NewDust(
                    Projectile.position, Projectile.width, Projectile.height,
                    DustID.Iron, dustVel.X, dustVel.Y,
                    100, default, 1.4f
                );
            }
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }

    public class AnchorJunk : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 0.8f;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.frame = Main.rand.Next(3);
                Projectile.localAI[0] = 1f;
            }
            float rotSpeed = (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f;
            Projectile.rotation += rotSpeed * Projectile.direction;

            Projectile.ai[0]++;

            if (Projectile.ai[0] > 45f)
                Projectile.velocity.Y += 0.15f; 
            Projectile.velocity *= 0.99f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle frame = texture.Frame(1, 3, 0, Projectile.frame);
            Vector2 origin = frame.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;
            for (int i = 1; i <= 4; i++)
            {
                float fade = 1f - i * 0.25f;
                Main.EntitySpriteDraw(
                    texture,
                    screenPos - Projectile.velocity * (i * 2f),
                    frame,
                    lightColor * fade * 0.5f,
                    Projectile.rotation + i * -0.3f * Projectile.direction,
                    origin,
                    Projectile.scale * (1f - i * 0.1f),
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                screenPos,
                frame,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(
                    Projectile.position, Projectile.width, Projectile.height,
                    DustID.Bone,
                    -Projectile.velocity.X * 0.25f, -Projectile.velocity.Y * 0.25f
                );
            }
        }
    }
}