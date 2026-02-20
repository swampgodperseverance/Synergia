using Synergia.Common.BloodBuffSeting.Core;
using Synergia.Content.Buffs;
using System;
using Terraria;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForBloodBuff : ModSystem {
        public override void Load() => On_Main.MouseText_DrawBuffTooltip += On_Main_MouseText_DrawBuffTooltip;
        void On_Main_MouseText_DrawBuffTooltip(On_Main.orig_MouseText_DrawBuffTooltip orig, Main self, string buffString, ref int X, ref int Y, int buffNameHeight) {
            orig(self, buffString, ref X, ref Y, buffNameHeight);

            Vector2 basePos = new(X, Y + buffNameHeight);
            AbstractBestiaryInfo info = BloodBuff.GetLevel();
            if (buffString == string.Format(info.Tooltips, info.Leveled)) {
                float globalTime = Main.GlobalTimeWrappedHourly;
                float leftTime = globalTime + 0f;
                float leftFrac = leftTime % 1.2f / 1.2f;
                float leftEase = leftFrac * leftFrac;
                float leftFall = leftEase * 90f; // 95 || 100
                float leftAlpha = MathHelper.Clamp(MathF.Sin(leftFrac * MathF.PI), 0f, 1f);

                Vector2 leftPos = new(basePos.X + -20f, basePos.Y + -20f + leftFall);
                Vector2 rightPos = new(basePos.X + 110f, basePos.Y + -20f + leftFall);

                Texture2D tex = Request<Texture2D>(GetUIElementName("Blood")).Value;
                Vector2 origin = new(tex.Width * 0.5f, tex.Height * 0.5f);

                Main.spriteBatch.Draw(tex, leftPos, null, Color.White * leftAlpha, 0f, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(tex, rightPos, null, Color.White * leftAlpha, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void Unload() => On_Main.MouseText_DrawBuffTooltip -= On_Main_MouseText_DrawBuffTooltip;
    }
}