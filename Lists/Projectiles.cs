using Bismuth.Content.Projectiles;
using NewHorizons.Content.Projectiles.Ranged;
using NewHorizons.Content.Projectiles.Throwing;
using ValhallaMod.Projectiles.Thrown;
using System.Collections.Generic;
using static Synergia.ModList;
using Terraria.ID;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Lists {
    public static class Projectiles {
       public static HashSet<int> ThrowingProj = [
            ProjectileType<ValhallaMod.Projectiles.Boomerang.TeethBreaker>(),
            ProjectileType<ValhallaMod.Projectiles.Glaive.CrimiteGlaive>(),
            ProjectileType<ValhallaMod.Projectiles.Javelin.AzraelsHeartstopper>(),
            ProjectileType<Consolaria.Content.Projectiles.Friendly.AlbinoMandible>(),
            ProjectileType<ValhallaMod.Projectiles.Glaive.SnowGlaive>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.NightGlaiveProj>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.MythrilJavelinProj>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.OrichalcumKamaProj>(),
            ProjectileType<CarnwennanProj>(),
            ProjectileType<AdamantiteDaggerProj>(),
            ProjectileType<TitaniumWarhammerProj>(),
            ProjectileType<CrystalDaggerProj>(),
            ProjectileType<PulsarProj>(),
            ProjectileType<ChlorophytePiercerProj>(),
            ProjectileType<NanoStarProj>(),
            ProjectileType<NanoStarProj>(),
            ProjectileType<BlazingSawsProj>(),
            ProjectileType<CrystalGrenadeProj>(),
            ProjectileType<SunJavelinRework>(),
            ProjectileType<AeglosP>(),
            ProjectileType<NaturalSelectionProj>(),
            ProjectileID.Anchor,
            ProjectileID.Shroomerang,
            ProjectileID.Flamarang,
            ProjectileID.FruitcakeChakram,
            ProjectileType<ScarletGungnirProj>(),
            ProjectileType<LanceaP>(),
            Valhalla.Find<ModProjectile>("Garlic").Type
        ];
    }
}
