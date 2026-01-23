using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using ValhallaMod.Projectiles.AI;
using Synergia.Helpers;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.Thrower
{
    public class Flamarang2 : ValhallaGlaive
    {
        private LavaRainbowRod lavaTrail = new LavaRainbowRod();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.5f;

            extraUpdatesHoming = 1;
            extraUpdatesComingBack = 1;
            rotationSpeed = 0.4f;
            bounces = 0;
            tileBounce = true;
            timeFlying = 22;
            speedHoming = 18f;
            speedFlying = 18f;
            speedComingBack = 20f;
            homingDistanceMax = 100f;
            homingStyle = 0;
            homingStart = false;
            homingIgnoreTile = false;
        }

        public override void AI()
        {
            base.AI();

            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.4f);
                d.noGravity = true;
                d.velocity *= 0.3f;
            }

            Projectile.localAI[0]++;

            if (Projectile.localAI[1] == 0f && Projectile.localAI[0] >= timeFlying - 8f)
            {
                Projectile.localAI[1] = 1f;

                int dir = Main.rand.NextBool() ? 1 : -1;

                Vector2 vel = Projectile.velocity.SafeNormalize(Vector2.Zero)
                    .RotatedBy(MathHelper.ToRadians(18f * dir)) * 14f;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    vel,
                    ModContent.ProjectileType<FlamarangProjectile>(),
                    (int)(Projectile.damage * 0.7f),
                    Projectile.knockBack * 0.8f,
                    Projectile.owner
                );

            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lavaTrail.Draw(Projectile);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            for (int k = 1; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2f;
                Color color = new Color(255, 100, 0, 0) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
                float scale = Projectile.scale * (1f - k / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, scale, effects, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
            return false;
        }
    }

    public class FlamarangProjectile : ModProjectile
    {
        private LavaRainbowRod lavaTrail = new LavaRainbowRod();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.9f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.alpha = 255;
            Projectile.scale = 0.3f;
        }

        public override void AI()
        {
            if (Projectile.owner < 0 || Projectile.owner >= Main.maxPlayers)
                Projectile.owner = Main.myPlayer;

            Player owner = Main.player[Projectile.owner];

            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.localAI[1] < 1f)
            {
                Projectile.localAI[1] += 0.08f;
                float t = EaseFunctions.EaseOutCubic(Projectile.localAI[1]);
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, t);
                Projectile.scale = MathHelper.Lerp(0.3f, 1f, t);
                Projectile.rotation += 0.4f;
                Lighting.AddLight(Projectile.Center, 1.6f * t, 0.8f * t, 0.2f * t);
                return;
            }

            Vector2 toPlayer = owner.Center - Projectile.Center;
            float dist = toPlayer.Length();

            if (dist < 40f)
            {
                Projectile.Kill();
                return;
            }

            float progress = MathHelper.Clamp(1f - dist / 1200f, 0f, 1f);
            float eased = EaseFunctions.EaseOutCubic(progress);
            float targetSpeed = MathHelper.Lerp(10f, 32f, eased);

            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                toPlayer.SafeNormalize(Vector2.Zero) * targetSpeed,
                0.12f
            );

            Projectile.rotation += 0.8f + eased * 1.4f;
            Lighting.AddLight(Projectile.Center, 1.4f, 0.7f, 0.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lavaTrail.Draw(Projectile);

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int k = 1; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2f;
                Color color = new Color(255, 180, 50, 0) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.6f;
                float scale = Projectile.scale * (1f - k / 15f);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
