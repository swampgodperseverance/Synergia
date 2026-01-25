using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Utilities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;
namespace Synergia.Content.Projectiles.Hostile
{
    public class BigBobLaser : ModProjectile
    {
        private static readonly UnifiedRandom rng = new UnifiedRandom();
        private const float HomingAccel = 0.75f;
        private const float MaxSpeed = 14f;

        private int lifeTimer;
        private float alpha;

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 125;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            lifeTimer++;

            if (lifeTimer < 20)
                alpha = MathHelper.Lerp(alpha, 1f, 0.15f);
            else if (Projectile.timeLeft < 25)
                alpha = MathHelper.Lerp(alpha, 0f, 0.2f);
            else
                alpha = 1f;

            if (Projectile.timeLeft == 95)
            {
                Projectile.velocity += new Vector2(
                    rng.NextBool() ? -6f : 9f,
                    rng.NextBool() ? -10f : 6f
                );
            }

            if (lifeTimer > 40)
            {
                Player target = FindClosestPlayer();
                if (target != null)
                {
                    Vector2 dir = Projectile.DirectionTo(target.Center);
                    Projectile.velocity += dir * HomingAccel;

                    if (Projectile.velocity.Length() > MaxSpeed)
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * MaxSpeed;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        private Player FindClosestPlayer()
        {
            Player result = null;
            float dist = float.MaxValue;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (!p.active || p.dead)
                    continue;

                float d = Vector2.Distance(Projectile.Center, p.Center);
                if (d < dist)
                {
                    dist = d;
                    result = p;
                }
            }
            return result;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = tex.Size() * 0.5f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color col = new Color(255, 60, 60, 0) * fade * alpha;

                Vector2 drawPos =
                    Projectile.oldPos[i]
                    + Projectile.Size * 0.5f
                    - Main.screenPosition;

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    col,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 60, 60, 0) * alpha;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
    }
}
    