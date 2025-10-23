using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class SeabornSword2 : ModProjectile
    {
        private float appearProgress = 0f;
        private bool hasAppeared = false;
        private const int appearDuration = 60; // немного быстрее проявление
        private int timer = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // увеличим длину трейла
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 64;
            Projectile.width = 22;
            Projectile.height = 48;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            timer++;

            if (!hasAppeared)
            {
                appearProgress = MathHelper.Clamp(timer / (float)appearDuration, 0f, 1f);
                Projectile.alpha = (int)(255 * (1f - appearProgress));

                // плавное появление
                if (Main.rand.NextBool(6))
                {
                    int dust = Dust.NewDust(Projectile.Center - new Vector2(8), 16, 16,
                        DustID.DungeonWater, Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-1f, 0f),
                        150, new Color(120, 200, 255), 1f);
                    Main.dust[dust].velocity *= 0.5f;
                    Main.dust[dust].noGravity = true;
                }

                // когда полностью проявился — начинаем падение
                if (appearProgress >= 1f)
                {
                    hasAppeared = true;
                    Projectile.velocity = new Vector2(0, 14f); // быстрое падение
                    SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/Watersound"), Projectile.Center);
                }
            }
            else
            {
                // резкое падение
                Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, 22f, 0.15f);

                // всегда смотрит вниз
                Projectile.rotation = MathHelper.PiOver2;

                // мягкий голубой свет
                Lighting.AddLight(Projectile.Center, new Vector3(0.2f, 0.4f, 0.8f));

                // лёгкий след пузырьков
                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.DungeonWater, 0f, 1.2f, 150, new Color(80, 160, 255), 1f);
                    Main.dust[dust].velocity *= 0.3f;
                    Main.dust[dust].noGravity = true;
                }
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            // Трейл строго за мечом
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(
                    texture,
                    Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition,
                    null,
                    drawColor * trailAlpha * 0.6f,
                    MathHelper.PiOver2,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0);
            }

            // Основная отрисовка меча
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                MathHelper.PiOver2,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.DungeonWater, Projectile.oldVelocity.X * 0.3f, Projectile.oldVelocity.Y * 0.3f);
                Main.dust[dust].velocity *= 2.5f;
                Main.dust[dust].noGravity = true;
            }

            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/Watersound"), Projectile.Center);
        }
    }
}
