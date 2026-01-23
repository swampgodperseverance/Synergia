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

        // Calamity
        public const float DefaultPosX = 50.104603f;
        public const float DefaultPosY = 58.05169f;

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
        public static int GetNextItemType(IEnumerable<int> itemList) {
            if (itemList == null) {
                return ItemID.None;
            }

            IList<int> list = itemList as IList<int> ?? [.. itemList];

            if (list.Count == 0) {
                return ItemID.None;
            }

            nexItemTime++;
            if (nexItemTime >= nexItemDelay) {
                nexItemTime = 0;
                currentItemIndex = (currentItemIndex + 1) % list.Count;
            }

            return list[currentItemIndex];
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
        // Calamity
        public static Vector2 PlayerPos(float x = 0, float y = 0) {
            Vector2 screenRatioPosition = new(DefaultPosX, DefaultPosY);
            if (screenRatioPosition.X < 0f || screenRatioPosition.X > 100f) {
                screenRatioPosition.X = DefaultPosX;
            }
            if (screenRatioPosition.Y < 0f || screenRatioPosition.Y > 100f) {
                screenRatioPosition.Y = DefaultPosY;
            }

            Vector2 screenPos = screenRatioPosition;
            screenPos.X = (int)(screenPos.X * 0.01f * Main.screenWidth);
            screenPos.Y = (int)(screenPos.Y * 0.01f * Main.screenHeight);

            return new Vector2(screenPos.X + x, screenPos.Y + y);
        }
        //public static Vector2 Size(Texture2D texture, out float offset) {
        //    offset = (texture.Width - texture.Width) * 0.5f;
        //    return texture.Size() * 0.5f;
        //}
    }
}