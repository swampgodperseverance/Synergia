using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Synergia.Helpers;
using Avalon.Particles;
using System;
using Terraria.DataStructures;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class GloomRingAttack : ModProjectile
    {
        private const float ShrinkDuration = 60f;
        private bool exploded = false;

        private Vector2 offsetFromPlayer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = (int)ShrinkDuration + 10;
            Projectile.alpha = 0;
            Projectile.scale = 0.8f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            offsetFromPlayer = Projectile.Center - player.Center;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Проверяем, что игрок жив
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // === Привязка движения ===
            // Центр проджектайла = позиция игрока + запомненное смещение
            Projectile.Center = player.Center + offsetFromPlayer;

            Projectile.rotation += 0.05f;

            float progress = 1f - (Projectile.timeLeft / ShrinkDuration);
            progress = MathHelper.Clamp(progress, 0f, 1f);
            Projectile.scale = MathHelper.Lerp(1.2f, 0.2f, progress);

            Lighting.AddLight(Projectile.Center, 0.08f, 0.15f, 0.3f);

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
                d.velocity = Main.rand.NextVector2Circular(2f, 2f);
                d.scale = Main.rand.NextFloat(0.8f, 1.3f);
                d.noGravity = true;
                d.fadeIn = 1.1f;
            }

            if (!exploded && Projectile.scale <= 0.25f)
            {
                exploded = true;
                Explode(player);
            }
        }

        private void Explode(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            // Голубой энергетический всплеск
            for (int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
                d.velocity = Main.rand.NextVector2Circular(4f, 4f);
                d.scale = Main.rand.NextFloat(1f, 1.5f);
                d.noGravity = true;
            }

            ParticleSystem.AddParticle(
                new EnergyRevolverParticle(),
                Projectile.Center,
                Vector2.Zero,
                new Color(80, 160, 255),
                0,
                1.2f,
                25
            );

            // Направление к мыши
            Vector2 toMouse = Main.MouseWorld - Projectile.Center;
            Vector2 direction = toMouse.SafeNormalize(Vector2.UnitX);

            // Выпуск 5–6 GloomProj1
            int count = Main.rand.Next(5, 7);
            for (int i = 0; i < count; i++)
            {
                Vector2 perturbedDir = direction.RotatedByRandom(MathHelper.ToRadians(12f));
                Vector2 velocity = perturbedDir * Main.rand.NextFloat(10f, 14f);

                Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromAI(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<GloomProj1>(),
                    Projectile.damage,
                    2f,
                    player.whoAmI
                );
            }

            // Если игрок имеет бафф GloomBuff, выпускаем 6 дополнительных GloomProj2
            if (player.HasBuff(ModContent.BuffType<Buffs.GloomBuff>()))
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 extraDir = direction.RotatedByRandom(MathHelper.ToRadians(20f));
                    Vector2 velocity = extraDir * Main.rand.NextFloat(8f, 12f);

                    Projectile.NewProjectileDirect(
                        Projectile.GetSource_FromAI(),
                        Projectile.Center,
                        velocity,
                        ModContent.ProjectileType<GloomProj2>(),
                        Projectile.damage,
                        2f,
                        player.whoAmI
                    );
                }
            }

            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float trailAlpha = ((float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length) * 0.6f;
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float scale = Projectile.scale * ((float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length * 0.5f + 0.5f);

                Color trailColor = new Color(100, 180, 255) * trailAlpha;
                Main.EntitySpriteDraw(
                    texture,
                    pos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            Color ringColor = new Color(100, 180, 255) * (1f - Projectile.alpha / 255f);
            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                ringColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override bool? CanDamage() => false;
    }
}
