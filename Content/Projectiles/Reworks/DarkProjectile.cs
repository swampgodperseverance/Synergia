using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class DarkProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.alpha = 100;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            if (Projectile.velocity.Y > 8f)
                Projectile.velocity.Y = 8f;
                
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Shadowflame,
                    Projectile.velocity * 0.5f,
                    100,
                    new Color(80, 0, 120),
                    0.8f
                );
                dust.noGravity = true;
            }
            
            if (Projectile.timeLeft < 20)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2);
                Color color = new Color(80, 0, 120, 100) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }
            
            Color darkPurple = new Color(120, 0, 180, 150);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                darkPurple,
                Projectile.rotation,
                texture.Size() / 2,
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Shadowflame,
                    Main.rand.NextVector2Circular(3f, 3f),
                    100,
                    new Color(80, 0, 120),
                    1.2f
                );
                dust.noGravity = true;
            }
            
            SoundEngine.PlaySound(SoundID.Item10 with { Pitch = -0.3f }, Projectile.Center);
        }
    }
}