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
        
            }
        }


        
    }
}