using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Synergia.Helpers;
using Synergia.Trails;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Projectiles.AI;
using ValhallaMod.Visual;

namespace Synergia.Content.Projectiles.Thrower
{
    public class OmegaDiscRework : ValhallaGlaive
    {
        private OmegaDisc trail1 = new OmegaDisc();
        private OmegaDisc2 trail2 = new OmegaDisc2();

        private bool exploded = false;
        private int slowTimer = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.4f;

            extraUpdatesHoming = 1;
            extraUpdatesComingBack = 1;
            rotationSpeed = 0.5f;
            bounces = 0;
            tileBounce = true;
            timeFlying = 20;
            speedHoming = 17.5f;
            speedFlying = 17.5f;
            speedComingBack = 15f;
            homingDistanceMax = 320f;
            homingStyle = 0;
            homingStart = false;
            homingIgnoreTile = false;
        }

        public override void AI()
        {
            base.AI();

            slowTimer++;

            if (slowTimer >= 60 && !exploded)
            {
                exploded = true;
                Projectile.velocity *= 0.35f;

                ParticleOrchestrator.RequestParticleSpawn(
                    false,
                    ParticleOrchestraType.Excalibur,
                    new ParticleOrchestraSettings { PositionInWorld = Projectile.Center },
                    Projectile.owner
                );

                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 up = new Vector2(0, -8f);
                    Vector2 down = new Vector2(0, 8f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        up,
                        ModContent.ProjectileType<OmegaDiscSmall>(),
                        Projectile.damage / 2,
                        0f,
                        Projectile.owner
                    );

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        down,
                        ModContent.ProjectileType<OmegaDiscSmall>(),
                        Projectile.damage / 2,
                        0f,
                        Projectile.owner
                    );
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.oldPos.Length < 2)
                return true;

            trail1.Draw(Projectile);
            trail2.Draw(Projectile);

            return true;
        }
    }
    public class OmegaDiscSmall : ModProjectile
    {
        private int timer = 0;
        private const int AppearDuration = 24;
        private const int StayDuration = 36;
        private const int ReturnStart = AppearDuration + StayDuration;
        private const int ReturnDuration = 60;
        private const int TotalLifetime = ReturnStart + ReturnDuration + 5;

        private PrimDrawer primDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = TotalLifetime;
            Projectile.alpha = 255;
            Projectile.scale = 0.01f;
            Projectile.ignoreWater = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            primDrawer = new PrimDrawer(
                widthFunc: p => 11f * Projectile.scale * (1f - p * p),
                colorFunc: p => GetGoldenMarbleColor(p)
            );
        }
        private Color GetGoldenMarbleColor(float progress)
        {
            float fade = 1f - progress * progress;
            byte r = (byte)(255 * fade);
            byte g = (byte)(MathHelper.Lerp(210, 245, (float)Math.Sin(progress * 18f + Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f) * fade);
            byte b = (byte)(MathHelper.Lerp(120, 180, (float)Math.Cos(progress * 24f + Main.GlobalTimeWrappedHourly * 5.5f) * 0.5f + 0.5f) * fade);
            float shine = 1f + 0.35f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 22f + progress * 40f);
            Color baseColor = new Color(r, g, b);
            return baseColor * (0.38f + 0.12f * shine) * fade * 0.55f;
        }

        public override void AI()
        {
            timer++;
            Player owner = Main.player[Projectile.owner];

            if (timer <= AppearDuration)
            {
                float t = timer / (float)AppearDuration;
                float ease = EaseFunctions.EaseOutBack(t);
                Projectile.scale = ease * 1.05f;
                Projectile.alpha = (int)(255 * (1f - EaseFunctions.EaseOutQuad(t)));
                Projectile.rotation = MathHelper.Lerp(0f, 12f, t) * (timer % 2 == 0 ? 1 : -1);
            }
            else if (timer < ReturnStart)
            {
                Projectile.scale = 1.05f + 0.04f * (float)Math.Sin(timer * 0.14f);
                Projectile.rotation += 0.22f;
                Projectile.alpha = 40 + (int)(30 * Math.Sin(timer * 0.09f));
            }
            else
            {
                float progress = (timer - ReturnStart) / (float)ReturnDuration;
                progress = MathHelper.Clamp(progress, 0f, 1f);
                float ease = EaseFunctions.EaseInOutCubic(progress);
                float currentSpeed = MathHelper.Lerp(7f, 28f, ease);
                Vector2 toPlayer = owner.Center - Projectile.Center;
                if (toPlayer != Vector2.Zero)
                {
                    toPlayer.Normalize();
                    Projectile.velocity = toPlayer * currentSpeed;
                }
                Projectile.scale = MathHelper.Lerp(1.05f, 0.4f, ease);
                Projectile.alpha = (int)MathHelper.Lerp(40f, 255f, ease * ease);
                Projectile.rotation += 0.35f - progress * 0.18f;
            }

            Lighting.AddLight(Projectile.Center, 0.55f, 0.42f, 0.18f);
        }

        public override bool PreKill(int timeLeft)
        {
            if (Projectile.alpha < 240)
            {
                ParticleOrchestrator.RequestParticleSpawn(
                    false,
                    ParticleOrchestraType.TrueExcalibur,
                    new ParticleOrchestraSettings
                    {
                        PositionInWorld = Projectile.Center,
                        MovementVector = Vector2.Zero
                    },
                    Projectile.owner
                );

                for (int i = 0; i < 14; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(5.2f, 5.2f);
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center + Main.rand.NextVector2Circular(12f, 12f),
                        DustID.AncientLight,
                        vel,
                        140,
                        default,
                        1.4f
                    );
                    d.noGravity = true;
                    d.fadeIn = 0.9f;
                }
            }
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (primDrawer != null)
            {
                primDrawer.DrawPrims(
                    Projectile.oldPos,
                    -Main.screenPosition,
                    180
                );
            }

            Texture2D ringTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Color glowColor = new Color(255, 220, 140) * 0.55f;
            float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 9f) * 0.5f + 0.5f;
            Color shineColor = new Color(255, 240, 170) * 0.7f;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // Внешнее кольцо (медленнее, чуть больше)
            Main.spriteBatch.Draw(
                ringTex,
                drawPos,
                null,
                glowColor * (0.5f + 0.25f * pulse),
                Main.GlobalTimeWrappedHourly * 0.9f,
                ringTex.Size() / 2f,
                0.14f + 0.04f * pulse,
                SpriteEffects.None,
                0f
            );

            // Внутреннее кольцо (быстрее, меньше, ярче)
            Main.spriteBatch.Draw(
                ringTex,
                drawPos,
                null,
                shineColor * (0.6f + 0.3f * pulse),
                Main.GlobalTimeWrappedHourly * -2.2f,
                ringTex.Size() / 2f,
                0.09f + 0.03f * pulse,
                SpriteEffects.None,
                0f
            );

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            Color mainColor = new Color(255, 240, 180, 255 - Projectile.alpha) * 1.1f;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 255 - Projectile.alpha);
        }
    }
}