using Microsoft.CodeAnalysis;
using ReLogic.Content;
using SteelSeries.GameSense;
using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Content.Buffs.Debuff.FadingHellFires;
using Synergia.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using static Synergia.Common.GlobalItems.FadingHellData;

namespace Synergia.Common.GlobalItems.Visual
{
    public class FadingHellSuitableItem : GlobalItem
    {
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            FireType fireType = FireType.None;
            if (!(player.GetModPlayer<FadingHellPlayer>().isSetBonus && IsSuitableItem(item, ref fireType)))
                return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            
            Texture2D texture = TextureAssets.Item[item.type].Value;
            FadingHellFireData? fireData = GetFireData(fireType);
            Color? fireColor = fireData?.Color;
            if(!fireColor.HasValue) fireColor = Color.White;
            Color outlineColor = fireColor.Value.MultiplyAlpha(0);
            spriteBatch.Draw(
                texture,
                position + new Vector2(0f, -2f),
                frame, outlineColor, 0f, origin, scale, SpriteEffects.None, 0
            );
            spriteBatch.Draw(
                texture,
                position + new Vector2(0f, 2f),
                frame, outlineColor, 0f, origin, scale, SpriteEffects.None, 0
            );
            spriteBatch.Draw(
                texture,
                position + new Vector2(-2f, 0f),
                frame, outlineColor, 0f, origin, scale, SpriteEffects.None, 0
            );
            spriteBatch.Draw(
                texture,
                position + new Vector2(2f, 0f),
                frame, outlineColor, 0f, origin, scale, SpriteEffects.None, 0
            );

            Asset<Texture2D> rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
            if (rayTexture != null && rayTexture.IsLoaded)
            {
                Texture2D rayTex = rayTexture.Value;
                Vector2 rayOrigin = new Vector2(rayTex.Width / 2f, rayTex.Height);
                for(int i = 0; i < 13; i++)
                {
                    float seed = i + item.type * 13.97f;
                    float rotation = (float)Math.Sin(seed) * Main.GlobalTimeWrappedHourly;
                    Vector2 rayScale = new Vector2(0.2f, 0.20f + (float)Math.Cos(seed + Main.GlobalTimeWrappedHourly * 0.005f) * 0.05f);
                    spriteBatch.Draw(
                        rayTex,
                        position,
                        null,
                        fireColor.Value,
                        rotation,
                        rayOrigin,
                        rayScale,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0);

            Asset<Texture2D> flameAsset = ModContent.Request<Texture2D>("Synergia/Assets/Textures/FadingHellSuitableItem_Flame");
            if(flameAsset != null && flameAsset.IsLoaded)
            {
                Texture2D flameTexture = flameAsset.Value;
                int frameHeight = flameTexture.Height / 4;
                Vector2 flameOrigin = new Vector2(flameTexture.Width, frameHeight) / 2f;
                int frameCounter = (int)fireType - 1;
                Rectangle flameFrame = new Rectangle(0, frameCounter * frameHeight, flameTexture.Width, frameHeight);
                spriteBatch.Draw(
                    flameTexture,
                    position + new Vector2(14f, 14f),
                    flameFrame,
                    Color.White,
                    0f,
                    flameOrigin,
                    1f,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }
    }
}
