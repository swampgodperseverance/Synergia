using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Vanilla.Common.GlobalPlayer
{
	public class BookPlayer : ModPlayer
	{
		private static Mod avalon = ModLoader.GetMod("Avalon");
		private static Mod vanilla = ModLoader.GetMod("Vanilla");
		private static Mod consolaria = ModLoader.GetMod("Consolaria");
		public bool BookVisible = false;
		public float BookOpacity = 0f; // Добавляем прозрачность

		public override void PostUpdate()
		{
			// Плавное увеличение или уменьшение прозрачности
			float targetOpacity = BookVisible ? 1f : 0f;
			float opacitySpeed = 0.1f; // скорость появления/исчезновения (можешь менять)

			BookOpacity = MathHelper.Lerp(BookOpacity, targetOpacity, opacitySpeed);
		}

		public void DrawBook(SpriteBatch spriteBatch)
		{
			if (BookOpacity > 0.01f)
			{
				Texture2D texture = ModContent.Request<Texture2D>("Vanilla/Assets/UIs/AncientBook").Value;

				// Центр экрана
				Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
				Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;

				spriteBatch.Draw(
					texture,
					position,
					null,
					Color.White * BookOpacity,
					0f,
					origin,
					1f, // Масштаб 1:1
					SpriteEffects.None,
					0f
				);

				Point mousePos = Main.MouseScreen.ToPoint();

				// 1. Гель
				Rectangle GelZone = new Rectangle((int)(position.X - 220), (int)(position.Y - 315), 20, 20);

				if (GelZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Gel).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 2. Корона
				Rectangle CrownZone = new Rectangle((int)(position.X - 190), (int)(position.Y - 315), 20, 20);

				if (CrownZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.GoldCrown).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 3. Алтарь из Авалона
				Rectangle IckyZone = new Rectangle((int)(position.X - 207), (int)(position.Y - 288), 26, 20);

				if (IckyZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("IckyAltar").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 4. История Короля Слизней
				Rectangle KingSlimeZone = new Rectangle((int)(position.X - 150), (int)(position.Y - 300), 140, 60);

				if (KingSlimeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("KingSlimeHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 5. Корона Слизня
				Rectangle SlimeCrownZone = new Rectangle((int)(position.X - 208), (int)(position.Y - 265), 20, 20);

				if (SlimeCrownZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.SlimeCrown).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 6. Яйко
				Rectangle EggZone = new Rectangle((int)(position.X - 208), (int)(position.Y - 210), 20, 25);

				if (EggZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(consolaria.Find<ModItem>("SuspiciousLookingEgg").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 7. История Лепуса
				Rectangle LepusZone = new Rectangle((int)(position.X - 150), (int)(position.Y - 215), 140, 45);

				if (LepusZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("LepusHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 8. Линза
				Rectangle LensZone = new Rectangle((int)(position.X - 208), (int)(position.Y - 155), 20, 25);

				if (LensZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Lens).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 9. Алтарь из Авалона 2
				Rectangle IckyZone2 = new Rectangle((int)(position.X - 207), (int)(position.Y - 122), 26, 20);

				if (IckyZone2.Contains(mousePos))
				{
					Main.HoverItem = new Item(avalon.Find<ModItem>("IckyAltar").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 10. Подозрительно выглядящий глаз
				Rectangle EyeZone = new Rectangle((int)(position.X - 206), (int)(position.Y - 100), 25, 20);

				if (EyeZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.SuspiciousLookingEye).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 11. История Глаза
				Rectangle EyeBossZone = new Rectangle((int)(position.X - 140), (int)(position.Y - 145), 115, 70);

				if (EyeBossZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(vanilla.Find<ModItem>("EyeHistory").Type).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
			}
		}
	}
}