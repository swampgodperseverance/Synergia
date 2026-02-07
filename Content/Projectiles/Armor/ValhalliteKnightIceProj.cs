using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Armor
{
    public class ValhalliteKnightIceProj : ModProjectile
    {
        private readonly VertexStrip vertexStrip = new VertexStrip();
        private bool framePicked;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 40;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            if (!framePicked)
            {
                Projectile.frame = Main.rand.Next(3);
                framePicked = true;
            }

            if (Projectile.timeLeft < 18)
                Projectile.tileCollide = true;

            Projectile.rotation += 0.18f;
            Projectile.velocity *= 0.985f;
        }

        public override void OnKill(int timeLeft)
        {
            Texture2D flashTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null,
                Main.GameViewMatrix.ZoomMatrix);

            for (int i = 0; i < 3; i++)
            {
                float scale = 1.2f + i * 0.9f; 
                float opacity = 0.75f - i * 0.22f;     
                Color bloomColor = new Color(140, 220, 255, 255) * opacity;

                float rot = Main.rand.NextFloat(-0.25f, 0.25f);

                Main.spriteBatch.Draw(
                    flashTex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    bloomColor,
                    Projectile.rotation + rot + MathHelper.PiOver4 * i,
                    flashTex.Size() / 2f,
                    scale * Projectile.scale,
                    SpriteEffects.None,
                    0f
                );
            }

            Main.spriteBatch.Draw(
                flashTex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White * 0.9f,
                Projectile.rotation,
                flashTex.Size() / 2f,
                0.9f * Projectile.scale,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.End();

            for (int i = 0; i < 12; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(3.8f, 3.8f);
                float scale = Main.rand.NextFloat(0.9f, 1.6f);

                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(8f, 8f),
                    267,
                    speed,
                    Alpha: 0,                
                    newColor: new Color(90, 210, 255),
                    Scale: scale
                );
                d.noGravity = true;
                d.fadeIn = 0.6f + Main.rand.NextFloat(0.7f);
            }

            for (int i = 0; i < 7; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    264,
                    Main.rand.NextVector2Circular(5.2f, 5.2f),
                    Alpha: 0,          
                    newColor: Color.White * 0.7f,
                    Scale: 1.3f
                );
                d.noGravity = true;
                d.fadeIn = 1.1f;
            }

            Lighting.AddLight(Projectile.Center, 0.45f, 0.65f, 1.1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            float fade = Projectile.timeLeft < 15 ? Projectile.timeLeft / 15f : 1f;

            sb.BeginBlendState(BlendState.Additive, isUI2: true);
            GameShaders.Misc["MagicMissile"].Apply();

            vertexStrip.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                p => Color.Lerp(
                    new Color(120, 220, 255).MultiplyAlpha(fade),
                    new Color(180, 240, 255).MultiplyAlpha(0.3f),
                    p
                ),
                p => 38f * Projectile.scale * (1f - p),
                -Main.screenPosition + Projectile.Size / 2,
                true
            );

            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = tex.Frame(1, 3, 0, Projectile.frame);

            sb.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                frame,
                new Color(200, 230, 255, 180),
                Projectile.rotation,
                frame.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}
    