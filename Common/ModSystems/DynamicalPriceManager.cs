using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Core.PriceSystem
{
    public class DynamicPriceManager : ModSystem
    {
        public static DynamicPriceManager Instance;
        private readonly Dictionary<int, ItemPriceData> _priceData = new();

        private const int RECOVERY_SECONDS = 25 * 60 * 20 * 20 * 20 * 10;
        private const float MIN_MULTIPLIER = 0.3f;
        private const float PENALTY_PER_STACK = 0.055f;

        public class ItemPriceData
        {
            public float Multiplier = 1f;
            public int LastSellTime;
            public int BaseValue;
            public bool HasBeenSold = false;
        }

        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;

        private ItemPriceData GetData(int type, int? baseValue = null)
        {
            if (!_priceData.TryGetValue(type, out var data))
            {
                data = new ItemPriceData();
                if (baseValue.HasValue)
                    data.BaseValue = baseValue.Value;
                _priceData[type] = data;
            }
            UpdateRecovery(data);
            return data;
        }

        private void UpdateRecovery(ItemPriceData data)
        {
            if (data.Multiplier >= 1f) return;

            int secondsPassed = UnixNow() - data.LastSellTime;
            if (secondsPassed <= 0) return;

            float recoveryPerSecond = 1f / RECOVERY_SECONDS;
            float recovered = secondsPassed * recoveryPerSecond;

            data.Multiplier = Math.Min(1f, data.Multiplier + recovered);

            if (data.Multiplier > 0.995f)
                data.Multiplier = 1f;
        }

        public float GetMultiplier(int itemType)
        {
            if (itemType <= ItemID.None) return 1f;
            var data = GetData(itemType);
            return data.HasBeenSold ? data.Multiplier : 1f;
        }

        public void ApplyPriceToItem(Item item)
        {
            if (item == null || item.IsAir || item.value <= 0) return;

            var data = GetData(item.type, item.value);

            if (!data.HasBeenSold) return;

            if (data.BaseValue == 0)
                data.BaseValue = item.value;

            float mult = GetMultiplier(item.type);
            item.value = (int)(data.BaseValue * mult);
        }

        public void RegisterSell(int itemType, int stack, int originalValue)
        {
            if (itemType <= ItemID.None) return;
            var data = GetData(itemType, originalValue);

            data.HasBeenSold = true;
            data.LastSellTime = UnixNow();

            float penalty = PENALTY_PER_STACK * stack;
            data.Multiplier = Math.Max(MIN_MULTIPLIER, data.Multiplier - penalty);
        }

        private int UnixNow() => (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public static string FormatPrice(int value)
        {
            if (value <= 0) return "0 c";
            int p = value / 1000000;
            int g = (value % 1000000) / 10000;
            int s = (value % 10000) / 100;
            int c = value % 100;

            string txt = "";
            if (p > 0) txt += p + "p ";
            if (g > 0) txt += g + "g ";
            if (s > 0) txt += s + "s ";
            txt += c + "c";
            return txt.Trim();
        }

        public int GetBaseValue(int itemType)
        {
            if (itemType <= ItemID.None) return 0;
            var data = GetData(itemType);
            return data.BaseValue;
        }

        public int GetOriginalItemValue(int itemType)
        {
            Item dummyItem = new Item();
            dummyItem.SetDefaults(itemType);
            return dummyItem.value;
        }

        public bool HasItemBeenSold(int itemType)
        {
            if (itemType <= ItemID.None) return false;
            var data = GetData(itemType);
            return data.HasBeenSold;
        }

        public override void PostUpdateEverything()
        {
            if (Main.LocalPlayer != null)
            {
                for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
                {
                    Item item = Main.LocalPlayer.inventory[i];
                    if (item != null && !item.IsAir && item.value > 0)
                    {
                        if (HasItemBeenSold(item.type))
                        {
                            ApplyPriceToItem(item);
                        }
                    }
                }
            }
        }
    }

    public class PriceUpdateGlobalNPC : GlobalNPC
    {
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
        }
    }

    public class DynamicPricePlayer : ModPlayer
    {
        public override void PostSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            int originalValue = DynamicPriceManager.Instance.GetOriginalItemValue(item.type);
            DynamicPriceManager.Instance.RegisterSell(item.type, item.stack, originalValue);
        }
    }

    public class DynamicPriceUISystem : ModSystem
    {
        private float opacity = 0f;
        private float colorTransition = 0f;
        private float percentDisplayTimer = 0f;
        private int lastDiscountPercent = 0;

        private Texture2D bgTex;
        private Texture2D borderTex;

        public override void PostDrawInterface(SpriteBatch sb)
        {
            if (!Main.playerInventory || Main.HoverItem == null || Main.HoverItem.IsAir)
            {
                opacity = Math.Max(0f, opacity - 0.05f);
                colorTransition = Math.Max(0f, colorTransition - 0.03f);
                percentDisplayTimer = Math.Max(0f, percentDisplayTimer - 0.02f);
                return;
            }

            Item item = Main.HoverItem;

            if (!DynamicPriceManager.Instance.HasItemBeenSold(item.type))
            {
                opacity = Math.Max(0f, opacity - 0.05f);
                colorTransition = Math.Max(0f, colorTransition - 0.03f);
                percentDisplayTimer = Math.Max(0f, percentDisplayTimer - 0.02f);
                return;
            }

            float mult = DynamicPriceManager.Instance.GetMultiplier(item.type);
            int originalBaseValue = DynamicPriceManager.Instance.GetOriginalItemValue(item.type);
            int currentValue = (int)(originalBaseValue * mult);
            int discountPercent = (int)((1f - mult) * 100);

            if (mult >= 0.99f)
            {
                opacity = Math.Max(0f, opacity - 0.05f);
                colorTransition = Math.Max(0f, colorTransition - 0.03f);
                percentDisplayTimer = Math.Max(0f, percentDisplayTimer - 0.02f);
                return;
            }

            opacity = Math.Min(1f, opacity + 0.04f);

            if (discountPercent > 0)
            {
                colorTransition = Math.Min(1f, colorTransition + 0.025f);
                percentDisplayTimer = Math.Min(1f, percentDisplayTimer + 0.025f);
                lastDiscountPercent = discountPercent;
            }
            else
            {
                colorTransition = Math.Max(0f, colorTransition - 0.02f);
                percentDisplayTimer = Math.Max(0f, percentDisplayTimer - 0.015f);
            }

            float hue;
            if (mult < 0.65f)
                hue = 0f; 
            else if (mult < 0.85f)
                hue = 0.1f; 
            else
                hue = 0.3f; 

            float targetR, targetG, targetB;
            if (hue < 0.05f)
            {
                targetR = 1f;
                targetG = 0.3f + (1f - colorTransition) * 0.3f;
                targetB = 0.3f + (1f - colorTransition) * 0.3f;
            }
            else if (hue < 0.2f)
            {
                targetR = 1f;
                targetG = 0.6f + (1f - colorTransition) * 0.2f;
                targetB = 0.2f + (1f - colorTransition) * 0.3f;
            }
            else
            {
                targetR = 0.4f + colorTransition * 0.3f;
                targetG = 0.8f + colorTransition * 0.2f;
                targetB = 0.3f + colorTransition * 0.2f;
            }

            Color textColor = new Color(targetR, targetG, targetB) * opacity;

            Vector2 pos = new Vector2(Main.mouseX + 32, Main.mouseY - 68);

            if (bgTex == null)
            {
                bgTex = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
                bgTex.SetData(new[] { Color.White });
            }
            if (borderTex == null)
            {
                borderTex = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
                borderTex.SetData(new[] { Color.White });
            }

            Color bgColor = Color.Black * (0.85f * opacity);
            sb.Draw(bgTex, new Rectangle((int)pos.X - 12, (int)pos.Y - 32, 270, 78), bgColor);

            Rectangle rect = new Rectangle((int)pos.X - 12, (int)pos.Y - 32, 270, 78);
            float borderAlpha = 0.6f * opacity;
            DrawBorder(sb, borderTex, rect, Color.Goldenrod * borderAlpha);

            Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value,
                $"Base Price: {DynamicPriceManager.FormatPrice(originalBaseValue)}",
                pos.X, pos.Y - 26, Color.LightGray * opacity, Color.Black * opacity, Vector2.Zero, 0.78f);

            Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value,
                $"Sell Price: {DynamicPriceManager.FormatPrice(currentValue)}",
                pos.X, pos.Y + 2, textColor, Color.Black * opacity, Vector2.Zero, 0.85f);

            if (percentDisplayTimer > 0.05f || discountPercent > 0)
            {
                float percentAlpha = percentDisplayTimer * opacity;
                Color percentColor = Color.OrangeRed * percentAlpha;

                float scale = 0.9f + (float)(Math.Sin(Main.GameUpdateCount * 0.05) * 0.05f) * percentDisplayTimer;

                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value,
                    $"-{lastDiscountPercent}%",
                    pos.X + 175, pos.Y - 12,
                    percentColor, Color.Black * (percentAlpha * 0.5f),
                    Vector2.Zero, 0.9f * scale);
            }
        }

        private void DrawBorder(SpriteBatch sb, Texture2D tex, Rectangle rect, Color color)
        {
            int t = 2;
            sb.Draw(tex, new Rectangle(rect.X, rect.Y, rect.Width, t), color);
            sb.Draw(tex, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), color);
            sb.Draw(tex, new Rectangle(rect.X, rect.Y, t, rect.Height), color);
            sb.Draw(tex, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), color);
        }

        public override void PostUpdateEverything()
        {
            if (!Main.playerInventory)
            {
                opacity = Math.Max(0f, opacity - 0.08f);
                colorTransition = Math.Max(0f, colorTransition - 0.05f);
                percentDisplayTimer = Math.Max(0f, percentDisplayTimer - 0.04f);
            }
        }
    }
}