using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class AquaImpactGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null) return false;
            return entity.ModItem.Mod?.Name == "Avalon" && entity.ModItem.Name == "AquaImpact";
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;

            item.DamageType = DamageClass.Magic;
            item.knockBack = 4f;
            item.useTime = 22;      
            item.useAnimation = 22;
            item.reuseDelay = 0;
            item.mana = 10;     
            item.shootSpeed = 11f;
            item.shoot = ModContent.ProjectileType<AquaSmall>();
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 direction = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);

            float spreadAngle = MathHelper.ToRadians(3f);
            Vector2 finalVelocity = direction.RotatedByRandom(spreadAngle) * item.shootSpeed;

            Projectile.NewProjectile(
                source,
                player.Center + direction * 20f, 
                finalVelocity,
                ModContent.ProjectileType<AquaSmall>(),
                damage,
                knockback,
                player.whoAmI
            );

            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    player.Center + direction * 25f,
                    DustID.Water,
                    finalVelocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f),
                    0,
                    new Color(100, 180, 255),
                    Main.rand.NextFloat(0.8f, 1.2f)
                );
                d.noGravity = true;
            }

            return false;
        }

    }
}