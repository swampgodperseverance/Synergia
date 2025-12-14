using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class QueenBeeProj : ModProjectile
    {
        private const int TotalFrames = 4;
        private int frameCounter = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = TotalFrames;
        }

        public override void SetDefaults()
        {
            Projectile.width = 168;
            Projectile.height = 134;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Roar, Projectile.position);
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<ScreenShakePlayer>().TriggerShake(7, 1.1f);
        }


        public override void AI()
        {
            frameCounter++;
            if (frameCounter >= 3)
            {
                frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= TotalFrames)
                    Projectile.frame = 0;
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            float speed = 18f;
            Vector2 target = new Vector2(-Projectile.width, Projectile.position.Y);
            Vector2 direction = target - Projectile.position;
            direction.Normalize();
            Projectile.velocity = direction * speed;

            if (Projectile.position.X < 100)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha > 255)
                    Projectile.Kill();
            }

            Projectile.damage = 500;
            Projectile.knockBack = 5f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = new Rectangle(0, Projectile.frame * texture.Height / TotalFrames, texture.Width, texture.Height / TotalFrames);
            Vector2 origin = frame.Size() / 2f;
            SpriteEffects effects = SpriteEffects.None;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, Color.White * ((255 - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, effects, 0f);
            return false;
        }
    }
}
