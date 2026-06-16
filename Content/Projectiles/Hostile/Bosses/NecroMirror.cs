using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Bismuth.Content.NPCs;
using System;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroMirror : ModProjectile
    {
        private float orbitAngle;
        private int spawnTimer;
        private const int FadeInTime = 120;
        private float outlineIntensity = 0f;
        private int outlineTimer = 0;
        private bool dying = false;
        private int deathTimer = 0;
        private const int DeathDuration = 45;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
        }

        public override void AI()
        {
            if (dying)
            {
                deathTimer++;
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, deathTimer / (float)DeathDuration);
                Projectile.scale = 1f - (deathTimer / (float)DeathDuration) * 0.5f;

                for (int i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 32, 32,
                        DustID.Shadowflame,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        80, Color.Black, Main.rand.NextFloat(1.5f, 2.5f) * (1f - deathTimer / (float)DeathDuration));
                    d.noGravity = true;
                }

                if (deathTimer >= DeathDuration)
                {
                    Projectile.Kill();
                }
                return;
            }

            NPC necro = FindNecromancer();
            if (necro == null || !necro.active)
            {
                StartDeath();
                return;
            }

            if (outlineTimer > 0)
            {
                outlineTimer--;
                outlineIntensity = outlineTimer / 20f;
            }
            else
            {
                outlineIntensity = MathHelper.Lerp(outlineIntensity, 0f, 0.1f);
            }

            if (spawnTimer < FadeInTime)
            {
                spawnTimer++;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, spawnTimer / (float)FadeInTime);
                Projectile.scale = 0.6f + (spawnTimer / (float)FadeInTime) * 0.5f;

                if (Main.rand.NextBool(2))
                {
                    Dust d = Dust.NewDustDirect(
                        Projectile.Center - new Vector2(16, 16),
                        32, 32,
                        DustID.Smoke,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        100,
                        Color.Black,
                        Main.rand.NextFloat(0.8f, 1.6f) * (1f - spawnTimer / (float)FadeInTime)
                    );
                    d.noGravity = true;
                    d.fadeIn = 1.2f;
                }

                for (int i = 0; i < 2; i++)
                {
                    Dust d2 = Dust.NewDustDirect(
                        Projectile.Center - new Vector2(12, 12),
                        24, 24,
                        DustID.Shadowflame,
                        Main.rand.NextFloat(-1f, 1f),
                        Main.rand.NextFloat(-1f, 1f),
                        60,
                        Color.Black,
                        Main.rand.NextFloat(0.5f, 1.2f)
                    );
                    d2.noGravity = true;
                }
            }
            else
            {
                Projectile.alpha = 0;
                Projectile.scale = 1f;
            }

            float orbitRadius = 120f;
            orbitAngle += 0.03f;
            if (orbitAngle > MathHelper.TwoPi)
                orbitAngle -= MathHelper.TwoPi;

            Vector2 offset = new Vector2((float)Math.Cos(orbitAngle), (float)Math.Sin(orbitAngle)) * orbitRadius;
            Projectile.Center = necro.Center + offset;

            Projectile.rotation = (necro.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2;

            if (outlineIntensity > 0.1f)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust shadowDust = Dust.NewDustDirect(
                        Projectile.Center - new Vector2(16, 16),
                        32, 32,
                        DustID.Shadowflame,
                        Main.rand.NextFloat(-2f, 2f),
                        Main.rand.NextFloat(-2f, 2f),
                        80,
                        Color.Black,
                        1.2f * outlineIntensity
                    );
                    shadowDust.noGravity = true;
                }
            }

            ReflectFriendlyProjectiles();
        }

        private void StartDeath()
        {
            if (dying) return;
            dying = true;
            deathTimer = 0;
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
            for (int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20), 40, 40,
                    DustID.Shadowflame,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    120, Color.Black, Main.rand.NextFloat(1.8f, 2.8f));
                d.noGravity = true;
            }
        }

        private void ReflectFriendlyProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile target = Main.projectile[i];
                if (!target.active || !target.friendly || target.hostile || target.minion)
                    continue;
                if (target.Hitbox.Intersects(Projectile.Hitbox))
                {
                    target.Kill();
                    SoundEngine.PlaySound(SoundID.Item74, Projectile.position);

                    outlineTimer = 20;
                    outlineIntensity = 1f;

                    for (int d = 0; d < 12; d++)
                    {
                        Dust burstDust = Dust.NewDustDirect(
                            Projectile.Center - new Vector2(12, 12),
                            24, 24,
                            DustID.Shadowflame,
                            Main.rand.NextFloat(-4f, 4f),
                            Main.rand.NextFloat(-4f, 4f),
                            100,
                            Color.Black,
                            Main.rand.NextFloat(1.5f, 2.2f)
                        );
                        burstDust.noGravity = true;
                    }

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int projType = ModContent.ProjectileType<NecroFire>();
                        Vector2 dir = (target.velocity.Length() > 0.1f ? target.velocity.SafeNormalize(Vector2.Zero) : Vector2.UnitY) * 8f;
                        Projectile.NewProjectileDirect(
                            Projectile.GetSource_FromAI(),
                            Projectile.Center,
                            -dir,
                            projType,
                            40,
                            2f,
                            Main.myPlayer
                        );
                    }
                }
            }
        }

        private NPC FindNecromancer()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.type == ModContent.NPCType<EvilNecromancer>())
                    return npc;
            }
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition;

            float pulsatingOutline = 0.25f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f) * 0.1f;
            float currentOutline = MathHelper.Clamp(pulsatingOutline + outlineIntensity * 0.7f, 0.1f, 1.2f);

            if (currentOutline > 0.05f)
            {
                Color outlineColor = Color.Black * (0.55f * currentOutline);
                float outlineRadius = 2f + currentOutline * 1.5f;

                for (int i = 0; i < 12; i++)
                {
                    float angle = i * MathHelper.TwoPi / 12f;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * outlineRadius;
                    Main.spriteBatch.Draw(
                        texture,
                        position + offset,
                        null,
                        outlineColor,
                        Projectile.rotation,
                        origin,
                        Projectile.scale,
                        SpriteEffects.None,
                        0f
                    );
                }

                Color glowColor = new Color(30, 30, 40, 80) * (0.5f * currentOutline);
                float glowRadius = 4f + currentOutline * 2f;
                for (int i = 0; i < 8; i++)
                {
                    float angle = i * MathHelper.PiOver4;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * glowRadius;
                    Main.spriteBatch.Draw(
                        texture,
                        position + offset,
                        null,
                        glowColor,
                        Projectile.rotation,
                        origin,
                        Projectile.scale * 1.03f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.spriteBatch.Draw(
                texture,
                position,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}