using Microsoft.Xna.Framework;
using Synergia.Common.Rarities;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Throwing
{
	public class Ghalihieri : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.DamageType = DamageClass.Throwing;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.useStyle = 1;
			base.Item.shootSpeed = 12f;
			base.Item.damage = 58;
			base.Item.knockBack = 4f;
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.UseSound = new SoundStyle?(SoundID.Item1);
			base.Item.useAnimation = 34;
			base.Item.useTime = 34;
			base.Item.noUseGraphic = true;
			base.Item.rare = 4;
			base.Item.shoot = ModContent.ProjectileType<GhalihieriProj>();
			base.Item.value = Item.sellPrice(0, 1, 88, 0);
		
		}
        private Texture2D GlowTexture => ModContent.Request<Texture2D>(Texture + "_Glow").Value;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
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
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            int extraCount = Main.rand.Next(1, 1); 

            for (int i = 0; i < extraCount; i++)
            {
                float angleOffset = MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f));
                Vector2 extraVel = velocity.RotatedBy(angleOffset);
                float speedMult = Main.rand.NextFloat(0.85f, 1.15f);
                extraVel *= speedMult;

                Projectile.NewProjectile(
                    source,
                    position,
                    extraVel,
                    ModContent.ProjectileType<Ghalihieri2>(),
                    (int)(damage * 1f),       
                    knockback * 0.7f,
                    player.whoAmI
                );
            }

            return false; 
        }
    }
}
