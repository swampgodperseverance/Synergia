using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class Pulsar2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 480;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.985f;
            Projectile.rotation += Projectile.velocity.Length() * 0.045f * Projectile.direction;

            int inset = 24;
            if (Collision.SolidCollision(Projectile.position + new Vector2(inset), Projectile.width - inset * 2, Projectile.height - inset * 2))
            {
                Projectile.velocity *= 0.82f;
                Projectile.alpha += 8;
            }

            Projectile.alpha += 2;
            if (Projectile.alpha >= 240)
            {
                Projectile.Kill();
                return;
            }

            if (Main.rand.NextBool(12))
            {
                int idx = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    323,
                    0f, 0f,
                    80,
                    default,
                    1f
                );

                Dust d = Main.dust[idx];
                d.velocity = Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(0.8f, 0.8f);
                d.noGravity = true;
                d.noLight = true;
                d.scale = Main.rand.NextFloat(0.9f, 1.4f);
                d.color = new Color(80, 220, 255);
            }

            Lighting.AddLight(Projectile.Center, 0.18f, 0.45f, 0.65f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < 6; i++)
            {
                float progress = i * 0.2f;
                Vector2 position = Vector2.Lerp(Projectile.Center, Projectile.oldPos[Projectile.oldPos.Length - 1], progress);
                float rotation = MathHelper.Lerp(Projectile.rotation, Projectile.oldRot[Projectile.oldRot.Length - 1], progress);
                Color color = Projectile.GetAlpha(lightColor) * MathHelper.Lerp(0.9f, 0f, progress) * 0.7f;

                Main.EntitySpriteDraw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    color,
                    rotation,
                    origin,
                    Projectile.scale * (1f - progress * 0.4f),
                    SpriteEffects.None,
                    0
                );
            }

            Color mainColor = Projectile.GetAlpha(new Color(200, 240, 255, 255) * lightColor.ToVector4().Length());
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 24; i++)
            {
                int d = Dust.NewDust(target.position, target.width, target.height, DustID.Shadowflame, 0f, 0f, 60, default, 1.4f);
                Main.dust[d].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 10f);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale = Main.rand.NextFloat(1.0f, 1.8f);
                Main.dust[d].color = new Color(20, 20, 40);
            }

            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(target.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 80, default, 1f);
                Main.dust[d].velocity = Main.rand.NextVector2Circular(7f, 7f);
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 0.8f;
            }

            if (target.boss)
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(target.Center, Main.rand.NextVector2Circular(3.5f, 3.5f), 12f, 0.75f, 200, 70, FullName));
            }
        }
    }
}