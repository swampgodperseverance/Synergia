using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Synergia.Content.Items.QuestItem
{
    public class LothorSkull : ModItem
    {
        public override void SetDefaults()
        {
            Item.questItem = true;
            Item.rare = ItemRarityID.Quest;
            Item.width = 40;
            Item.height = 25;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(Item.width / 2f, Item.height / 2f);
            Vector2 worldPos = Item.Center - Main.screenPosition;

            Player player = Main.LocalPlayer;
            float healthPercent = (float)player.statLife / player.statLifeMax2;
            float missingHealth = 1f - healthPercent;
            float bloodIntensity = MathHelper.Clamp(missingHealth * 0.8f, 0.05f, 0.7f);
            float darkPower = missingHealth * 0.5f;

            float time = (float)Main.timeForVisualEffects * 0.02f;
            float pulseSpeed = 2f + missingHealth * 3f;
            float pulse = (float)Math.Sin(time * pulseSpeed) * 0.1f * missingHealth;

            Color bloodColor = new Color(180, 20, 20, (int)(120 * bloodIntensity));
            Color darkAura = new Color(80, 0, 40, (int)(100 * darkPower));

            for (int i = 0; i < 6; i++)
            {
                float angle = time * 1.5f + (i * MathHelper.PiOver4);
                float auraRadius = 5f + darkPower * 15f;
                Vector2 offset = new Vector2(auraRadius, 0).RotatedBy(angle);
                spriteBatch.Draw(texture, worldPos + offset,
                    frame, darkAura, rotation, origin, scale * 1.1f, SpriteEffects.None, 0f);
            }

            Color innerGlow = Color.Lerp(new Color(100, 0, 0), new Color(255, 50, 50), bloodIntensity);
            innerGlow.A = (byte)(80 + bloodIntensity * 100);
            spriteBatch.Draw(texture, worldPos + new Vector2(0, pulse),
                frame, innerGlow, rotation, origin, scale * 1.03f, SpriteEffects.None, 0f);

            Color drawColor = alphaColor;
            if (missingHealth > 0.3f)
            {
                drawColor = Color.Lerp(alphaColor, new Color(255, 100, 100), missingHealth * 0.6f);
            }
            spriteBatch.Draw(texture, worldPos, frame, drawColor, rotation, origin, scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}