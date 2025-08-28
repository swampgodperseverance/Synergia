using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Friendly
{
    public class LightningStrike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.position, 0.8f, 0.8f, 1.2f);
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position + new Vector2(0, Main.rand.Next(Projectile.height)), 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Electric, 
                    0f, 
                    0f, 
                    100, 
                    default, 
                    1.5f 
                );
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(3))
            {
                Vector2 sparkPos = Projectile.position + new Vector2(
                    Main.rand.Next(-Projectile.width, Projectile.width),
                    Main.rand.Next(Projectile.height)
                );
                Dust.NewDustPerfect(
                    sparkPos,
                    DustID.Vortex,
                    new Vector2(0, 0),
                    50,
                    Color.LightBlue,
                    1.8f
                );
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 150, default, 2f);
            }
            SoundEngine.PlaySound(SoundID.Item122.WithPitchOffset(-0.3f), Projectile.Center);
        }
    }
}