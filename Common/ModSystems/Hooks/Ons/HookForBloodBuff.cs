// Code by 𝒜𝑒𝓇𝒾𝓈
using Microsoft.Xna.Framework.Input;
using Synergia.Common.BloodBuffSeting.Core;
using Synergia.Content.Buffs;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Terraria.Utilities;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForBloodBuff : ModSystem {
        public override void Load() => On_Main.MouseText_DrawBuffTooltip += On_Main_MouseText_DrawBuffTooltip;
        void On_Main_MouseText_DrawBuffTooltip(On_Main.orig_MouseText_DrawBuffTooltip orig, Main self, string buffString, ref int X, ref int Y, int buffNameHeight) {
            orig(self, buffString, ref X, ref Y, buffNameHeight);
            AbstractBloodBuffInfo info = BloodBuff.GetLevel();
            bool currentBuff;
            if (info.AdditionalTooltips != "") { currentBuff = buffString == string.Format(info.Tooltips, info.AdditionalTooltips); }
            else { currentBuff = buffString == info.Tooltips; }
            if (currentBuff) {
                Tooltips(ref X, ref Y, info);
                SpawnBloodyDrops(ref X, ref Y, buffNameHeight);
            }
        }
        static void Tooltips(ref int X, ref int Y, AbstractBloodBuffInfo info) {
            Vector2 posTooltips = new(X, Y + 58); // 30
            float maxScale = 60;
            bool drawShiftTooltips = false;
            bool drawCtrlTooltips = false;
            if (info.Leveled == 10) {
                DrawTooltips(posTooltips, "CurrentLevel");
                DrawTooltips(new Vector2(posTooltips.X + 10 + FontAssets.MouseText.Value.MeasureString(Loc("CurrentLevel")).X, posTooltips.Y), "LevelMax", color: Color.IndianRed);
            }
            else { DrawLevelTooltips(posTooltips, Color.PaleVioletRed, info); }
            if (info.Leveled != 0) {
                drawShiftTooltips = Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);
                drawCtrlTooltips = Main.keyState.IsKeyDown(Keys.LeftControl) || Main.keyState.IsKeyDown(Keys.RightControl);
                if (drawShiftTooltips) {
                    maxScale = 0;
                    for (int i = info.Leveled; i > -1;) {
                        maxScale += 30;
                        AbstractBloodBuffInfo info2 = BloodBuffManger.Instance.GetLevelBloodBuff(i);
                        if (info2.AdditionalTooltips == "") { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale), $"{Loc("Level")} {info2.Leveled}: {info2.Tooltips}", false); }
                        else { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale), $"{Loc("Level")} {info2.Leveled}: {info2.AdditionalTooltips}", false); }
                        i--;
                    }
                }
                if (drawCtrlTooltips) {
                    float scale2 = drawShiftTooltips ? 30 : 0;
                    DrawTooltips(new Vector2(posTooltips.X + 60, posTooltips.Y + maxScale + scale2), $"------{Loc("Bonus")}------", false, Color.SteelBlue);
                    maxScale += 30;
                    bool drawDodge = false;
                    bool drawAura = false;
                    bool drawShot = false;
                    bool drawDash = false;
                    bool drawVampirism = false;
                    int speed = 0;
                    int attackSpeed = 0;
                    int damageResist = 0;
                    if (info.Leveled >= 0) { speed += 8; }
                    if (info.Leveled >= 1) { attackSpeed += 8; }
                    if (info.Leveled >= 3) { drawDodge = true; }
                    if (info.Leveled >= 3) { speed += 7; }
                    if (info.Leveled >= 4) { attackSpeed += 7; }
                    if (info.Leveled >= 5) { drawAura = true; }
                    if (info.Leveled >= 6) { damageResist += 8; }
                    if (info.Leveled >= 7) { drawShot = true; }
                    if (info.Leveled >= 8) { attackSpeed += 6; }
                    if (info.Leveled >= 9) { drawDash = true; }
                    if (info.Leveled == 10) { drawVampirism = true; }
                    if (speed != 0) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), string.Format(Loc("MovmentBonus"), speed), false, Color.LightGreen); maxScale += 30; }
                    if (attackSpeed != 0) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), string.Format(Loc("ThrowingSpeedBonus"), attackSpeed), false, Color.PaleVioletRed); maxScale += 30; }
                    if (damageResist != 0) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), string.Format(Loc("DamageResist"), damageResist), false, Color.DodgerBlue); maxScale += 30; }
                    if (drawDodge) { DrawTooltips(new Vector2(posTooltips.X + 60, posTooltips.Y + maxScale + scale2), $"------{Loc("Pasive")}------", false, Color.PaleGreen); maxScale += 30; } 
                    if (drawDodge) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), "Dodge", color: Color.DarkSeaGreen); maxScale += 30; }
                    if (drawAura) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), "Aura", color: Color.DarkSeaGreen); maxScale += 30; }
                    if (drawShot) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), "Shot", color: Color.DarkSeaGreen); maxScale += 30; }
                    if (drawDash) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), "Dash", color: Color.DarkSeaGreen); maxScale += 30; }
                    if (drawVampirism) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), "Vampirism", color: Color.DarkSeaGreen); maxScale += 30; }
                }
                if (!drawShiftTooltips) { DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + 30), "PressShift", true, Color.Silver); }
                if (!drawCtrlTooltips) {
                    float scale2 = drawShiftTooltips ? 30 : 0;
                    DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + scale2), "PressCtrl", true, Color.Silver);
                }
            }
            if (info.Leveled != 10) {
                float infoScale = !drawShiftTooltips ? 30 : 60; infoScale -= drawCtrlTooltips ? 30 : 0;
                DrawTooltips(new Vector2(posTooltips.X, posTooltips.Y + maxScale + infoScale), "Info", color: Color.PaleVioletRed); // Plum // PaleVioletRed // DarkCyan // DeepPink
            }
        }
        static Vector2 DrawTooltips(Vector2 pos, string locKey, bool loc = true, Color? color = null) => ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, loc ? Loc(locKey) : locKey, pos, color ?? new(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, 255), 0f, Vector2.Zero, Vector2.One);
        static void DrawLevelTooltips(Vector2 posTooltips, Color color, AbstractBloodBuffInfo info) {
            DrawTooltips(posTooltips, "CurrentLevel");
            DrawTooltips(new Vector2(posTooltips.X + 10 + FontAssets.MouseText.Value.MeasureString(Loc("CurrentLevel")).X, posTooltips.Y), $"{info.Leveled}", false, color: color);
        }
        static string Loc(string name) => BloodBuffInfo.Localization(name);
        static void SpawnBloodyDrops(ref int X, ref int Y, int buffNameHeight) {
            Vector2 basePos = new(X, Y + buffNameHeight);
            float globalTime = Main.GlobalTimeWrappedHourly;

            const int numDrops = 8;
            float[] xOffsets = new float[numDrops];
            float[] phaseOffsets = new float[numDrops];
            float[] cycleLengths = new float[numDrops];
            float[] fallMaxes = new float[numDrops];
            float[] scales = new float[numDrops];
            float[] baseRots = new float[numDrops];

            UnifiedRandom rand = new(Main.LocalPlayer.whoAmI);

            int leftCount = 2 + rand.Next(3);
            int rightCount = 8 - leftCount;

            for (int i = 0; i < leftCount; i++) {
                int idx = i;
                xOffsets[idx] = -35f + rand.NextFloat() * 20f;
                phaseOffsets[idx] = rand.NextFloat();
                cycleLengths[idx] = 1.0f + rand.NextFloat() * 0.5f;
                fallMaxes[idx] = 80f + rand.NextFloat() * 20f;
                scales[idx] = 0.85f + rand.NextFloat() * 0.3f;
                baseRots[idx] = (rand.NextFloat() - 0.5f) * 0.2f;
            }
            for (int i = 0; i < rightCount; i++) {
                int idx = leftCount + i;
                xOffsets[idx] = 100f + rand.NextFloat() * 20f;
                phaseOffsets[idx] = rand.NextFloat();
                cycleLengths[idx] = 1.0f + rand.NextFloat() * 0.5f;
                fallMaxes[idx] = 80f + rand.NextFloat() * 20f;
                scales[idx] = 0.85f + rand.NextFloat() * 0.3f;
                baseRots[idx] = (rand.NextFloat() - 0.5f) * 0.2f;
            }

            Texture2D tex = Request<Texture2D>(GetUIElementName("Blood")).Value;
            Vector2 origin = new(tex.Width * 0.5f, tex.Height * 0.5f);

            for (int i = 0; i < numDrops; i++) {
                float phaseTime = globalTime + phaseOffsets[i];
                float cycle = cycleLengths[i];
                float frac = phaseTime % cycle / cycle;
                float ease = frac * frac * (3f - 2f * frac);
                float fall = ease * fallMaxes[i];
                float alpha = MathF.Sin(frac * MathF.PI);
                float rot = baseRots[i] + (frac - 0.5f) * 0.4f;

                Vector2 pos = new(basePos.X + xOffsets[i], basePos.Y - 20f + fall);

                Main.spriteBatch.Draw(tex, pos, null, Color.White * alpha, rot, origin, scales[i], SpriteEffects.None, 0f);
            }
        }
        public override void Unload() => On_Main.MouseText_DrawBuffTooltip -= On_Main_MouseText_DrawBuffTooltip;
    }
}