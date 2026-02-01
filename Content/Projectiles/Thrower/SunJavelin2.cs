using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;


namespace Synergia.Content.Projectiles.Thrower
{
    public class SunJavelin2 : JavelinAI
    {
        private const int MaxStickingJavelins = 5;
        private const int ExplosionDamage = 40;
        private int skyJavelinsLeft = 0;
        private int skyJavelinTimer = 0;
        private int skyTarget = -1;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.localAI[0] == 1f)
                return new Color(255, 220, 80);
            return Color.White;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2)) 
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
            if (skyJavelinsLeft > 0 && skyTarget >= 0)
            {
                NPC target = Main.npc[skyTarget];

                if (!target.active)
                {
                    skyJavelinsLeft = 0;
                    skyTarget = -1;
                    return;
                }

                skyJavelinTimer++;

                if (skyJavelinTimer >= 3) //interval
                {
                    skyJavelinTimer = 0;
                    skyJavelinsLeft--;

                    Vector2 spawnPos = new Vector2(
                        target.Center.X + Main.rand.NextFloat(-190f, 190f),
                        Main.screenPosition.Y - 120f - Main.rand.NextFloat(0f, 80f)
                    );

                    Vector2 velocity = target.Center - spawnPos;
                    velocity.Normalize();
                    velocity *= Main.rand.NextFloat(18f, 22f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ProjectileType<SunJavelinProj>(),
                        (int)(Projectile.damage * 0.6f),
                        1f,
                        Projectile.owner
                    );
                }
            }

            base.AI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (Main.myPlayer != Projectile.owner)
                return;

            skyJavelinsLeft = Main.rand.Next(4, 7); // 4–7
            skyJavelinTimer = 0;
            skyTarget = target.whoAmI;
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            int javelinCount = CountStickingJavelins(target);

            if (javelinCount >= MaxStickingJavelins)
            {
                Projectile.localAI[0] = 1f; 

                Explode(target);
            }
        }

        private int CountStickingJavelins(NPC target)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.owner == Projectile.owner && 
                    other.type == Projectile.type && 
                    other.ai[0] == 1f && 
                    other.ai[1] == target.whoAmI)
                {
                    count++;
                }
            }
            return count;
        }

        private void Explode(NPC target)
        {

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GoldFlame, 
                    Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
            }

            int extraDamage = ExplosionDamage + (int)(Projectile.damage * 0.25f);

            int radius = 80; 
            
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.CanBeChasedBy() && 
                    Vector2.Distance(npc.Center, Projectile.Center) < radius)
                {
                    int hitDirection = npc.Center.X < Projectile.Center.X ? -1 : 1;
                    
                    npc.SimpleStrikeNPC(extraDamage, hitDirection);
                }
            }

            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.localAI[0] != 1f)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }

            Vector2 usePos = Projectile.position;
            Vector2 rotVector = Utils.ToRotationVector2(Projectile.rotation - MathHelper.ToRadians(90f));
            usePos += rotVector * 16f;

            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(usePos, Projectile.width, Projectile.height, 
                    DustID.GoldFlame, 0f, 0f, 0, default, 1.2f);
                Dust dust = Main.dust[dustIndex];
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.velocity += rotVector * 2f;
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                usePos -= rotVector * 8f;
            }
        }
    }
}