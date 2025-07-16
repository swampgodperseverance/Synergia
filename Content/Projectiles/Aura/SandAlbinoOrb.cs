using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class SandAlbinoOrb : AuraDamageAI
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void CustomAI()
        {
            Projectile.rotation += 0.15f;
            Projectile.velocity *= 0.98f;

            // Create dust particles less frequently
            if (Main.rand.NextBool(5))
            {
                CreateDustEffect();
            }
        }

        private void CreateDustEffect()
        {
            int dustType = DustID.PurpleTorch;
            Vector2 dustVelocity = new Vector2(
                Projectile.velocity.X * 0.1f,
                Projectile.velocity.Y * 0.1f
            );

            int dust = Dust.NewDust(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                dustType,
                dustVelocity.X,
                dustVelocity.Y,
                150,
                default,
                1.4f
            );

            if (dust < Main.maxDust)
            {
                Dust dustObj = Main.dust[dust];
                dustObj.noGravity = true;
                dustObj.velocity *= 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                CreateDustEffect();
            }

            // Small explosion effect
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.PurpleTorch,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f),
                    100,
                    default,
                    Main.rand.NextFloat(1f, 1.5f)
                );
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 180, 255, 200);
        }
    }
}