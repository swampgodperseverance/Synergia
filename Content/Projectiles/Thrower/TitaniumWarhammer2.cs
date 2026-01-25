using System; using System.Collections.Generic; using Microsoft.Xna.Framework; using NewHorizons.Content.Items.Weapons.Throwing; using NewHorizons.Globals; using Terraria; using Terraria.Audio; using Terraria.ID; using Terraria.DataStructures; using Terraria.GameContent; using Terraria.ModLoader;
namespace Synergia.Content.Projectiles.Thrower
{
    public class TitaniumWarhammer2 : ModProjectile
    {
        private const int ExplodeTime = 40;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation += 0.188f * Projectile.direction;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.velocity.Y += 0.15f;
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= ExplodeTime)
                Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int count = Main.rand.Next(3, 7);
                for (int i = 0; i < count; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(4f, 4f);
                    vel.Y -= Main.rand.NextFloat(2f, 4f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        vel,
                        ModContent.ProjectileType<TMicroHammer>(),
                        Projectile.damage / 2,
                        Projectile.knockBack,
                        Projectile.owner,
                        Projectile.Center.X,     
                        Projectile.Center.Y          
                    );
                }

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<TitaniumWarhammerReturn>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

   

            NewHorizonsDraw.SpawnRing(Projectile.Center, new Color(150, 150, 150, 120), 0.13f, 1f, 0f);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i < 12; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 146,
                    Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3), 0, default, 1.3f);
                Main.dust[d].noGravity = true;
            }
        }
    }

    public class TMicroHammer : ModProjectile
    {
        private const int SpinTime = 60;
        private const float GatherSpeed = 12f;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.35f * Projectile.direction;
            Projectile.velocity.Y += 0.12f;
            Projectile.ai[2]++;

            if (Projectile.ai[2] < SpinTime)
            {
                return;
            }

            Vector2 target = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 toTarget = target - Projectile.Center;
            float distance = toTarget.Length();

            if (distance < 18f)
            {
                FindAndJoinReturnHammer();
                Projectile.Kill();
                return;
            }

            toTarget.Normalize();
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget * GatherSpeed, 0.25f);
            Projectile.tileCollide = false;
        }

        private void FindAndJoinReturnHammer()
        {
            Projectile closest = null;
            float bestDist = 260f;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                var p = Main.projectile[i];
                if (!p.active) continue;
                if (p.owner != Projectile.owner) continue;
                if (p.type != ModContent.ProjectileType<TitaniumWarhammerReturn>()) continue;

                float dist = Vector2.Distance(Projectile.Center, p.Center);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    closest = p;
                }
            }

            if (closest != null)
            {
                closest.localAI[0]++;                   
                closest.localAI[1] = Math.Max(closest.localAI[1], bestDist);

                for (int d = 0; d < 8; d++)
                {
                    Vector2 dv = Main.rand.NextVector2Circular(3.4f, 3.4f);
                    int dust = Dust.NewDust(Projectile.Center, 0, 0, 146, dv.X, dv.Y, 0, default, 1.25f);
                    Main.dust[dust].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.35f, PitchVariance = 0.15f }, Projectile.position);
            }
        }
    }
    public class TitaniumWarhammerReturn : ModProjectile
    {
        private const int DELAY_BEFORE_RETURN = 120;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.localAI[0] = 0;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            UpdateSizeAndDamage();

            Projectile.localAI[2]++;

            Vector2 toPlayer = player.Center - Projectile.Center;
            float distanceToPlayer = toPlayer.Length();

            if (distanceToPlayer < 24f)
            {
                Projectile.Kill();
                return;
            }

            toPlayer.Normalize();

            if (Projectile.localAI[2] < DELAY_BEFORE_RETURN)
            {
                Projectile.velocity *= 0.92f;
            }
            else
            {
                float accel = 1.4f + Projectile.localAI[0] * 0.12f;
                Projectile.velocity += toPlayer * accel;

                float maxSpeed = 18f + Projectile.localAI[0] * 1.2f;
                if (Projectile.velocity.Length() > maxSpeed)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= maxSpeed;
                }
            }

            Projectile.rotation += 0.48f * Projectile.direction;

            if (Projectile.localAI[0] > 0 && Main.rand.NextBool(15))
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.4f, Projectile.height * 0.4f),
                    146, Vector2.Zero, 80, default, 0.7f);
                d.noGravity = true;
            }
        }

        private void UpdateSizeAndDamage()
        {
            float count = Math.Min(Projectile.localAI[0], 6f);
            float multiplier = 0.6f + count * 0.0667f;

            Projectile.scale = multiplier;
            int baseWidth = 34;
            Projectile.width = (int)(baseWidth * multiplier);
            Projectile.height = (int)(baseWidth * multiplier);
            Projectile.Center = Projectile.position + new Vector2(Projectile.width, Projectile.height) * 0.5f;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 146,
                    Main.rand.NextVector2Circular(2.2f, 2.2f), 100, default, 0.9f);
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f * Projectile.scale;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                float trailScale = Projectile.scale * (0.3f + alpha * 0.7f);

                Main.EntitySpriteDraw(texture, drawPos, null, Color.White * alpha * 0.6f,
                    Projectile.rotation, origin / Projectile.scale * trailScale, trailScale,
                    SpriteEffects.None, 0);
            }

            Vector2 drawOrigin = texture.Size() / 2f;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor,
                Projectile.rotation, drawOrigin, Projectile.scale,
                Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            return false;
        }
    }
}