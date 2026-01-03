using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Content.Global
{
    public class BeestingGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int cooldown = 0;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "Beesting", StringComparison.OrdinalIgnoreCase);
        }

        public override void HoldItem(Item item, Player player)
        {
            if (cooldown > 0)
            {
                cooldown--;
                return;
            }

            if (player.controlUseTile) 
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 cursorPos = Main.MouseWorld;
                    Vector2 spawnPos = cursorPos + new Vector2(800f, -60f); // справа от курсора
                    Vector2 velocity = Vector2.Zero;

                    Projectile.NewProjectile(
                        player.GetSource_ItemUse(item),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<QueenBeeProj>(),
                        500,
                        5f,
                        player.whoAmI
                    );

                    cooldown = 30 * 60;
                }
            }
        }
    }
}
