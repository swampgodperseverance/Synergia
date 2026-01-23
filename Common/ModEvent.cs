using Synergia.Common.ModConfigs;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using static Synergia.Helpers.SynegiaHelper;
using static Synergia.Reassures.Reassures;
using static Terraria.Main;

namespace Synergia.Common;

public abstract class ModEvent : ModSystem {
    public static ModEvent Instance { get; private set; }

    float barAlpha;
    int barTimer;
    bool barVisible;
    bool spawnNPC = false;

    public int MaxWave;
    public int CurrentWave;
    public int EventSize;
    public int EventProgress;
    public int EventPoint;
    public int TimeToSpawnNPC = 2100;
    public int CurrentTimeToSpawnNPC;
    public string EventName;
    public List<int> EventEnemies = [];
    public bool IsActive;
    public bool FistText = false;

    public virtual void SettingEvent() { }
    public virtual void OnStart(int currentWave) { }
    public virtual void DoWave(int currentWave) { }
    public virtual void OnKillNPC(NPC npc, int currentWave) { }
    public virtual void SpawnNPC(ref IDictionary<int, float> pool, int currentWave) { }
    public virtual void OnNextWave(int currentWave) { }
    public virtual void OnEnd() { }

    public virtual void Load(int currentWave) { }
    public virtual void PostUpdateWorld(int currentWave) { }

    public override void SaveWorldData(TagCompound tag) {
        EzSave(tag, "MaxWave", ref MaxWave);
        EzSave(tag, "CurrentWave", ref CurrentWave);
        EzSave(tag, "EventSize", ref EventSize);
        EzSave(tag, "EventProgress", ref EventProgress);
        EzSave(tag, "EventPoint", ref EventPoint);
        EzSave(tag, "fistText", ref FistText);
        EzSave(tag, "IsActive", ref IsActive);
        EzSave(tag, "spawnNPC", ref spawnNPC);
    }
    public override void LoadWorldData(TagCompound tag) {
        EzLoad(tag, "MaxWave", ref MaxWave);
        EzLoad(tag, "CurrentWave", ref CurrentWave);
        EzLoad(tag, "EventSize", ref EventSize);
        EzLoad(tag, "EventProgress", ref EventProgress);
        EzLoad(tag, "EventPoint", ref EventPoint);
        EzLoad(tag, "fistText", ref FistText);
        EzLoad(tag, "IsActive", ref IsActive);
        EzLoad(tag, "spawnNPC", ref spawnNPC);
    }
    public override void PostUpdateWorld() {
        if (IsActive) {
            SpawnNPC();
            NextWave();
            EndEvent();
            PostUpdateWorld(CurrentWave);
        }
    }
    public override void Load() {
        On_Main.DrawInterface_15_InvasionProgressBars += (orig) => {
            orig();
            DrawProgressBar();
        };
        On_NPC.checkDead += (orig, self) => {
            orig(self);
            DoOnKillNPC(self);
        };
        GetInstance<EventManger>().Register(this);
        Instance = this;
        Load(CurrentWave);
    }
    public void ActiveEvent() {
        IsActive = true;
        if (MaxWave != 0 && CurrentWave == -1 && FistText == false) {
            OnStart(CurrentWave);
        }
    }
    void UpdateProgressBarState() {
        if (IsActive) {
            barVisible = true;
            barTimer = 180; 
        }
        else if (barTimer > 0) {
            barTimer--; 
        }
        else {
            barVisible = false;
        }

        float targetAlpha = barVisible ? 1f : 0f;
        barAlpha = MathHelper.Lerp(barAlpha, targetAlpha, 0.1f);
    }
    void DrawProgressBar() {
        if (!IsActive || EventName == null) {
            return;
        }
        UpdateProgressBarState();
        //bool npcInMap = false;
        //foreach (int type in EventEnemies) {
        //    if (NPC.AnyNPCs(type)) {
        //        npcInMap = true;
        //        UpdateProgressBarState();
        //    }
        //}
        if (barAlpha <= 0f) {
            return;
        }
        if (GetInstance<BossConfig>().ActiveNewUI) {
            Texture2D colorBar = Request<Texture2D>(GetUIElementName(Name + "_Bar")).Value;
            Texture2D value = Request<Texture2D>(GetUIElementName(Name + "_Icon")).Value;
            Texture2D emptyBar = Request<Texture2D>("Bismuth/UI/OrcishInvasionEmptyBar").Value;
            Vector2 barPos = new(screenWidth - emptyBar.Width, screenHeight - emptyBar.Height);
            float progress = EventSize == 0 ? 1f : (float)EventProgress / (float)EventSize;
            string waveStr;

            spriteBatch.Draw(emptyBar, barPos, Color.White * barAlpha);
            spriteBatch.Draw(colorBar, new Vector2(barPos.X + 45, barPos.Y + 70), new Rectangle(0, 0, (int)(colorBar.Width * progress), colorBar.Height), Color.White * barAlpha);

            if (MaxWave > 0) {
                waveStr = Language.GetTextValue(arg1: (EventSize != 0) ? ((int)((float)EventProgress * 100f / (float)EventSize) + "%") : Language.GetTextValue("Game.InvasionPoints", EventProgress), key: "Game.WaveMessage", arg0: CurrentWave + 1);
            }
            else {
                waveStr = ((EventSize != 0) ? ((int)((float)EventProgress * 100f / (float)EventSize) + "%") : EventProgress.ToString());
                waveStr = Language.GetTextValue("Game.WaveCleared", waveStr);
            }

            Vector2 vector6 = FontAssets.MouseText.Value.MeasureString(EventName);
            float num13 = 120f;
            if (vector6.X > 200f) {
                num13 += vector6.X - 200f;
            }

            float num = 0.5f + 1f * 0.5f;

            Rectangle r3 = Utils.CenteredRectangle(new Vector2((float)screenWidth - num13, screenHeight - 95), (vector6 + new Vector2(value.Width + 12, 6f)) * num);
            spriteBatch.Draw(value, r3.Left() + Vector2.UnitX * num * 8f, null, Color.White * barAlpha, 0f, new Vector2(0f, value.Height / 2), num * 0.8f, SpriteEffects.None, 0f);
            Utils.DrawBorderString(spriteBatch, EventName, r3.Right() + Vector2.UnitX * num * -22f, Color.White * barAlpha, num * 0.9f, 1f, 0.4f);
            Utils.DrawBorderString(spriteBatch, waveStr, barPos + new Vector2(emptyBar.Width / 2 - FontAssets.MouseText.Value.MeasureString(waveStr).X / 2, +100f), Color.White * barAlpha, 0.9f);
        }
        else {
            float num = 0.5f + barAlpha * 0.5f;
            Color c = new Color(112, 86, 114) * 0.5f;
            Texture2D value = Request<Texture2D>(GetUIElementName(Name + "_Icon")).Value;

            if (MaxWave > 0) {
                int num2 = (int)(200f * num);
                int num3 = (int)(45f * num);
                Vector2 vector = new(screenWidth - 120, screenHeight - 40);
                Utils.DrawInvBG(R: new Rectangle((int)vector.X - num2 / 2, (int)vector.Y - num3 / 2, num2, num3), sb: spriteBatch, c: new Color(63, 65, 151, 255) * 0.785f);
                string text2 = "";
                text2 = Language.GetTextValue(arg1: (EventSize != 0) ? ((int)((float)EventProgress * 100f / (float)EventSize) + "%") : Language.GetTextValue("Game.InvasionPoints", EventProgress), key: "Game.WaveMessage", arg0: CurrentWave + 1);
                Texture2D value2 = TextureAssets.ColorBar.Value;
                _ = TextureAssets.ColorBlip.Value;
                float num4 = MathHelper.Clamp((float)EventProgress / (float)EventSize, 0f, 1f);
                if (EventSize == 0)
                    num4 = 1f;

                float num5 = 169f * num;
                float num6 = 8f * num;
                Vector2 vector2 = vector + Vector2.UnitY * num6 + Vector2.UnitX * 1f;
                Utils.DrawBorderString(spriteBatch, text2, vector2, Color.White * barAlpha, num, 0.5f, 1f);
                spriteBatch.Draw(value2, vector, null, Color.White * barAlpha, 0f, new Vector2(value2.Width / 2, 0f), num, SpriteEffects.None, 0f);
                vector2 += Vector2.UnitX * (num4 - 0.5f) * num5;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle(0, 0, 1, 1), new Color(255, 241, 51) * barAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num5 * num4, num6), SpriteEffects.None, 0f);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle(0, 0, 1, 1), new Color(255, 165, 0, 127) * barAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num6), SpriteEffects.None, 0f);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle(0, 0, 1, 1), Color.Black * barAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num5 * (1f - num4), num6), SpriteEffects.None, 0f);
            }
            else {
                int num7 = (int)(200f * num);
                int num8 = (int)(45f * num);
                Vector2 vector3 = new(screenWidth - 120, screenHeight - 40);
                Utils.DrawInvBG(R: new Rectangle((int)vector3.X - num7 / 2, (int)vector3.Y - num8 / 2, num7, num8), sb: spriteBatch, c: new Color(63, 65, 151, 255) * 0.785f);
                string text3 = "";
                text3 = ((EventSize != 0) ? ((int)((float)EventProgress * 100f / (float)EventSize) + "%") : EventProgress.ToString());
                text3 = Language.GetTextValue("Game.WaveCleared", text3);
                Texture2D value3 = TextureAssets.ColorBar.Value;
                _ = TextureAssets.ColorBlip.Value;
                if (EventSize != 0) {
                    spriteBatch.Draw(value3, vector3, null, Color.White * barAlpha, 0f, new Vector2(value3.Width / 2, 0f), num, SpriteEffects.None, 0f);
                    float num9 = MathHelper.Clamp((float)EventProgress / (float)EventSize, 0f, 1f);
                    Vector2 vector4 = FontAssets.MouseText.Value.MeasureString(text3);
                    float num10 = num;
                    if (vector4.Y > 22f)
                        num10 *= 22f / vector4.Y;

                    float num11 = 169f * num;
                    float num12 = 8f * num;
                    Vector2 vector5 = vector3 + Vector2.UnitY * num12 + Vector2.UnitX * 1f;
                    Utils.DrawBorderString(spriteBatch, text3, vector5 + new Vector2(0f, -4f), Color.White * barAlpha, num10, 0.5f, 1f);
                    vector5 += Vector2.UnitX * (num9 - 0.5f) * num11;
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle(0, 0, 1, 1), new Color(255, 241, 51) * barAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num11 * num9, num12), SpriteEffects.None, 0f);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle(0, 0, 1, 1), new Color(255, 165, 0, 127) * barAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num12), SpriteEffects.None, 0f);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle(0, 0, 1, 1), Color.Black * barAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num11 * (1f - num9), num12), SpriteEffects.None, 0f);
                }
            }

            Vector2 vector6 = FontAssets.MouseText.Value.MeasureString(EventName);
            float num13 = 120f;
            if (vector6.X > 200f)
                num13 += vector6.X - 200f;

            Rectangle r3 = Utils.CenteredRectangle(new Vector2((float)screenWidth - num13, screenHeight - 80), (vector6 + new Vector2(value.Width + 12, 6f)) * num);
            Utils.DrawInvBG(spriteBatch, r3, c);
            spriteBatch.Draw(value, r3.Left() + Vector2.UnitX * num * 8f, null, Color.White * barAlpha, 0f, new Vector2(0f, value.Height / 2), num * 0.8f, SpriteEffects.None, 0f);
            Utils.DrawBorderString(spriteBatch, EventName, r3.Right() + Vector2.UnitX * num * -22f, Color.White * barAlpha, num * 0.9f, 1f, 0.4f);
        }
    }
    void DoOnKillNPC(NPC npc) {
        if ((npc.realLife >= 0 && npc.realLife != npc.whoAmI) || npc.life > 0) {
            return;
        }
        if (IsActive) {
            OnKillNPC(npc, CurrentWave);
        }
    }
    void SpawnNPC() {
        if (CurrentTimeToSpawnNPC <= TimeToSpawnNPC) {
            CurrentTimeToSpawnNPC++;
        }
        if (CurrentTimeToSpawnNPC >= TimeToSpawnNPC) {
            CurrentTimeToSpawnNPC = TimeToSpawnNPC;
        }
        if (CurrentTimeToSpawnNPC - 150 == TimeToSpawnNPC - 150 && !FistText) {
            DoWave(CurrentWave);
            FistText = true;
        }
        if (CurrentTimeToSpawnNPC == TimeToSpawnNPC) {
            spawnNPC = true;
        }
    }
    void NextWave() {
        if (CurrentWave != MaxWave) {
            if (EventProgress >= EventSize) {
                CurrentWave++;
                EventProgress = 0;
                OnNextWave(CurrentWave);
            }
        }
    }
    void EndEvent() {
        if (CurrentWave == MaxWave) {
            if (EventProgress >= EventSize) {
                ClearEvent();
                OnEnd();
            }
        }
    }
    void ClearEvent() {
        EventProgress = 0;
        CurrentWave = 0;
        CurrentTimeToSpawnNPC = 0;
        FistText = false;
        barAlpha = 0f;
        IsActive = false;
        spawnNPC = false;
    }
}