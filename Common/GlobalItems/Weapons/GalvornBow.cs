using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Synergia.Content.Global
{
    public class GalvornBowGlobal : GlobalItem
    {
        private static Dictionary<int, int> playerShotCounters = new Dictionary<int, int>();

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem != null &&
                   entity.ModItem.Mod.Name == "Bismuth" &&
                   entity.ModItem.Name == "GalvornBow";
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
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

                int projType = ModContent.ProjectileType<Content.Projectiles.RangedProjectiles.GalvornPiece>();
                int reducedDamage = damage / 2;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(25f)) * 0.9f;

                    Projectile.NewProjectile(
                        source,
                        position,
                        newVelocity,
                        projType,
                        reducedDamage,
                        knockback * 0.8f,
                        player.whoAmI
                    );
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