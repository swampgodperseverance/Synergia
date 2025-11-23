using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public sealed class MagicStalactiteSpawn : ModProjectile
    {
        bool Moved;
        Vector2 StartVelocity;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 2;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 700;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[1]++;

            if (!Moved && Projectile.ai[1] >= 0)
                Moved = true;

            if (Projectile.ai[1] == 1)
                StartVelocity = Projectile.velocity;

            if (Projectile.ai[1] == 2)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;
                Projectile.velocity = -Projectile.velocity;
            }

            if (Projectile.ai[1] == 5 || Projectile.ai[1] == 10 || Projectile.ai[1] == 15 || Projectile.ai[1] == 20)
                {
                    var EntitySource = Projectile.GetSource_FromThis();

                    if (Main.rand.NextBool(12)) // реже
                    {
                        Projectile.NewProjectile(EntitySource, Projectile.Center.X + Main.rand.Next(-50, 50), Projectile.Center.Y + Main.rand.Next(-50, 50),
                            StartVelocity.X, StartVelocity.Y, ModContent.ProjectileType<MagicStalactite>(), (int)(Projectile.damage * 1.2f), 1, Projectile.owner);
                    }

                    if (Main.rand.NextBool(5)) // реже
                    {
                        Projectile.NewProjectile(EntitySource, Projectile.Center.X + Main.rand.Next(-50, 50), Projectile.Center.Y + Main.rand.Next(-50, 50),
                            StartVelocity.X, StartVelocity.Y, ModContent.ProjectileType<MagicStalactite>(), (int)(Projectile.damage * 1.2f), 1, Projectile.owner);
                    }
                    else
                    {
                        Projectile.NewProjectile(EntitySource, Projectile.Center.X + Main.rand.Next(-50, 50), Projectile.Center.Y + Main.rand.Next(-50, 50),
                            StartVelocity.X, StartVelocity.Y, ModContent.ProjectileType<MagicStalactite>(), (int)(Projectile.damage * 0.8f), 1, Projectile.owner);
                    }
                }

            if (Projectile.ai[1] >= 0 && Projectile.ai[1] <= 20)
                Projectile.velocity *= 0.8f;

            if (Projectile.ai[1] == 20)
                Projectile.velocity = -Projectile.velocity;

            if (Projectile.ai[1] >= 21 && Projectile.ai[1] <= 60)
                Projectile.velocity /= 0.90f;

            if (Projectile.ai[1] == 60)
                Projectile.tileCollide = true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dust1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, -2f, 0, default, 1f);
                Main.dust[dust1].noGravity = true;
                Main.dust[dust1].scale = 1.3f;
                Main.dust[dust1].velocity *= 2f;

                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -2f, 0, default, 1.2f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 1.8f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 100, 0, 255); // Лавово-оранжевый
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.rand.NextBool(4))
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 150, Color.OrangeRed, 1.2f);
                Main.dust[dustnumber].velocity *= 0.4f;
                Main.dust[dustnumber].noGravity = true;
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = new Color(255, 80, 10, 150) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length / 2);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            // Более яркое лавовое свечение
            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3() * 2.4f * Main.essScale);
        }
    }
}
