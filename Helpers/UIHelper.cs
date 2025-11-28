using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;

namespace Synergia.Helpers {
    public class UIHelper {
        const int nexItemDelay = 60;
        static int nexItemTime = 0;
        static int currentItemIndex = 0;

        const int nexItemDelay2 = 60;
        static int nexItemTime2 = 0;
        static int currentItemIndex2 = 0;

        public static bool MousePositionInUI(float startX, float endX, float statrtY, float endY) => Main.mouseX > startX && Main.mouseX < endX && Main.mouseY > statrtY && Main.mouseY < endY && !PlayerInput.IgnoreMouseInterface;
        public static int GetNextItemType(List<int> itemList) {
            if (itemList == null || itemList.Count == 0) {
                return ItemID.None;
            }
            nexItemTime++;
            if (nexItemTime >= nexItemDelay) {
                nexItemTime = 0;
                currentItemIndex = (currentItemIndex + 1) % itemList.Count;
            }
            return itemList[currentItemIndex];
        }
        public int NoStaticGetNextItemType(List<int> itemList) {
            if (itemList == null || itemList.Count == 0) {
                return ItemID.None;
            }
            nexItemTime2++;
            if (nexItemTime2 >= nexItemDelay2) {
                nexItemTime2 = 0;
                currentItemIndex2 = (currentItemIndex2 + 1) % itemList.Count;
            }
            return itemList[currentItemIndex2];
        }
    }
}