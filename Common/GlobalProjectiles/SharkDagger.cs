using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalProjectiles
{
    public class ShaarkDaggerGP : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) =>
            projectile.type == ModContent.ProjectileType<Bismuth.Content.Projectiles.SharkKnifeP>();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[ModContent.ProjectileType<Bismuth.Content.Projectiles.SharkKnifeP>()] = 2;
            ProjectileID.Sets.TrailingMode[ModContent.ProjectileType<Bismuth.Content.Projectiles.SharkKnifeP>()] = 0;
        }

        public override void AI(Projectile projectile)
        {
            if (Main.rand.NextBool(2)) 
            {
                Dust dust = Dust.NewDustDirect(
                    projectile.position - projectile.velocity * 1.5f,
                    projectile.width,
                    projectile.height,
                    DustID.Smoke, 
                    projectile.velocity.X * 0.2f,
                    projectile.velocity.Y * 0.2f,
                    100, 
                    Color.Gray,
                    0.7f 
                );
                dust.noGravity = true;
                dust.fadeIn = 0.5f;
            }

            if (Main.rand.NextBool(4)) 
            {
                Dust glowDust = Dust.NewDustDirect(
                    projectile.Center - new Vector2(4, 4),
                    8, 8,
                    DustID.Ash, 
                    projectile.velocity.X * 0.1f,
                    projectile.velocity.Y * 0.1f,
                    80,
                    Color.LightGray,
                    0.5f
                );
                glowDust.noGravity = true;
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;

            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            Color glowColor = Color.LightGray * 0.35f;

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.PiOver2 * i) * 2f;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    glowColor,
                    projectile.rotation,
                    origin,
                    projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true; 
        }
    }
}