using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class PotionGlobalItem : GlobalItem{
        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue) {
            if (healValue > 0 && item.healLife > 0)
            {
                var modPlayer = player.GetModPlayer<SynergiaPlayer>();
                if (modPlayer.potionHealMultiplier > 1f)
                {
                    healValue = (int)(healValue * modPlayer.potionHealMultiplier);
                }
            }
        }

        public override void GetHealMana(Item item, Player player, bool quickHeal, ref int healValue){
            if (healValue > 0 && item.healMana > 0)
            {
                var modPlayer = player.GetModPlayer<SynergiaPlayer>();
                if (modPlayer.potionHealMultiplier > 1f)
                {
                    healValue = (int)(healValue * modPlayer.potionHealMultiplier);
                }
            }
        }
    }
}