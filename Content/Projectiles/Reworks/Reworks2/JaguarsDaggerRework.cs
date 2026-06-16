using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.Players;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class JaguarDaggerRework : ModProjectile
    {
        private float pulseCounter = 0f;
        private Vector2 oldPosition;

        public override string Texture
        {
            get
            {
                return "Bismuth/Content/Projectiles/JaguarsDaggerP";
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.tileCollide = true;
            Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
        }

        public override void AI()
        {
            pulseCounter += 0.05f;
            if (pulseCounter > MathHelper.TwoPi)
                pulseCounter -= MathHelper.TwoPi;

            oldPosition = Projectile.oldPos[0];
            for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1];
            }
            Projectile.oldPos[0] = Projectile.position;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float pulse = 0.5f + (float)Math.Sin(pulseCounter) * 0.25f;
            float scale = 1f + pulse * 0.08f;

            Color glowColor = new Color(120, 255, 140); 

            for (int i = 0; i < 6; i++) 
            {
                float alpha = 0.45f - i * 0.07f;
                float offsetDist = 1.5f + i * 0.8f;

                for (int k = 0; k < 4; k++)
                {
                    Vector2 offset = Vector2.Zero;
                    switch (k)
                    {
                        case 0: offset = new Vector2(-offsetDist, 0); break;
                        case 1: offset = new Vector2(offsetDist, 0); break;
                        case 2: offset = new Vector2(0, -offsetDist); break;
                        case 3: offset = new Vector2(0, offsetDist); break;
                    }

                    Main.EntitySpriteDraw(texture, drawPos + offset, null,
                        glowColor * (alpha * (0.7f + pulse * 0.3f)),
                        Projectile.rotation, drawOrigin, scale + i * 0.03f,
                        SpriteEffects.None, 0);
                }
            }

            Main.EntitySpriteDraw(texture, drawPos, null,
                Color.White * (0.25f + pulse * 0.1f),
                Projectile.rotation, drawOrigin, scale * 1.15f, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(texture, drawPos, null, lightColor,
                Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);

            if (Projectile.oldPos[1] != default)
            {
                Vector2 trailPos = Projectile.oldPos[1] - Main.screenPosition;
                Main.EntitySpriteDraw(texture, trailPos, null,
                    new Color(80, 255, 120) * 0.35f,
                    Projectile.rotation, drawOrigin, scale * 0.9f, SpriteEffects.None, 0);
            }

            return false;
        }

        const int dust_count = 10;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int counter = 0; counter < dust_count; counter++)
            {
                Vector2 velocity = Projectile.velocity * ((float)Main.rand.Next(20, 140) / 100f);
                Dust.NewDust(Projectile.Center, 1, 1, 2, velocity.X, velocity.Y);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }
            return true;
        }
    }
}