using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Friendly; 

public class HeliosYoyo : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    private int shootTimer = 0;

    public override void AI(Projectile projectile)
    {
        if (projectile.ModProjectile != null &&
            projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
            projectile.ModProjectile.Name == "SolarYoyo")
        {
            shootTimer++;

            if (shootTimer >= 90) 
            {
                shootTimer = 0;

                Vector2 pos = projectile.Center;
                int damage = projectile.damage / 2; 
                float knockback = projectile.knockBack;
                int owner = projectile.owner;
                Vector2[] directions = new Vector2[]
                {
                    new Vector2(1f, 0f),   // вправо
                    new Vector2(-1f, 0f),  // влево
                    new Vector2(0f, -1f),  // вверх
                    new Vector2(0f, 1f)    // вниз
                };

                foreach (var dir in directions)
                {
                    Projectile.NewProjectile(
                        projectile.GetSource_FromAI(),
                        pos,
                        dir * 8f, 
                        ModContent.ProjectileType<SunballProj>(),
                        damage,
                        knockback,
                        owner
                    );
                }
            }
        }
    }
}
