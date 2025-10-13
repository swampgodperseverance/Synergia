using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class JadeGlaive : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 24;
            Item.height = 25;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(silver: 50);
            Item.shootSpeed = 14f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<JadeGlaiveProj>();
            Item.consumable = false;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
    }
}