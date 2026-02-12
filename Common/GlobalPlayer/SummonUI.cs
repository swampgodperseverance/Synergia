using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;
using ValhallaMod;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Common.GlobalPlayer {
    public class SummonUI : ModPlayer {

        Vector2 dragOffset;

        Vector2 basePosition;
        Vector2 minionPos;
        Vector2 sentryPos;
        Vector2 auraPos;

        bool dragMinion;
        bool dragSentry;
        bool dragAura;

        bool initialized = false;

        public static bool IsActiveSummonUI;
        public static bool HoverUIElement;
        public static bool ResetUIPositions;

        public override void SaveData(TagCompound tag) {
            MySaveData(tag, nameof(minionPos), ref minionPos);
            MySaveData(tag, nameof(sentryPos), ref sentryPos);
            MySaveData(tag, nameof(auraPos), ref auraPos);
            tag[nameof(initialized)] = initialized;
        }
        public override void LoadData(TagCompound tag) {
            MyLoadData(tag, nameof(minionPos), ref minionPos);
            MyLoadData(tag, nameof(sentryPos), ref sentryPos);
            MyLoadData(tag, nameof(auraPos), ref auraPos);
            initialized = tag.GetBool(nameof(initialized));
        }
        static void MySaveData(TagCompound tag, string saveName, ref Vector2 save) => tag[saveName] = save;
        static void MyLoadData(TagCompound tag, string saveName, ref Vector2 save) => save = tag.Get<Vector2>(saveName);
        public void ResetPositions() {
            basePosition = new(Main.screenWidth - 289, 43);
            minionPos = basePosition - new Vector2(120, -50);
            sentryPos = basePosition - new Vector2(120, -90); 
            auraPos = basePosition - new Vector2(120, -130);
            initialized = true; 
        }
        public void DrawSummonUI(SpriteBatch spriteBatch) {
            Player player = Main.LocalPlayer;
            AuraPlayer auraPlayer = player.GetModPlayer<AuraPlayer>();
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            if (!initialized) {
                basePosition = new(Main.screenWidth - 289, 43);

                minionPos = basePosition - new Vector2(-200, -50);
                sentryPos = basePosition - new Vector2(-200, -90);
                auraPos = basePosition - new Vector2(-200, -130);

                initialized = true;
            }
            if (ResetUIPositions) {
                ResetPositions();
                ResetUIPositions = false;
            }
            if (IsActiveSummonUI && (player.maxMinions >= 2 || player.maxTurrets >= 2)) {
                Texture2D summonTexture = Request<Texture2D>(GetUIElementName("MinionDisplay")).Value;
                Texture2D sentryTexture = Request<Texture2D>(GetUIElementName("SentryDisplay")).Value;
                Texture2D auraTexture = Request<Texture2D>(GetUIElementName("AuraDisplay")).Value;

                spriteBatch.Draw(summonTexture, minionPos, Color.White);
                Utils.DrawBorderString(spriteBatch, $"{player.numMinions}/{player.maxMinions}", minionPos + new Vector2(10, 10), Color.White);

                spriteBatch.Draw(sentryTexture, sentryPos, Color.White);
                Utils.DrawBorderString(spriteBatch, $"{CountSentrySlots(player)}/{player.maxTurrets}", sentryPos + new Vector2(10, 10), Color.White);

                spriteBatch.Draw(auraTexture, auraPos, Color.White);
                Utils.DrawBorderString(spriteBatch, $"{CountAuraSlots(player)}/{auraPlayer.maxAuras}", auraPos + new Vector2(10, 10), Color.White);

                Rectangle summonBounds = new((int)minionPos.X, (int)minionPos.Y, summonTexture.Width, summonTexture.Height);
                Rectangle sentryBounds = new((int)sentryPos.X, (int)sentryPos.Y, sentryTexture.Width, sentryTexture.Height);
                Rectangle auraBounds = new((int)auraPos.X, (int)auraPos.Y, auraTexture.Width, auraTexture.Height);

                Point mouse = Main.MouseScreen.ToPoint();

                if (!Main.mouseText) {
                    string mouseText;
                    if (summonBounds.Contains(Main.MouseScreen.ToPoint())) {
                        mouseText = $"1"; ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, mouseText, new Vector2(Main.mouseX + 17, Main.mouseY + 17), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One);
                    }
                    else if (sentryBounds.Contains(Main.MouseScreen.ToPoint())) {
                        mouseText = $"2"; ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, mouseText, new Vector2(Main.mouseX + 17, Main.mouseY + 17), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One);
                    }
                    else if (auraBounds.Contains(Main.MouseScreen.ToPoint())) {
                        mouseText = $"3"; ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, mouseText, new Vector2(Main.mouseX + 17, Main.mouseY + 17), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One);
                    }
                }
                if (HoverUIElement) {
                    if (Main.mouseLeft && !Main.mouseLeftRelease) {
                        if (!dragMinion && summonBounds.Contains(mouse)) {
                            dragMinion = true;
                            dragOffset = Main.MouseScreen - minionPos;
                        }
                        else if (!dragSentry && sentryBounds.Contains(mouse)) {
                            dragSentry = true;
                            dragOffset = Main.MouseScreen - sentryPos;
                        }
                        else if (!dragAura && auraBounds.Contains(mouse)) {
                            dragAura = true;
                            dragOffset = Main.MouseScreen - auraPos;
                        }
                    }
                    if (dragMinion) {
                        minionPos = Main.MouseScreen - dragOffset;
                        Main.LocalPlayer.mouseInterface = true;
                    }
                    else if (dragSentry) {
                        sentryPos = Main.MouseScreen - dragOffset;
                        Main.LocalPlayer.mouseInterface = true;
                    }
                    else if (dragAura) {
                        auraPos = Main.MouseScreen - dragOffset;
                        Main.LocalPlayer.mouseInterface = true;
                    }
                    if (Main.mouseLeftRelease) {
                        dragMinion = dragSentry = dragAura = false;
                    }  
                }
            }
        }
        static int CountSentrySlots(Player player) {
            int count = 0;
            foreach (Projectile proj in Main.projectile) {
                if (proj.active && proj.owner == player.whoAmI && proj.sentry) {
                    count++;
                }
            }
            return count;
        }
        static int CountAuraSlots(Player player) {
            int count = 0;

            for (int i = 0; i < Main.maxProjectiles; i++) {
                Projectile p = Main.projectile[i];

                if (!p.active)
                    continue;

                if (p.ModProjectile is AuraAI aura) {
                    if (p.owner == player.whoAmI) {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}