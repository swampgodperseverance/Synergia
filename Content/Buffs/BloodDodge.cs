using Terraria;
using Terraria.ModLoader;

namespace Vanilla.Content.Buffs
{
    public class BloodDodge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Dodge");
            // Description.SetDefault("You may counter incoming damage, but bleed from the strain...");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }
}