using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.Audio;
using NewHorizons.Content.Projectiles.Throwing;
using NewHorizons.Content.Items.Weapons.Throwing;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class ChlorophytePiercer2 : ModProjectile
    {
        private int origDamage;

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 12;
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        public bool IsStickingToTarget
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public float TargetWhoAmI
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            origDamage = Projectile.damage;
            Projectile.damage = 0;

            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<RingFlash>(),
                    0,
                    0f,
                    Projectile.owner
                );
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            IsStickingToTarget = true;
            TargetWhoAmI = target.whoAmI;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true;

            Point[] stickingJavelins = new Point[20];
            int javelinIndex = 0;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (i != Projectile.whoAmI && p.active && p.owner == Main.myPlayer &&
                    p.type == Projectile.type && p.ai[0] == 1f && p.ai[1] == target.whoAmI)
                {
                    stickingJavelins[javelinIndex++] = new Point(i, p.timeLeft);
                    if (javelinIndex >= stickingJavelins.Length) break;
                }
            }

            if (javelinIndex >= stickingJavelins.Length)
            {
                int oldest = 0;
                for (int j = 1; j < stickingJavelins.Length; j++)
                {
                    if (stickingJavelins[j].Y < stickingJavelins[oldest].Y)
                        oldest = j;
                }
                Main.projectile[stickingJavelins[oldest].X].Kill();
            }
        }

        public override void AI()
        {
            if (IsStickingToTarget)
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                bool shouldKill = false;
                Projectile.localAI[0] += 1f;

                bool hitEffect = Projectile.localAI[0] % 30f == 0f;
                int targetIndex = (int)TargetWhoAmI;

                if (Projectile.localAI[0] > 156f || targetIndex < 0 || targetIndex >= 200)
                {
                    shouldKill = true;
                }
                else if (Main.npc[targetIndex].active && !Main.npc[targetIndex].dontTakeDamage)
                {
                    Projectile.Center = Main.npc[targetIndex].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[targetIndex].gfxOffY;

                    if (hitEffect)
                    {
                        Main.npc[targetIndex].HitEffect(0, 1.0);
                    }

                    if (Main.myPlayer == Projectile.owner && Projectile.localAI[0] % 30f == 0f && Projectile.localAI[0] <= 120f)
                    {
                        int cloudCount = Main.rand.Next(3, 5); 

                        for (int i = 0; i < cloudCount; i++)
                        {
                            Vector2 direction = Main.rand.NextVector2CircularEdge(1f, 1f);
                            direction *= Main.rand.NextFloat(5f, 9f); 

                            int damage = origDamage / 3;        
                            if (damage < 1) damage = 1;

                            Projectile.NewProjectile(
                                Projectile.GetSource_FromAI(),
                                Main.npc[targetIndex].Center,
                                direction,
                                ProjectileID.SporeCloud,
                                damage,
                                1f,                          
                                Projectile.owner
                            );
                        }

                        SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
                    }

                    if (Main.rand.NextBool(5))
                    {
                        Vector2 offset = Main.rand.NextVector2Circular(18f, 18f);
                        Dust d = Dust.NewDustPerfect(
                            Main.npc[targetIndex].Center + offset,
                            107,
                            Vector2.Zero,
                            70,
                            default,
                            Main.rand.NextFloat(1.1f, 1.6f)
                        );
                        d.noGravity = true;
                        d.fadeIn = 0.8f + Main.rand.NextFloat(0.4f);
                    }
                }
                else
                {
                    shouldKill = true;
                }

                if (shouldKill)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.velocity.Y += 0.06f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Color glowColor = new Color(100, 255, 140, 0) * 0.75f;
            float pulse = 1f + 0.12f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 14f);

            Main.EntitySpriteDraw(
                texture,
                screenPos,
                null,
                glowColor,
                Projectile.rotation,
                origin,
                Projectile.scale * pulse * 1.22f,
                SpriteEffects.None
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(
                texture,
                screenPos,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None
            );

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            if (Main.rand.NextBool(10))
            {
                Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.Hitbox, ModContent.ItemType<ChlorophyteJavelin>());
                return;
            }

            Vector2 dir = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();
            Vector2 spawnPos = Projectile.Center + dir * 16f;

            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(spawnPos, 128, dir * 2f, 0, default, 1f);
                d.position = (d.position + Projectile.Center) / 2f;
                d.velocity *= 0.5f;
                d.noGravity = true;
                spawnPos -= dir * 8f;
            }

            for (int i = 0; i < 16; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 128, 0f, 0f, 0, default, 1.1f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, oldVelocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }
    }
    public class RingFlash : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/Ring";

        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 24;
            Projectile.alpha = 0;
            Projectile.scale = 0.1f;
        }

        public override bool? CanDamage() => false;

        public override void AI()
        {
            float progress = 1f - Projectile.timeLeft / 24f;
            Projectile.scale = MathHelper.Lerp(0.1f, 1.4f, progress);
            Projectile.alpha = (int)(255 * progress);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            Color drawColor = new Color(100, 255, 140, 255) * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                screenPos,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            return false;
        }
    }
}