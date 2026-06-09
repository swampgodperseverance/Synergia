using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Thrower;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material.Bar;

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class ThunderChakram : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 76;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 24;
            Item.height = 25;
            Item.useTime = 47;
            Item.useAnimation = 47;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 3);
            Item.shootSpeed = 14f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<ThunderChakramProj>();
            Item.consumable = false;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                {
                    return false;
                }
            }
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.HeldItem.type != Item.type)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.type == Item.shoot && proj.owner == player.whoAmI)
                    {
                        proj.Kill();
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ThunderBar>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}