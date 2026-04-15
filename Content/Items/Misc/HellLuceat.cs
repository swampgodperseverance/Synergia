using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.Rarities;
using Synergia.UIs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Misc
{
    public class HellLuceat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.rare = ModContent.RarityType<CoreburnedRarity>();
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 1;
            Item.useAnimation = 15;
        }

        public override bool? UseItem(Player player)
        {
            GetInstance<Synergia>().LuceatInterface.SetState(new LuceatUI());
            return false;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>(Texture + "Glow").Value;

            float pulseIntensity = 0.3f + 0.2f * (float)Math.Sin(Main.GameUpdateCount * 0.1f);

            Color lavaColor = new Color(1f, 0.6f, 0.2f, 0f) * pulseIntensity;

            for (int i = 0; i < 6; i++)
            {
                float angle = (MathHelper.TwoPi * i / 6f) + Main.GameUpdateCount * 0.02f;
                Vector2 afterimageOffset = angle.ToRotationVector2() * (2 + pulseIntensity);

                spriteBatch.Draw(
                    glowTexture,
                    position + afterimageOffset,
                    frame,
                    lavaColor,
                    0,
                    origin,
                    scale * 1.05f,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.Draw(
                texture,
                position,
                frame,
                Color.White,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>(Texture + "Glow").Value;

            Vector2 drawPosition = Item.position - Main.screenPosition;
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);

            float pulseIntensity = 0.25f + 0.15f * (float)Math.Sin(Main.GameUpdateCount * 0.1f);

            Color lavaColor = new Color(1f, 0.6f, 0.2f, 0f) * pulseIntensity;

            for (int i = 0; i < 6; i++)
            {
                float angle = (MathHelper.TwoPi * i / 6f) + Main.GameUpdateCount * 0.02f;
                Vector2 afterimageOffset = angle.ToRotationVector2() * (3 + pulseIntensity * 2);

                spriteBatch.Draw(
                    glowTexture,
                    drawPosition + afterimageOffset,
                    frame,
                    lavaColor,
                    0,
                    new Vector2(frame.Width / 2, frame.Height / 2),
                    scale * 1.1f,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.Draw(
                texture,
                drawPosition,
                frame,
                lightColor,
                0,
                new Vector2(frame.Width / 2, frame.Height / 2),
                scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}