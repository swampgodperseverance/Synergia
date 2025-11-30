using StramsSurvival.Items;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Helpers;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems {
    public class SynergiaWorld : ModSystem {
        List<int> SkyChest = [ItemID.Starfury, ItemID.LuckyHorseshoe, ItemID.ShinyRedBalloon, ItemID.CelestialMagnet];
        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ModContent.ItemType<Starcaller>());
            WorldHelper.DestroyerContainersLoot(13, ModContent.ItemType<JungleSeedPacket>());
            WorldHelper.AddContainersLoot(13, 1, ModContent.ItemType<JungleSeedPacket>(), 1, 3);
        }
    }
}