using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class SpiritualDisc : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4; 
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.damage = 30;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            AnimateProjectile();

              Projectile.rotation += 0.0f;

              CreateBlueDust();

             AdjustVelocity();

              Lighting.AddLight(Projectile.Center, 0.2f, 0.4f, 1f);
        }

        private void AnimateProjectile()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5) 
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        private void CreateBlueDust()
        {

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position - Projectile.velocity * 0.5f, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.BlueFairy,
                    0f, 0f, 100, default, 1.2f
                );
                
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.position -= Projectile.velocity * 0.5f;
            }

            if (Main.rand.NextBool(3))
            {
                Vector2 offset = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                Dust sideDust = Dust.NewDustDirect(
                    Projectile.Center + offset, 
                    10, 10, 
                    DustID.BlueTorch,
                    0f, 0f, 150, default, 0.8f
                );
                
                sideDust.noGravity = true;
                sideDust.velocity = offset * 0.05f + Projectile.velocity * 0.1f;
            }

            if (Main.rand.NextBool(5))
            {
                Dust spark = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Electric,
                    0f, 0f, 0, default, 1.1f
                );
                
                spark.noGravity = true;
                spark.velocity = Main.rand.NextVector2Circular(2f, 2f) + Projectile.velocity * 0.1f;
            }
        }

        private void AdjustVelocity()
        {

            if (Projectile.velocity.Length() < 12f)
            {
                Projectile.velocity *= 1.02f;
            }

            if (Main.rand.NextBool(30))
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2;
                Color trailColor = new Color(100, 150, 255, 100) * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length) * 0.5f;
                float trailScale = Projectile.scale * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length) * 0.8f;
                
                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height),
                    trailColor,
                    Projectile.rotation,
                    new Vector2(Projectile.width / 2, Projectile.height / 2),
                    trailScale,
                    SpriteEffects.None,
                    0
                );
            }


            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height),
                lightColor,
                Projectile.rotation,
                new Vector2(Projectile.width / 2, Projectile.height / 2),
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            Color glowColor = new Color(100, 150, 255, 0) * 0.4f;
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height),
                glowColor,
                Projectile.rotation,
                new Vector2(Projectile.width / 2, Projectile.height / 2),
                Projectile.scale * 1.1f,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item10 with { Pitch = 0.5f }, Projectile.Center);

            for (int i = 0; i < 25; i++)
            {
                Dust explosionDust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.BlueFairy,
                    Main.rand.NextFloat(-3f, 3f), 
                    Main.rand.NextFloat(-3f, 3f), 
                    100, default, 1.5f
                );
                
                explosionDust.noGravity = true;
                explosionDust.velocity *= 2f;
            }

            for (int i = 0; i < 10; i++)
            {
                Dust electricDust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Electric,
                    Main.rand.NextFloat(-5f, 5f), 
                    Main.rand.NextFloat(-5f, 5f), 
                    0, default, 1.2f
                );
                
                electricDust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust hitDust = Dust.NewDustDirect(
                    target.position, 
                    target.width, 
                    target.height, 
                    DustID.BlueTorch,
                    Main.rand.NextFloat(-2f, 2f), 
                    Main.rand.NextFloat(-2f, 2f), 
                    0, default, 1f
                );
                
                hitDust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item78 with { Pitch = 0.2f }, Projectile.Center);
        }
    }
}