using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Hostile;

namespace Synergia.Content.GlobalProjectiles
{
    public class JadeOrbGoreGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "JadeOrbGore")
            {
                projectile.rotation += 0.2f;
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "JadeOrbGore")
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int count = 5;
                    float baseSpeedY = -8f; 
                    float spreadX = 2f; 

                    for (int i = 0; i < count; i++)
                    {
                        float offsetX = MathHelper.Lerp(-spreadX, spreadX, i / (float)(count - 1));
                        Vector2 velocity = new Vector2(offsetX, baseSpeedY);

                        Projectile.NewProjectile(
                            projectile.GetSource_Death(),
                            projectile.Center,
                            velocity,
                            ModContent.ProjectileType<MicroOrb>(),
                            projectile.damage / 2,
                            projectile.knockBack,
                            projectile.owner
                        );
                    }
                }
            }
        }
    }
}