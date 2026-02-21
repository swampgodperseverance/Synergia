using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Armor;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class FreezeBoltFI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null) return false;
            return entity.ModItem.Mod?.Name == "Avalon" && entity.ModItem.Name == "FreezeBolt";
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;

            item.DamageType = DamageClass.Magic;
            item.knockBack = 4f;
            item.useTime = 85;
            item.useAnimation = 85;
            item.reuseDelay = 0;
            item.mana = 23;
            item.shootSpeed = 11f;
            item.shoot = 1;//ModContent.ProjectileType<FreezeSpawn>();
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override bool Shoot(
    Item item,
    Player player,
    EntitySource_ItemUse_WithAmmo source,
    Vector2 position,
    Vector2 velocity,
    int type,
    int damage,
    float knockback)
        {
            Vector2 spawnPos = Main.MouseWorld;

            Vector2 dir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitY);
            Vector2 finalVelocity = dir * item.shootSpeed;

            Projectile.NewProjectile(
                source,
                spawnPos,
                finalVelocity,
                ModContent.ProjectileType<FreezeSpawn>(),
                damage,
                knockback,
                player.whoAmI
            );

            for (int i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    spawnPos,
                    DustID.Ice,
                    Main.rand.NextVector2Circular(1.8f, 1.8f),
                    0,
                    new Color(150, 220, 255),
                    Main.rand.NextFloat(0.9f, 1.4f)
                );
                d.noGravity = true;
            }

            return false;
        }

    }
}