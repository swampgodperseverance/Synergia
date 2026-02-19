using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Synergia.Content.Buffs;

namespace Synergia.Content.Projectiles.Thrower
{
    public class EverwoodJavelin2 : ModProjectile
    {
        private Vector2[] oldPositions = new Vector2[4];

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60 * 5;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.HallowedPlants, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f,
                    100, default, 1.2f);
            }

            for (int i = 0; i < 40; i++)
            {
                float angle = MathHelper.TwoPi * i / 40f;
                Vector2 vel = new Vector2(3.2f, 0).RotatedBy(angle);
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.HallowedPlants, vel.X, vel.Y, 80, default, 1.6f);
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 0.9f;
            }

            if (Main.rand.NextFloat() < 0.2f)
            {
                if (target.boss)
                {
                    target.AddBuff(ModContent.BuffType<EverwoodJavelinDebuff>(), 270);
                }
                target.AddBuff(ModContent.BuffType<EverwoodJavelinDebuff>(), 90);
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.HallowedPlants, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }

            for (int i = 0; i < 40; i++)
            {
                float angle = MathHelper.TwoPi * i / 40f;
                Vector2 vel = new Vector2(4f, 0).RotatedBy(angle);
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.HallowedPlants, vel.X, vel.Y, 60, default, 1.35f);
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 1.1f;
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

            Lighting.AddLight(Projectile.Center + new Vector2(0, -10f), 0.05f, 0.1f, 0.9f);

            for (int i = oldPositions.Length - 1; i > 0; i--)
            {
                oldPositions[i] = oldPositions[i - 1];
            }
            oldPositions[0] = Projectile.Center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < oldPositions.Length; i++)
            {
                if (oldPositions[i] == Vector2.Zero) continue;

                float alpha = 1f - ((float)i / oldPositions.Length);
                float scale = 1f - (0.5f * ((float)i / oldPositions.Length));

                Vector2 drawPos = oldPositions[i] - Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPos, null, lightColor * alpha * 0.5f,
                    Projectile.rotation, origin, Projectile.scale * scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}