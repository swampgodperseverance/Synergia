using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using ValhallaMod.Projectiles.Ranged.Thrown;

namespace Synergia.Content.Projectiles.Thrower
{
    public class CactusStar2 : ModProjectile
    {
        private bool stuck;
        private int stuckNPC = -1;
        private int extraHits = 2;
        private int hitTimer;
        private int pulseTimer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (stuck)
            {
                if (stuckNPC < 0 || stuckNPC >= Main.maxNPCs || !Main.npc[stuckNPC].active)
                {
                    Projectile.Kill();
                    return;
                }

                NPC npc = Main.npc[stuckNPC];

                Projectile.Center = npc.Center;
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation += 0.25f;

                hitTimer++;
                pulseTimer++;

                if (hitTimer >= 30 && extraHits > 0)
                {
                    hitTimer = 0;
                    extraHits--;

                    npc.SimpleStrikeNPC( (int)(Projectile.damage * 0.5f),Projectile.direction);//yeah that part of code is definetely strange  idc
                    for (int i = 0; i < Main.rand.Next(3, 6); i++)
                    {
                        Vector2 speed = new Vector2(5f, 0f)
                            .RotatedByRandom(MathHelper.TwoPi) *
                            Main.rand.NextFloat(0.8f, 1.4f);

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            Projectile.Center,
                            speed,
                            ModContent.ProjectileType<CactusSpike>(),
                            (int)(Projectile.damage * 0.4f),
                            Projectile.knockBack,
                            Projectile.owner);
                    }

                    if (extraHits <= 0)
                        Projectile.Kill();
                }

                return;
            }

            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;
            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 30f)
            {
                Projectile.velocity.Y += 0.3f;
                Projectile.velocity.X *= 0.98f;
            }

            if (Projectile.velocity.Y > 12f)
                Projectile.velocity.Y = 12f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!stuck)
            {
                stuck = true;
                stuckNPC = target.whoAmI;
                Projectile.velocity = Vector2.Zero;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 180;
                Projectile.netUpdate = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            float opacity = 1f - Projectile.alpha / 255f;

            float glowPulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 12f + Projectile.whoAmI) * 0.5f + 0.5f;
            float pulseScale = 1f + glowPulse * 0.25f;

            Color glowColor = Color.Lerp(
                new Color(120, 255, 120),
                new Color(200, 255, 160),
                glowPulse
            ) * opacity * 0.8f;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            if (stuck)
            {
                sb.Draw(tex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    glowColor * 0.6f,
                    Projectile.rotation,
                    tex.Size() / 2f,
                    Projectile.scale * pulseScale * 1.4f,
                    SpriteEffects.None,
                    0f);

                sb.Draw(tex,
                    Projectile.Center - Main.screenPosition,
                    null,
                    glowColor * 0.35f,
                    Projectile.rotation,
                    tex.Size() / 2f,
                    Projectile.scale * pulseScale * 1.8f,
                    SpriteEffects.None,
                    0f);
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor * opacity,
                Projectile.rotation,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 40);
        }
    }
}
