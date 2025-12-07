using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Buffs
{
    public class ShardflingPotionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shardfling Power");
            // Tooltip.SetDefault("Ranged damage increased by 10%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Throwing) += 0.10f;
        }
    }
}
