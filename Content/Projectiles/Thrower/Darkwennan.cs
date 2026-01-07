using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Thrower
{
    public class Darkwennan : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 46;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 22f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Shadowflame
                );
                d.noGravity = true;
                d.velocity *= 0.2f;
            }

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke
                );
                d.color = Color.Black;
                d.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_OnHit(target),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DarkwennanBlood>(),
                    0,
                    0f,
                    Projectile.owner
                );
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 18; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Shadowflame
                );
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(3f, 3f);
            }

            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke
                );
                d.color = Color.Black;
                d.noGravity = true;
                d.velocity = Main.rand.NextVector2Circular(2f, 2f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    Color.Black * alpha * 0.6f,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0f
                );
            }

            return true;
        }
    }

    public class DarkwennanBlood : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 500;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 toPlayer = player.Center - Projectile.Center;

            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                toPlayer.SafeNormalize(Vector2.Zero) * 6f,
                0.15f
            );

            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Blood
                );
                d.noGravity = true;
                d.velocity *= 0.1f;
            }

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                int heal = Main.rand.Next(4, 7);
                player.statLife += heal;
                player.HealEffect(heal);
                Projectile.Kill();
            }
        }
    }
}
