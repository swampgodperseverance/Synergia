using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Throwing
{
    public class BlasphemousHeavens : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 24;
            Item.height = 25;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(silver: 50);
            Item.shootSpeed = 14f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<BlasphemousHeavensProj>();
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
    }
}