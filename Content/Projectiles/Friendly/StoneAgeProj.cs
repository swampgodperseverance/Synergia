using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Synergia.Content.Projectiles.Friendly.StoneAgeProj;

namespace Synergia.Content.Projectiles.Friendly
{
    public class StoneAgeProj : ModProjectile
    {
            private bool isSpecialActive = false;
            private int specialTimer = 0;
            private int abilityCooldown = 0;
            private float originalMaxRange;
            private float originalTopSpeed;

            public override void SetStaticDefaults()
            {
            }

            public override void SetDefaults()
            {
                Projectile.extraUpdates = 0;
                Projectile.width = 16;
                Projectile.height = 16;
                Projectile.aiStyle = 99;
                Projectile.friendly = true;
                Projectile.penetrate = -1;
                Projectile.DamageType = DamageClass.Melee;

                ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 12;
                ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 210f;
                ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 10f;

                originalMaxRange = 210f;
                originalTopSpeed = 10f;
            }

            public override void AI()
            {
                Player player = Main.player[Projectile.owner];

                if (abilityCooldown > 0)
                    abilityCooldown--;

                if (!isSpecialActive && abilityCooldown == 0 && Main.rand.NextBool(60))
                {
                    ActivateSpecialAbility();
                }

                if (isSpecialActive)
                {
                    specialTimer++;

                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                            DustID.Asphalt, 0, 0, 100, Color.Gray, 1.2f);
                        dust.noGravity = true;
                        dust.velocity = Projectile.velocity * 0.5f;
                    }

                    if (specialTimer >= 180)
                    {
                        DeactivateSpecialAbility();
                    }

                    if (Projectile.velocity.Y < 8f)
                        Projectile.velocity.Y += 0.15f;

                    if (Projectile.velocity.Y == 0 && Main.rand.NextBool(4))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Dust dust = Dust.NewDustDirect(Projectile.Bottom, Projectile.width, 4,
                                DustID.Asphalt, Main.rand.NextFloat(-2f, 2f), -Main.rand.NextFloat(1f, 3f),
                                80, Color.Gray, 1f);
                        }
                    }
                }
            }

            private void ActivateSpecialAbility()
            {
                isSpecialActive = true;
                specialTimer = 0;
                abilityCooldown = 300;

                Player player = Main.player[Projectile.owner];

                SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                SoundEngine.PlaySound(SoundID.Item42, Projectile.Center);

                for (int i = 0; i < 30; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Asphalt, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f),
                        100, Color.Gray, 1.5f);
                    dust.noGravity = true;
                }

                ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = originalMaxRange * 0.8f;
                ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = originalTopSpeed * 0.7f;

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.LocalPlayer.AddBuff(BuffID.Slow, 10);
                }
            }

            private void DeactivateSpecialAbility()
            {
                isSpecialActive = false;

                ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = originalMaxRange;
                ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = originalTopSpeed;

                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        DustID.Asphalt, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, 1f),
                        80, Color.Gray, 1.2f);
                }
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                Player player = Main.player[Projectile.owner];

                if (isSpecialActive)
                {
                    int doubleDamage = damageDone * 2;
                    target.life -= damageDone;
                    target.checkDead();

                    SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                    SoundEngine.PlaySound(SoundID.Item38, Projectile.Center);

                    for (int i = 0; i < 15; i++)
                    {
                        Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                            DustID.Asphalt, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-3f, 2f),
                            120, Color.Gray, 1.8f);
                        dust.noGravity = true;

                        Dust dust2 = Dust.NewDustDirect(target.position, target.width, target.height,
                            DustID.Asphalt, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                            100, Color.Gray, 1.2f);
                    }

                    hit.HitDirection = (target.Center.X - player.Center.X > 0) ? 1 : -1;
                    hit.Knockback = hit.Knockback * 1.5f;

                    target.AddBuff(BuffID.ShadowFlame, 60);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);

                    if (Main.rand.NextBool(3))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                                DustID.Asphalt, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                                70, Color.Gray, 0.8f);
                        }
                    }
                }

                Dust.NewDust(target.position, target.width, target.height, DustID.Smoke, 0, 0, 100, Color.Gray, 1f);
            }

            public override bool PreDraw(ref Color lightColor)
            {
                if (isSpecialActive)
                {
                    Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                    Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 offset = new Vector2(
                            (i % 2 == 0 ? -2 : 2) * (i < 2 ? 1 : -1),
                            (i < 2 ? -2 : 2)
                        );

                        Main.spriteBatch.Draw(
                            texture,
                            Projectile.Center - Main.screenPosition + offset,
                            new Rectangle(0, 0, texture.Width, texture.Height),
                            Color.Gray * 0.6f,
                            Projectile.rotation,
                            drawOrigin,
                            Projectile.scale,
                            SpriteEffects.None,
                            0f
                        );
                    }

                    Main.spriteBatch.Draw(
                        texture,
                        Projectile.Center - Main.screenPosition,
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        Color.Gray * 0.8f,
                        Projectile.rotation,
                        drawOrigin,
                        Projectile.scale,
                        SpriteEffects.None,
                        0f
                    );

                    return false;
                }

                return true;
            }
        
    }
}