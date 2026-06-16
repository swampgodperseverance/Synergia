using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class PurpleCog : ModProjectile
    {
        private Vector2 startPos;
        private bool initialized = false;
        private bool exploded = false;
        private float pulseTimer = 0f;
        private Texture2D trailTexture;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 400;
        }

        public override void Load()
        {
            trailTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }

        public override void AI()
        {
            pulseTimer += 0.05f;

            if (!initialized)
            {
                startPos = Projectile.Center;
                initialized = true;
            }

            Projectile.rotation += 0.6f + Projectile.velocity.Length() * 0.08f;

            float heightTarget = 160f;
            float distance = startPos.Y - Projectile.Center.Y;
            if (distance < heightTarget)
            {
                float t = distance / heightTarget;
                Projectile.velocity.Y = MathHelper.Lerp(-10f, -16f, t * t);
            }
            else
            {
                Projectile.velocity *= 0.9f;

                if (!exploded && Projectile.velocity.Length() < 0.4f)
                {
                    exploded = true;
                    Explode();
                }
            }

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 100, default, 1.3f);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].noGravity = true;
            }
        }

        private void Explode()
        {
            Projectile.timeLeft = 3;

            NPC target = null;
            float maxDist = 1000f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(Projectile.Center, npc.Center) < maxDist)
                {
                    maxDist = Vector2.Distance(Projectile.Center, npc.Center);
                    target = npc;
                }
            }

            if (target != null)
            {
                Mod bismuth = ModLoader.GetMod("Bismuth");
                if (bismuth != null)
                {
                    var waveType = bismuth.Find<ModProjectile>("WaveOfForceP").Type;
                    Vector2 dir = Vector2.Normalize(target.Center - Projectile.Center);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        dir * 20f,
                        waveType,
                        Projectile.damage,
                        1f,
                        Projectile.owner
                    );
                }
            }

            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6), 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float pulseScale = 0.95f + (float)Math.Sin(pulseTimer) * 0.08f;
            float glowRotation = Projectile.rotation + (float)Math.Sin(pulseTimer * 0.8f) * 0.3f;

            Color darkPurple = new Color(45, 0, 75);
            Color midPurple = new Color(75, 0, 130);
            Color lightPurple = new Color(138, 43, 226);
            Color glowPurple = new Color(100, 0, 200, 180);

            if (trailTexture != null && Projectile.oldPos.Length > 1)
            {
                for (int i = 1; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero) continue;

                    float trailProgress = (float)i / Projectile.oldPos.Length;
                    float trailAlpha = 1f - trailProgress * 1.2f;
                    float trailScale = 0.2f * (1f - trailProgress * 0.5f);

                    Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                    Color trailColor = new Color(75, 0, 130) * trailAlpha * 0.7f;

                    Main.EntitySpriteDraw(
                        trailTexture,
                        trailPos,
                        null,
                        trailColor,
                        Projectile.rotation + pulseTimer * (float)Math.Sin(trailProgress * Math.PI),
                        trailTexture.Size() / 2f,
                        trailScale * 0.2f,
                        SpriteEffects.None,
                        0
                    );

                    Color trailColor2 = new Color(138, 43, 226) * trailAlpha * 0.5f;

                    Main.EntitySpriteDraw(
                        trailTexture,
                        trailPos,
                        null,
                        trailColor2,
                        Projectile.rotation - pulseTimer * (float)Math.Cos(trailProgress * Math.PI),
                        trailTexture.Size() / 2f,
                        trailScale * 0.15f,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            for (int i = 0; i < 8; i++)
            {
                float offsetAngle = i * MathHelper.PiOver4 + pulseTimer * 2f;
                Vector2 offset = offsetAngle.ToRotationVector2() * (3f + (float)Math.Sin(pulseTimer * 3f) * 1.5f);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    darkPurple * 0.6f,
                    glowRotation,
                    origin,
                    1.05f + pulseScale * 0.1f,
                    SpriteEffects.None,
                    0
                );
            }

            for (int i = 0; i < 4; i++)
            {
                float offsetAngle = i * MathHelper.PiOver2 + pulseTimer * 3f;
                Vector2 offset = offsetAngle.ToRotationVector2() * (5f + (float)Math.Sin(pulseTimer * 4f) * 2f);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    midPurple * 0.5f,
                    glowRotation - 0.2f,
                    origin,
                    1.08f + pulseScale * 0.15f,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                glowPurple,
                glowRotation,
                origin,
                1.0f + pulseScale * 0.12f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightPurple * 0.8f,
                Projectile.rotation,
                origin,
                0.92f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Color.White * 0.5f,
                Projectile.rotation,
                origin,
                0.85f,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}