using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Other
{
    public class Meatknife1 : MeatknifeBase
    {
        public override string Texture => "Synergia/Content/Projectiles/Other/Meatknifes";
        protected override int FrameIndex => 0;
        protected override float OffsetX => 0f;
    }

    public class Meatknife2 : MeatknifeBase
    {
        public override string Texture => "Synergia/Content/Projectiles/Other/Meatknifes";
        protected override int FrameIndex => 1;
        protected override float OffsetX => -45f;
    }

    public class Meatknife3 : MeatknifeBase
    {
        public override string Texture => "Synergia/Content/Projectiles/Other/Meatknifes";
        protected override int FrameIndex => 2;
        protected override float OffsetX => 45f;
    }

    public abstract class MeatknifeBase : ModProjectile
    {
        protected abstract int FrameIndex { get; }
        protected abstract float OffsetX { get; }

        private Player Owner => Main.player[Projectile.owner];
        private Vector2 HoverTarget;
        private int dashTimer = 0;
        private bool isDashing = false;
        private Vector2 dashVelocity;
        private int spawnTime = 0;
        private int hitCooldown = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 999999;
            Projectile.alpha = 255;
            Projectile.ownerHitCheck = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            HoverTarget = Owner.Center + new Vector2(OffsetX, -80);
            Projectile.Center = HoverTarget;
            spawnTime = 0;
            hitCooldown = 0;
        }

        public override void AI()
        {
            if (!Owner.active || Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            spawnTime++;
            
            if (hitCooldown > 0) hitCooldown--;

            Vector2 desiredPosition = Owner.Center + new Vector2(OffsetX, -80);
            HoverTarget = Vector2.Lerp(HoverTarget, desiredPosition, 0.28f);

            if (!isDashing)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, HoverTarget, 0.25f);
                Projectile.rotation = (Owner.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2;

                dashTimer++;
                if (dashTimer > 40 + Main.rand.Next(30, 60))
                    StartDash();
            }
            else
            {
                Projectile.velocity = dashVelocity;
                dashVelocity *= 0.92f;
    
                if (hitCooldown == 0)
                {
                    float distanceToPlayer = Vector2.Distance(Projectile.Center, Owner.Center);
                    if (distanceToPlayer < 45f)
                    {
                        DealDamageToPlayer(45, 80);
                        hitCooldown = 10;
                       
                        for (int i = 0; i < 15; i++)
                        {
                            Dust.NewDustDirect(Owner.Center - new Vector2(10), 20, 20, DustID.Blood,
                                Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 100, default, 1.3f);
                        }
                    }
                }
                
                if (dashVelocity.Length() < 3f)
                    EndDash();
            }

            Projectile.frame = FrameIndex;
            Projectile.alpha = spawnTime < 30 ? (int)(255 * (1f - spawnTime / 30f)) : 0;
        }

        private void DealDamageToPlayer(int minDamage, int maxDamage)
        {
            int damage = Main.rand.Next(minDamage, maxDamage + 1);
            PlayerDeathReason reason = PlayerDeathReason.ByProjectile(Owner.whoAmI, Projectile.whoAmI);
            Owner.Hurt(reason, damage, 0, false, false, -1, true);
        }

        private void StartDash()
        {
            isDashing = true;
            dashTimer = 0;

            Vector2 dir = (Owner.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            float speed = 12f + Main.rand.NextFloat(4.5f);

            dashVelocity = dir * speed;
        }

        private void EndDash()
        {
            isDashing = false;
            Projectile.velocity *= 0.18f;

            float distanceToPlayer = Vector2.Distance(Projectile.Center, Owner.Center);
            if (distanceToPlayer < 60f)
            {
                DealDamageToPlayer(20, 35);
                
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDustDirect(Owner.Center - new Vector2(10), 20, 20, DustID.Blood,
                        Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 120, default, 1.6f);
                }
            }

            for (int i = 0; i < 14; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood,
                    Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 120, default, 1.4f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);

            float alpha = 1f - Projectile.alpha / 255f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color outlineColor = new Color(180, 25, 25, (int)(55 * alpha));

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = i switch
                {
                    0 => new Vector2(1.5f, 0),
                    1 => new Vector2(-1.5f, 0),
                    2 => new Vector2(0, 1.5f),
                    _ => new Vector2(0, -1.5f)
                };

                Main.EntitySpriteDraw(texture, drawPos + offset, frame, outlineColor,
                    Projectile.rotation, frame.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, frame, Color.White * alpha,
                Projectile.rotation, frame.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0, 0, 100, default, 1.4f);
            }
        }
    }
}