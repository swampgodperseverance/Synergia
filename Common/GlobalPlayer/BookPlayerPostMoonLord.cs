using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Items.Histories;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Common.GlobalPlayer
{
    public class BookPlayerPostMoonLord : BaseBookPlayer
    {
        public bool BookVisible = false;
        public float BookOpacity = 0f;

        public override void PostUpdate()
        {
            float targetOpacity = BookVisible ? 1f : 0f;
            float opacitySpeed = 0.1f;
            BookOpacity = MathHelper.Lerp(BookOpacity, targetOpacity, opacitySpeed);
        }
        //public void DrawBook(SpriteBatch spriteBatch)
        //{
        //    Vector2 position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
        //    if (BookOpacity > 0.01f)
        //    {
        //        // Базывая логика отрисовки текстуры книги
        //        BaseDrawBook(spriteBatch, "AncientBook3", BookOpacity);
        //        // пример использования WhenHovering 
        //        // intItem - ID предмета, который будет отображаться при наведении
        //        // rectangle - область, при наведении на которую будет отображаться предмет
        //        // можешь использовать BookRectangle для создания области или Rectangle
        //        int intItem = 0;
        //        Rectangle rectangle = new(0, 0, 0, 0);
        //        if (WhenHovering(intItem, rectangle)) return;
        //        if (WhenHovering(ItemType<ColdSummonHistory>(), BookRectangle(position.X - 215, position.Y - 288, 26, 20))) return;
        //    }
        //}
    }
}