using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Synergia.Common.GlobalPlayer
{
    public class BookPlayerHardMode : BaseBookPlayer
    {
        public bool BookVisible = false;
        public float BookOpacity = 0f;
        int CurrentPage = 1;

        public override void PostUpdate()
        {
            float targetOpacity = BookVisible ? 1f : 0f;
            float opacitySpeed = 0.1f;
            BookOpacity = MathHelper.Lerp(BookOpacity, targetOpacity, opacitySpeed);
        }
        public void DrawBook(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            if (BookOpacity > 0.01f && CurrentPage == 1)
            {
                BaseDrawBook(spriteBatch, "AncientBook2", BookOpacity);

                BookStage(position, 2);
                if (DrawBookButtons(spriteBatch, false)) CurrentPage = 2;
            }
            if (BookOpacity > 0.01f && CurrentPage == 2)
            {
                BaseDrawBook(spriteBatch, "AncientBook", BookOpacity);

                BookStage(position, 1);
                if (DrawBookButtons(spriteBatch, true)) CurrentPage = 1;
            }
        }
    }
}