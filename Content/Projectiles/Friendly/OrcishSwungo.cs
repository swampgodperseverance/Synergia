using Synergia.Common.ModSystems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Friendly
{
    public class OrcishSwungoP : ModProjectile {
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 50; 
            Projectile.height = 48;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.alpha = 215;
            Projectile.extraUpdates = 1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = 10; 
            fallThrough = true; 
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) 
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) {
                Projectile.velocity.X += Projectile.direction * 2.5f;
                Projectile.velocity.Y = -oldVelocity.Y * 1.2f;
            }
            return false;
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item10.WithVolumeScale(0.9f).WithPitchOffset(0.25f), Projectile.Center);
            for (byte i = 0; i < 7; i++) {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, Color.Purple, 1.4f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 2f;
            }
        }

        public override void AI() {
            if (Projectile.alpha > 90) Projectile.alpha -= 4;
            else Projectile.alpha = 90;

            if (Projectile.timeLeft > 135) Projectile.velocity *= 0.92f;
            else {
                Projectile.velocity.X *= 0.963f;
                if (Projectile.timeLeft > 120) Projectile.velocity.Y += 0.04f;
                else Projectile.velocity.Y = Utils.Clamp(Projectile.velocity.Y - 0.045f, -8f, 0f);
            }

            if (Projectile.velocity.Length() > 10f)
                Projectile.velocity /= 1.1f;

            Projectile.rotation += -Projectile.direction * 0.28f;

            if (Main.rand.NextBool(2)) {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, Projectile.velocity.X, Projectile.velocity.Y, 100, Color.Purple, 1.1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 0.25f;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            byte colValue = (byte)((Projectile.timeLeft + 75) * Projectile.Opacity);
            lightColor = new Color(colValue, colValue, colValue, colValue);
            MUtils.DrawSimpleAfterImage(lightColor, Projectile, TextureAssets.Projectile[Type].Value, 1f, 1f, 0.2f, 2f);
            return true;
        }
    }
}
