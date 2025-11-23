using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Friendly;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public class MagicClawRework : ModProjectile
    {
        private const float MaxSpeed = 12f;    
        private float speedProgress = 0f;     

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 25;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            
            speedProgress += 0.015f; 
            speedProgress = MathHelper.Clamp(speedProgress, 0f, 1f);
            float easedSpeed =      Helpers.EaseFunctions.EaseInOutSine(speedProgress); 
            Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MaxSpeed * easedSpeed;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }
            Projectile.rotation = (float)Math.Atan2(-Projectile.velocity.Y, -Projectile.velocity.X);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle sourceRectangle = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color color = Color.White * fade * 0.6f;
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }


        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(2f, 2f);
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>(), speed, 150, default, 1.2f);
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(2.5f, 2.5f);
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>(), speed, 120, default, 1.3f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Vector2 spawnPos = target.Center;
                Vector2 direction = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, -1f));
                Projectile.NewProjectile(
                    Projectile.GetSource_OnHit(target),
                    spawnPos,
                    direction,
                    ModContent.ProjectileType<WaternadoFriendly>(),
                    Projectile.damage / 2,
                    0f,
                    Projectile.owner
                );
            }
        }
    }
}
