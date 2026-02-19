using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using ValhallaMod.Projectiles.Thrown;

namespace Synergia.Content.Projectiles.Thrower
{
    public class ThornChakram2 : ValhallaGlaive
    {
        private bool stopping;
        private bool stopped;
        private bool returning;

        private int pulseTimer;
        private int pulsesDone;

        private float glowStrength;
        private float returnFade;
        private float rotationBoost;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.5f;

            extraUpdatesHoming = 1;
            extraUpdatesComingBack = 1;
            rotationSpeed = 0.4f;
            bounces = 0;
            tileBounce = true;
            timeFlying = 22;
            speedHoming = 18f;
            speedFlying = 18f;
            speedComingBack = 20f;
            homingDistanceMax = 100f;
            homingStyle = 0;
            homingStart = false;
            homingIgnoreTile = false;
        }

        public override void AI()
        {
            if (!stopping && !stopped)
            {
                base.AI();

                Projectile.localAI[0]++;
                if (Projectile.localAI[0] >= 45f)
                    stopping = true;

                return;
            }

            if (stopping && !stopped)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.08f);

                rotationBoost += 0.002f;
                Projectile.rotation += 0.15f + rotationBoost;

                glowStrength = MathHelper.Lerp(glowStrength, 1f, 0.05f);

                if (Projectile.velocity.Length() < 0.15f)
                {
                    Projectile.velocity = Vector2.Zero;
                    stopped = true;
                }
            }

            if (stopped && !returning)
            {
                pulseTimer++;

                Projectile.rotation += 0.18f + rotationBoost;
                rotationBoost += 0.0005f;

                if (pulseTimer >= 20 && pulsesDone < 3)
                {
                    pulseTimer = 0;
                    pulsesDone++;

                    NPC target = null;
                    float dist = 500f;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                        {
                            float d = Vector2.Distance(npc.Center, Projectile.Center);
                            if (d < dist)
                            {
                                dist = d;
                                target = npc;
                            }
                        }
                    }

                    if (target != null)
                    {
                        Vector2 dir = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            Projectile.Center,
                            dir * 16f,
                            ModContent.ProjectileType<CactusSpike>(),
                            (int)(Projectile.damage * 0.3f),
                            Projectile.knockBack,
                            Projectile.owner
                        );
                    }

                    if (pulsesDone >= 3)
                        returning = true;
                }
            }

            if (returning)
            {
                Player player = Main.player[Projectile.owner];
                Vector2 toPlayer = player.Center - Projectile.Center;

                Projectile.velocity = Vector2.Lerp(
                    Projectile.velocity,
                    toPlayer.SafeNormalize(Vector2.Zero) * 22f,
                    0.1f
                );

                glowStrength = MathHelper.Lerp(glowStrength, 0f, 0.08f);

                if (toPlayer.Length() < 40f)
                {
                    returnFade += 0.08f;
                    Projectile.scale = MathHelper.Lerp(1f, 0f, returnFade);
                    Projectile.alpha = (int)MathHelper.Lerp(0, 255, returnFade);

                    if (returnFade >= 1f)
                        Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;

                float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = new Color(100, 255, 140, 0) * progress * 0.5f;

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition,
                    null,
                    color,
                    Projectile.oldRot[i],
                    origin,
                    Projectile.scale * progress,
                    SpriteEffects.None,
                    0
                );
            }

            if (stopping || stopped)
            {
                float pulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f + Projectile.whoAmI) * 0.5f + 0.5f;
                float scale = Projectile.scale * (1f + pulse * 0.15f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.Additive,
                    SamplerState.LinearClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    null,
                    Main.GameViewMatrix.TransformationMatrix
                );

                Color glow = new Color(100, 255, 140) * 0.6f * glowStrength;

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.Center - Main.screenPosition,
                    null,
                    glow,
                    Projectile.rotation,
                    origin,
                    scale * 1.3f,
                    SpriteEffects.None
                );

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.LinearClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    null,
                    Main.GameViewMatrix.TransformationMatrix
                );
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor * (1f - returnFade),
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
