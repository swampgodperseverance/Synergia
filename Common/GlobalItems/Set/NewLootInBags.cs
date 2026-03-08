using Avalon.Items.Material.OreChunks;
using Consolaria.Content.Items.Consumables;
using Synergia.Content.Items.Accessories;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Consumable.Bag;
using ValhallaMod.Items.Weapons.Ranged.Javelins;
using static Synergia.Helpers.ItemHelper;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalItems.Set { 
    public class NewLootInBags : GlobalItem {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
            AddLoot(item, itemLoot, ItemType<PirateSquidBag>(), ItemType<GoldGlove>(), 3);
            AddLoot(item, itemLoot, ItemID.GolemBossBag, ItemType<CaesiumChunk>(), 1, 60, 110);
            AddLoot(item, itemLoot, ItemType<LepusBag>(), ItemType<CarrotDagger>(), 2, 150, 150);
        }
        public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.type == ItemType<PirateSquidBag>() || item.type == ItemID.GolemBossBag || item.type == ItemType<LepusBag>();
    }
}
