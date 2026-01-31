using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewHorizons.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class MythrilJavelin2 : ModProjectile
    {
        public override void SetDefaults()
        {
            base.Projectile.width = 18;
            base.Projectile.height = 18;
            base.Projectile.friendly = true;
            base.Projectile.timeLeft = 600;
            base.Projectile.penetrate = 5;
            base.Projectile.DamageType = DamageClass.Throwing;
            base.Projectile.extraUpdates = 2;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Player player = Main.player[base.Projectile.owner];
            base.Projectile.rotation = base.Projectile.velocity.ToRotation() + 1.57f;
            Projectile projectile = base.Projectile;
            projectile.velocity.Y = projectile.velocity.Y + 0.02f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == base.Projectile.owner)
            {
                Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);

                for (int i = 0; i < 3; i++)
                {
                    bool spawnLeft = Main.rand.NextBool();
                    float spawnX = spawnLeft ? Main.screenPosition.X - 100 : Main.screenPosition.X + Main.screenWidth + 100;
                    float spawnY = Main.screenPosition.Y + Main.screenHeight / 2f + Main.rand.Next(-100, 101);

                    Vector2 spawnPos = new Vector2(spawnX, spawnY);
                    Vector2 direction = (target.Center - spawnPos).SafeNormalize(Vector2.UnitX);
                    float speed = 12f + Main.rand.NextFloat(0f, 3f);

                    Projectile.NewProjectile(
                        base.Projectile.GetSource_FromThis(),
                        spawnPos,
                        direction * speed,
                        ModContent.ProjectileType<MythrilKnife>(),
                        (int)(base.Projectile.damage * 0.5f),
                        base.Projectile.knockBack * 0.5f,
                        base.Projectile.owner
                    );
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, new Vector2?(base.Projectile.position), null);
            if (Main.rand.NextBool(10))
            {
                Item.NewItem(base.Projectile.GetSource_DropAsItem(null), (int)base.Projectile.position.X, (int)base.Projectile.position.Y, base.Projectile.width, base.Projectile.height, ModContent.ItemType<MythrilJavelin>(), 1, false, 0, false, false);
                return;
            }
            Vector2 vector9 = base.Projectile.position;
            Vector2 value19 = (base.Projectile.rotation - 1.5707964f).ToRotationVector2();
            vector9 += value19 * 16f;
            for (int num257 = 0; num257 < 20; num257++)
            {
                int newDust = Dust.NewDust(vector9, base.Projectile.width, base.Projectile.height, 49, 0f, 0f, 0, default(Color), 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + base.Projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
                vector9 -= value19 * 8f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return true;
        }
    }
        public class MythrilKnife : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.TrailCacheLength[Type] = 12;
                ProjectileID.Sets.TrailingMode[Type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.width = 12;
                Projectile.height = 12;
                Projectile.friendly = true;
                Projectile.timeLeft = 300;
                Projectile.penetrate = 1;
                Projectile.DamageType = DamageClass.Throwing;

                Projectile.extraUpdates = 1;

                Projectile.tileCollide = false;
                Projectile.ignoreWater = true;

                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
            }

            public override void AI()
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            public override void Kill(int timeLeft)
            {
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

                for (int i = 0; i < 15; i++)
                {
                    Vector2 dustVel = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.2f, 0.4f);
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.Silver,
                        0f, 0f, 100,
                        default, 1.2f
                    );
                    dust.velocity = dustVel;
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                }

                for (int i = 0; i < 8; i++)
                {
                    Dust.NewDust(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.Mythril,
                        Projectile.velocity.X * 0.1f,
                        Projectile.velocity.Y * 0.1f,
                        100,
                        default,
                        0.8f
                    );
                }
            }

            public override bool PreDraw(ref Color lightColor)
            {
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                Texture2D trailTexture = ModContent.Request<Texture2D>(
                    "Synergia/Assets/Textures/Trails/MythrilKnife_Trail"
                ).Value;

                Vector2 origin = texture.Size() * 0.5f;

                // Трейл
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                    Color color = Color.White * progress * 0.6f;

                    Vector2 drawPos =
                        Projectile.oldPos[i]
                        + Projectile.Size / 2f
                        - Main.screenPosition;

                    Main.EntitySpriteDraw(
                        trailTexture,
                        drawPos,
                        null,
                        color,
                        Projectile.rotation,
                        trailTexture.Size() * 0.5f,
                        Projectile.scale,
                        SpriteEffects.None,
                        0
                    );
                }

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.Center - Main.screenPosition,
                    null,
                    lightColor,
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