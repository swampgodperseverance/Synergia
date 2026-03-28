using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class PoisonGasTrap : Avalon.Projectiles.Hostile.TuhrtlOutpost.PoisonGasTrap
    {
        public override string Texture => "Avalon/Projectiles/Hostile/TuhrtlOutpost/PoisonGasTrap";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.ForcePlateDetection[base.Projectile.type] = new bool?(false);
            ProjectileID.Sets.NoLiquidDistortion[base.Type] = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}