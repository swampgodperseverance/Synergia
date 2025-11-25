using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;

namespace Synergia.Helpers {
    public class UIHelper {
        const int nexItemDelay = 60;
        static int nexItemTime = 0;
        static int currentItemIndex = 0;

        public static bool MousePositionInUI(int startX, int endX, int statrtY, int endY) => Main.mouseX > startX && Main.mouseX < endX && Main.mouseY > statrtY && Main.mouseY < endY && !PlayerInput.IgnoreMouseInterface;
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
    }
}