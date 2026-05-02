using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Items.Tools
{
    public class ThunderniteHamaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 56;

            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.knockBack = 6f;
            Item.crit = 4;

            Item.axe = 30;
            Item.hammer = 90;
            Item.tileBoost = 3;

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item1;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Rectangle frame = texture.Bounds;
            Vector2 origin = frame.Size() / 2f;
            Vector2 position = Item.Center - Main.screenPosition;

            spriteBatch.Draw(texture, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(glowTexture, position, frame, Color.White, rotation, origin, scale, SpriteEffects.None, 0);

            return false;
        }

        public override void HoldItem(Player player)
        {
            if (player.ItemAnimationActive)
            {
                Lighting.AddLight(player.MountedCenter, 0.2f, 0.4f, 0.8f);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Electrified, 120);
            }
        }
    }
}