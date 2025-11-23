using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Global
{
    public class BismuthumCrossbowGlobal : GlobalItem
    {
        private static Dictionary<int, int> playerShotCounters = new Dictionary<int, int>();

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem != null &&
                   entity.ModItem.Mod.Name == "Bismuth" &&
                   entity.ModItem.Name == "BismuthumCrossbow";
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int playerWhoAmI = player.whoAmI;

            if (!playerShotCounters.ContainsKey(playerWhoAmI))
            {
                playerShotCounters[playerWhoAmI] = 0;
            }

            playerShotCounters[playerWhoAmI]++;

            if (playerShotCounters[playerWhoAmI] >= 3)
            {
                playerShotCounters[playerWhoAmI] = 0;

                int projType = ModContent.ProjectileType<Content.Projectiles.Friendly.BismuthumShrapnel>();
                if (projType > 0)
                {
                    Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI);
                }
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void Unload()
        {
            playerShotCounters?.Clear();
            playerShotCounters = null;
        }
    }
}