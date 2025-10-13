using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;

namespace Synergia.Common.ModSystems
{
    public class MinionSentryDisplay : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Synergia: Inventory"));
            if (inventoryLayerIndex != -1)
            {
                layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
                    "Synergia: MinionSentryDisplay",
                    delegate
                    {
                        if (Main.playerInventory && !Main.hideUI)
                        {
                            DrawMinionSentryCount(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        private void DrawMinionSentryCount(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;

            int currentMinions = player.GetModPlayer<LocalMinionData>().CountMinionSlots();
            int maxMinions = player.maxMinions;

            int currentSentries = player.GetModPlayer<LocalMinionData>().CountSentrySlots();
            int maxSentries = player.maxTurrets;

            string minionText = $"Minions: {currentMinions}/{maxMinions}";
            string sentryText = $"Sentries: {currentSentries}/{maxSentries}";

            Vector2 position = new Vector2(Main.screenWidth - 400, 100);

            Utils.DrawBorderStringFourWay(
                spriteBatch,
                FontAssets.MouseText.Value,
                minionText,
                position.X,
                position.Y,
                Color.White,
                Color.Black,
                Vector2.Zero
            );

            Utils.DrawBorderStringFourWay(
                spriteBatch,
                FontAssets.MouseText.Value,
                sentryText,
                position.X,
                position.Y + 24,
                Color.White,
                Color.Black,
                Vector2.Zero
            );
        }
    }

    public class LocalMinionData : ModPlayer
    {
        public int CountMinionSlots()
        {
            float usedSlots = 0f;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == Player.whoAmI && proj.minion)
                {
                    usedSlots += proj.minionSlots;
                }
            }
            return (int)usedSlots;
        }

        public int CountSentrySlots()
        {
            int count = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == Player.whoAmI && proj.sentry)
                {
                    count++;
                }
            }
            return count;
        }
    }
}