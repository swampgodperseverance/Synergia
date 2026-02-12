using Synergia.Content.Projectiles.Reworks.Reworks2;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Synergia.Content.Global
{
    public class WheezeBow : GlobalItem
    {
        public override bool InstancePerEntity => true;
        private int shootTimer = 0;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "RoA", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "BeastBow", StringComparison.OrdinalIgnoreCase);
        }

        public override void HoldItem(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer || player.itemAnimation <= 0)
                return;

            shootTimer++;

            if (shootTimer >= 90)
            {
                shootTimer = 0;
                SpawnTreeProjectile(player);
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (player.whoAmI != Main.myPlayer || player.itemAnimation <= 0)
            {
                shootTimer = 0;
            }
        }

        private void SpawnTreeProjectile(Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return;

            Vector2 mouseWorld = Main.MouseWorld;
            Vector2 direction = mouseWorld - player.Center;
            direction.Normalize();

            int projectileType = ModContent.ProjectileType<Tree>();
            IEntitySource source = player.GetSource_ItemUse(player.HeldItem);

            Projectile.NewProjectile(source, mouseWorld, Vector2.Zero, projectileType, 50, 2f, player.whoAmI);
        }

        public override void SetDefaults(Item item)
        {
            item.autoReuse = true;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.shootSpeed = 10f;
        }
    }
}