using Bismuth.Content.Projectiles;
using NewHorizons.Content.Projectiles.Ranged;
using NewHorizons.Content.Projectiles.Throwing;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Synergia.Content.Projectiles.Thrower;
using System.Collections.Generic;
using Terraria.ID;
using ValhallaMod.Projectiles.Ranged.Thrown;
using static Synergia.ModList;

namespace Synergia.Lists {
    public static class Projectiles {
        public static HashSet<int> ThrowingProj = [
            Valhalla.Find<ModProjectile>("TeethBreaker").Type,
            Valhalla.Find<ModProjectile>("CrimiteGlaive").Type,
            Valhalla.Find<ModProjectile>("SnowGlaive").Type,
            Valhalla.Find<ModProjectile>("OmegaDisc").Type,
            Valhalla.Find<ModProjectile>("Garlic").Type,
            ProjectileType<AzraelsHeartstopper2>(),
            ProjectileType<Consolaria.Content.Projectiles.Friendly.AlbinoMandible>(),
            ProjectileType<NewHorizons.Content.Projectiles.Throwing.NightGlaiveProj>(),
            ProjectileType<MythrilJavelinProj>(),
            ProjectileType<OrichalcumKamaProj>(),
            ProjectileType<CarnwennanProj>(),
            ProjectileType<AdamantiteDaggerProj>(),
            ProjectileType<TitaniumWarhammerProj>(),
            ProjectileType<EverwoodJavelinProjectile>(),
            ProjectileType<CrystalDaggerProj>(),
            ProjectileType<PulsarProj>(),
            ProjectileType<FlarionProj>(),
            ProjectileType<AirflowProjectile>(),
            ProjectileType<ChlorophytePiercerProj>(),
            ProjectileID.ThornChakram,
            ProjectileType<CactusStar>(),
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
            ProjectileID.SnowBallFriendly,
            ProjectileID.Trimarang,
            ProjectileType<StalloyScrew>()
        ];
    }
}
