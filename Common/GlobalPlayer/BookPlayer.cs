using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Synergia.Common.GlobalPlayer
{
    public class BookPlayer : BaseBookPlayer
    {
        public bool BookVisible = false;
        public float BookOpacity = 0f;

        public override void PostUpdate()
        {
            float targetOpacity = BookVisible ? 1f : 0f;
            float opacitySpeed = 0.1f;
            BookOpacity = MathHelper.Lerp(BookOpacity, targetOpacity, opacitySpeed);
        }
        public void DrawBook(SpriteBatch spriteBatch)
        {
            if (BookOpacity > 0.01f)
            {
                Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;

                BaseDrawBook(spriteBatch, "AncientBook", BookOpacity);
                BookStage(position, 1);
            }
        }
    }
}