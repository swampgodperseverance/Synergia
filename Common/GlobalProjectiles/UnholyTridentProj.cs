using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.GlobalProjectiles
{
    public class UnholyTridentProj : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type == ProjectileID.UnholyTridentFriendly)
                target.AddBuff(BuffID.ShadowFlame, 360, false);
        }
    }
}
