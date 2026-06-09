using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Armor;

namespace Synergia.Content.Compat
{
    public class PalladiumArmorEdit : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => ModLoader.TryGetMod("Bismuth", out Mod bismuth) && projectile.type == bismuth.Find<ModProjectile>("SphereOfLight").Type;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Vector2 pos = projectile.Center;
            Vector2 vel = projectile.velocity;
            int damage = projectile.damage;
            float knockback = projectile.knockBack;

            projectile.Kill();

            Projectile.NewProjectile(
                source,
                pos,
                vel,
                ModContent.ProjectileType<PalladiumArmorRework>(),
                damage,
                knockback,
                projectile.owner
            );
        }
    }
}