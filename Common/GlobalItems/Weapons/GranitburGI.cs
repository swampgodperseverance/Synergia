using Microsoft.Xna.Framework;
using NewHorizons;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class granitburGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null) return false;
            return entity.ModItem.Mod?.Name == "ValhallaMod" && entity.ModItem.Name == "Granitbur";
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;
            item.scale = 1.2f;
            item.shoot = ModContent.ProjectileType<GranitburRework>();
        }

        public int swingCounter = 0;

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
            if (!AppliesToEntity(item, false))
                return true;

            if (Main.myPlayer == player.whoAmI)
            {
                float swingTime = item.useAnimation;
                int projType = ModContent.ProjectileType<GranitburRework>();
                int proj = Projectile.NewProjectile(
                    source,
                    player.MountedCenter,
                    velocity,
                    projType,
                    damage,
                    knockback,
                    player.whoAmI,
                    swingCounter % 2, // ai0
                    swingTime // ai1
                );

                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].netUpdate = true;
                }

                SoundEngine.PlaySound(SoundID.Item1 with { PitchVariance = 0.15f, Volume = 0.9f }, player.Center);
            }

            swingCounter++;
            if (swingCounter > 1) swingCounter = 0;

            return false;
        }
    }
}