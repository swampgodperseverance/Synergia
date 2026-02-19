using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Synergia.Helpers;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.Thrower
{
    public class FleshGrinder2 : ModProjectile   
    {
        private int timer;
        private PrimDrawer primDrawer;

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing; 
            Projectile.penetrate = -1;                      
            Projectile.alpha = 0;
            Projectile.scale = 1.1f;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = ProjAIStyleID.Boomerang;    
            AIType = ProjectileID.WoodenBoomerang;           
        }

        public override void OnSpawn(IEntitySource source)
        {
            primDrawer = new PrimDrawer(
                progress =>
                {
                    float pulse = 1f + 0.4f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 14f);
                    return MathHelper.Lerp(5f, 14f, EaseFunctions.EaseInOutSine(progress)) * pulse;
                },
                progress =>
                {
                    Color bright = new Color(220, 40, 40);
                    Color dark = new Color(80, 0, 0);
                    Color veryDark = new Color(30, 0, 0);
                    Color c = Color.Lerp(bright, dark, EaseFunctions.EaseInCubic(progress));
                    c = Color.Lerp(c, veryDark, progress * progress * 1.3f);
                    float pulse = 0.7f + 0.6f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f + progress * 6f);
                    c *= pulse;
                    c.A = (byte)(180 * (1f - progress * 0.4f));
                    return c;
                },
                null
            );
        }

        public override void AI()
        {
            timer++;

            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, Projectile.velocity * -0.2f, 120, default, 0.9f);
            d.noGravity = true;

            if (Main.rand.NextBool(3))
            {
                Vector2 vel = Projectile.velocity.RotatedByRandom(0.9) * Main.rand.NextFloat(0.3f, 0.9f);
                Dust.NewDustPerfect(Projectile.Center + vel * 8, DustID.Blood, vel, 60, default, 1.1f).noGravity = true;
            }
            if (timer == 40 && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Projectile.velocity.SafeNormalize(Vector2.UnitX) * 14f,
                    ModContent.ProjectileType<FleshGrinderProj>(),
                    Projectile.damage / 2,
                    1f,
                    Projectile.owner
                );

                for (int i = 0; i < 36; i++)
                {
                    float rot = i / 36f * MathHelper.TwoPi;
                    Vector2 dir = rot.ToRotationVector2();
                    Dust.NewDustPerfect(Projectile.Center, DustID.Blood, dir * 5.5f, 100, default, 1.4f).noGravity = true;
                }
            }

            Projectile.rotation += 0.75f * (Projectile.velocity.X > 0 ? 1 : -1);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            primDrawer?.DrawPrims(Projectile.oldPos, -Main.screenPosition, 180);

            Texture2D ringTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Color glowColor = new Color(255, 60, 60) * 0.5f;
            float pulse = 0.9f + 0.35f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 18f);

            Main.spriteBatch.Draw(
                ringTex,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * pulse,
                0f,
                ringTex.Size() / 2f,
                0.22f + 0.08f * pulse,
                SpriteEffects.None,
                0f
            );

            return true;
        }

    }

    public class FleshGrinderProj : ModProjectile
    {
        private int hits;
        private PrimDrawer smallPrim;

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            smallPrim = new PrimDrawer(
                p => MathHelper.Lerp(4f, 11f, EaseFunctions.EaseOutQuad(p)) * (1f - p * 0.6f),
                p =>
                {
                    Color c = Color.Lerp(
                        new Color(240, 70, 50),
                        new Color(100, 10, 10),
                        EaseFunctions.EaseInOutCubic(p)
                    );
                    c.A = (byte)(220 * (1f - p));
                    return c * (0.8f + 0.4f * (float)Math.Sin(p * 25f));
                }
            );

            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, Main.rand.NextVector2Circular(5.4f, 5.4f), 80, default, 1.3f);
                d.noGravity = true;
                d.fadeIn = 0.9f;
            }
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f * Projectile.direction;

            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, Projectile.velocity * -0.15f, 100, default, 0.8f);
            d.noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            smallPrim?.DrawPrims(Projectile.oldPos, -Main.screenPosition, 90);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hits++;
            if (hits >= 2)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.Blood,
                        Main.rand.NextVector2Circular(4f, 4f),
                        0,
                        default,
                        1.4f
                    ).noGravity = true;
                }
                Projectile.Kill();
            }
        }
    }
}