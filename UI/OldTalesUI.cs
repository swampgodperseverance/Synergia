using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Vanilla.Assets.UIs;

namespace Vanilla.UI
{
    public class OldTalesUI : UIState
    {   
        public AncientBook image;

        public override void OnInitialize()
        {
            Texture2D tex = ModContent.Request<Texture2D>("Vanilla/Assets/UIs/AncientBook").Value;
            image = new AncientBook(tex);
            image.Left.Set((Main.screenWidth - tex.Width) / 2f, 0f);
            image.Top.Set((Main.screenHeight - tex.Height) / 2f, 0f);
            Append(image);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.mouseRight && Main.mouseRightRelease)
            {
                ModContent.GetInstance<OldTalesSystem>().HideUI();
            }
        }
    }
}
