using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Avalon.Common;
using Avalon.Common.Extensions;
using Avalon.Common.Templates;
using Avalon.Particles;
using Synergia.Content.Projectiles.RangedProjectiles;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class PhoenixDownfall : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 20;
            Item.damage = 68;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2.5f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet; 
            Item.shootSpeed = 10f;
            Item.UseSound = SoundID.Item11;
            Item.useAmmo = AmmoID.Bullet;
        }
         public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (ModContent.GetInstance<AvalonClientConfig>().AdditionalScreenshakes)
            {
                UseStyles.gunStyle(player, 0, 2);
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6f, 0f);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            int projCount = Main.rand.Next(7, 13);
            float spread = MathHelper.ToRadians(25f);
            for (int i = 0; i < projCount; i++)
            {
                float offset = Main.rand.NextFloat(-spread, spread);
                Vector2 projVelocity = velocity.RotatedBy(offset) * Main.rand.NextFloat(0.8f, 1.2f);
                Projectile.NewProjectile(source, position, projVelocity, ModContent.ProjectileType<PhoenixProj>(), damage, knockback, player.whoAmI);
            }

            player.velocity -= Vector2.Normalize(velocity) * 1.2f;

            SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/Shotgun"), player.Center);

            return false; 
        }
    }
}
