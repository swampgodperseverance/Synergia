using Synergia.Content.Projectiles.RangedProjectiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class Enfer : ModItem
    {
        private static Texture2D glowTexture;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 64;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 8, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EnferArrow>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
                                   Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 2;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                Projectile.NewProjectile(
                    source,
                    position,
                    perturbedSpeed,
                    ModContent.ProjectileType<EnferArrow>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
            }
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-4f, 0f);
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
            Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.2f + 0.8f);
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            if (glowTexture != null)
            {
                Color glowColor = new Color(255, 120, 40, 255) * (0.5f + pulse * 0.5f);
                spriteBatch.Draw(glowTexture, position, frame, glowColor, 0f, origin, scale, SpriteEffects.None, 0f);
            }

            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Vector2 position = Item.position - Main.screenPosition + new Vector2(Item.width / 2f, Item.height - texture.Height / 2f);
            Rectangle? frame = texture.Frame();
            Vector2 origin = frame.Value.Size() / 2f;
            spriteBatch.Draw(texture, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
            if (glowTexture != null)
            {
                float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 5f) * 0.2f + 0.8f);
                Color glowColor = new Color(255, 100, 20) * (0.6f + pulse * 0.4f);

                spriteBatch.Draw(glowTexture, position, frame, glowColor, rotation, origin, scale * 1.05f, SpriteEffects.None, 0f);
            }

            if (Main.rand.NextFloat() < 0.1f)
            {
                int dust = Dust.NewDust(Item.position, Item.width, Item.height, DustID.Torch, 0f, -0.5f, 150, Color.Orange, 1.2f);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].noGravity = true;
            }

            return false;
        }
    }
}
