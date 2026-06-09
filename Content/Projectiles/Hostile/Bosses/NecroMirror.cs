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

        public override void SetStaticDefaults()
        {
        }

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
            NPC necro = FindNecromancer();
            if (necro == null || !necro.active)
            {
                Projectile.Kill();
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

                if (Main.rand.NextBool(3))
                {
                    Dust d = Dust.NewDustDirect(
                        Projectile.Center - new Vector2(8, 8),
                        16, 16,
                        DustID.Smoke,
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        100,
                        Color.Black,
                        Main.rand.NextFloat(1.2f, 1.8f)
                    );
                    d.noGravity = true;
                    d.fadeIn = 1.4f;
                }
            }
            else
            {
                Projectile.alpha = 0;
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

            if (outlineIntensity > 0.05f)
            {
                Color outlineColor = Color.Black * (0.4f * outlineIntensity);

                for (int i = 0; i < 8; i++)
                {
                    float angle = i * MathHelper.PiOver4;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (3f + outlineIntensity * 3f);

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

                Color glowColor = new Color(50, 50, 50, 100) * (0.6f * outlineIntensity);
                for (int i = 0; i < 4; i++)
                {
                    float angle = i * MathHelper.PiOver2;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (6f + outlineIntensity * 4f);

                    Main.spriteBatch.Draw(
                        texture,
                        position + offset,
                        null,
                        glowColor,
                        Projectile.rotation,
                        origin,
                        Projectile.scale * 1.05f,
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