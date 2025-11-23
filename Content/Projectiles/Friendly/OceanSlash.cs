using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public class OceanSlash : ModProjectile
    {
        private const int FadeInTime = 8;  // время появления
        private const int FadeOutTime = 10; // время исчезновения

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 40; // Быстро исчезает
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255; // полностью прозрачный при спавне
            Projectile.scale = 1f; // оставляем масштаб неизменным
        }

        public override void AI()
        {
            // Поворот по направлению скорости
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation();

            // Постепенное замедление
            Projectile.velocity *= 0.92f;

            // Плавное появление
            if (Projectile.timeLeft > 40 - FadeInTime)
            {
                float t = 1f - (Projectile.timeLeft - (40 - FadeInTime)) / (float)FadeInTime;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, t);
            }
            // Плавное исчезновение
            else if (Projectile.timeLeft < FadeOutTime)
            {
                float t = 1f - Projectile.timeLeft / (float)FadeOutTime;
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, t);
            }
            else
            {
                Projectile.alpha = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Применяем текущую прозрачность
            lightColor *= 1f - (Projectile.alpha / 255f);
            return true;
        }
    }
}
