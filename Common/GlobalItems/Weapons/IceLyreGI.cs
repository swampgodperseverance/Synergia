using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class IceLyreGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null) return false;
            return entity.ModItem.Mod?.Name == "Avalon" && entity.ModItem.Name == "FrozenLyre";
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
                    Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int count = Main.rand.Next(1, 4); 

            for (int i = 0; i < count; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10f));

                float speedMult = Main.rand.NextFloat(0.8f, 1.25f);
                newVelocity *= speedMult;

                Projectile.NewProjectile(
                    source,
                    position,
                    newVelocity,
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