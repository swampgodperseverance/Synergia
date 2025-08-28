using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Buffs
{
    public class HellishAir : ModBuff
    {
        public override void SetStaticDefaults()
        {
           // DisplayName.SetDefault("Hellish Air");
           // Description.SetDefault("You can't breathe in Hell before Skeletron is defeated...");
            
            Main.debuff[Type] = true;        
            Main.pvpBuff[Type] = true;       
            Main.buffNoSave[Type] = true;    
            Main.buffNoTimeDisplay[Type] = false; 
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen = -400; 

            if (player.statLife > 1)
            {
                player.lifeRegenTime = 0;
            }
        }
    }
}
