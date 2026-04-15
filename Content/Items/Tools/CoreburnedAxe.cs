using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Tools
{
    public class CoreburnedAxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 70;
            Item.scale = 1.15f;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 7;
            Item.damage = 24;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Melee;
            Item.axe = 140;
            Item.hammer = 90;
            Item.rare = ModContent.RarityType<CoreburnedRarity>();
            Item.value = Item.sellPrice(silver: 0);
            Item.UseSound = SoundID.Item1;
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.OnFire3))
                return 0.8f;
            return 1f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            spriteBatch.Draw(
                glowTexture,
                Item.Center - Main.screenPosition,
                null,
                Color.White,
                rotation,
                glowTexture.Size() / 2f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            spriteBatch.Draw(
                glowTexture,
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
    }
}