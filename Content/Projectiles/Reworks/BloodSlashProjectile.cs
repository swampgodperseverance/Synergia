using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Synergia.Content.Projectiles.Reworks
{
    public class BloodSlashProjectile : ModProjectile
    {
        private bool isLaunched = false;
        private Vector2 targetPosition;
        private float orbitAngle = 0f;
        private bool disappearing = false;
        private bool bloodExplosionDone = false;
        private float appearProgress = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255; 
            Projectile.scale = 0.1f; 
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            if (appearProgress < 1f)
            {
                appearProgress += 0.05f;
                if (appearProgress > 1f) appearProgress = 1f;
                
                Projectile.alpha = (int)(255 * (1 - appearProgress)); 
                Projectile.scale = 0.1f + 0.9f * appearProgress; 
            }

            if (!isLaunched)
            {
                orbitAngle += 0.1f;
                if (orbitAngle > MathHelper.TwoPi)
                    orbitAngle -= MathHelper.TwoPi;

                Vector2 orbitOffset = new Vector2(0, -50).RotatedBy(orbitAngle);
                Projectile.Center = player.MountedCenter + orbitOffset;

                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 
                        Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 100, default, Main.rand.NextFloat(0.6f, 1f));
                    dust.noGravity = true;
                }

                if (Projectile.ai[0] >= 120)
                {
                    isLaunched = true;
                    Projectile.ai[0] = 0;
                    float minDistance = 1000f;
                    targetPosition = Vector2.Zero;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy() && Vector2.Distance(Projectile.Center, npc.Center) < minDistance)
                        {
                            minDistance = Vector2.Distance(Projectile.Center, npc.Center);
                            targetPosition = npc.Center;
                        }
                    }

                    if (targetPosition != Vector2.Zero)
                    {
                        Vector2 startDir = Vector2.Normalize(targetPosition - Projectile.Center);
                        Projectile.velocity = startDir * 2f; 
                    }
                    else
                    {
                        disappearing = true; 
                    }
                }
                Projectile.ai[0]++;
            }
            else
            {
                if (!disappearing)
                {
                    bool targetAlive = false;
                    float closestDist = 1000f;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy())
                        {
                            float dist = Vector2.Distance(Projectile.Center, npc.Center);
                            if (dist < closestDist)
                            {
                                closestDist = dist;
                                targetPosition = npc.Center;
                                targetAlive = true;
                            }
                        }
                    }

                    if (targetAlive)
                    {

                        Vector2 desiredVelocity = Vector2.Normalize(targetPosition - Projectile.Center) * 12f;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.1f);
                    }
                    else
                    {
                        disappearing = true;
                    }

                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                    if (Main.rand.NextBool(2))
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 
                            Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, Main.rand.NextFloat(0.5f, 1f));
                        dust.noGravity = true;
                    }
                }
                else
                {
                    Projectile.velocity *= 0.95f;
                    Projectile.alpha += 10;

                    if (!bloodExplosionDone && Projectile.alpha >= 128)
                    {
                        bloodExplosionDone = true;
                        for (int i = 0; i < 30; i++)
                        {
                            Vector2 dustVel = Main.rand.NextVector2Circular(5f, 5f);
                            Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood, dustVel.X, dustVel.Y, 100, default, Main.rand.NextFloat(0.8f, 1.5f));
                            dust.noGravity = true;
                        }
                        SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
                    }

                    if (Projectile.alpha >= 255)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width / 2, Projectile.height / 2) - Main.screenPosition;
                float scale = Projectile.scale * (1f - k / (float)Projectile.oldPos.Length);
                Color color = new Color(200, 0, 0, 100) * scale * (1f - Projectile.alpha / 255f);
                spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }

            Color drawColor = Color.Red * (1f - Projectile.alpha / 255f);
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}