using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Vanilla.Common.GlobalPlayer
{
	public class BookPlayer : ModPlayer
	{
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

				// Общие настройки для иконок
				float iconScale = 0.5f; // Масштаб иконок
				Color iconColor = Color.White * BookOpacity * 0.9f; // Прозрачность иконок

				// 1. Гель
				Rectangle GelZone = new Rectangle((int)(position.X - 235), (int)(position.Y - 330), 40, 40);
				Texture2D GelIcon = Terraria.GameContent.TextureAssets.Item[ItemID.Gel].Value;
				spriteBatch.Draw(
					GelIcon,
					new Vector2(GelZone.X + GelZone.Width/2, GelZone.Y + GelZone.Height/2),
					null,
					iconColor,
					0f,
					new Vector2(GelIcon.Width/2, GelIcon.Height/2),
					iconScale,
					SpriteEffects.None,
					0f
				);

				if (GelZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.Gel).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 2. Корона
				Rectangle CrownZone = new Rectangle((int)(position.X - 195), (int)(position.Y - 330), 40, 40);
				Texture2D CrownIcon = Terraria.GameContent.TextureAssets.Item[ItemID.GoldCrown].Value;
				spriteBatch.Draw(
					CrownIcon,
					new Vector2(CrownZone.X + CrownZone.Width/2, CrownZone.Y + CrownZone.Height/2),
					null,
					iconColor,
					0f,
					new Vector2(CrownIcon.Width/2, CrownIcon.Height/2),
					iconScale,
					SpriteEffects.None,
					0f
				);

				if (CrownZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.GoldCrown).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}

				// 3. Зона меча (иконка)
				Rectangle swordZone = new Rectangle((int)(position.X - 20), (int)(position.Y + 40), 40, 40);
				Texture2D swordIcon = Terraria.GameContent.TextureAssets.Item[ItemID.IronBroadsword].Value;
				spriteBatch.Draw(
					swordIcon,
					new Vector2(swordZone.X + swordZone.Width/2, swordZone.Y + swordZone.Height/2),
					null,
					iconColor,
					0f,
					new Vector2(swordIcon.Width/2, swordIcon.Height/2),
					iconScale,
					SpriteEffects.None,
					0f
				);

				if (swordZone.Contains(mousePos))
				{
					Main.HoverItem = new Item(ItemID.IronBroadsword).Clone();
					Main.instance.MouseText(Main.HoverItem.Name, Main.HoverItem.rare, 0);
					return;
				}
			}
		}
	}
}