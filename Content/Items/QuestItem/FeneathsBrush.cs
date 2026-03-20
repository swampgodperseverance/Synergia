using Avalon.Items.Material.Shards;
using Avalon.Items.Placeable.Crafting;
using Avalon.Tiles;
using PrimeRework;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Synergia.Content.Items.QuestItem {
    public class FeneathsBrush : ModItem {

        public override void SetDefaults()
        {
            Item.questItem = true;
            Item.rare = ItemRarityID.Quest;
            Item.width = 40;
            Item.height = 25;
        }

        private Texture2D GlowTexture => ModContent.Request<Texture2D>(Texture + "_Glow").Value;

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            spriteBatch.Draw(
                GlowTexture,
                position,
                frame,
                Color.White,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture = GlowTexture;

            Vector2 position = Item.position - Main.screenPosition + Item.Size / 2f;
            Rectangle frame = texture.Frame();
            Vector2 origin = frame.Size() / 2f;

            spriteBatch.Draw(
                texture,
                position,
                frame,
                Color.White,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}