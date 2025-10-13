using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Synergia.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class Blazes : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 150;  
            Item.DamageType = DamageClass.Throwing;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.shootSpeed = 12f;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<Rapire1>(); 
        }

        public override bool AltFunctionUse(Player player) => true; 

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) 
            {
                Item.shoot = ModContent.ProjectileType<Rapire2>();
                Item.damage = 70; 
                Item.useTime = 16;
                Item.useAnimation = 16;
                Item.shootSpeed = 10f;
            }
            else 
            {
                Item.shoot = ModContent.ProjectileType<Rapire1>();
                Item.damage = 150; 
                Item.useTime = 24;
                Item.useAnimation = 24;
                Item.shootSpeed = 16f;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
                                   Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(8));
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
                return false; 
            }
            return true; 
        }
    }
}
    