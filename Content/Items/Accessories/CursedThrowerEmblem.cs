using Microsoft.Xna.Framework;
using NewHorizons.Content.Items.Accessories;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;

namespace Synergia.Content.Items.Accessories
{
	public class CursedThrowerEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(this.Item.type, (DrawAnimation) new DrawAnimationVertical(5, 5, false));
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 17);
			Item.rare = ItemRarityID.Red;
		}
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NinjaEmblem>(), 1)
                .AddIngredient(ModContent.ItemType<JadeCloth>(), 10)
                .AddIngredient(ItemID.SoulofMight, 3)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddIngredient(ItemID.SoulofSight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            if (ModLoader.TryGetMod("PrimeRework", out Mod primeReworkMod))
            {
                if (primeReworkMod.TryFind<ModItem>("SoulofFreight", out var freight) &&
                    primeReworkMod.TryFind<ModItem>("SoulofPlight", out var plight) &&
                    primeReworkMod.TryFind<ModItem>("SoulofDight", out var dight))
                {
                    recipe
                        .AddIngredient(freight.Type, 3)
                        .AddIngredient(dight.Type, 3);
                }
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Throwing) += 0.55f;
			player.GetDamage(DamageClass.Generic) -= 0.4f;
		}
	}
}
