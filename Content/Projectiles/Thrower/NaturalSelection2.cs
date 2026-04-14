using System;
using Avalon.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class NaturalSelection2 : ModProjectile
    {
        private float pulseTimer;
        private float shootPulseIntensity = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            pulseTimer += 0.05f;

            if (shootPulseIntensity > 0f)
                shootPulseIntensity -= 0.05f;
            else
                shootPulseIntensity = 0f;

            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<ContagionDust>(),
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1.2f
                );
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(45))
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * 6f;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<RangedProjectiles.Sludge>(),
                    Projectile.damage / 2,
                    1f,
                    Projectile.owner
                );

                shootPulseIntensity = 1f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D mainTex = TextureAssets.Projectile[Type].Value;
            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Vector2 origin = mainTex.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float shootBoost = 1f + shootPulseIntensity * 0.3f;

            float pulse = 0.92f + 0.08f * (float)Math.Sin(pulseTimer * 3f);
            float pulseSlow = 0.94f + 0.06f * (float)Math.Sin(pulseTimer * 1.2f);

            Color outerGlow = new Color(30, 180, 60, 20) * pulse;
            Color midGlow = new Color(80, 255, 100, 40) * pulse;
            Color innerGlow = new Color(150, 255, 130, 70) * pulseSlow;
            Color coreGlow = new Color(255, 255, 200, 100) * (pulse * 0.6f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            Main.EntitySpriteDraw(glowTex, drawPos, null, outerGlow * shootBoost, Projectile.rotation, origin, Projectile.scale * 1.3f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glowTex, drawPos, null, midGlow * shootBoost, Projectile.rotation, origin, Projectile.scale * 1.1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glowTex, drawPos, null, innerGlow * shootBoost, Projectile.rotation, origin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glowTex, drawPos, null, coreGlow * shootBoost, Projectile.rotation, origin, Projectile.scale * 0.6f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            Main.EntitySpriteDraw(mainTex, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}