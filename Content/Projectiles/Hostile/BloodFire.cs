using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile   
{
    public class BloodFire : ModProjectile
    {
        private const int HoverTime = 120;
        private const float MaxSpeed = 15f;
        private const float Acceleration = 0.3f;
        
        private ref float Timer => ref Projectile.ai[0];
        private ref float CurrentSpeed => ref Projectile.ai[1];
        
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            Timer++;

            if (Timer <= HoverTime)
            {
                Projectile.velocity *= 0.95f; 

                if (Timer % 30 == 0)
                {
                    Projectile.velocity = Main.rand.NextVector2Circular(1f, 1f);
                }
                
                if (Main.rand.NextBool(3))
                {
                    Dust bloodDust = Dust.NewDustDirect(
                        Projectile.position, 
                        Projectile.width, 
                        Projectile.height, 
                        DustID.Blood, 
                        0f, 0f, 100, default, 1.5f);
                    bloodDust.noGravity = true;
                    bloodDust.velocity *= 0.5f;
                }
                
                return;
            }

            Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            
            if (target.active && !target.dead)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();

                CurrentSpeed = MathHelper.Clamp(CurrentSpeed + Acceleration, 0f, MaxSpeed);

                float turnSpeed = 0.1f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * CurrentSpeed, turnSpeed);
                
                if (Main.rand.NextBool(2))
                {
                    Dust trailDust = Dust.NewDustDirect(
                        Projectile.position, 
                        Projectile.width, 
                        Projectile.height, 
                        DustID.Blood, 
                        0f, 0f, 100, default, 2f);
                    trailDust.noGravity = true;
                    trailDust.velocity = -Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f);
                }
            }


            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    CreateBloodExplosion();
                    Projectile.Kill();
                }
            }
        }

        private void CreateBloodExplosion()
        {

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.8f, Pitch = -0.2f }, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust explosionDust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Blood, 
                    0f, 0f, 100, default, 2.5f);
                explosionDust.noGravity = true;
                explosionDust.velocity = Main.rand.NextVector2Circular(10f, 10f);
                
                if (i % 3 == 0)
                {
                    Dust fireDust = Dust.NewDustDirect(
                        Projectile.position, 
                        Projectile.width, 
                        Projectile.height, 
                        DustID.Torch, 
                        0f, 0f, 150, new Color(255, 50, 50), 3f);
                    fireDust.noGravity = true;
                    fireDust.velocity = Main.rand.NextVector2Circular(8f, 8f);
                }
            }

            for (int i = 0; i < 15; i++)
            {
                float angle = MathHelper.TwoPi * (i / 15f);
                Vector2 velocity = angle.ToRotationVector2() * 8f;
                
                Dust waveDust = Dust.NewDustDirect(
                    Projectile.Center, 
                    0, 0, 
                    DustID.Blood, 
                    velocity.X, velocity.Y, 100, default, 2f);
                waveDust.noGravity = true;
            }

            float explosionRadius = 150f;
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && 
                    Vector2.Distance(player.Center, Projectile.Center) <= explosionRadius)
                {
                    player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByProjectile(player.whoAmI, Projectile.whoAmI), 
                              40, player.direction);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, 4, 0, Projectile.frame);
            
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f / 4f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            
            Main.EntitySpriteDraw(
                texture,
                drawPos,
                frame,
                Color.White * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0);
            
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.alpha < 255)
            {
                CreateBloodExplosion();
            }
        }
    }
}