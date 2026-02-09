using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class AquaRework : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];
        private ref float Pulses => ref Projectile.ai[1];
        private ref float Spin => ref Projectile.ai[2];

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.timeLeft = 360;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 1.1f;
            Spin = 0.48f;
        }

        public override void AI()
        {
            Timer++;

            if (Timer <= 40f)
            {
                Projectile.rotation += Spin;
                Spin *= 0.915f;
                Projectile.velocity *= 0.93f;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
                float t = (Timer - 40f) % 60f;
                float pulse = MathF.Sin(t / 60f * MathHelper.TwoPi);
                Projectile.scale = 1.0f + pulse * 0.28f + MathF.Sin((Timer - 40f) * 0.07f) * 0.08f;

                if (t <= 1f)
                {
                    Pulses++;

                    for (int i = 0; i < 16; i++)
                    {
                        Vector2 dir = Main.rand.NextVector2Circular(1.4f, 1.4f);
                        Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.Water,
                            dir * Main.rand.NextFloat(2.2f, 4.8f),
                            0,
                            new Color(90, 170, 255),
                            Main.rand.NextFloat(1.1f, 1.6f)
                        );
                        d.noGravity = true;
                    }

                    for (int i = 0; i < 7; i++)
                    {
                        Dust d = Dust.NewDustPerfect(
                            Projectile.Center,
                            ModContent.DustType<NewHorizons.Content.Dusts.GlowDust>(),
                            Vector2.Zero,
                            0,
                            new Color(100, 180, 255, 220) * 0.9f,
                            0.65f + Main.rand.NextFloat(0.4f)
                        );
                        d.velocity = Main.rand.NextVector2Circular(0.7f, 0.7f);
                        d.fadeIn = 0.04f + Main.rand.NextFloat(0.03f);
                        d.noLight = false;
                    }

                    if (Pulses >= 3)
                    {
                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            Projectile.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<AquaBoom>(),
                            Projectile.damage,
                            Projectile.knockBack,
                            Projectile.owner
                        );
                        Projectile.Kill();
                    }
                }
            }

            Lighting.AddLight(Projectile.Center, 0.13f, 0.36f, 0.58f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = tex.Size() / 2f;
            int trailLength = 12;

            for (int i = trailLength; i >= 1; i--)
            {
                float progress = (float)i / trailLength;
                Vector2 pos = Projectile.oldPos[i - 1] + Projectile.Size / 2f - Main.screenPosition;
                float scale = Projectile.scale * (1f - progress * 0.75f) * 0.7f;
                Color trailColor = new Color(140, 210, 255, 0) * (1f - progress * 0.85f);

                Main.EntitySpriteDraw(
                    tex,
                    pos,
                    null,
                    trailColor,
                    Projectile.oldRot[i - 1],
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }

    public class AquaBoom : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];

        public override string Texture => "Synergia/Assets/Textures/Glow";

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 140;
            Projectile.timeLeft = 24;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Timer++;
            float progress = Timer / (float)Projectile.timeLeft;
            Projectile.scale = MathHelper.Lerp(0.1f, 1.35f, MathF.Pow(progress, 0.45f)) * (1f - progress * 0.4f);
            Projectile.rotation += 0.09f;

            if (Timer == 1)
            {
                SoundStyle boom = Reassures.Reassures.RSounds.BigBoom;
                boom.Volume = 0.8f;
                boom.PitchVariance = 0.12f;

                if (Main.netMode != NetmodeID.Server)
                {
                    var player = Main.player[Projectile.owner];
                    if (player.TryGetModPlayer<ScreenShakePlayer>(out var shakePlayer))
                    {
                        shakePlayer.TriggerShake(12, 0.85f);
                    }
                }

                for (int i = 0; i < 18; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center,
                        ModContent.DustType<NewHorizons.Content.Dusts.GlowDust>(),
                        Main.rand.NextVector2Circular(2.8f, 2.8f),
                        0,
                        new Color(100, 190, 255, 255) * 0.95f,
                        0.9f + Main.rand.NextFloat(0.6f)
                    );
                    d.fadeIn = 0.05f + Main.rand.NextFloat(0.05f);
                    d.noLight = false;
                }

                for (int i = 0; i < 24; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.Water,
                        Main.rand.NextVector2Circular(5f, 5f),
                        0,
                        new Color(120, 200, 255),
                        Main.rand.NextFloat(1.2f, 1.8f)
                    );
                    d.noGravity = true;
                    d.velocity *= 1.3f;
                }
            }

            Lighting.AddLight(Projectile.Center, 0.22f, 0.48f, 0.70f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null,
                Main.GameViewMatrix.TransformationMatrix);

            float fade = 1f - Timer / (float)Projectile.timeLeft;
            Color glowColor = new Color(90, 180, 240, 255) * fade * 0.9f;

            sb.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor,
                Projectile.rotation,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            sb.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * 0.4f,
                Projectile.rotation + 0.45f,
                tex.Size() / 2f,
                Projectile.scale * 1.35f,
                SpriteEffects.None,
                0
            );

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<NewHorizons.Content.Dusts.GlowDust>(),
                    Main.rand.NextVector2Circular(1.8f, 1.8f),
                    0,
                    new Color(110, 190, 255, 200),
                    0.8f + Main.rand.NextFloat(0.5f)
                );
                d.fadeIn = 0.06f;
            }
        }
    }
}
