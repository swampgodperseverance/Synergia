using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Synergia.Content.Projectiles.Hostile
{
    public class Shockwave : ModProjectile
    {
        private const float MAX_SCALE = 1f;
        private const float DAMAGE_THRESHOLD = 0.2f;
        private const float SCALE_LERP_SPEED = 0.05f;
        private const float OPACITY_LERP_SPEED = 0.05f;
        private const int FADE_TIME = 60;
        private const int FROZEN_DURATION = 100;
        
        private float opacity = 0f;
        private float initialSpeed = 0f;
        private float acceleration = 0.4f;
        private bool hasDirection = false;
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 1000;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 14;
            Projectile.damage = 40;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 260;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.01f;
            Projectile.alpha = 255;
            Projectile.rotation = 0f;
            Projectile.hide = false;
        }
        
        public override void AI()
        {
            if (!hasDirection && Projectile.velocity != Vector2.Zero)
            {
                hasDirection = true;
                Projectile.rotation = Projectile.velocity.ToRotation();
                initialSpeed = Projectile.velocity.Length();
            }
            
            Projectile.scale = MathHelper.Lerp(Projectile.scale, MAX_SCALE, SCALE_LERP_SPEED);
            
            opacity = MathHelper.Lerp(opacity, 1f, OPACITY_LERP_SPEED);
            
            if (Projectile.timeLeft <= FADE_TIME)
            {
                opacity = Projectile.timeLeft / (float)FADE_TIME;
            }
            
            Projectile.alpha = (int)(255 * (1f - opacity));
            
            Projectile.Size = new Vector2(50) * Projectile.scale;
            
            if (hasDirection && initialSpeed > 0)
            {
                float progress = 1f - (Projectile.timeLeft / 260f);
                float easedProgress = Helpers.EaseFunctions.EaseOutQuad(progress);
                float currentSpeed = initialSpeed * (1f + acceleration * easedProgress);
                
                if (Projectile.velocity != Vector2.Zero)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * currentSpeed;
                }
            }
            
            SpawnDustEffects();
        }
        
        private void SpawnDustEffects()
        {
            if (Main.rand.NextBool(3))
            {
                float progress = Projectile.timeLeft / 260f;
                Vector2 dustPos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Dust dust = Dust.NewDustPerfect(
                    dustPos, 
                    DustID.IceTorch, 
                    Vector2.Zero, 
                    Scale: Projectile.scale * Main.rand.NextFloat(1.2f, 1.8f) * progress
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(1.5f, 1.5f);
                dust.fadeIn = 1f;
            }
        }
        
        public override bool? CanDamage() => Projectile.scale > DAMAGE_THRESHOLD;
        
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!target.HasBuff(BuffID.Frozen) && !target.HasBuff(BuffID.Chilled))
            {
                target.AddBuff(BuffID.Frozen, FROZEN_DURATION);
            }
        }
        
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor * opacity;
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Color drawColor = lightColor * opacity * 0.8f;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;
                
                float progress = 1f - (i / (float)Projectile.oldPos.Length);
                Color trailColor = drawColor * progress * 0.5f;
                float trailScale = Projectile.scale * progress * 0.8f;
                
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    trailScale,
                    SpriteEffects.None,
                    0
                );
            }
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor * 1.2f,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            
            return false;
        }
        
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, 
            List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
    }
}