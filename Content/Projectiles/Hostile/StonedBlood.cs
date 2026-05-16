using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Hostile
{
    public class StonedBlood : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            Projectile.velocity.Y += 0.08f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float glowIntensity = 0.8f + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 8f) * 0.2f;

            for (int i = 0; i < 3; i++)
            {
                float offset = (i + 1) * 2.5f;
                Vector2 glowOffset = new Vector2(offset, offset);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + glowOffset,
                    null,
                    new Color(180, 30, 30, 40) * glowIntensity,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.2f,
                    SpriteEffects.None,
                    0
                );

                Main.EntitySpriteDraw(
                    texture,
                    drawPos - glowOffset,
                    null,
                    new Color(180, 30, 30, 40) * glowIntensity,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.2f,
                    SpriteEffects.None,
                    0
                );
            }

            for (int i = 0; i < 8; i++)
            {
                float angle = i * MathHelper.TwoPi / 8f;
                Vector2 radialOffset = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle)) * 3f;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + radialOffset,
                    null,
                    new Color(220, 50, 50, 60) * (glowIntensity * 0.7f),
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.15f,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                new Color(255, 80, 80, 100) * glowIntensity,
                Projectile.rotation,
                origin,
                Projectile.scale * 1.25f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                new Color(255, 150, 150, 120) * (glowIntensity * 0.5f),
                Projectile.rotation,
                origin,
                Projectile.scale * 1.1f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}