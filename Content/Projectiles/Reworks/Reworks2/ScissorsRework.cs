using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class ScissorsRework : ModProjectile
    {
        private float baseRotationSpeed = 0.3f; // базовая скорость вращения
        private float rotationAmplitude = 0.25f; // амплитуда колебаний скорости
        private int rotationTimer = 0; 
        private int fadeTimer = 0; 
        private const int fadeDuration = 30; // кадры для fade-in/fade-out

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.scale = 1.25f;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 999999;

            Projectile.alpha = 255; 
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (fadeTimer < fadeDuration)
            {
                fadeTimer++;
                Projectile.alpha = 255 - (int)(255f * (fadeTimer / (float)fadeDuration));
                if (fadeTimer == 1)
                {
       
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverCoin, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 150, default, 1.2f);
                    }
                }
            }

            Projectile.Center = player.Center;

            rotationTimer++;
            float speedOffset = (float)System.Math.Sin(rotationTimer * 0.05f) * rotationAmplitude;
            float currentRotationSpeed = baseRotationSpeed + speedOffset;
            Projectile.rotation += currentRotationSpeed;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation * player.direction;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                Projectile.rotation + (player.direction == 1 ? 0f : MathHelper.Pi));
            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full,
                Projectile.rotation + (player.direction == 1 ? 0f : MathHelper.Pi));
            player.fullRotationOrigin = player.Size / 2f;
            player.fullRotation = (float)System.Math.Sin(rotationTimer * 0.1f) * 0.1f;
            Projectile.velocity = Vector2.Zero;
            if (!player.channel || player.dead)
            {
                if (Projectile.alpha < 255)
                    Projectile.alpha += 10; 
                else
                {
                    for (int i = 0; i < 20; i++)
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverCoin, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 150, default, 1.3f);

                    player.fullRotation = 0f;
                    Projectile.Kill();
                }
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int extraSize = 24;
            hitbox.Inflate(extraSize, extraSize);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D trailTexture = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/ScissorsRework_Trail").Value;
            Texture2D projectileTexture = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawOrigin = new Vector2(projectileTexture.Width * 0.5f, projectileTexture.Height * 0.5f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = Color.White * alpha * 0.8f * (1f - Projectile.alpha / 255f);

                Main.EntitySpriteDraw(
                    trailTexture,
                    drawPos,
                    null,
                    color,
                    Projectile.rotation,
                    new Vector2(trailTexture.Width / 2f, trailTexture.Height / 2f),
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                projectileTexture,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
