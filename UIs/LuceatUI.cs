// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Content.Items.Misc;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Synergia.UIs {
    public class LuceatUI : UIState {
        readonly Dictionary<string, bool> buttonHoverPrev = [];
        readonly Dictionary<string, bool> buttonHoverNow = [];
        bool closeUI = false;
        bool blockClickThisFrame;
        bool anyButtonHovered = false;
        bool spawnDust = false;
        int timeToSpawn = 0;

        public override void OnInitialize() {
            Left.Set(-57.5f, 0.5f);
            Top.Set(420f, 0f);
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            Player player = Main.LocalPlayer;

            blockClickThisFrame = false;
            if (closeUI || player.inventory[player.selectedItem].type != ItemType<HellLuceat>()) {
                GetInstance<Synergia>().LuceatInterface.SetState(null);
            }
            if (spawnDust) {
                timeToSpawn++;
                if (timeToSpawn >= 2) {
                    for (int k = 0; k < 80; k++) {
                        Dust.NewDust(player.position, player.width, player.height, DustID.LifeDrain, 0, 0, 0, default, 0.8f);
                        if (k == 79) {
                            spawnDust = false;
                            timeToSpawn = 0;
                            closeUI = true;
                        }
                    }
                }
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            buttonHoverNow.Clear();
            anyButtonHovered = false;

            float x = GetInnerDimensions().X; float y = GetInnerDimensions().Y;
            Vector2 baseVector = new(x, y);
            Vector2 villageButton = new(baseVector.X + 40, baseVector.Y);
            Vector2 arenaButton = new(baseVector.X + 80, baseVector.Y);
            Player player = Main.LocalPlayer;
            DrawButton(spriteBatch, "Lake", baseVector, 0, LocUIKey("LuceatUI", "Lake"), player, TpLake);
            DrawButton(spriteBatch, "Village", villageButton, 1, LocUIKey("LuceatUI", "Village"), player, TpVillage);
            DrawButton(spriteBatch, "Arena", arenaButton, 2, LocUIKey("LuceatUI", "Hell"), player, TpArena);

            Main.LocalPlayer.mouseInterface |= anyButtonHovered;

            buttonHoverPrev.Clear();
            foreach (KeyValuePair<string, bool> kv in buttonHoverNow) {
                buttonHoverPrev[kv.Key] = kv.Value;
            }
        }
        void DrawButton(SpriteBatch sb, string id, Vector2 pos, int index, string message, Player player, Action<Player> onClick) {
            Texture2D texture = RTextures.Location[index].Value;
            Rectangle rect = new((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            Point mousePos = new(Main.mouseX, Main.mouseY);
            bool hover = rect.Contains(mousePos);
            buttonHoverNow[id] = hover;
            anyButtonHovered |= hover;
            bool wasHover = buttonHoverPrev.TryGetValue(id, out bool v) && v;
            sb.Draw(texture, pos, Color.White);
            if (hover && !wasHover) {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            if (hover) {
                if (hover && Main.mouseLeft && Main.mouseLeftRelease && !blockClickThisFrame) {
                    blockClickThisFrame = true;
                    onClick?.Invoke(player);
                }
                sb.Draw(RTextures.GlowLocation[index].Value, pos, Color.Gold);
                ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.MouseText.Value, message, new Vector2(pos.X + 20, pos.Y+40), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, new Vector2(0.95f), -1f, 2f);
            }
        }
        void TpLake(Player player) {
            Vector2 pos = new((SynergiaGenVars.HellLakeX - 236 + 40) * 16, (SynergiaGenVars.HellLakeY - 101) * 16);
            BaseTp(player, pos);
        }
        void TpVillage(Player player) {
            Vector2 pos = new((SynergiaGenVars.HellVillageX - 280 + 70) * 16, (SynergiaGenVars.HellVillageY - 69) * 16);
            BaseTp(player, pos);
        }
        void TpArena(Player player) {
            Vector2 pos = new((SynergiaGenVars.HellArenaPositionX - 199 + 110) * 16, (SynergiaGenVars.HellArenaPositionY - 12) * 16);
            BaseTp(player, pos);
        }
        void BaseTp(Player player, Vector2 pos) {
            SoundEngine.PlaySound(SoundID.Item6);
            player.position = pos;
            player.immune = true;
            player.immuneTime = 10;
            player.velocity.Y = 0f;
            player.noFallDmg = true;
            spawnDust = true;
        }
    }
}