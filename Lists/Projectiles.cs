using Bismuth.Content.Projectiles;
using NewHorizons.Content.Projectiles.Ranged;
using NewHorizons.Content.Projectiles.Throwing;
using ValhallaMod.Projectiles.Thrown;
using System.Collections.Generic;
using static Synergia.ModList;
using Terraria.ID;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using ValhallaMod.Projectiles.Boomerang;
using Synergia.Content.Projectiles.Thrower;

namespace Synergia.Lists {
    public static class Projectiles {
       public static HashSet<int> ThrowingProj = [
            ProjectileType<ValhallaMod.Projectiles.Boomerang.TeethBreaker>(),
            ProjectileType<ValhallaMod.Projectiles.Glaive.CrimiteGlaive>(),
            ProjectileType<AzraelsHeartstopper2>(),
            ProjectileType<Consolaria.Content.Projectiles.Friendly.AlbinoMandible>(),
            ProjectileType<ValhallaMod.Projectiles.Glaive.SnowGlaive>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.NightGlaiveProj>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.MythrilJavelinProj>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.OrichalcumKamaProj>(),
            ProjectileType<CarnwennanProj>(),
            ProjectileType<AdamantiteDaggerProj>(),
            ProjectileType<TitaniumWarhammerProj>(),
            ProjectileType<ValhallaMod.Projectiles.Boomerang.OmegaDisc>(),
            ProjectileType<EverwoodJavelinProjectile>(),
            ProjectileType<CrystalDaggerProj>(),
            ProjectileType<PulsarProj>(),
            ProjectileType<FlarionProj>(),
            ProjectileType<AirflowProjectile>(),
            ProjectileType<ChlorophytePiercerProj>(),
            ProjectileID.ThornChakram,
            ProjectileType<ValhallaMod.Projectiles.Thrown.CactusStar>(), 
            ProjectileType<NanoStarProj>(),
            ProjectileType<BlazingSawsProj>(),
            ProjectileType<CrystalGrenadeProj>(),
            ProjectileType<SunJavelinRework>(),
            ProjectileType<AeglosP>(),
            ProjectileType<NaturalSelectionProj>(),
            ProjectileID.Anchor,
            ProjectileID.WoodenBoomerang,
            ProjectileID.Bone,
            ProjectileID.Shroomerang,
            ProjectileID.Flamarang,
            ProjectileID.FruitcakeChakram,
            ProjectileType<ScarletGungnirProj>(),
            ProjectileType<LanceaP>(),
            Valhalla.Find<ModProjectile>("Garlic").Type
        ];
    }
}
