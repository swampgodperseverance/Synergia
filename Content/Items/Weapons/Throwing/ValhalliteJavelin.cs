using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Friendly; 

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class ValhalliteJavelin : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Throwing; 
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ValhalliteJavelinProj>();
            Item.shootSpeed = 12f;
            Item.maxStack = 999;
            Item.consumable = true;
        }
    }
}
