using Bismuth.Content.Tiles;
using MonoMod.RuntimeDetour;
using Synergia.Content.NPCs.Swamp;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Synergia.Common.ModSystems.WorldGens.SynergiaGenVars;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class NewSwamp : ModSystem {
        Hook openChest;
        Hook chestInfo;

        delegate bool orig_OnRightClick(SwampChest chest, int x, int y);
        delegate bool get_OnRightClicDetour(orig_OnRightClick orig, SwampChest chest, int x, int y);

        delegate void orig_MouseOver(SwampChest chest, int x, int y);
        delegate void get_MouseOverDetour(orig_MouseOver orig, SwampChest chest, int x, int y);

        public override void Load() {
            MethodInfo info = typeof(SwampChest).GetMethod(nameof(SwampChest.RightClick), BindingFlags.Public | BindingFlags.Instance);
            openChest = new Hook(info, (get_OnRightClicDetour)Set_RightClic);
            info = typeof(SwampChest).GetMethod(nameof(SwampChest.MouseOver), BindingFlags.Public | BindingFlags.Instance);
            chestInfo = new Hook(info, (get_MouseOverDetour)Set_MouseOver);
        }
        bool Set_RightClic(orig_OnRightClick orig, SwampChest chest, int x, int y) {
            if ((x == HLOX - 2 && y == HLTY + 34) || (x == HLOX - 2 && y == HLTY + 33) || (x == HLOX - 1 && y == HLTY + 34) || (x == HLOX - 1 && y == HLTY + 33)) {
                if (!SynergiaWorld.OpenChest) {
                    for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++) {
                        if (Main.LocalPlayer.inventory[i].type == ItemID.GoldenKey) {
                            --Main.LocalPlayer.inventory[i].stack;
                            SynergiaWorld.OpenChest = true;
                            SoundEngine.PlaySound(SoundID.Unlock, new((x * 16), (y * 16)));
                            break; 
                        } 
                    }
                }
                else {
                    if (!SynergiaWorld.mossWitchDead) {
                        if (!NPC.AnyNPCs(NPCType<MossWitch>())) { NPC.NewNPC(Main.LocalPlayer.GetSource_TileInteraction(x, y), (x * 16), (y * 16), NPCType<MossWitch>()); }
                    }
                    else { return orig(chest, x, y); }
                }
                return false;
            }
            else { return orig(chest, x, y); }
        }
        void Set_MouseOver(orig_MouseOver orig, SwampChest chest, int x, int y) {
            if ((x == HLOX - 2 && y == HLTY + 34) || (x == HLOX - 2 && y == HLTY + 33) || (x == HLOX - 1 && y == HLTY + 34) || (x == HLOX - 1 && y == HLTY + 33)) {
                if (!SynergiaWorld.OpenChest) {
                    Main.LocalPlayer.cursorItemIconEnabled = true;
                    Main.LocalPlayer.cursorItemIconID = ItemID.GoldenKey;
                }
                else { orig(chest, x, y); }
            }
            else { orig(chest, x, y); }
        }
        public override void Unload() {
            openChest?.Undo();
            openChest = null;
            chestInfo?.Undo();
            chestInfo = null;
        }
    }
}
