using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Dusts;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class MercuriumAuraScytheCut : ModProjectile
    { 
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 4;
        }

        public override void AI()
        {
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 0f)
            {
                for (int i = 0; i < 360; i += 20)
                {
                    float angle = MathHelper.ToRadians(i);
                    Vector2 pos = Projectile.Center + angle.ToRotationVector2() * 2f;

                    int dustType = ModContent.DustType<MercuriumSparkDust>();
                    Dust d = Dust.NewDustPerfect(pos, dustType);
                    d.velocity = Vector2.Zero;
                    d.scale = Main.rand.NextFloat(1.1f, 1.5f);
                    d.noGravity = true;
                    d.fadeIn = 1.3f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 300); // 5 секунд
        }
    }
}
