using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Consumables
{
    public class ShardflingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 3);

            Item.consumable = true;
            Item.buffType = ModContent.BuffType<Buffs.ShardflingPotionBuff>();
            Item.buffTime = 21600; // 6 минут
        }
    }
}
