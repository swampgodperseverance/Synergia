using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class Waternado : ModProjectile
    {
        private const float MaxScale = 1.0f;
        private const float GrowthRate = 0.01f;

        private float currentScale = 0.1f;

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 300;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true; 
            Projectile.friendly = false;
            Projectile.damage = 100;
            Projectile.timeLeft = 540;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 60)
            {
                if (currentScale < MaxScale)
                {
                    currentScale = Math.Min(currentScale + GrowthRate, MaxScale);
                    Projectile.scale = currentScale;
                }

                if (Projectile.alpha > 0)
                    Projectile.alpha -= 8;
            }
            else
            {
                currentScale = MathHelper.Lerp(currentScale, 0f, 0.05f);
                Projectile.scale = currentScale;
                Projectile.alpha = (int)MathHelper.Lerp(Projectile.alpha, 255f, 0.05f);
            }

            Projectile.rotation += 0.1f;
            Projectile.ai[0]++;

            if (Main.rand.NextBool(4))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2),
                    DustID.Water,
                    Main.rand.NextVector2Circular(2f, 4f),
                    100,
                    Color.LightBlue,
                    1.5f
                ).noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);

            for (int j = 0; j < 2; j++)
            {
                float opacity = 0;
                float scale = 0.1f * currentScale;
                float heightDivision = 4;

                for (int i = 0; i < (int)(Math.Floor(Projectile.height / heightDivision)); i++)
                {
                    Vector2 drawPos = Projectile.Bottom - Main.screenPosition + new Vector2(j * 4)
                        .RotatedBy(i * MathHelper.PiOver2 + Main.timeForVisualEffects * 0.1f);
                    opacity = MathHelper.Clamp(opacity + 0.01f, 0, 1);
                    scale = MathHelper.Clamp(scale + 0.03f, 0, 1.1f * currentScale);

                    Color color = j == 0 ?
                        Color.LightBlue * opacity * Projectile.Opacity :
                        new Color(200, 230, 255, 128) * opacity * 0.8f * Projectile.Opacity;

                    Main.EntitySpriteDraw(
                        texture,
                        drawPos + new Vector2(0, i * -heightDivision),
                        frame,
                        color,
                        Projectile.rotation + MathHelper.PiOver4 * i * -0.1f * Projectile.direction,
                        new Vector2(texture.Width, texture.Height) / 2,
                        scale - (j * 0.1f),
                        SpriteEffects.None,
                        0
                    );
                }
            }
            return false;
        }
    }
}
