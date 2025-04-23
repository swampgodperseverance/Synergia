using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Vanilla.Common.GlobalItems.Set
{
    public class RoASentinel : GlobalItem
    {
        public override void UpdateEquip(Item item, Player player)
        {
            // Проверяем, что предмет из мода RoA
            if (item.ModItem != null && item.ModItem.Mod.Name == "RoA")
            {
                switch (item.Name)
                {
                    case "Sentinel Helmet":
                        player.GetDamage(DamageClass.Throwing) += 0.15f;
                        player.GetCritChance(DamageClass.Throwing) += 5;
                        break;

                    case "Sentinel Chestplate":
                        player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
                        break;

                    case "Sentinel Leggings":
                        player.moveSpeed += 0.2f;
                        break;
                }
            }
        }
    }
}