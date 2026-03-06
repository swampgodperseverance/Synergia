using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Synergia.Content.Projectiles.Other
{
    public abstract class BaseShortsword : ModProjectile
    {
        public override bool ShouldUpdatePosition() => false;

        public float speed = 1;
        public float offset = 1;
        public int timeMax = 7;

        public float bonusSpeed = 0;
        public Vector2 vel;

        public virtual void SetStats(ref float speed, ref float offset, ref int timeMax)
        {
        }

        public float SetSwingSpeed(float speedBonus)
        {
            Terraria.Player player = Main.player[Projectile.owner];
            return speedBonus * player.GetAttackSpeed(DamageClass.Melee);
        }

        public override void OnSpawn(IEntitySource source)
        {
            SetStats(ref speed, ref offset, ref timeMax);
            SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            vel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * offset * 35;
            bonusSpeed = SetSwingSpeed(1);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);

            if (Projectile.spriteDirection == 1)
            {
                DrawOffsetX = -68;
                DrawOriginOffsetY = -19;
                DrawOriginOffsetX = 20;
            }
            else
            {
                DrawOffsetX = 30;
                DrawOriginOffsetY = -19;
                DrawOriginOffsetX = -20;
            }

            vel += Projectile.velocity * speed * bonusSpeed * 20;
            Projectile.Center = player.MountedCenter + vel;
            Projectile.alpha -= 255 / timeMax;

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= timeMax / speed / bonusSpeed)
                Projectile.Kill();
        }
    }
}