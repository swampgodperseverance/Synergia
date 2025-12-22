using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Avalon.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ValhallaMod.Projectiles.AI;
using System;

namespace Synergia.Content.Projectiles.Thrower
{
    public class ThunderChakramProj : ValhallaGlaive
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            DrawOffsetX = 4;
            DrawOriginOffsetY = 0;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.alpha = 0;
            Projectile.penetrate = 106;
            Projectile.scale = 0.9f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;

            bounces = 4;
            timeFlying = 20;
            speedHoming = 17f;
            speedFlying = 17f;
            speedComingBack = 28f;
            homingDistanceMax = 200f;
            homingStyle = HomingStyle.BasicGlaive;
            homingStart = true;
            homingIgnoreTile = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
          
            target.AddBuff(ModContent.BuffType<Electrified>(), 180);

            for (int i = 0; i < 12; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(2.5f, 2.5f);

                Dust dust = Dust.NewDustPerfect(
                    target.Center,
                    DustID.GemTopaz,
                    velocity,
                    150,
                    default,
                    1.1f
                );

                dust.noGravity = true;
            }
        }
        public override void AI()
        {
            base.AI();


            if (Main.rand.NextBool(2)) 
            {
                Vector2 offset = Main.rand.NextVector2Circular(4f, 4f);

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    DustID.GemTopaz,
                    -Projectile.velocity * 0.2f,
                    150,
                    default,
                    0.9f
                );

                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Dust spark = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GemTopaz,
                    Main.rand.NextVector2Circular(1.5f, 1.5f),
                    120,
                    default,
                    0.8f
                );

                spark.noGravity = true;
            }
        }



        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float progress = 1f - i / (float)Projectile.oldPos.Length;

                Color color = Color.MediumPurple * progress * 0.6f;

                Main.spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0f
                );
            }

            return true; // рисуем сам проджектайл стандартно
        }
    }
}
