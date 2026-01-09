using Terraria;
using Terraria.Audio;
using Terraria.ID;
using ValhallaMod.Projectiles.Enemy;

namespace Synergia.Content.Projectiles.Other
{
    public class WhitePresentProj : BasePresentProj {
        public override void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.45f, Pitch = -0.3f }, Projectile.Center);

            for (int i = 0; i < 28; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch);
                d.velocity *= 2.6f;
                d.noGravity = true;
                d.scale = 1.4f;
            }
            if (Main.myPlayer == Projectile.owner) {
                float heartChance = 0.5f;

                if (Main.masterMode) {
                    heartChance = 0.3f;
                }
                else if (Main.expertMode) {
                    heartChance = 0.4f;
                }
                if (Main.rand.NextFloat() < heartChance) {
                    Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), ItemID.Heart);
                }
                else {
                    Vector2 spawnPos = Projectile.Center;
                    Vector2 velocity = new(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-6f, -4f));

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, velocity, ModContent.ProjectileType<SnowmanLivingBall>(), 0, 0f, Projectile.owner);
                }
            }
            Projectile.Kill();
        }
    }
}