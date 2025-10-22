using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroFire : ModProjectile
    {
        public static readonly SoundStyle NecroSword = new("Synergia/Assets/Sounds/NecroSword");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 70;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.6f, 0.1f, 0.7f);

            if (Projectile.timeLeft > 55)
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, (70f - Projectile.timeLeft) / 15f);
            else if (Projectile.timeLeft < 10)
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, (10f - Projectile.timeLeft) / 10f);
            else
                Projectile.alpha = 0;

            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 150, Color.Purple, 1.2f);
                d.noGravity = true;
                d.velocity *= 0.3f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            Color trailColor = new Color(180, 80, 255, 120);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float progress = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;
                float scale = Projectile.scale * (0.8f + 0.2f * progress);
                Color color = trailColor * progress * 0.8f;
                Main.spriteBatch.Draw(texture, trailPos, frame, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame,
                new Color(200, 100, 255, 255) * ((255 - Projectile.alpha) / 255f),
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch,
                    Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 150, Color.Purple, 1.4f);
                d.noGravity = true;
            }

            SoundEngine.PlaySound(NecroSword with { Volume = 0.7f, Pitch = 0.2f }, Projectile.Center);
        }
    }
}
