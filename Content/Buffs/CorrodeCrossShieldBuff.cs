using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Synergia.Content.Buffs
{
    public class CorrodeCrossShieldBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Corrode Cross Shield");
            // Description.SetDefault("Defense and regeneration increased\nDamage-over-time effects weakened");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 20;
            player.lifeRegen += 3;

            WeakenDoTEffects(player);
        }

        private void WeakenDoTEffects(Player player)
        {
            if (player.lifeRegen < 0)
            {
                player.lifeRegen = (int)(player.lifeRegen * 0.5f);
            }

        
            if (player.onFire) player.onFire = false;
            if (player.poisoned) player.poisoned = false;
    
           
        }
    }
}
