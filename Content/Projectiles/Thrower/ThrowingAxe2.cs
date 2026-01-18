using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Synergia.Content.Projectiles.Thrower
{
    public class ThrowingAxe2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenBoomerang);

            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 3;

            Projectile.aiStyle = 3;
            AIType = ProjectileID.WoodenBoomerang;

            Projectile.extraUpdates = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == Projectile.owner &&
                    p.type == Type &&
                    p.whoAmI != Projectile.whoAmI)
                {
                    Projectile.Kill();
                    return;
                }
            }
        }

        public override void AI()
        {
            float homingRange = 60f;
            float homingStrength = 0.08f;

            NPC target = null;
            float minDist = homingRange;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy(this))
                    continue;

                float dist = Projectile.Distance(npc.Center);
                if (dist < minDist)
                {
                    minDist = dist;
                    target = npc;
                }
            }

            if (target != null)
            {
                Vector2 desiredVelocity =
                    Projectile.DirectionTo(target.Center) * Projectile.velocity.Length();

                Projectile.velocity = Vector2.Lerp(
                    Projectile.velocity,
                    desiredVelocity,
                    homingStrength
                );
            }
        }
    }
}
