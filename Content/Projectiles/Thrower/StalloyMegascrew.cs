using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Thrower
{
    public class StalloyMegascrew : JavelinAI
    {
        private static readonly SoundStyle StalloyCrashSound = new SoundStyle("ValhallaMod/Sounds/StalloyCrash")
        {
            Volume = 0.9f,
            PitchVariance = 0.3f
        };

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            stickingJavelins = new Point[3];
        }

        public override void AI()
        {
            base.AI();

            // Сохраняем текущий поворот в oldRot для правильного трэйл
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            // Если oldRot не хватает, инициализируем
            if (Projectile.oldRot.Length != ProjectileID.Sets.TrailCacheLength[Projectile.type])
                Projectile.oldRot = new float[ProjectileID.Sets.TrailCacheLength[Projectile.type]];
            // Сдвигаем старые повороты
            for (int i = Projectile.oldRot.Length - 1; i > 0; i--)
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
            Projectile.oldRot[0] = Projectile.rotation;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(StalloyCrashSound, Projectile.position);

            Vector2 usePos = Projectile.position;
            Vector2 rotVector = Utils.ToRotationVector2(Projectile.rotation - MathHelper.ToRadians(90f));
            usePos += rotVector * 16f;

            for (int i = 0; i < 5; i++)
                Dust.NewDust(usePos, Projectile.width, Projectile.height, DustID.Iron, 0f, 0f, 0, default, 1.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new(texture.Width / 2, texture.Height / 2);
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // --- ТРЕЙЛ с правильным поворотом ---
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color trailColor = lightColor * 0.4f * fade;
                float scale = Projectile.scale * (0.9f + 0.1f * fade);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.oldRot[i], // теперь используем сохранённый поворот
                    origin,
                    scale,
                    effects,
                    0
                );
            }

            // Основное тело
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }
    }
}
