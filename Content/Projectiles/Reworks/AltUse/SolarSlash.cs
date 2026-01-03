using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class SolarSlash : ModProjectile
    {
        private const int FadeInTime = 6;
        private const int HoldTime = 8;
        private const int FadeOutTime = 12;
        private const int TotalTime = FadeInTime + HoldTime + FadeOutTime;
        
        private float squish;
        private Vector2 scaleMultiplier = Vector2.One;
        private bool mirrored = false;
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 1000;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 86;
            Projectile.height = 209;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = TotalTime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.scale = 0.8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        
        public override void OnSpawn(IEntitySource source)
        {
            mirrored = Main.LocalPlayer.GetModPlayer<SolarSlashMirrorPlayer>().ShouldMirror();
        }
        
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            float timer = Projectile.localAI[0];
            
            if (timer <= FadeInTime)
            {
                float fadeProgress = timer / FadeInTime;
                fadeProgress = fadeProgress * fadeProgress;
                Projectile.alpha = (int)(255f * (1f - fadeProgress));
                scaleMultiplier = new Vector2(1f + (1f - fadeProgress) * 0.3f, 1f - (1f - fadeProgress) * 0.2f);
            }
            else if (timer > FadeInTime + HoldTime)
            {
                float fadeOutProgress = (timer - (FadeInTime + HoldTime)) / FadeOutTime;
                fadeOutProgress = fadeOutProgress * fadeOutProgress;
                Projectile.alpha = (int)(255f * fadeOutProgress);
                scaleMultiplier = new Vector2(1f + fadeOutProgress * 0.2f, 1f - fadeOutProgress * 0.4f);
            }
            else
            {
                Projectile.alpha = 0;
                scaleMultiplier = Vector2.One;
            }
            
            float rotationSpeed = 0.15f;
            if (Projectile.velocity.X != 0f)
            {
                float targetRotation = Projectile.velocity.ToRotation();
                
                Projectile.rotation = Projectile.rotation.AngleLerp(targetRotation, rotationSpeed);
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
            }
            
            float pulse = (float)Math.Sin(timer * 0.3f) * 0.1f + 1f;
            squish = (float)Math.Sin(timer * 0.4f) * 0.15f;
            
            Projectile.scale = 0.8f * pulse * (scaleMultiplier.X + scaleMultiplier.Y) / 2f;
            
            if (timer > FadeInTime)
            {
                float slowdownFactor = 1f - (timer - FadeInTime) / (TotalTime - FadeInTime) * 0.8f;
                slowdownFactor = Math.Max(slowdownFactor, 0.2f);
                Projectile.velocity *= slowdownFactor;
            }
            
            if (Main.rand.NextBool(3) && Projectile.alpha < 200)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(20, 20),
                    DustID.Torch,
                    Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(2, 2),
                    100,
                    new Color(255, 200, 100),
                    1.2f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }
            
            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.6f, 0.2f) * (1f - Projectile.alpha / 255f));
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            
            SpriteEffects effects = SpriteEffects.None;
            if (mirrored)
                effects = SpriteEffects.FlipHorizontally;
            
            Color glowColor = new Color(255, 200, 100, 150) * (1f - Projectile.alpha / 255f);
            Color mainColor = Color.Lerp(lightColor, Color.White, 0.6f) * (1f - Projectile.alpha / 255f);
            
            Vector2 scale = new Vector2(
                Projectile.scale + squish * scaleMultiplier.X,
                Projectile.scale - squish * 0.7f * scaleMultiplier.Y
            );
            
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;
                    
                float progress = i / (float)Projectile.oldPos.Length;
                float trailAlpha = (1f - progress) * 0.4f * (1f - Projectile.alpha / 255f);
                
                float rotation;
                if (i < Projectile.oldRot.Length - 1 && Projectile.oldRot[i + 1] != 0)
                    rotation = Projectile.oldRot[i];
                else
                    rotation = Projectile.rotation;
                
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Vector2 trailScale = scale * (0.6f + progress * 0.4f);
                
                Color trailColor = new Color(200, 80, 20, 100) * trailAlpha;
                Main.EntitySpriteDraw(
                    texture,
                    trailPos,
                    null,
                    trailColor,
                    rotation,
                    origin,
                    trailScale * 0.9f,
                    effects,
                    0
                );
                
                Color lightTrailColor = new Color(255, 180, 60, 50) * trailAlpha;
                Main.EntitySpriteDraw(
                    texture,
                    trailPos,
                    null,
                    lightTrailColor,
                    rotation,
                    origin,
                    trailScale * 1.1f,
                    effects,
                    0
                );
            }
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * 0.3f,
                Projectile.rotation,
                origin,
                scale * 1.4f,
                effects,
                0
            );
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * 0.6f,
                Projectile.rotation,
                origin,
                scale * 1.2f,
                effects,
                0
            );
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                origin,
                scale,
                effects,
                0
            );
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                new Color(255, 255, 200, 100) * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                origin,
                scale * 0.7f,
                effects,
                0
            );
            
            return false;
        }
        
        public override void PostDraw(Color lightColor)
        {
            if (Projectile.alpha < 200)
            {
                Texture2D bloomTexture = ModContent.Request<Texture2D>("Terraria/Images/Extra_89").Value;
                Color bloomColor = new Color(255, 180, 60, 100) * (1f - Projectile.alpha / 255f) * 0.5f;
                float bloomScale = Projectile.scale * 0.8f;
                
                Main.EntitySpriteDraw(
                    bloomTexture,
                    Projectile.Center - Main.screenPosition,
                    null,
                    bloomColor,
                    Projectile.rotation * 0.5f,
                    bloomTexture.Size() * 0.5f,
                    bloomScale,
                    SpriteEffects.None,
                    0
                );
            }
        }
        
        public override bool? CanHitNPC(NPC target)
        {
            if (target.friendly || Projectile.alpha > 220)
                return false;
            
            return null;
        }
        
        public override bool? CanCutTiles() => false;
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
            
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    target.Center,
                    DustID.Torch,
                    Main.rand.NextVector2Circular(5, 5),
                    100,
                    new Color(255, 200, 50),
                    1.5f
                );
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }
        }
    }
    
    public class SolarSlashMirrorPlayer : ModPlayer
    {
        public int slashCounter = 0;
        
        public bool ShouldMirror()
        {
            bool shouldMirror = slashCounter % 2 == 0;
            slashCounter++;
            return shouldMirror;
        }
        
        public override void OnEnterWorld()
        {
            slashCounter = 0;
        }
    }
}