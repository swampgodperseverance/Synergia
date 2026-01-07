using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using static Terraria.Main;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForNewProgressBar : ModSystem {
        Hook NewInvasionProgressStyle;

        delegate void Orig_DrawInvasionProgress();

        public static bool NewUI;

        public override void Load() {
            MethodInfo target = typeof(Main).GetMethod(nameof(DrawInvasionProgress), BindingFlags.Public | BindingFlags.Static);
            if (target != null) {
                NewInvasionProgressStyle = new Hook(target, DrawInvasionProgress_detour);
            }
        }
        public override void Unload() {
            NewInvasionProgressStyle?.Dispose();
            NewInvasionProgressStyle = null;
        }
        static void DrawInvasionProgress_detour(Orig_DrawInvasionProgress orig) {
            if (invasionType == 2) {
                return;
            }

            if (NewUI) {
                if (invasionProgress == -1) { return; }
                if (invasionProgressMode == 2 && invasionProgressNearInvasion && invasionProgressDisplayLeft < 160) { invasionProgressDisplayLeft = 160; }
                if (!gamePaused && invasionProgressDisplayLeft > 0) { invasionProgressDisplayLeft--; }
                if (invasionProgressDisplayLeft > 0) { invasionProgressAlpha += 0.05f; }
                else { invasionProgressAlpha -= 0.05f; }
                if (invasionProgressMode == 0) {
                    invasionProgressDisplayLeft = 0;
                    invasionProgressAlpha = 0f;
                }
                if (invasionProgressAlpha < 0f) { invasionProgressAlpha = 0f; }
                if (invasionProgressAlpha > 1f) { invasionProgressAlpha = 1f; }
                if (invasionProgressAlpha <= 0f) { return; }

                float num = 0.5f + invasionProgressAlpha * 0.5f;
                Texture2D value = TextureAssets.Extra[ExtrasID.EventIconSnowLegion].Value;
                Texture2D colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("FrostBar")).Value;
                string text = Lang.inter[87].Value;      

                if (invasionProgressIcon == 1) {
                    value = TextureAssets.Extra[ExtrasID.EventIconFrostMoon].Value;
                    text = Lang.inter[83].Value;
                    colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("IceMoonBar")).Value;
                }
                else if (invasionProgressIcon == 2) {
                    value = TextureAssets.Extra[ExtrasID.EventIconPumpkinMoon].Value;
                    text = Lang.inter[84].Value;
                    colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("PumpkinMoonBar")).Value;
                }
                else if (invasionProgressIcon == 3) {
                    value = TextureAssets.Extra[ExtrasID.EventIconOldOnesArmy].Value;
                    text = Language.GetTextValue("DungeonDefenders2.InvasionProgressTitle");
                    colorBar = ModContent.Request<Texture2D>("Bismuth/UI/OrcishInvasionFullBar").Value;
                }
                else if (invasionProgressIcon == 4) {
                    value = TextureAssets.Extra[ExtrasID.EventIconGoblinArmy].Value;
                    text = Lang.inter[88].Value;
                    colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("GoblinBar")).Value;
                }
                else if (invasionProgressIcon == 7) {
                    value = TextureAssets.Extra[ExtrasID.EventIconMartianMadness].Value;
                    text = Lang.inter[85].Value;
                    colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("MartianBar")).Value;
                }
                else if (invasionProgressIcon == 6) {
                    value = TextureAssets.Extra[ExtrasID.EventIconPirateInvasion].Value;
                    text = Lang.inter[86].Value;
                    colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("PiraticBar")).Value;
                }
                //else if (invasionProgressIcon == 5) {
                //    value = TextureAssets.Extra[ExtrasID.EventIconSnowLegion].Value;
                //    text = Lang.inter[87].Value;
                //    colorBar = ModContent.Request<Texture2D>(Reassures.Reassures.GetUIElementName("FrostBar")).Value;
                //}

                Texture2D emptyBar = ModContent.Request<Texture2D>("Bismuth/UI/OrcishInvasionEmptyBar").Value;
                Vector2 barPos = new(screenWidth - emptyBar.Width, screenHeight - emptyBar.Height);
                float progress = invasionProgressMax == 0 ? 1f : (float)invasionProgress / (float)invasionProgressMax;
                string waveStr;

                spriteBatch.Draw(emptyBar, barPos, Color.White * invasionProgressAlpha);
                spriteBatch.Draw(colorBar, new Vector2(barPos.X + 45, barPos.Y + 70), new Rectangle(0, 0, (int)(colorBar.Width * progress), colorBar.Height), Color.White * invasionProgressAlpha);

                if (invasionProgressWave > 0) {
                    waveStr = Language.GetTextValue(arg1: (invasionProgressMax != 0) ? ((int)((float)invasionProgress * 100f / (float)invasionProgressMax) + "%") : Language.GetTextValue("Game.InvasionPoints", invasionProgress), key: "Game.WaveMessage", arg0: invasionProgressWave);
                }
                else {
                    waveStr = ((invasionProgressMax != 0) ? ((int)((float)invasionProgress * 100f / (float)invasionProgressMax) + "%") : invasionProgress.ToString());
                    waveStr = Language.GetTextValue("Game.WaveCleared", waveStr);
                }
                if (waveStr == "0") {
                    waveStr = "";
                }
                Vector2 vector6 = FontAssets.MouseText.Value.MeasureString(text);
                float num13 = 120f;
                if (vector6.X > 200f) {
                    num13 += vector6.X - 200f;
                }

                Rectangle r3 = Utils.CenteredRectangle(new Vector2((float)screenWidth - num13, screenHeight - 95), (vector6 + new Vector2(value.Width + 12, 6f)) * num);
                spriteBatch.Draw(value, r3.Left() + Vector2.UnitX * num * 8f, null, Color.White * invasionProgressAlpha, 0f, new Vector2(0f, value.Height / 2), num * 0.8f, SpriteEffects.None, 0f);
                Utils.DrawBorderString(spriteBatch, text, r3.Right() + Vector2.UnitX * num * -22f, Color.White * invasionProgressAlpha, num * 0.9f, 1f, 0.4f);
                Utils.DrawBorderString(spriteBatch, waveStr, barPos + new Vector2(emptyBar.Width / 2 - FontAssets.MouseText.Value.MeasureString(waveStr).X / 2, +100f), Color.White * invasionProgressAlpha, 0.9f);
            }
            else {
                orig();
                return;
            }
        }
    }
}