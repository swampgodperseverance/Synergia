using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public enum OrbitDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class GloomProjOrbit : ModProjectile
    {
        private const float LifeTime = 1160f; 
        private const float PulseSpeed = 5f;  
        private const float ShakeAmplitude = 5f; // насколько сильно качается
        private const float ShakeSpeed = 2f;    
        private OrbitDirection direction = OrbitDirection.Up;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 3;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = (int)LifeTime;
            Projectile.alpha = 255;  
            Projectile.scale = 0.8f;
        }

       
        public void SetDirection(OrbitDirection dir)
        {
            direction = dir;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

      
            Vector2 baseOffset = Vector2.Zero;
            switch (direction)
            {
                case OrbitDirection.Up:
                    baseOffset = new Vector2(0, -40); 
                    Projectile.rotation = MathHelper.PiOver2;
                    break;
                case OrbitDirection.Down:
                    baseOffset = new Vector2(0, 40);
                    Projectile.rotation = -MathHelper.PiOver2;
                    break;
                case OrbitDirection.Left:
                    baseOffset = new Vector2(-40, 0);
                    Projectile.rotation = MathHelper.Pi;
                    break;
                case OrbitDirection.Right:
                    baseOffset = new Vector2(40, 0);
                    Projectile.rotation = 0f;
                    break;
            }

        
            float shake = ShakeAmplitude * (float)Math.Sin(Main.GlobalTimeWrappedHourly * ShakeSpeed + Projectile.whoAmI);
            Vector2 offset = baseOffset;

            if (direction == OrbitDirection.Left || direction == OrbitDirection.Right)
                offset.Y += shake; 
            else
                offset.X += shake; 

       
            Projectile.Center = player.Center + offset;

     
            if (Projectile.timeLeft > LifeTime - 30 && Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0) Projectile.alpha = 0;
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255) Projectile.alpha = 255;
            }

       
            float pulse = 1f + 0.05f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * PulseSpeed);
            Projectile.scale = pulse;

         
            Lighting.AddLight(Projectile.Center, 0.08f, 0.12f, 0.25f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

     
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = ((float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length) * 0.6f;
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float scale = Projectile.scale * (0.7f + 0.3f * ((float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length));
                Color trailColor = Color.Cyan * alpha;
                Main.EntitySpriteDraw(texture, pos, null, trailColor, Projectile.rotation, origin, scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override bool? CanDamage() => false;

        public override void OnKill(int timeLeft)
        {

            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard);
                d.velocity = Main.rand.NextVector2Circular(2f, 2f);
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                d.noGravity = true;
            }
        }
    }
}
