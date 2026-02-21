// Code by 𝒜𝑒𝓇𝒾𝓈
using Microsoft.Xna.Framework.Input;
using Synergia.Common.BloodBuffSeting.Core;
using Synergia.Content.Buffs;
using System;
using Terraria;
using Terraria.Utilities;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForBloodBuff : ModSystem {
        public override void Load() => On_Main.MouseText_DrawBuffTooltip += On_Main_MouseText_DrawBuffTooltip;
        void On_Main_MouseText_DrawBuffTooltip(On_Main.orig_MouseText_DrawBuffTooltip orig, Main self, string buffString, ref int X, ref int Y, int buffNameHeight) {
            orig(self, buffString, ref X, ref Y, buffNameHeight);
            Vector2 basePos = new(X, Y + buffNameHeight);
            AbstractBestiaryInfo info = BloodBuff.GetLevel();
            bool currentBuff;
            if (info.AdditionalTooltips != "") { currentBuff = buffString == string.Format(info.Tooltips, info.AdditionalTooltips, info.Leveled); }
            else { currentBuff = buffString == string.Format(info.Tooltips, info.Leveled); }
            if (currentBuff) {
                string a = "";
                string Shift = nameof(Shift);
                Keys[] pressedKeys = Main.keyState.GetPressedKeys();
                a = ItemTooltip(WEP, Shift);
                foreach (Keys key in pressedKeys) {
                    if (key == Keys.LeftShift) {
                        a = ItemTooltip(WEP, "Aeris");
                    }
                }
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
        }
        public override void Unload() => On_Main.MouseText_DrawBuffTooltip -= On_Main_MouseText_DrawBuffTooltip;
    }
}