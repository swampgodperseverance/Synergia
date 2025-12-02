using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Spear;
using static Synergia.ModList;

namespace Synergia.Common.ModSystems.Hooks {
    public class NewLootInShadowOrbs : ModSystem {
        readonly static List<int> CorruptionItem = [ItemID.Musket, ItemID.BandofStarpower, ItemID.ShadowOrb, ItemID.BallOHurt, Roa.Find<ModItem>("PlanetomaStaff").Type, Roa.Find<ModItem>("Bookworms").Type, Roa.Find<ModItem>("Vilethorn").Type];
        static bool _droppingOrbItem;

        public override void Load() {
            On_WorldGen.CheckOrb += On_WorldGen_CheckOrb;
            On_Item.NewItem_Inner += On_Item_NewItem_Inner;
        }
        void On_WorldGen_CheckOrb(On_WorldGen.orig_CheckOrb orig, int i, int j, int type) {
            _droppingOrbItem = true;
            orig(i, j, type);
            if (!WorldGen.destroyObject) {
                _droppingOrbItem = false;
            }
        }
        int On_Item_NewItem_Inner(On_Item.orig_NewItem_Inner orig, IEntitySource source, int X, int Y, int Width, int Height, Item itemToClone, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup) {
            int index = orig(source, X, Y, Width, Height, itemToClone, Type,Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);

            if (!_droppingOrbItem || !CorruptionItem.Contains(Type)) {
                return index;
            }
            if (Main.rand.NextBool(4)) {
                if (index >= 0 && index < Main.maxItems) {
                    Main.item[index].TurnToAir();
                    return Item.NewItem(source, X, Y, Width, Height, ModContent.ItemType<ScaleBreaker>());
                }
                return index;
            }
            return index;
        }
    }
}