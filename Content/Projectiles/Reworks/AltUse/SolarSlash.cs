using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class SolarSlash : ModProjectile
    {
        private const int FadeInTime = 6;
        private const int HoldTime = 4;
        private const int FadeOutTime = 20;
        private const int TotalTime = FadeInTime + HoldTime + FadeOutTime;

        private float squish;
        private Vector2 scaleMultiplier = Vector2.One;
        private bool mirrored;
        private int particleCooldown; 

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 1000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 86;
            Projectile.height = 209;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = TotalTime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.scale = 0.9f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            mirrored = Main.LocalPlayer.GetModPlayer<SolarSlashMirrorPlayer>().ShouldMirror();
            particleCooldown = Main.rand.Next(3); 
        }

        public override void AI()
        {
            Projectile.localAI[0]++;
            float timer = Projectile.localAI[0];

            if (timer <= FadeInTime)
            {
                float p = timer / FadeInTime;
                p *= p;
                Projectile.alpha = (int)(255f * (1f - p));
                scaleMultiplier = new Vector2(1.2f, 0.9f);
            }
            else
            {
                float p = MathHelper.Clamp(
                    (timer - FadeInTime) / (TotalTime - FadeInTime),
                    0f, 1f
                );
                p = p * p;
                Projectile.alpha = (int)(255f * p);
                scaleMultiplier = Vector2.Lerp(Vector2.One, new Vector2(1.1f, 0.6f), p);
            }

            Projectile.rotation += Projectile.direction * 0.6f;

            if (Projectile.velocity.X != 0)
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);

            float pulse = MathF.Sin(timer * 0.35f) * 0.12f + 1f;
            squish = MathF.Sin(timer * 0.45f) * 0.18f;
            Projectile.scale = 0.9f * pulse;

            Projectile.velocity *= 0.96f;

            if (Main.rand.NextBool(2) && Projectile.alpha < 220)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(22, 22),
                    DustID.Torch,
                    Projectile.velocity * 0.25f,
                    80,
                    new Color(255, 160, 40),
                    1.6f
                );
                d.noGravity = true;
            }

            particleCooldown--;
            if (particleCooldown <= 0 && Projectile.alpha < 200)
            {
                particleCooldown = Main.rand.Next(2, 8);
                int cornersToSpawn = Main.rand.Next(1, 4);

                for (int i = 0; i < cornersToSpawn; i++)
                {
                    Vector2 randomCorner = Projectile.Center;

                    int side = Main.rand.Next(4); 

                    float offsetX = Projectile.width * 0.5f;
                    float offsetY = Projectile.height * 0.5f;

                    switch (side)
                    {
                        case 0: 
                            randomCorner = new Vector2(
                                Projectile.Left.X - Main.rand.NextFloat(-10f, 15f),
                                Projectile.Top.Y - Main.rand.NextFloat(5f, 25f)
                            );
                            break;
                        case 1:
                            randomCorner = new Vector2(
                                Projectile.Right.X - Main.rand.NextFloat(-15f, 10f),
                                Projectile.Top.Y - Main.rand.NextFloat(5f, 25f)
                            );
                            break;
                        case 2: 
                            randomCorner = new Vector2(
                                Projectile.Left.X - Main.rand.NextFloat(-10f, 15f),
                                Projectile.Bottom.Y + Main.rand.NextFloat(5f, 25f)
                            );
                            break;
                        case 3: 
                            randomCorner = new Vector2(
                                Projectile.Right.X - Main.rand.NextFloat(-15f, 10f),
                                Projectile.Bottom.Y + Main.rand.NextFloat(5f, 25f)
                            );
                            break;
                    }

                    float particleScale = Main.rand.NextFloat(0.6f, 1f);

                    ParticleOrchestrator.RequestParticleSpawn(
                        true,
                        ParticleOrchestraType.FlameWaders,
                        new ParticleOrchestraSettings
                        {
                            PositionInWorld = randomCorner,
                            MovementVector = Projectile.velocity * Main.rand.NextFloat(0.3f, 0.8f)
                        },
                        Projectile.owner
                    );
                }
            }

            Lighting.AddLight(
                Projectile.Center,
                new Vector3(1.6f, 0.75f, 0.2f) * (1f - Projectile.alpha / 255f)
            );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = tex.Size() * 0.5f;
            SpriteEffects fx = mirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Color core = new(255, 140, 40);
            Color glow = new(255, 200, 90);
            Color trail = new(255, 90, 20);

            Vector2 scale = new(
                Projectile.scale + squish * 0.6f,
                Projectile.scale - squish * 0.5f
            );

            // ---------- ТРЕЙЛ ----------
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float p = i / (float)Projectile.oldPos.Length;
                float a = (1f - p) * 0.45f * (1f - Projectile.alpha / 255f);

                Main.EntitySpriteDraw(
                    tex,
                    Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition,
                    null,
                    trail * a,
                    Projectile.rotation,
                    origin,
                    scale * (0.8f + p * 0.2f),
                    fx,
                    0
                );
            }

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                glow * 0.7f * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                origin,
                scale * 1.3f,
                fx,
                0
            );

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                core * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                origin,
                scale,
                fx,
                0
            );

            return false;
        }

        public override bool? CanHitNPC(NPC target)
            => target.friendly || Projectile.alpha > 230 ? false : null;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240);
        }
    }

    public class SolarSlashMirrorPlayer : ModPlayer
    {
        private int slashCounter;

        public bool ShouldMirror()
        {
            bool mirror = slashCounter % 2 == 0;
            slashCounter++;
            return mirror;
        }

        public override void OnEnterWorld()
        {
            slashCounter = 0;
        }
    }
}