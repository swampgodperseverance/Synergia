using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class FreezeBoltRework : ModProjectile
    {
        private readonly VertexStrip vertexStrip = new VertexStrip();
        private bool framePicked;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 14;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 2;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            if (!framePicked)
            {
                Projectile.frame = Main.rand.Next(3);
                framePicked = true;
            }

            Projectile.rotation += 0.2f;
            Projectile.velocity *= 0.992f;

            NPC target = FindTarget();
            if (target != null)
            {
                Vector2 desired = Projectile.DirectionTo(target.Center) * 11.5f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.14f);
            }

            if (Projectile.timeLeft < 18)
                Projectile.velocity *= 0.88f;
        }

        private NPC FindTarget()
        {
            NPC closest = null;
            float dist = 520f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;

                float d = Vector2.Distance(Projectile.Center, npc.Center);
                if (d < dist)
                {
                    dist = d;
                    closest = npc;
                }
            }

            return closest;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    267,
                    Main.rand.NextVector2Circular(3.2f, 3.2f),
                    0,
                    new Color(120, 210, 255),
                    Main.rand.NextFloat(1f, 1.6f)
                );
                d.noGravity = true;
            }

            for (int i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    264,
                    Main.rand.NextVector2Circular(2f, 2f),
                    0,
                    Color.White * 0.7f,
                    1.1f
                );
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            float fade = Projectile.timeLeft < 15 ? Projectile.timeLeft / 15f : 1f;

            sb.BeginBlendState(BlendState.Additive, isUI2: true);
            GameShaders.Misc["MagicMissile"].Apply();

            vertexStrip.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                p => Color.Lerp(
                    new Color(120, 220, 255).MultiplyAlpha(fade),
                    new Color(180, 240, 255).MultiplyAlpha(0.3f),
                    p
                ),
                p => 38f * Projectile.scale * (1f - p),
                -Main.screenPosition + Projectile.Size / 2,
                true
            );

            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = tex.Frame(1, 3, 0, Projectile.frame);

            sb.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                frame,
                new Color(200, 230, 255, 180),
                Projectile.rotation,
                frame.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
    public class FreezeSpawn : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void AI()
        {
            SpawnSnowflake();
            SpawnBolts();
            Projectile.Kill();
        }

        private void SpawnBolts()
        {
            if (Projectile.owner != Main.myPlayer) return;

            float baseSpeed = 9f;
            float spread = MathHelper.ToRadians(35f);

            for (int i = 0; i < 4; i++)
            {
                Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.UnitY)
                    .RotatedBy(MathHelper.Lerp(-spread, spread, i / 3f));

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    dir * baseSpeed,
                    ModContent.ProjectileType<FreezeBoltRework>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        private void SpawnSnowflake()
        {
            int shape = Main.rand.Next(4);
            int arms = shape == 0 ? 6 : shape == 1 ? 4 : shape == 2 ? 8 : 3;
            int points = shape == 3 ? 7 : 5;
            float length = 24f + shape * 4f;

            for (int a = 0; a < arms; a++)
            {
                float angle = MathHelper.TwoPi * a / arms;

                for (int p = 1; p <= points; p++)
                {
                    float prog = p / (float)points;
                    Vector2 offset = angle.ToRotationVector2() * length * prog;

                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center + offset,
                        267,
                        Vector2.Zero,
                        0,
                        new Color(150, 225, 255),
                        0.9f + prog * 0.7f
                    );
                    d.noGravity = true;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    264,
                    Main.rand.NextVector2Circular(2.4f, 2.4f),
                    0,
                    Color.White,
                    1.1f
                );
                d.noGravity = true;
            }
        }
    }
}
