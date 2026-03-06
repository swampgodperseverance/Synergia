using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Other
{
    public class BaseShortswordCommon : BaseShortsword
    {
        public override string Texture => "Synergia/Assets/Textures/Shortsword";

        public override void SetStats(ref float speed, ref float offset, ref int timeMax)
        {
            speed = 1f;
            offset = 1f;
            timeMax = 10; 
        }

        public override void SetDefaults()
        {
            DrawOffsetX = -68;
            DrawOriginOffsetY = -19;
            DrawOriginOffsetX = 20;

            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;

            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;

            Projectile.scale = 1f;
            Projectile.timeLeft = 10; 

            Projectile.alpha = 0;
            Projectile.ownerHitCheck = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Black;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Projectile.Center = player.MountedCenter + direction * 60f;

            Projectile.rotation = direction.ToRotation();

            Projectile.alpha += 25;
            Projectile.scale *= 0.94f;

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Shadowflame
                );

                d.velocity *= 0.3f;
                d.color = Color.Black;
                d.scale = 1.1f;
            }
        }
    }
}