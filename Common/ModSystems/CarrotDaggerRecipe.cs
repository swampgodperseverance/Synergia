using Terraria;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Javelin;

namespace Synergia.Common.ModSystems
{
	public class CarrotDaggerRecipe : ModSystem
	{
		public override void PostAddRecipes()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				
				if (recipe.TryGetResult(ModContent.ItemType<CarrotDagger>(), out _))
				{
					recipe.DisableRecipe();
				}

			}
		}
	}
}
