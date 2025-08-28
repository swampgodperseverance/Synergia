using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageBone : ModProjectile
    {
        private enum BoneState { Appearing, Flying, Splitting }
        private BoneState currentState = BoneState.Appearing;
        private int timer = 0;
        private const int AppearTime = 30; 
        private const int FlyTime = 100;   
        private const float StartSpeed = 5f;
        private float currentSpeed = StartSpeed;
        private float currentRotationSpeed = 0f; 
        private const float MaxRotationSpeed = 0.4f; 

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = AppearTime + FlyTime + 60;
            Projectile.penetrate = 1;
            Projectile.alpha = 255; 
        }

        public override void AI()
        {
            timer++;

            switch (currentState)
            {
                case BoneState.Appearing:
                    HandleAppearingState();
                    break;

                case BoneState.Flying:
                    HandleFlyingState();
                    break;

                case BoneState.Splitting:
                    HandleSplittingState();
                    break;
            }

            Projectile.rotation += currentRotationSpeed * Projectile.direction;
        }

        private void HandleAppearingState()
        {

            Projectile.alpha = (int)MathHelper.Lerp(255, 0, timer / (float)AppearTime);
            

            currentRotationSpeed = MathHelper.Lerp(0f, MaxRotationSpeed * 0.5f, timer / (float)AppearTime);

            if (Main.rand.NextBool(10))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(15, 15),
                    DustID.GoldFlame,
                    Vector2.Zero,
                    100,
                    default,
                    1.2f
                ).noGravity = true;
            }

            if (timer >= AppearTime)
            {
                currentState = BoneState.Flying;
                timer = 0;
            }
        }

        private void HandleFlyingState()
        {

            currentRotationSpeed = MathHelper.Lerp(MaxRotationSpeed * 0.5f, MaxRotationSpeed, timer / (float)FlyTime * 2f);

            currentSpeed = MathHelper.Lerp(StartSpeed, 1f, timer / (float)FlyTime);
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * currentSpeed;

            if (Main.rand.NextBool(6))
            {
                Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(12, 12),
                    DustID.GoldFlame,
                    Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f),
                    120,
                    default,
                    1.1f
                ).noGravity = true;
            }

            if (timer >= FlyTime)
            {
                SplitIntoParts();
                currentState = BoneState.Splitting;
                SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/BrokenBone"), Projectile.Center);
                timer = 0;
            }
        }

        private void HandleSplittingState()
        {
            Projectile.alpha += 10; // Быстро исчезаем после разделения
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }

        private void SplitIntoParts()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            for (int i = 0; i < 2; i++)
            {
                Vector2 velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * (i == 0 ? -1 : 1)) * 1.2f; 
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    i == 0 ? ModContent.ProjectileType<CarnageBone1>() : ModContent.ProjectileType<CarnageBone2>(),
                    Projectile.damage / 2,
                    0f,
                    Projectile.owner
                );
            }

            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.Center,
                    Main.rand.NextBool(3) ? DustID.Blood : DustID.GoldFlame,
                    Main.rand.NextVector2Circular(5f, 5f),
                    Main.rand.NextBool(3) ? 0 : 150,
                    default,
                    Main.rand.NextFloat(1.5f, 2.5f)
                ).noGravity = Main.rand.NextBool(3);
            }
        }

   public override bool PreDraw(ref Color lightColor)
        {
            Color drawColor = Projectile.GetAlpha(lightColor) * ((255 - Projectile.alpha) / 255f);
            
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
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