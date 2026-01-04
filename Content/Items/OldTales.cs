using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.Items
{
	public class OldTales : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.RaiseLamp;
			//Item.holdStyle = ItemHoldStyleID.HoldLamp;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			tooltips.Add(new TooltipLine(Mod, $"Base{nameof(OldTales)}", ItemTooltip(WEP, "SadTooltips")));
			string a = "";
			string Shift = nameof(Shift);
            Keys[] pressedKeys = Main.keyState.GetPressedKeys();
            a = ItemTooltip(WEP, Shift);
            foreach (Keys key in pressedKeys) {
				if (key == Keys.LeftShift) {
					a = ItemTooltip(WEP, "Aeris");
                }
			}
            tooltips.Add(new TooltipLine(Mod, $"Developers", a));
        }
	}
}