using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class SplutterinkRework : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Terraria/Images/Extra_57";
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 35;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.12f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            if (Projectile.velocity.X != 0f)
            {
                Projectile.direction = Projectile.velocity.X > 0f ? 1 : -1;
            }

            Projectile.oldRot[0] = Projectile.velocity.ToRotation();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<TarWaterSplash>(),
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f),
                    0,
                    Color.Black,
                    1.4f
                );
            }

            Projectile.position = Projectile.Center;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.position.X -= Projectile.width / 2;
            Projectile.position.Y -= Projectile.height / 2;
            Projectile.Damage();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/Extra_98").Value;

            Color trailColor = Color.Black;
            trailColor.A = 0;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (float)i / Projectile.oldPos.Length;

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition,
                    null,
                    Color.Black * MathHelper.Lerp(1f, 0f, progress),
                    Projectile.oldRot[i] + MathHelper.PiOver2,
                    texture.Size() / 2f,
                    Projectile.scale * MathHelper.Lerp(0.6f, 0.05f, progress),
                    SpriteEffects.None,
                    0f
                );
            }

            texture = ModContent.Request<Texture2D>(Texture).Value;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.Black,
                Projectile.rotation,
                texture.Size() / 2f,
                Projectile.scale * 0.5f,
                SpriteEffects.None,
                0f
            );

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.Black,
                -Projectile.rotation,
                texture.Size() / 2f,
                Projectile.scale * 0.5f,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}