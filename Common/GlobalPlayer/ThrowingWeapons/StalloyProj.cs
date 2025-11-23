using Microsoft.Xna.Framework;
using ValhallaMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.Players;

namespace Synergia.Common.GlobalProjectiles
{
    public class StalloyScrewGlobalProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            // ✅ ИЩЕМ ПО TYPE, а не ModProjectile!
            return entity.type == ModContent.ProjectileType<StalloyScrew>() || 
                   entity.ModProjectile?.Name == "StalloyScrew"; // на всякий
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
            {
                Player player = Main.player[projectile.owner];
                // ✅ Проверяем HeldItem, как у Trimarang
                if (player.HeldItem.ModItem != null &&
                    player.HeldItem.ModItem.Mod?.Name == "ValhallaMod" &&
                    player.HeldItem.ModItem.Name == "StalloyScrew")
                {
                    player.GetModPlayer<StalloyScrewPlayer>().AddCombo();
                }
            }
        }
    }
}