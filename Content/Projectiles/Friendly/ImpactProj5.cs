using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Projectiles.Friendly
{
    public class CogwormProj5 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;  
            Projectile.timeLeft = 200;
            Projectile.damage = 90;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            AIType = ProjectileID.EnchantedBoomerang;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-10, -5));
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<HellFriendlyMeteor1>(),
                    Projectile.damage / 2, 
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;

          
                Color trailColor = Color.Lerp(Color.Red, Color.Orange, progress);
                trailColor = Color.Lerp(trailColor, Color.Yellow, progress * 0.5f);

                trailColor *= progress; 

                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);

                Main.spriteBatch.Draw(texture, drawPos, null, trailColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

                Main.spriteBatch.Draw(texture, drawPos, null, Color.White * (progress * 0.3f), Projectile.rotation, drawOrigin, Projectile.scale * 1.2f, SpriteEffects.None, 0f);
            }

            return true; 
        }

    }
}
