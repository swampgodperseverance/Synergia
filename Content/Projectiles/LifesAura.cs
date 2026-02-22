// Code by SerNik
using Synergia.Content.Buffs;
using System;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Projectiles {
    public class LifesAura : ModProjectile {
        public override void SetDefaults() {
            Projectile.width = 170;
            Projectile.height = 170;
            Projectile.alpha = 150;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
        }
        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff<BloodBuff>()) {
                Projectile.Center = player.Center;
                Projectile.timeLeft = 2;
                int maxDist = 85;
                for (int i = 0; i < 30; i++) {
                    double angle = Main.rand.NextDouble() * 2d * Math.PI;
                    Vector2 offset = new((float)Math.Sin(angle) * maxDist, (float)Math.Cos(angle) * maxDist);
                    Dust dust = Main.dust[Dust.NewDust(Projectile.Center + offset - Vector2.One * 4, 0, 0, DustID.CrimsonTorch, 0, 0, 100)];
                    dust.noGravity = true;
                }
                Projectile.ai[0]++;
                for (int i = 0; i < Main.maxNPCs; i++) {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.townNPC && npc.type != NPCID.TargetDummy) {
                        float dist = Vector2.Distance(Projectile.Center, npc.Center);
                        if (dist < maxDist) {
                            if (Projectile.ai[0] % 25 == 0) {
                                int amount = Main.rand.Next(3, 9);
                                npc.life -= amount;
                                if (npc.life <= 0) {
                                    NPC.HitInfo hitInfo = new() { Damage = 9999 };
                                    npc.HitEffect(hitInfo);
                                    npc.checkDead();
                                }
                                Helpers.NPCHelper.HitEffect(npc, amount);
                                Projectile.ai[0] = 0;
                            }
                        }
                    }
                }
            }
            else { Projectile.Kill(); }
        }
    }
}