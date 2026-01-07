using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Collections.Generic;
using Bismuth.Content.Buffs;
using Avalon.Buffs.Debuffs;
namespace Synergia.Content.Projectiles.Other
{
    public class YellowPresentProj : ModProjectile
    {
        private bool landed;
        private bool initialized;
        private int groundTime;

        private readonly List<Vector2> oldPositions = new();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 36;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2500;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

            if (!initialized)
            {
                initialized = true;
                Projectile.Center = new Vector2(
                    Projectile.Center.X,
                    Main.screenPosition.Y - 120f
                );
                Projectile.velocity.Y = 0.6f;
            }

            if (!landed)
            {
                Projectile.velocity.Y += 0.045f;
                Projectile.rotation = (float)System.Math.Sin(Main.GameUpdateCount * 0.06f) * 0.25f;
                Projectile.frame = 0;
            }
            else
            {
                groundTime++;

                if (Projectile.frame < 3 && groundTime % 8 == 0)
                    Projectile.frame++;

                float dist = Vector2.Distance(player.Center, Projectile.Center);
                float shake = MathHelper.Clamp(1f - dist / 200f, 0f, 1f);

                Projectile.position += Main.rand.NextVector2Circular(1.3f * shake, 1.3f * shake);

                if (dist < 80f)
                    Explode();
            }

            if (landed)
            {
                oldPositions.Insert(0, Projectile.Center + Main.rand.NextVector2Circular(0.7f, 0.7f));
                if (oldPositions.Count > 6)
                    oldPositions.RemoveAt(oldPositions.Count - 1);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!landed)
            {
                landed = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation = 0f;
                groundTime = 0;

                SoundEngine.PlaySound(
                    new SoundStyle("Synergia/Assets/Sounds/SilentPunch")
                    {
                        Volume = 0.35f
                    },
                    Projectile.Center
                );
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!landed || oldPositions.Count == 0)
                return true;

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Type];

            Rectangle frameRect = new(
                0,
                Projectile.frame * frameHeight,
                texture.Width,
                frameHeight
            );

            Vector2 origin = frameRect.Size() / 2f;

            for (int i = 0; i < oldPositions.Count; i++)
            {
                float alpha = 0.4f * (1f - i / 5f);

                Vector2 drawPos = oldPositions[i] - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    frameRect,
                    Color.Yellow * alpha,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14 with
            {
                Volume = 0.45f,
                Pitch = -0.3f
            }, Projectile.Center);

            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.YellowTorch
                );
                d.velocity *= 2.7f;
                d.noGravity = true;
                d.scale = 1.5f;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Main.rand.NextVector2Circular(2f, 2f),
                    ModContent.ProjectileType<YellowBuffer>(),
                    0,
                    0f,
                    Projectile.owner
                );
            }

            Projectile.Kill();
        }
    }
    public class YellowBuffer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 360;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 toPlayer = player.Center - Projectile.Center;

            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                toPlayer.SafeNormalize(Vector2.Zero) * 10f,
                0.25f
            );

            Projectile.rotation += 0.25f;

            Lighting.AddLight(Projectile.Center, 0.9f, 0.8f, 0.2f);

            Dust d = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.YellowTorch
            );
            d.noGravity = true;
            d.scale = 1.1f;
            d.velocity *= 0.1f;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                ApplyRandomEffects(player);
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
                Projectile.Kill();
            }
        }

        private void ApplyRandomEffects(Player player)
        {
            int[] buffs =
            {
                BuffID.Regeneration,
                BuffID.Swiftness,
                BuffID.Ironskin,
                BuffID.Wrath,
                BuffID.ManaRegeneration,
                ModContent.BuffType<FlowOfWind>(),
                ModContent.BuffType<Glaciation>()
            };

            int[] debuffs =
            {
                BuffID.Poisoned,
                BuffID.Slow,
                BuffID.Weak,
                BuffID.BrokenArmor,
                BuffID.Darkness,
                ModContent.BuffType<BrokenWeaponry>()

            };

            for (int i = 0; i < 2; i++)
            {
                bool debuff = Main.rand.NextBool();

                if (debuff)
                    player.AddBuff(Main.rand.Next(debuffs), Main.rand.Next(180, 360));
                else
                    player.AddBuff(Main.rand.Next(buffs), Main.rand.Next(240, 480));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (1f - i / (float)Projectile.oldPos.Length) * 0.6f;

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    Color.Yellow * alpha,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }
    }
}
