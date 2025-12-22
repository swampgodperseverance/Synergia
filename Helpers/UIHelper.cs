using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace Synergia.Helpers {
    public class UIHelper {
        const int nexItemDelay = 60;
        static int nexItemTime = 0;
        static int currentItemIndex = 0;

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
        public int NoStaticGetNextItemType(List<int> itemList, int nexItemDelay2 = 60) {
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
        public int GetAnimatedItemType(List<int> list, int delay = 60) {
            if (list == null || list.Count == 0)
                return ItemID.None;

            long t = Main.GameUpdateCount / delay;
            int index = (int)(t % list.Count);

            return list[index];
        }

        public static void AddLayer(List<GameInterfaceLayer> layers, int layerIndex, string name, GameInterfaceDrawMethod drawMethod, InterfaceScaleType scaleType = InterfaceScaleType.UI) {
            if (layerIndex != -1) {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer(name, drawMethod, scaleType));
            }
        }
        public static Vector2 PlayerPos(float x = 0, float y = 0) {
            Vector2 screenCenter = new(Main.screenWidth / 2f, Main.screenHeight / 2f);
            Vector2 drawPos = screenCenter + new Vector2(x, y);
            return drawPos;
        }
    }
}