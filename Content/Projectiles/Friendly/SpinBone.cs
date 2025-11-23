using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Friendly
{
    public class SpinBone : ModProjectile
    {
        private int fadeInTimer = 0;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.damage = 52;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (fadeInTimer < 30)
            {
                fadeInTimer++;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, fadeInTimer / 30f);
            }

            Projectile.rotation += 0.4f * Projectile.direction;

            if (Main.rand.NextBool(3))
            {
                Dust smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 180, default, 1f);
                smoke.velocity *= 0.3f;
                smoke.scale = Main.rand.NextFloat(0.9f, 1.2f);
                smoke.noGravity = true;
            }

            Projectile.velocity.Y += 0.25f;
            if (Projectile.velocity.Y > 20f)
                Projectile.velocity.Y = 20f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<ScreenShakePlayer>().TriggerShake(10, 0.9f);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke,
                    Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 150, default, 1.3f);
            }

            Projectile.Kill();
            return false;
        }
    }
}
