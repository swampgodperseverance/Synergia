using Avalon.Items.Material.Bars;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Friendly;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Throwing
{
	public class NaquadahJavelin : ModItem
	{
		public override void SetDefaults()
		{
			var item = Item;

			item.damage = 30;
			item.DamageType = DamageClass.Throwing;
			item.width = 24;
			item.height = 25;
			item.useTime = 12;
			item.useAnimation = 12;
			item.knockBack = 2;
			item.value = Item.buyPrice(silver: 50);
			item.shootSpeed = 11f;
			item.maxStack = 1;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.Swing;
			item.shoot = ModContent.ProjectileType<NaquadahProjectile>();
			item.consumable = false;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NaquadahBar>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
