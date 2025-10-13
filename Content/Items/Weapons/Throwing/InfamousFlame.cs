using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class InfamousFlame : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true; 
            Item.noMelee = true; 
            Item.shoot = ModContent.ProjectileType<InfamousFlameProjectile>();
            Item.shootSpeed = 12f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}