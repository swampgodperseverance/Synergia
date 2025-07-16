using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Vanilla.Content.Dusts;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class AllotropicAuraOrb : AuraDamageAI
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 40;
            Projectile.penetrate = 1;
        }

        public override void CustomAI()
        {
            Projectile.rotation += 0.2f;

            // Dust effect
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald,
                Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;

            // Homing behavior
            NPC target = FindTarget();

            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                float speed = 6f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, 0.1f); // smooth homing
            }
        }

        private NPC FindTarget()
        {
            float maxRange = 480f;
            NPC closest = null;
            float closestDist = maxRange;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < closestDist)
                    {
                        closest = npc;
                        closestDist = dist;
                    }
                }
            }

            return closest;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; ++i)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    ModContent.DustType<AvalonIridiumDust>(), 
                    Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 
                    150, default, 1.2f);
            }
        }
    }
}