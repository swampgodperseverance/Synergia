using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewHorizons.Content.Projectiles.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    internal class CrystallGranade2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 80;
            Projectile.friendly = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            float pulse = 1f + 0.15f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 14f);

            Color innerGlow = new Color(180, 230, 255, 140) * 0.9f;
            Color outerGlow = new Color(100, 200, 255, 60) * 0.6f;

            for (int i = 0; i < 8; i++)
            {
                Vector2 offset = (i * MathHelper.TwoPi / 8f).ToRotationVector2() * 5f;
                Main.EntitySpriteDraw(
                    texture,
                    screenPos + offset,
                    null,
                    outerGlow,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * pulse * 1.15f,
                    SpriteEffects.None
                );
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = (i * MathHelper.PiOver2 / 1.5f).ToRotationVector2() * 3f;
                Main.EntitySpriteDraw(
                    texture,
                    screenPos + offset,
                    null,
                    innerGlow,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * pulse,
                    SpriteEffects.None
                );
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(
                texture,
                screenPos,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    31,
                    0f,
                    0f,
                    100,
                    default,
                    1.5f
                );
                Main.dust[d].noGravity = true;
            }

            for (int i = 0; i < 4; i++)
            {
                int g = Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.position,
                    Vector2.Zero,
                    Main.rand.Next(61, 64),
                    1f
                );
                Main.gore[g].velocity *= 0.3f;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int count = Main.rand.Next(5, 8);
                float speed = 7f;

                for (int i = 0; i < count; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * speed;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        ModContent.ProjectileType<CrystalGrenadeShardProj>(),
                        (int)(Projectile.damage * 0.6f),
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }

            Projectile.netUpdate = true;
        }
    }
}