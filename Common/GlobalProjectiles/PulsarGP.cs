using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalProjectiles
{
    public class PulsaroGP : GlobalProjectile
    {
        private float timer;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => projectile.ModProjectile != null && projectile.ModProjectile.Mod.Name == "NewHorizons" && projectile.ModProjectile.Name == "PulsarProj";

        public override void PostAI(Projectile projectile)
        {
            timer += 0.05f;
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>(projectile.ModProjectile.Texture).Value;
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            float rot = projectile.rotation;
            float scale = projectile.scale;
            Vector2 origin = tex.Size() / 2f;

            float pulse = (float)Math.Sin(timer * 2f) * 0.3f + 0.7f;

            Color outlineColor = new Color(80, 180, 255);
            Color glowColor = new Color(120, 220, 255);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            for (int i = 0; i < 8; i++)
            {
                float offset = 3f + i * 0.8f + pulse * 1.5f;
                float alpha = (0.7f - i * 0.075f) * pulse;
                float rotationOffset = i * 0.2f + timer * 0.5f;

                for (int k = 0; k < 6; k++)
                {
                    Vector2 offsetVec = new Vector2(offset, 0).RotatedBy(k * (MathHelper.Pi / 3f) + rotationOffset);
                    sb.Draw(tex, drawPos + offsetVec, null, outlineColor * alpha,
                        rot, origin, scale * 1.1f, SpriteEffects.None, 0f);
                }
            }

            for (int i = 0; i < 12; i++)
            {
                float offset = 5f + i * 1.2f + pulse * 2f;
                float alpha = (0.4f - i * 0.03f) * pulse * 0.5f;
                float rotationOffset = i * 0.15f + timer * 0.3f;

                for (int k = 0; k < 4; k++)
                {
                    Vector2 offsetVec = new Vector2(offset, 0).RotatedBy(k * MathHelper.PiOver2 + rotationOffset);
                    sb.Draw(tex, drawPos + offsetVec, null, glowColor * alpha,
                        rot, origin, scale * 1.2f, SpriteEffects.None, 0f);
                }
            }

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            sb.Draw(tex, drawPos, null, lightColor, rot, origin, scale, SpriteEffects.None, 0f);

            Color coreGlow = new Color(150, 230, 255) * (0.4f + pulse * 0.3f);
            sb.Draw(tex, drawPos, null, coreGlow, rot, origin, scale * 0.85f, SpriteEffects.None, 0f);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }
    }
}