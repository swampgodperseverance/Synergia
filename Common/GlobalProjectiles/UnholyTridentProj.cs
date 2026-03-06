using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.GlobalProjectiles
{
    public class UnholyTridentProj : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => projectile.type == ProjectileID.UnholyTridentFriendly;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.ShadowFlame, 360, false);
    }
}
