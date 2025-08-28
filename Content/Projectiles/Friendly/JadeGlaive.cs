using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using ValhallaMod.Projectiles.AI;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public class JadeGlaiveProj : ValhallaGlaive
    {
        private int trailCounter = 0;
        private const int TrailInterval = 3; 
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2; 
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 40;
            DrawOffsetX = 4;
            Projectile.height = 48;
            DrawOriginOffsetY = -0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.alpha = 0;
            Projectile.penetrate = 106;
            Projectile.scale = 0.9f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            
            bounces = 3;
            timeFlying = 24;
            speedHoming = 29f;
            speedFlying = 20f;
            speedComingBack = 100f;
            homingDistanceMax = 600f;
            homingStyle = HomingStyle.BasicGlaive;
            homingStart = true;
            homingIgnoreTile = true;
        }
        
        public override void AI()
        {
            base.AI(); 
            for (int i = 0; i < 3; i++) 
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<ValhallaMod.Dusts.JadeDust>(),
                    Projectile.velocity.X * 0.1f,
                    Projectile.velocity.Y * 0.1f,
                    100,
                    default,
                    1.5f
                );

                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1.0f, 1.5f); 
                dust.velocity *= 0.3f;
                dust.fadeIn = 1.2f;
            }

            if (Main.rand.NextBool(5)) 
   

            trailCounter++;
            if (trailCounter >= TrailInterval)
            {
                trailCounter = 0;

                int dustIndex = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<ValhallaMod.Dusts.JadeDust>(),
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100,
                    default,
                    1f
                );

                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].scale = 0.5f;
            }

            base.AI();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = 1f - (float)i / Projectile.oldPos.Length;
                Color color = lightColor * alpha;
                
                if (Projectile.oldPos[i] != Vector2.Zero)
                {
                    Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width/2, Projectile.height/2);
                    Main.EntitySpriteDraw(
                        texture,
                        drawPos,
                        null,
                        color,
                        Projectile.rotation,
                        new Vector2(texture.Width/2, texture.Height/2),
                        Projectile.scale * (1f - (float)i / Projectile.oldPos.Length * 0.5f), 
                        SpriteEffects.None,
                        0
                    );
                }
            }
            
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(
                texture,
                position,
                null,
                lightColor,
                Projectile.rotation,
                new Vector2(texture.Width/2, texture.Height/2),
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            
            return false; 
        }
    }
}