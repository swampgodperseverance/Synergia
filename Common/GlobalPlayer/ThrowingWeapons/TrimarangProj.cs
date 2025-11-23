using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.Players;

namespace Synergia.Common.GlobalProjectiles
{
    public class TrimarangGlobalProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.Trimarang;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers)
            {
                Player player = Main.player[projectile.owner];
                if (player.HeldItem.type == ItemID.Trimarang)
                {
                    player.GetModPlayer<TrimarangPlayer>().AddCombo();
                }
            }
        }
    }
}