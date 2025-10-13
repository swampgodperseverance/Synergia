using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Accessories
{
    public class RingofVengeanceseeker : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ring of Vengeanceseeker");
            /* Tooltip.SetDefault("Summons up to 4 Vengeance Spheres that orbit you\n" +
                               "Each sphere increases Throwing damage by 4%\n" +
                               "Spheres vanish when you take damage and respawn one by one every 5 seconds"); */
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 10, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VengeancePlayer>().hasRing = true;
        }
    }

    public class VengeancePlayer : ModPlayer
    {
        public bool hasRing;
        private int spawnTimer;
        private int maxSpheres = 4;

        public override void ResetEffects()
        {
            hasRing = false;
        }

        public override void PostUpdate()
        {
            if (!hasRing)
                return;

            spawnTimer++;
            if (spawnTimer >= 60 * 5) // 5 секунд
            {
                spawnTimer = 0;

                int owned = Player.ownedProjectileCounts[ModContent.ProjectileType<VengeanceSphere>()];
                if (owned < maxSpheres)
                {
                    // создаём одну сферу
                    Projectile.NewProjectile(
                        Player.GetSource_Accessory(Player.HeldItem),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<VengeanceSphere>(),
                        0,
                        0f,
                        Player.whoAmI,
                        owned // индекс для положения
                    );
                }
            }

            // Добавляем урон за сферы
            int count = Player.ownedProjectileCounts[ModContent.ProjectileType<VengeanceSphere>()];
            Player.GetDamage(DamageClass.Throwing) += 0.04f * count;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            // Уничтожаем сферы красиво
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<VengeanceSphere>() && proj.owner == Player.whoAmI)
                {
                    proj.Kill();
                }
            }

            // сбрасываем таймер, чтобы новый цикл шёл с нуля
            spawnTimer = 0;
        }
    }
}
