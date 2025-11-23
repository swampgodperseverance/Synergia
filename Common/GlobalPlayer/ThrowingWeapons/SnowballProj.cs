using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.Players;

namespace Synergia.Common.GlobalProjectiles
{
    public class SnowballGlobalProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            // ✅ Срабатывает только для снежков (ванильных или модовых, если названы Snowball)
            return entity.type == ProjectileID.SnowBallFriendly ||
                   entity.ModProjectile?.Name?.Contains("Snowball", System.StringComparison.OrdinalIgnoreCase) == true;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
            {
                Player player = Main.player[projectile.owner];

                // ✅ Проверяем, что игрок действительно держит снежок
                if (player.HeldItem != null &&
                    (player.HeldItem.type == ItemID.Snowball ||
                     player.HeldItem.ModItem?.Name?.Contains("Snowball", System.StringComparison.OrdinalIgnoreCase) == true))
                {
                    // Добавляем эффект комбо для снежков
                    player.GetModPlayer<SnowballPlayer>().AddCombo();
                }
            }
        }
    }
}
