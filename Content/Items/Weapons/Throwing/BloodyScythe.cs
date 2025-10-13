using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class BloodyScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;

            Item.damage = 50;
            Item.DamageType = DamageClass.Throwing; 
            Item.knockBack = 2.5f;
            Item.crit = 4; 

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.noMelee = true;    
            Item.noUseGraphic = true; 

            Item.shoot = ModContent.ProjectileType<Content.Projectiles.Friendly.BloodyScytheProjectile>();
            Item.shootSpeed = 15f;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 20);
        }
    }
}