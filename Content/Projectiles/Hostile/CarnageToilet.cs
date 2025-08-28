using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class CarnageToilet : ModProjectile
    {
        private const float Gravity = 0.4f;
        private const float RotationSpeed = 0.1f;
        private float spinFactor = 1f;

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.velocity.Y += Gravity * Main.rand.NextFloat(0.9f, 1.1f);

            spinFactor += 0.01f;
            Projectile.rotation += RotationSpeed * spinFactor * Projectile.direction;

            if (++Projectile.frameCounter > 15)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            if (Main.rand.NextBool(12))
            {
                Vector2 dustPosition = Projectile.Center + new Vector2(0, 8).RotatedBy(Projectile.rotation);
                Dust.NewDustPerfect(
                    dustPosition,
                    DustID.GoldFlame,
                    new Vector2(Projectile.velocity.X * 0.3f, Main.rand.NextFloat(-0.5f, 0.5f)),
                    100,
                    default,
                    1.4f
                ).noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            BreakToilet();
            return true;
        }

        private void BreakToilet()
        {
            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_16") 
            { 
                Pitch = -0.7f,
                Volume = 1.2f
            }, Projectile.Center);

            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Pot,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    100,
                    default,
                    Main.rand.NextFloat(1f, 2f)
                );
            }

            for (int i = 0; i < 25; i++)
            {
                Vector2 velocity = new Vector2(0, Main.rand.NextFloat(-4f, -1f)).RotatedByRandom(MathHelper.Pi);
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GoldFlame,
                    velocity,
                    150,
                    default,
                    Main.rand.NextFloat(1.5f, 2.5f)
                ).noGravity = true;
            }

            for (int i = 0; i < 5; i++)
            {
                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-5f, -2f)),
                    Main.rand.Next(16, 18),
                    Main.rand.NextFloat(0.8f, 1.2f)
                );
            }
        }

        public override void OnKill(int timeLeft)
        {
            BreakToilet();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            Vector2 origin = frame.Size() / 2f + new Vector2(0, 4); 

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame,
                lightColor,
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