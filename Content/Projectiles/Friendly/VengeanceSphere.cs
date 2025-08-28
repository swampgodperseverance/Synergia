using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValhallaMod.Dusts;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Friendly
{
    public class VengeanceSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60 * 60 * 10; 
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 60;

            int index = (int)Projectile.ai[0];
            float angle = MathHelper.TwoPi * index / 4f + Main.GameUpdateCount * 0.03f;
            float distance = 64f;

            Projectile.Center = player.Center + angle.ToRotationVector2() * distance;


            if (Projectile.localAI[0] < 30f)
            {
                Projectile.localAI[0]++;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.localAI[0] / 30f);
            }
            else
            {
                Projectile.alpha = 0;
            }


            Lighting.AddLight(Projectile.Center, 0f, 0.8f, 0.2f);


            if (Main.rand.NextBool(25))
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>(), Main.rand.NextVector2Circular(0.5f, 0.5f));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {

            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = new Color(100, 255, 150, 150) * fade * 0.6f;

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor * (1f - Projectile.alpha / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false; 
        }

        public override void OnKill(int timeLeft)
        {

            for (int i = 0; i < 12; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(2f, 2f);
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>(), speed, 150, default, 1.2f);
            }
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {

            for (int i = 0; i < 18; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(2.5f, 2.5f);
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>(), speed, 120, default, 1.3f);
            }
            Projectile.alpha = 255;
            Projectile.localAI[0] = 0f; 
        }
    }
}
