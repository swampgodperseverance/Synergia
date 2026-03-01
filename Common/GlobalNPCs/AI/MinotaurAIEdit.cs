using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Synergia.Content.Compat
{
    public class MinotaurAI : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => ModLoader.TryGetMod("Bismuth", out Mod bismuth) && projectile.type == bismuth.Find<ModProjectile>("MinosBlastCallP").Type;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {

                    Vector2 pos = projectile.Center;
                    Vector2 vel = projectile.velocity;

                    projectile.Kill();

                    Projectile.NewProjectile(
                        source,
                        pos,
                        vel,
                        ModContent.ProjectileType<Content.Projectiles.Hostile.Bosses.NarsilEvil>(),
                        5,    
                        2f,   
                        Main.myPlayer
                    );
        }
    }
}
