using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Projectiles.Friendly
{
    public class ValhalliteJavelinProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 113; 
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(64)) 
            {
                int soul = Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<ValhalliteSoul>(),
                    0, 
                    0,
                    Projectile.owner
                );
                Main.projectile[soul].ai[0] = Projectile.identity;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.6f, 0.9f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(3f, 3f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.MagicMirror, speed.X, speed.Y, 150, default, 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale *= Main.rand.NextFloat(1f, 1.5f);
            }

            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(6f, 6f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueCrystalShard, speed.X, speed.Y, 200, default, 1.8f);
                Main.dust[dust].noGravity = true;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(5))
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 shardVelocity = Main.rand.NextVector2Circular(4f, 4f);
                    Projectile.NewProjectile(
                        Projectile.GetSource_Death(),
                        Projectile.Center,
                        shardVelocity,
                        ModContent.ProjectileType<ValhalliteSoul>(),
                        Projectile.damage / 2,
                        Projectile.knockBack / 2,
                        Projectile.owner
                    );
                }
            }



            Lighting.AddLight(Projectile.Center, 0.8f, 1.0f, 1.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + drawOrigin - Main.screenPosition;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
