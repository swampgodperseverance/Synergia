using StramsSurvival.Items;
using Synergia.Content.Items.Weapons.Summon;
using Synergia.Helpers;
using Terraria.ModLoader;
using static Synergia.Lists.Items;

namespace Synergia.Common.ModSystems {
    public class SynergiaWorld : ModSystem {
        public override void PostWorldGen() {
            WorldHelper.AddContainersLoot(13, 3, SkyChest, ModContent.ItemType<Starcaller>());
            WorldHelper.DestroyerContainersLoot(13, ModContent.ItemType<JungleSeedPacket>());
            WorldHelper.AddContainersLoot(13, 1, ModContent.ItemType<JungleSeedPacket>(), 1, 3);
        }
    }
}