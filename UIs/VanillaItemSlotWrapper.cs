using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;

namespace Synergia.UIs
{
	internal class VanillaItemSlotWrapper : UIElement
	{
		internal Texture2D ItemTypeTextyre;
		internal Item Item;
        private readonly int _context;
		private readonly float _scale;
		internal Func<Item, bool> ValidItemFunc;

		public VanillaItemSlotWrapper(int context = ItemSlot.Context.BankItem, float scale = 1f) {
			_context = context;
			_scale = scale;
			Item = new Item();
			Item.SetDefaults(0);

			Width.Set(TextureAssets.InventoryBack9.Value.Width * scale, 0f);
			Height.Set(TextureAssets.InventoryBack9.Value.Height * scale, 0f);
		}
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem)) {
                    ItemSlot.Handle(ref Item, _context);
                }
            }

            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());

            if (Item.IsAir && ItemTypeTextyre != null) {
                Vector2 itemOrig = new(ItemTypeTextyre.Width, ItemTypeTextyre.Height);
                Vector2 slotCenter = rectangle.TopLeft() + new Vector2(TextureAssets.InventoryBack9.Value.Width * _scale / 2f, TextureAssets.InventoryBack9.Value.Height * _scale / 2f);
                float drawScale = _scale * 1f;
                spriteBatch.Draw(ItemTypeTextyre, slotCenter, null, Color.White * 0.4f, 0f, itemOrig / 2f, drawScale, SpriteEffects.None, 0f);
            }

            Main.inventoryScale = oldScale;
        }
    }
}