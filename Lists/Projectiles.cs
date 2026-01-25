using Bismuth.Content.Projectiles;
using NewHorizons.Content.Projectiles.Ranged;
using NewHorizons.Content.Projectiles.Throwing;
using System.Collections.Generic;
using static Synergia.ModList;

namespace Synergia.Lists {
    public static class Projectiles {
       public static HashSet<int> ThrowingProj = [
            ProjectileType<ValhallaMod.Projectiles.Boomerang.TeethBreaker>(),
            ProjectileType<CarnwennanProj>(),
            ProjectileType<AdamantiteDaggerProj>(),
            ProjectileType<TitaniumWarhammerProj>(),
            ProjectileType<CrystalDaggerProj>(),
            ProjectileType<ChlorophytePiercerProj>(),
            ProjectileType<NanoStarProj>(),
            ProjectileType<BlazingSawsProj>(),
            ProjectileType<CrystalGrenadeProj>(),
            ProjectileType<AeglosP>(),
            ProjectileType<ScarletGungnirProj>(),
            ProjectileType<LanceaP>(),
            Valhalla.Find<ModProjectile>("Garlic").Type
        ];
    }
}
