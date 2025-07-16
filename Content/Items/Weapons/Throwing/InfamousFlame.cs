using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles;

namespace Vanilla.Content.Items.Weapons.Throwing
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
            Item.noUseGraphic = true; // не отображаем оружие при использовании
            Item.noMelee = true; // урон только от снаряда
            Item.shoot = ModContent.ProjectileType<InfamousFlameProjectile>();
            Item.shootSpeed = 12f;
        }

        public override bool CanUseItem(Player player)
        {
            // Блокировка повторного броска, пока предыдущий не вернулся
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}