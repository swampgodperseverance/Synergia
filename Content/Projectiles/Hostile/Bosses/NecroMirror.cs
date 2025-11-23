using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Bismuth.Content.NPCs; 
using System;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroMirror : ModProjectile
    {
        private float orbitAngle;
        private int spawnTimer; 
        private const int FadeInTime = 120; 

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000; 
        }

        public override void AI()
        {
            NPC necro = FindNecromancer();
            if (necro == null || !necro.active)
            {
                Projectile.Kill();
                return;
            }

            if (spawnTimer < FadeInTime)
            {
                spawnTimer++;
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, spawnTimer / (float)FadeInTime);

                if (Main.rand.NextBool(3))
                {
                    Dust d = Dust.NewDustDirect(
                        Projectile.Center - new Vector2(8, 8),
                        16, 16,
                        DustID.Smoke,
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        100,
                        Color.Black,
                        Main.rand.NextFloat(1.2f, 1.8f)
                    );
                    d.noGravity = true;
                    d.fadeIn = 1.4f;
                }
            }
            else
            {
                Projectile.alpha = 0; 
            }
            float orbitRadius = 120f; 
            orbitAngle += 0.03f; 
            if (orbitAngle > MathHelper.TwoPi)
                orbitAngle -= MathHelper.TwoPi;

            Vector2 offset = new Vector2((float)Math.Cos(orbitAngle), (float)Math.Sin(orbitAngle)) * orbitRadius;
            Projectile.Center = necro.Center + offset;

            Projectile.rotation = (necro.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2;

            ReflectFriendlyProjectiles();
        }

        private void ReflectFriendlyProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile target = Main.projectile[i];
                if (!target.active || !target.friendly || target.hostile || target.minion)
                    continue;
                if (target.Hitbox.Intersects(Projectile.Hitbox))
                {
                    target.Kill();
                    SoundEngine.PlaySound(SoundID.Item74, Projectile.position);

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int projType = ModContent.ProjectileType<NecroFire>();
                        Vector2 dir = (target.velocity.Length() > 0.1f ? target.velocity.SafeNormalize(Vector2.Zero) : Vector2.UnitY) * 8f;
                        Projectile.NewProjectileDirect(
                            Projectile.GetSource_FromAI(),
                            Projectile.Center,
                            -dir, 
                            projType,
                            40,
                            2f,
                            Main.myPlayer
                        );
                    }
                }
            }
        }

        private NPC FindNecromancer()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.type == ModContent.NPCType<EvilNecromancer>())
                    return npc;
            }
            return null;
        }
    }
}
