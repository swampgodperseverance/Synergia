using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class ThrowerInterface1 : ModProjectile
    {
        private const int MaxFrame = 5;
        private const int ChargeDuration = 120;
        private int chargeTimer;
        private bool fullyCharged;

        private float visibility; // 0 = невидим, 1 = полностью видно

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6; // 6 кадров: 0–5 заполненных клинков
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.scale = 0.88f;
            Projectile.friendly = false;
            Projectile.timeLeft = 2;
            Projectile.alpha = 255; // Начинаем с полной прозрачности
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // === ПРОВЕРКИ ДЛЯ РАЗНЫХ ОРУЖИЙ ===
            bool isSnowball = player.HeldItem.type == ItemID.Snowball ||
                              (player.HeldItem.ModItem?.Name?.Contains("Snowball", StringComparison.OrdinalIgnoreCase) == true);

            bool isStalloyScrew = player.HeldItem.ModItem != null &&
                                  player.HeldItem.ModItem.Mod?.Name == "ValhallaMod" &&
                                  player.HeldItem.ModItem.Name == "StalloyScrew";

            bool isTrimarang = player.HeldItem.type == ItemID.Trimarang ||
                              (player.HeldItem.ModItem?.Name?.Contains("Trimarang", StringComparison.OrdinalIgnoreCase) == true);
            bool holdingWeapon = player.active && !player.dead && (isSnowball || isStalloyScrew || isTrimarang);

            // === ЛОГИКА ИНТЕРФЕЙСА ===
            if (!holdingWeapon)
            {
                visibility = MathHelper.Lerp(visibility, 0f, 0.1f);
                if (visibility < 0.05f)
                    Projectile.Kill();
            }
            else
            {
                visibility = MathHelper.Lerp(visibility, 1f, 0.1f);

                // Разные позиции для разных оружий
                Vector2 targetPos = isSnowball 
                    ? player.Center + new Vector2(-27, 50) 
                    : player.Center + new Vector2(-27, 50);

                Projectile.Center = targetPos;
                Projectile.position.Y += (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.5f;
                Projectile.timeLeft = 2;

                if (fullyCharged)
                {
                    chargeTimer++;
                    if (chargeTimer >= ChargeDuration)
                    {
                        fullyCharged = false;
                        Projectile.frame = 0;
                        chargeTimer = 0;
                    }
                    else
                    {
                        Projectile.position += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    }
                }
            }

            Projectile.alpha = (int)MathHelper.Lerp(255, 0, visibility);
        }


        public void SetFrame(int frame)
        {
            if (frame < 0) frame = 0;
            if (frame > MaxFrame) frame = MaxFrame;

            if (frame == MaxFrame)
            {
                Projectile.frame = MaxFrame;
                fullyCharged = true;
                chargeTimer = 0;
            }
            else
            {
                if (fullyCharged)
                {
                    fullyCharged = false;
                    chargeTimer = 0;
                }

                Projectile.frame = frame;
            }
        }
    }
}
