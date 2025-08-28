using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile;
//roa
public sealed class PrimordialBlood : ModProjectile 
{
    public override void SetStaticDefaults() 
    {
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        Main.projFrames[Projectile.type] = 5;
    }

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public override bool PreDraw(ref Color lightColor) 
    {
        SpriteBatch spriteBatch = Main.spriteBatch;
        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("RoA/Resources/Textures/VisualEffects/DefaultSparkle");
        Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
        SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        for (int k = 0; k < Projectile.oldPos.Length - 1; k++) 
        {
            Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            // Red trail fading to dark red
            Color color = new Color(200, 50 - k * 2, 50 + k * 1, 50); 
            spriteBatch.Draw(texture, drawPos, null, color * 0.2f, Projectile.oldRot[k] + (float)Math.PI / 2, drawOrigin, 
                           Projectile.scale - k / (float)Projectile.oldPos.Length, effects, 0f);
        }
        return true;
    }

    public override void SetDefaults() 
    {
        Projectile.hostile = true;
        Projectile.width = Projectile.height = 12;
        Projectile.aiStyle = 0;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 1;
        Projectile.timeLeft = 360;
        DrawOffsetX = -6;
        DrawOriginOffsetY = -2;
    }

    public override void AI() 
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        
        Lighting.AddLight(Projectile.Center, 0.8f, 0.1f, 0.1f); 

        if (Main.netMode != NetmodeID.Server && Main.rand.NextBool(5)) 
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                                   DustID.Blood, 0f, 0f, 100, default, 1f);
            Main.dust[dust].velocity *= 0.3f;
            Main.dust[dust].noGravity = true;
        }
    }

    public override void PostAI() 
    {
        Projectile.frameCounter++;
        if (Projectile.frameCounter > 5) 
        {
            Projectile.frame++;
            Projectile.frameCounter = 0;
            if (Projectile.frame >= 5) 
            {
                Projectile.frame = 0;
            }
        }
    }

    public override void OnKill(int timeLeft) 
    {
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                        DustID.LifeDrain, Projectile.velocity.X * 0.5f, 
                        Projectile.velocity.Y * 0.5f, 125, default, 1.2f);
        }
    }
}