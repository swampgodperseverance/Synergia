using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Vanilla.Common.GlobalItems.Set
{
    public class OstaraSet : GlobalItem
    {
        public override void UpdateEquip(Item item, Player player)
        {           
            if (item.ModItem != null && item.ModItem.Mod.Name == "Consolaria")
            {
                switch (item.Name)
                {
                    case "Hat of Ostara":
                        player.GetDamage(DamageClass.Throwing) += 0.11f;
                        player.GetCritChance(DamageClass.Throwing) += 5;
                        break;

                    case "Jacket of Ostara":
                        player.GetAttackSpeed(DamageClass.Throwing) += 0.13f;
                        break;

                    case "Boots of Ostara":
                        player.jumpSpeedBoost += 2.4f;
                        break;
                }
            }
        }
    }
}