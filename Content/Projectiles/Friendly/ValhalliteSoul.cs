using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Projectiles.Friendly
{
    public class ValhalliteSoul : ModProjectile
    {
        private float appear;
        private float pulse;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.damage = 10;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (appear < 1f)
                appear += 0.08f;

            pulse += 0.08f;

            float scaleWave = 1f + (float)System.Math.Sin(pulse) * 0.08f;
            Projectile.scale = scaleWave * appear;

            Projectile.velocity *= 0.95f;

            Lighting.AddLight(Projectile.Center, 0.4f * appear, 0.8f * appear, 1f * appear);

            if (Projectile.timeLeft == 1)
            {
                for (int i = 0; i < 8; i++)
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, Scale: 1.2f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);

            Vector2 origin = frame.Size() * 0.5f;
            Vector2 pos = Projectile.Center - Main.screenPosition;

            float alpha = appear;
            float outlineStrength = 0.4f * alpha;

            Color glowColor = Color.Cyan * outlineStrength;

            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = new Vector2(1.5f, 0f).RotatedBy(i * MathHelper.TwoPi / 6f);
                Main.spriteBatch.Draw(texture, pos + offset, frame, glowColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(texture, pos, frame, Color.White * alpha, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slow, 180);
        }
    }
}