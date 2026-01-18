using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewHorizons.Globals;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class NanoStar1 : ModProjectile
    {
        public NPC Target
        {
            get => Main.npc[(int)Projectile.ai[0]];
            set => Projectile.ai[0] = value.whoAmI;
        }

        public float Time
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        private int shootTimer;
        public List<NPC> NPCsAlreadyHit = new();

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 120 * (1 + Projectile.extraUpdates);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            Vector2 origin = tex.Size() / 2f;

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor),
                0f,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                glow,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                0f,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override void AI()
        {
            Time++;

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 206);
                Main.dust[d].noGravity = true;
            }

            if ((Time >= 10f || NPCsAlreadyHit.Count > 0) && NPCsAlreadyHit.Count > 0)
            {
                float angle = MathHelper.WrapAngle(
                    Projectile.AngleTo(Target.Center) - Projectile.velocity.ToRotation()
                );

                angle = MathHelper.Clamp(angle, -0.2f, 0.2f);
                Projectile.velocity = Projectile.velocity.RotatedBy(angle);
            }

            if (Time == 14f)
                Projectile.friendly = true;

            shootTimer++;
            if (shootTimer >= 16) // 
            {
                shootTimer = 0;

                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 baseDir = Projectile.velocity.SafeNormalize(Vector2.UnitX);

                    for (int i = -1; i <= 1; i += 2)
                    {
                        Vector2 dir = baseDir.RotatedBy(MathHelper.ToRadians(30f * i));

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            Projectile.Center,
                            dir * 5f,
                            ModContent.ProjectileType<NanoStar2>(),
                            (int)(Projectile.damage * 0.8f),
                            Projectile.knockBack * 0.5f,
                            Projectile.owner
                        );
                    }
                }
            }
        }

        public override bool? CanDamage()
        {
            return Time > Projectile.MaxUpdates;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            DustHelper.DrawCircle(target.Center, 206, 5f);

            if (!NPCsAlreadyHit.Contains(target))
                NPCsAlreadyHit.Add(target);

            float minDist = 600f;
            List<NPC> targets = new();

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc != target && npc.CanBeChasedBy() && !NPCsAlreadyHit.Contains(npc))
                {
                    float dist = Projectile.Distance(npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        targets.Add(npc);
                    }
                }
            }

            if (targets.Count == 0)
            {
                Projectile.Kill();
                return;
            }

            NPC next = targets[Main.rand.Next(Math.Min(2, targets.Count))];

            Projectile proj = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Projectile.velocity,
                Type,
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner,
                next.whoAmI
            );

            var mod = (NanoStar1)proj.ModProjectile;
            mod.NPCsAlreadyHit.AddRange(NPCsAlreadyHit);
            mod.NPCsAlreadyHit.Add(next);

            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i < 12; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 206);
                Main.dust[d].velocity *= 3f;
                Main.dust[d].noGravity = true;
            }
        }
    }
    public class NanoStar2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.96f;
            Projectile.rotation += 0.25f * Projectile.direction;
            if (Main.rand.NextBool(5)) 
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Electric,
                    Projectile.velocity.X * 0.1f,
                    Projectile.velocity.Y * 0.1f,
                    150,
                    default,
                    0.9f
                );

                Main.dust[d].noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.15f, 0.35f, 0.7f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slow, 180);
        }
    }
}
