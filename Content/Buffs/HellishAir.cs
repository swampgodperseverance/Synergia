using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Synergia.Content.Buffs
{
    public class HellishAir : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen = -300;

            if (player.statLife > 1)
            {
                player.lifeRegenTime = 0;
            }

            player.GetModPlayer<HellishAirPlayer>();
        }
    }

    public class HellishAirPlayer : ModPlayer
    {
        private int hellishAirProjectileIndex = -1;

        public override void PostUpdate()
        {
            if (Player.HasBuff<HellishAir>())
            {
                if (hellishAirProjectileIndex == -1 || !Main.projectile[hellishAirProjectileIndex].active || Main.projectile[hellishAirProjectileIndex].type != ModContent.ProjectileType<HellishAirRune>())
                {
                    int proj = Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<HellishAirRune>(),
                        0,
                        0,
                        Player.whoAmI
                    );
                    hellishAirProjectileIndex = proj;
                }
            }
            else
            {
                if (hellishAirProjectileIndex != -1)
                {
                    Projectile proj = Main.projectile[hellishAirProjectileIndex];
                    if (proj.active && proj.type == ModContent.ProjectileType<HellishAirRune>())
                    {
                        proj.Kill();
                    }
                    hellishAirProjectileIndex = -1;
                }
            }
        }
    }

    public class HellishAirRune : ModProjectile
    {
        private float fadeIn = 0f;
        private float fadeOut = 1f;
        private bool isDying = false;
        private float frameLerp = 0f;
        private int currentFrame = 0;
        private int nextFrame = 1;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.hide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.HasBuff<HellishAir>())
            {
                if (!isDying)
                {
                    isDying = true;
                }

                fadeOut -= 0.03f;
                Projectile.alpha = (int)(255 * (1f - fadeOut));

                if (fadeOut <= 0f)
                {
                    Projectile.Kill();
                }
                return;
            }

            if (fadeIn < 1f && !isDying)
            {
                fadeIn += 0.03f;
                Projectile.alpha = (int)(255 * (1f - fadeIn));
            }

            Projectile.timeLeft = 2;
            Projectile.Center = player.Bottom + new Vector2(0, 20);

            frameLerp += 0.03f;

            if (frameLerp >= 1f)
            {
                frameLerp = 0f;
                currentFrame = nextFrame;
                nextFrame = (nextFrame + 1) % 3;
            }

            Projectile.rotation += 0.02f;
            Projectile.scale = 1f + (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 0.1f;

            if (Main.rand.NextBool(3))
            {
                Vector2 pos = player.Center + new Vector2(
                    Main.rand.NextFloat(-30, 30),
                    Main.rand.NextFloat(-40, 10)
                );

                Dust dust = Dust.NewDustPerfect(pos, 6, Vector2.Zero, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity = new Vector2(0, -Main.rand.NextFloat(1, 3));
                dust.fadeIn = 1.5f;

                if (Main.rand.NextBool(2))
                {
                    Dust dust2 = Dust.NewDustPerfect(pos, 174, Vector2.Zero, 0, default, 1.5f);
                    dust2.noGravity = true;
                    dust2.velocity = new Vector2(Main.rand.NextFloat(-2, 2), -Main.rand.NextFloat(1, 4));
                }
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 pos = player.Center + new Vector2(Main.rand.NextFloat(-40, 40), Main.rand.NextFloat(-30, 10));
                Gore.NewGore(Projectile.GetSource_FromThis(), pos, new Vector2(0, -1), 99, 0.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            Rectangle frame1 = texture.Frame(1, 3, 0, currentFrame);
            Rectangle frame2 = texture.Frame(1, 3, 0, nextFrame);

            Vector2 origin = frame1.Size() / 2f;
            Color color = Color.White * (1f - Projectile.alpha / 255f);

            float glowAlpha = (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.3f + 0.4f;

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(3, 0).RotatedBy(MathHelper.PiOver2 * i + Main.GameUpdateCount * 0.02f);
                Main.spriteBatch.Draw(
                    texture,
                    Projectile.Center - Main.screenPosition + offset,
                    frame1,
                    new Color(255, 30, 30, 0) * (1f - Projectile.alpha / 255f) * 0.4f * glowAlpha,
                    Projectile.rotation,
                    origin,
                    Projectile.scale * 1.2f,
                    SpriteEffects.None,
                    0f
                );
            }

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame1,
                color * (1f - frameLerp),
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame2,
                color * frameLerp,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            float pulseAlpha = (float)Math.Sin(Main.GameUpdateCount * 0.15f) * 0.2f + 0.3f;

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame1,
                Color.Red * (pulseAlpha * (1f - Projectile.alpha / 255f) * (1f - frameLerp)),
                Projectile.rotation,
                origin,
                Projectile.scale * 1.3f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame2,
                Color.OrangeRed * (pulseAlpha * (1f - Projectile.alpha / 255f) * frameLerp),
                Projectile.rotation,
                origin,
                Projectile.scale * 1.3f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
