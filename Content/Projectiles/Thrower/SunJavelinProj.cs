using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Thrower
{
    public class SunJavelinProj : ModProjectile
    {
        private static readonly Color SunColor = new Color(255, 245, 180);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 76;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 78;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1.1f, 1.0f, 0.6f);
            Projectile.ai[0]++;

            float speed = Projectile.velocity.Length();
            if (speed > 0f)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * speed;
            }
            else
            {
                Projectile.velocity = Vector2.UnitX * 10f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (Projectile.alpha > 0)
                Projectile.alpha -= 5;
            else
                Projectile.alpha = 0;

            if (Projectile.ai[0] > 5)
            {
                Rectangle projectileHitbox = Projectile.Hitbox;

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc.CanBeChasedBy() &&
                        projectileHitbox.Intersects(npc.Hitbox))
                    {
                        int damage = Projectile.damage;
                        float knockback = Projectile.knockBack;
                        bool crit = false;

                        NPC.HitInfo hitInfo = new NPC.HitInfo
                        {
                            Damage = damage,
                            Knockback = knockback,
                            Crit = crit,
                            HitDirection = npc.Center.X < Projectile.Center.X ? -1 : 1
                        };

                        npc.StrikeNPC(hitInfo, true, true);

                        if (Projectile.penetrate == 1)
                        {
                            Projectile.Kill();
                            return;
                        }
                        break;
                    }
                }
            }

            Vector2 perpOffset = Projectile.velocity.RotatedBy(MathHelper.PiOver2) * 3f;

            if (Projectile.ai[0] > 45)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 25f;

                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center + perpOffset,
                        DustID.GoldFlame,
                        -Projectile.velocity * Main.rand.NextFloat(0.5f, 1.2f),
                        100,
                        SunColor,
                        0.9f
                    );

                    dust.noGravity = true;
                    dust.fadeIn = 0.6f;
                }
            }
            else
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 pos = Projectile.Center
                        + Projectile.velocity * Main.rand.NextFloat(20f, 120f)
                        + perpOffset;

                    Dust dust = Dust.NewDustPerfect(
                        pos,
                        DustID.GoldFlame,
                        Vector2.Zero,
                        120,
                        SunColor,
                        0.7f
                    );

                    dust.noGravity = true;
                    dust.fadeIn = 0.5f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + speed * 5f,
                    DustID.GoldFlame,
                    speed,
                    0,
                    SunColor,
                    1.6f);

                dust.noGravity = true;
                dust.fadeIn = 1.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CombatText.NewText(target.Hitbox, Color.Red, "HIT");
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(
                "Synergia/Content/Projectiles/Thrower/SunJavelinProj").Value;

            Vector2 origin = texture.Size() * 0.5f;
            Vector2 pos = Projectile.Center - Main.screenPosition;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size * 0.5f - Main.screenPosition;
                float fade = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;

                Color color = SunColor * 0.45f * fade * (1f - Projectile.alpha / 255f);
                Main.EntitySpriteDraw(glow, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            }

            Main.EntitySpriteDraw(texture, pos, null,
                lightColor * (1f - Projectile.alpha / 255f),
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            Main.EntitySpriteDraw(glow, pos, null,
                SunColor * 0.45f * (1f - Projectile.alpha / 255f),
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            return false;
        }
    }
}