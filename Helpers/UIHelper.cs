using System.Collections.Generic;
using Terraria.ID;

namespace Synergia.Helpers {
    public class UIHelper {
        public static int GetNextItemType(List<int> itemList) {
            const int nexItemDelay = 60;
            int nexItemTime = 0;
            int currentItemIndex = 0;
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