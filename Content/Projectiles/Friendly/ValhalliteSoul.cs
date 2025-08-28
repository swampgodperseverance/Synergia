using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Projectiles.Friendly
{
    public class ValhalliteSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120; 
            Projectile.penetrate = 1;
            Projectile.damage = 10;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) 
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.velocity *= 0.95f;
            Lighting.AddLight(Projectile.Center, 0.4f, 0.8f, 1f);

            if (Projectile.timeLeft == 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, Scale: 1.2f);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slow, 180); 
        }
    }
}
