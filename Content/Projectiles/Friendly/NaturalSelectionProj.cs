using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Avalon.Dusts;

namespace Synergia.Content.Projectiles.Friendly
{
    public class NaturalSelectionProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 3; 
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
        
            if (Main.rand.NextBool(5)) 
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ContagionDust>(), 
                             Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
            }
        }
    }
}