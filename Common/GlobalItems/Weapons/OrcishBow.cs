using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.RangedProjectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Global
{
    public class OrcishBowGlobal : GlobalItem
    {
        private static Dictionary<int, int> playerShotCounters = new Dictionary<int, int>();

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem != null &&
                   entity.ModItem.Mod.Name == "Bismuth" &&
                   entity.ModItem.Name == "OrcishCrossbow";
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<OrcishArrow>();
            }
        }
    }
}