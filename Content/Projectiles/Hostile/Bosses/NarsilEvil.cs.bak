using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NarsilEvil : ModProjectile
    {
        private bool startedMoving = false;
        private float appearProgress = 0f;
        private Vector2 targetDirection = Vector2.Zero;
        
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 90;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player target = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];
            if (target == null || !target.active) return;

            if (Projectile.timeLeft > 80)
            {
                appearProgress += 0.1f;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, appearProgress);
                Projectile.velocity *= 0.9f;

                targetDirection = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                Projectile.rotation = targetDirection.ToRotation() + MathHelper.PiOver2;
            }

            if (Projectile.timeLeft == 80)
                targetDirection = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);

            if (Projectile.timeLeft <= 80 && !startedMoving)
            {
                startedMoving = true;
                Projectile.velocity = targetDirection * 0.5f;
            }

            if (startedMoving && Projectile.timeLeft <= 80 && Projectile.timeLeft > 10)
                Projectile.velocity *= 1.07f;

            if (Projectile.timeLeft <= 10)
            {
                float fadeOut = 1f - (Projectile.timeLeft / 10f);
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, fadeOut);
                Projectile.velocity *= 0.92f;
            }

            if (startedMoving && Projectile.velocity.Length() > 0.01f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White * ((255 - Projectile.alpha) / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                color,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.2f;
            }
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.6f, Pitch = 0.1f }, Projectile.Center);
        }
    }
}