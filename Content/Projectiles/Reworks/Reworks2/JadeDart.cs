
using ValhallaMod.Dusts;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class JadeDart : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
            Projectile.timeLeft = 3000;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 3f)
            {
                Projectile.alpha = 0;
            }

            if (Projectile.ai[0] >= 20f)
            {
                Projectile.ai[0] = 20f;
                Projectile.velocity.Y += 0.075f;
            }

         
            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
            d.velocity += Projectile.velocity * -0.5f;
            d.velocity *= 0.25f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            Vector2 size = new Vector2(20f, 20f);
            for (int i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                d.velocity *= 1.4f;
            }
        }
    }
}
