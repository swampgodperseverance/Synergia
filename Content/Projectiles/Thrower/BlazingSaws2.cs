using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class BlazingSaws2 : ModProjectile
    {
        private int jumpsLeft = 3;
        private int stickTimer;
        private bool isStuck;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Throwing;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.tileCollide = true;
            Projectile.frame = 1;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f * Projectile.direction;

            if (isStuck)
            {
                Projectile.velocity = Vector2.Zero;
                stickTimer++;

                if (stickTimer >= 20)
                {
                    if (jumpsLeft <= 0)
                    {
                        Projectile.Kill();
                        return;
                    }

                    NPC next = FindNextTarget();
                    if (next != null)
                    {
                        Vector2 dir = Projectile.DirectionTo(next.Center);
                        Projectile.velocity = dir * 14f;

                        isStuck = false;
                        stickTimer = 0;
                        jumpsLeft--;

                        Projectile.tileCollide = false;
                        SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
            }
            else
            {
                Projectile.velocity.Y += 0.14f;
            }

            // кровь
            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Blood,
                    -Projectile.velocity.X * 0.3f,
                    -Projectile.velocity.Y * 0.3f,
                    120,
                    default,
                    1.1f
                );
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!isStuck)
            {
                isStuck = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                stickTimer = 0;
                Projectile.netUpdate = true;
            }
        }

        private NPC FindNextTarget()
        {
            float range = 500f;
            NPC chosen = null;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy())
                    continue;

                float dist = Projectile.Distance(npc.Center);
                if (dist < range)
                {
                    range = dist;
                    chosen = npc;
                }
            }

            return chosen;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!isStuck)
            {
                isStuck = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.tileCollide = false;
                stickTimer = 0;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Blood,
                    Main.rand.NextFloat(-3, 3),
                    Main.rand.NextFloat(-3, 3),
                    0,
                    default,
                    1.6f
                );
                Main.dust[d].noGravity = true;
            }
        }
    }
}
