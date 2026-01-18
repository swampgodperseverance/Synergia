using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class ScissorsRework2 : ModProjectile
    {
        private float rotationSpeed = 1f; 

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120; 
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.localAI[0] == 0)
            {
                Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);
                if (direction.HasNaNs())
                    direction = Vector2.UnitX;

                Projectile.velocity = direction * 25f;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1.2f, PitchVariance = 0.2f }, player.Center);

                Projectile.localAI[0] = 1f;
            }

            Projectile.velocity *= 0.96f;

            rotationSpeed = MathHelper.Lerp(rotationSpeed, 0.15f, 0.05f);
            Projectile.rotation += rotationSpeed * Projectile.direction;

            if (Projectile.timeLeft < 30)
                rotationSpeed *= 0.95f;


            Vector2 start = player.Center;
            Vector2 end = Projectile.Center;
            Vector2 line = end - start;
            int steps = (int)(line.Length() / 7f); // немного плотнее, чтобы при меньших дастах линия оставалась видимой

            for (int i = 0; i < steps; i++)
            {
                Vector2 basePos = Vector2.Lerp(start, end, i / (float)steps);

                Vector2 offset = Main.rand.NextVector2Circular(6f, 6f);
                Vector2 pos = basePos + offset;

    
                float t = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f + i * 0.3f) * 0.5f + 0.5f;
                Color dustColor = Color.Lerp(new Color(240, 240, 255), new Color(180, 200, 255), t);


                Dust d = Dust.NewDustPerfect(pos, DustID.SilverCoin, Vector2.Zero, 100, dustColor, Main.rand.NextFloat(0.12f, 0.22f));
                d.noGravity = true;
                d.fadeIn = 0.6f;
                d.velocity = Main.rand.NextVector2Circular(0.4f, 0.4f);
            }


            if (Projectile.timeLeft <= 2)
            {
                player.Teleport(Projectile.Center, TeleportationStyleID.RodOfDiscord);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8 with { Volume = 0.9f, Pitch = -0.3f }, Projectile.Center);

                for (int i = 0; i < 70; i++)
                {
                    Vector2 randPos = Vector2.Lerp(start, end, Main.rand.NextFloat()) + Main.rand.NextVector2Circular(14f, 14f);
                    Vector2 vel = Main.rand.NextVector2Circular(5f, 5f);

                    Color dustColor = Color.Lerp(new Color(255, 255, 255), new Color(180, 200, 255), Main.rand.NextFloat());
                    Dust d = Dust.NewDustPerfect(randPos, DustID.SilverCoin, vel, 80, dustColor, Main.rand.NextFloat(0.15f, 0.25f));
                    d.noGravity = true;
                    d.fadeIn = 0.8f;
                }


                for (int i = 0; i < 20; i++)
                {
                    Vector2 vel = Main.rand.NextVector2CircularEdge(4f, 4f);
                    Color flashColor = Color.Lerp(new Color(255, 255, 255), new Color(200, 220, 255), Main.rand.NextFloat());
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.SilverCoin, vel, 60, flashColor, 0.25f);
                    d.noGravity = true;
                }
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = Color.White * alpha;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }


            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
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
