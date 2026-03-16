using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Friendly;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Mage
{
    public class OrcishSwungo : ModItem
    {
        private int shootCooldown = 0;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.mana = 60;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 40;
            Item.shoot = ModContent.ProjectileType<OrcishSwungoP>();
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
        }

        public override void UpdateInventory(Player player)
        {
            if (shootCooldown > 0)
            {
                shootCooldown--;
            }
        }

        public override bool CanUseItem(Player player)
        {
            return shootCooldown <= 0 && base.CanUseItem(player);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shootCooldown = 90;

            Vector2 center = player.Center;
            float radius = 60f;
            const int count = 4;
            float angleStep = MathHelper.TwoPi / count;
            float baseAngle = velocity.ToRotation();

            for (int i = 0; i < count; i++)
            {
                float angle = baseAngle + i * angleStep;
                Vector2 spawnPos = center + angle.ToRotationVector2() * radius;
                Vector2 shootVel = (spawnPos - center).SafeNormalize(Vector2.Zero) * 8f;

                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    shootVel,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );
            }

            return false;
        }
    }
}