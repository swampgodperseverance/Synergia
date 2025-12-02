using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Synergia.Helpers;
using Terraria.Audio;
using Terraria.ModLoader;
using Avalon.Dusts;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class TriArrow : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];
        private ref float Delay => ref Projectile.ai[1];
        private const float InitialSpeed = 7.5f;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Timer++;

            if (Delay > 0)
            {
                Delay--;
                Projectile.velocity = Vector2.Zero;
                return;
            }

            if (Timer <= 40)
            {
                Projectile.velocity *= 0.93f;

                if (Main.rand.NextBool(4))
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(6, 6),
                        ModContent.DustType<StarstoneDust>(),
                        Vector2.Zero,
                        160,
                        Color.White,
                        0.8f);
                    d.noGravity = true;
                }
            }
            else
                {
                float progress = MathHelper.Clamp((Timer - 40f) / 16f, 0f, 1f);
                float ease = 1f - (float)Math.Pow(1f - progress, 4.5f);

                float targetSpeed = InitialSpeed * 5f;
                float currentSpeed = MathHelper.Lerp(InitialSpeed * 0.2f, targetSpeed, ease);

                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * currentSpeed;

                if (Main.rand.NextBool(2))
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center,
                        ModContent.DustType<StarstoneDust>(),
                        Projectile.velocity.RotatedByRandom(0.35f) * -0.18f,
                        40,
                        Color.White,
                        Main.rand.NextFloat(1.3f, 2f));
                    d.noGravity = true;
                }

                if (Timer == 41)
                    SoundEngine.PlaySound(SoundID.Item91 with { Volume = 0.7f, Pitch = 0.3f }, Projectile.position);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 14; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center,
                    ModContent.DustType<StarstoneDust>(),
                    Main.rand.NextVector2Circular(7f, 7f),
                    50,
                    Color.White,
                    Main.rand.NextFloat(1.2f, 1.8f));
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex,
                Projectile.Center - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                tex.Size() * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0);
            return false;
        }
    }
}