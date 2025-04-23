using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Vanilla.Common.GlobalItems.Set
{
    public class GreediestSet : GlobalItem
    {
        public override void UpdateEquip(Item item, Player player)
        {           
            if (item.ModItem != null && item.ModItem.Mod.Name == "ValhallaMod")
            {
                switch (item.Name)
                {
                    case "Greediest Mask":
                        player.endurance += 0.13f; 
                        player.GetDamage(DamageClass.Throwing) += 0.25f; 
                        break;

                    case "Greediest Tunic":
                        player.endurance += 0.14f;
                        player.GetCritChance(DamageClass.Throwing) += 8;
                        break;

                    case "Greediest Greaves":; 
                        player.GetAttackSpeed(DamageClass.Throwing) += 0.09f; 
                        break;
                }
            }
        }
    }
}