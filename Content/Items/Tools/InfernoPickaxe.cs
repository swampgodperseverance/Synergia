using Synergia.Common.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Tools
{
    public class CoreburnedPickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1.15f;

            Item.useTurn = true;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 7;

            Item.damage = 24;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Melee;

            Item.pick = 200; 
            Item.rare = ModContent.RarityType<CoreburnedRarity>();
            Item.value = Item.sellPrice(silver: 0);

            Item.UseSound = SoundID.Item1;
        }

        public override float UseTimeMultiplier(Player player)
        {

            if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.OnFire3))
                return 0.6f; 

            return 1f; 
        }
    }
}
