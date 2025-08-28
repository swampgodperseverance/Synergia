using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageAxe2 : ModProjectile
    {
        private const int FadeInDuration = 30;    // 
        private const int SlowdownDuration = 60; 
        private const float MaxRotationSpeed = 0.5f; 
        private const float AttackSpeed = 12f;    
        private const float AttackRotationSpeed = 0.7f; 
        
        private enum AxeState
        {
            Spinning,      
            SlowingDown,   
            Attacking   
        }
        
        private AxeState _currentState = AxeState.Spinning;
        private float _rotationSpeed = MaxRotationSpeed;
        private Player _target;
        private int _timer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.damage = 10;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 255; 
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            _target ??= Main.player[Projectile.owner];

            _timer++;
            
            switch (_currentState)
            {
                case AxeState.Spinning:
                    HandleSpinningState();
                    break;
                    
                case AxeState.SlowingDown:
                    HandleSlowingDownState();
                    break;
                    
                case AxeState.Attacking:
                    HandleAttackingState();
                    break;
            }

            _rotationSpeed = MathHelper.Clamp(_rotationSpeed, 0f, MaxRotationSpeed);

            Projectile.rotation += _rotationSpeed * Projectile.direction;
        }

        private void HandleSpinningState()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 255 / FadeInDuration;
                if (Projectile.alpha < 0) 
                    Projectile.alpha = 0;
            }

            if (Projectile.alpha == 0 && _timer >= FadeInDuration)
            {
                _currentState = AxeState.SlowingDown;
                _timer = 0; 
            }
        }

        private void HandleSlowingDownState()
        {
            _rotationSpeed -= MaxRotationSpeed / SlowdownDuration;
            
            if (_rotationSpeed <= 0f || _timer >= SlowdownDuration)
            {
                _currentState = AxeState.Attacking;

                if (_target != null && _target.active)
                {
                    Vector2 direction = Vector2.Normalize(_target.Center - Projectile.Center);
                    Projectile.velocity = direction * AttackSpeed;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                }
                
                _rotationSpeed = AttackRotationSpeed;
                _timer = 0;
            }
        }

        private void HandleAttackingState()
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Color drawColor = Projectile.GetAlpha(lightColor);
            
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition,
                    null,
                    drawColor * trailAlpha * 0.5f,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale * (1f - i * 0.1f),
                    SpriteEffects.None,
                    0);
            }
            
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0);
            
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.Blood, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
            }
        }
    }
}