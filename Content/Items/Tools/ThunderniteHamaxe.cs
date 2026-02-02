using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Tools
{
    public class ThunderniteHamaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;

            Item.damage = 60;
            Item.DamageType = DamageClass.Melee;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.knockBack = 6f;
            Item.crit = 4;

            Item.axe = 30;    
            Item.hammer = 90;  
            Item.tileBoost = 3; 

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item1;
        }
    }
}
