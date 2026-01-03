using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.AltUse;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class IchormoreGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int swingDirection;
        private int attackCounter = 0; // Счётчик ударов

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "Ichormore", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            item.DamageType = DamageClass.Melee;
            item.knockBack = 6f;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTurn = false;
            item.autoReuse = true;
            item.ArmorPenetration = 5;
            item.noMelee = true;
            item.UseSound = null;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<IchormoreRework>();
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = Vector2.Zero;
            swingDirection = swingDirection == 1 ? -1 : 1;
            attackCounter++; // Увеличиваем счётчик ударов
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 handPos = player.MountedCenter;
            float handleOffset = 8f;
            Vector2 offset = new Vector2(handleOffset * player.direction, -2f * player.gravDir);
            Vector2 spawnPos = handPos + offset;

            // Основной reworked проектайл
            Projectile.NewProjectile(
                source,
                spawnPos,
                Vector2.Zero,
                type,
                damage,
                knockback,
                player.whoAmI,
                ai0: swingDirection,
                ai1: player.MountedCenter.AngleTo(Main.MouseWorld),
                ai2: 0
            );

            if (attackCounter % 3 == 0)
            {
                int spawnProjType = ModLoader.GetMod("ValhallaMod").Find<ModProjectile>("IchormoureSpawn").Type;

                float baseAngle = spawnPos.AngleTo(Main.MouseWorld);
                float spreadAngle = MathHelper.ToRadians(30f);
                float[] angles = new float[]
                {
                    baseAngle - spreadAngle,
                    baseAngle,
                    baseAngle + spreadAngle
                };

                float projSpeed = 12f;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 projVelocity = angles[i].ToRotationVector2() * projSpeed;

                    Projectile.NewProjectile(
                        source,
                        spawnPos,
                        Vector2.Zero,
                        spawnProjType,
                        (int)(damage * 0.9f),
                        knockback * 0.7f,
                        player.whoAmI,
                        ai0: 10f,
                        ai1: projVelocity.X,   
                        ai2: projVelocity.Y
                    );
                }
            }

            return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
    public class IchormoureSpawnDelay : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ModLoader.GetMod("ValhallaMod").Find<ModProjectile>("IchormoureSpawn").Type;
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.ai[0] > 0)
            {
                projectile.ai[0] -= 1;
                return;
            }

            if (projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = new Vector2(projectile.ai[1], projectile.ai[2]);
            }
        }
    }
}