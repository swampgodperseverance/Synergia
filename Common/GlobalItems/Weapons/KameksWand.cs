using System;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.AltUse;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Content.Global
{
    public class KamekGlobal : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "MagicWand2", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
         
            item.useTime = 8;       
            item.useAnimation = 8;
            item.reuseDelay = 0;
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
            Vector2 vector = Main.MouseWorld;


            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 60f;

            int[] projectiles = new int[]
            {
                ModContent.ProjectileType<RedP>(),
                ModContent.ProjectileType<BlueP>(),
                ModContent.ProjectileType<GreenP>(),
                ModContent.ProjectileType<YellowP>(),
                ModContent.ProjectileType<PurpleP>()
            };

            int chosenType = Main.rand.Next(projectiles);


            float speed = 4.5f + Main.rand.NextFloat() * 6.5f;
            Vector2 start = Vector2.UnitY.RotatedByRandom(6.32);


            Projectile.NewProjectile(
                source,
                position.X,
                position.Y,
                start.X * speed,
                start.Y * speed,
                chosenType,
                damage,
                knockback,
                player.whoAmI,
                vector.X,
                vector.Y
            );

            return false;
        }
    }
}
