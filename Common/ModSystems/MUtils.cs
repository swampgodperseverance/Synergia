using Terraria;

namespace Synergia.Common.ModSystems {
    public class MUtils : ModSystem {
        public static void DrawSimpleAfterImage(Color lightColor, Projectile projectile, Texture2D projectileTexture, float colorReduct = 1f, float scaleMult = 1f, float scaleReduct = 0.25f, float velOffset = 0f, float? yScaleDif = null, float extraRotate = 0f) {
            Vector2 toCenterTexture = new Vector2(projectileTexture.Width, projectileTexture.Height / Main.projFrames[projectile.type]) / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1) spriteEffects = SpriteEffects.FlipHorizontally;
            Rectangle source = new(0, projectileTexture.Height / Main.projFrames[projectile.type] * projectile.frame, projectileTexture.Width, projectileTexture.Height / Main.projFrames[projectile.type]);
            for (byte k = 0; k < (byte)projectile.oldPos.Length; k++) {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + new Vector2(projectile.width, projectile.height) / 2f;
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) * (float)(1f / (projectile.oldPos.Length * colorReduct)));
                Vector2 oldVel = (k == 0 ? projectile.oldPos[0] - projectile.position : projectile.oldPos[k] - projectile.oldPos[k - 1]).SafeNormalize(Vector2.Zero) * -velOffset;
                Vector2 scaling = new(projectile.scale * scaleMult - k / (float)projectile.oldPos.Length * scaleReduct);
                if (yScaleDif != null) scaling.Y *= (float)yScaleDif;
                float rotation = projectile.oldRot[k] * (1 + extraRotate);//*16f
                if (extraRotate == -69.1f) rotation = Main.GlobalTimeWrappedHourly * 16f * projectile.direction;
                if (extraRotate == -69.2f) rotation = Main.GlobalTimeWrappedHourly * 20f * projectile.direction;
                Main.spriteBatch.Draw(projectileTexture, drawPos - oldVel * k, new Rectangle?(source), color, rotation, toCenterTexture, scaling, spriteEffects, 0f);
            }
        }
    }
}
