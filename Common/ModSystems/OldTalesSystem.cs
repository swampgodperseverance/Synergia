using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Vanilla.Common.ModSystems
{
    public class OldTalesSystem : ModSystem
    {
        private UserInterface _interface;
        internal OldTalesUI UI;
        public bool UIVisible;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                UI = new OldTalesUI();
                _interface = new UserInterface();
                _interface.SetState(UI);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (UIVisible)
            {
                _interface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (UIVisible)
            {
                int i = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                if (i != -1)
                {
                    layers.Insert(i, new LegacyGameInterfaceLayer(
                        "OldTales: UI",
                        delegate
                        {
                            _interface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }

        public void ShowUI() => UIVisible = true;

        public void HideUI() => UIVisible = false;
    }
}
