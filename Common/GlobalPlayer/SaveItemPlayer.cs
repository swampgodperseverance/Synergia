using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Synergia.Common.GlobalPlayer {
    public class SaveItemPlayer : ModPlayer {
        public Item weaponSlotItem = new();
        public Item praceSlotItem = new();

        public override void Initialize() {
            weaponSlotItem = new Item();
            praceSlotItem = new Item();
            weaponSlotItem.TurnToAir();
            praceSlotItem.TurnToAir();
        }
        public override void SaveData(TagCompound tag) {
            // public const string ModName = "BaseModModName";
            if (weaponSlotItem is not null) {
                tag.Add(Synergia.ModName + "WeaponSlotItem" + nameof(weaponSlotItem), ItemIO.Save(weaponSlotItem));
            }
            if (weaponSlotItem is not null) {
                tag.Add(Synergia.ModName + "PraceSlotItem" + nameof(praceSlotItem), ItemIO.Save(praceSlotItem));
            }
        }
        public override void LoadData(TagCompound tag) {
            if (tag.TryGet(Synergia.ModName + "WeaponSlotItem" + nameof(weaponSlotItem), out TagCompound dye1)) {
                weaponSlotItem = ItemIO.Load(dye1);
            }
            if (tag.TryGet(Synergia.ModName + "PraceSlotItem" + nameof(praceSlotItem), out TagCompound dye2)) {
                praceSlotItem = ItemIO.Load(dye2);
            }
        }
    }
}
