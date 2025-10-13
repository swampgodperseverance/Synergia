using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class AccessoryСhanges : GlobalItem
    {
        /// <summary>
        /// Используется для ограничения одновременного ношения двух аксессуаров.Используй это вместо проверки For </summary>
        /// <param name="Accessory">Аксессуар который не совместим с</param>
        /// <param name="Accessory2">этим</param>
        /// <returns></returns>
        public static bool DesiredType(Item item, Player player, int Accessory, int Accessory2)
        {
            if (item.type == Accessory)
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    Item equippedItem = player.armor[i];
                    if (equippedItem != null && equippedItem.type == Accessory2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}