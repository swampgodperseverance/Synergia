using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Melee.Spears;
using static Synergia.Lists.Items;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForNewLootInShadowOrbs : ModSystem {
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
            else {
                if (Main.rand.NextBool(2)) {
                    if (Type == ItemID.MusketBall) {
                        Main.item[Type].TurnToAir();
                    }
                    Main.item[index].TurnToAir();
                    return orig(source, X, Y, Width, Height, itemToClone, ModContent.ItemType<ScaleBreaker>(), Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
                }
                else { return index; }
            }
        }
        public override void Unload() {
            On_WorldGen.CheckOrb -= On_WorldGen_CheckOrb;
            On_Item.NewItem_Inner -= On_Item_NewItem_Inner;
        }
    }
}