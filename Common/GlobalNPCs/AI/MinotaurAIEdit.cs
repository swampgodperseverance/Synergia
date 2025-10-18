using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Synergia.Content.Compat
{
    public class BismuthProjectileReplacer : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {

            if (ModLoader.TryGetMod("Bismuth", out Mod bismuth))
            {

                if (projectile.type == bismuth.Find<ModProjectile>("MinosBlastCallP").Type)
                {
                    Vector2 pos = projectile.Center;
                    Vector2 vel = projectile.velocity;

                    projectile.Kill();

                    Projectile.NewProjectile(
                        source,
                        pos,
                        vel,
                        ModContent.ProjectileType<Content.Projectiles.Hostile.Bosses.NarsilEvil>(),
                        5,    // урон
                        2f,    // отбрасывание
                        Main.myPlayer
                    );
                }
            }
        }
    }
}
