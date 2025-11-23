using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Synergia.Content.Projectiles.Thrower;
using Terraria.Audio;

namespace Synergia.Common.Players
{
    public class SnowballPlayer : ModPlayer
    {
        public int comboCount;
        public int comboTimer;
        public bool doubleMode;
        public Projectile interfaceProj;

        private bool wasHoldingSnowball;

        public override void Initialize()
        {
            comboCount = 0;
            comboTimer = 0;
            doubleMode = false;
            interfaceProj = null;
            wasHoldingSnowball = false;
        }

        public override void ResetEffects()
        {
            // ✅ Проверяем, держит ли игрок снежок (ванильный или модовый)
            bool isHoldingSnowball =
                Player.HeldItem != null &&
                (Player.HeldItem.type == ItemID.Snowball ||
                 Player.HeldItem.ModItem?.Name?.Contains("Snowball", System.StringComparison.OrdinalIgnoreCase) == true);

            // Если перестал держать снежок — сбрасываем комбо
            if (wasHoldingSnowball && !isHoldingSnowball)
            {
                ResetCombo();
            }

            wasHoldingSnowball = isHoldingSnowball;
        }

        public override void PostUpdate()
        {
            if (doubleMode)
            {
                comboTimer++;
                if (comboTimer >= 120)
                    ResetCombo();
            }

            bool isHoldingSnowball =
                Player.HeldItem != null &&
                (Player.HeldItem.type == ItemID.Snowball ||
                 Player.HeldItem.ModItem?.Name?.Contains("Snowball", System.StringComparison.OrdinalIgnoreCase) == true);

            if (isHoldingSnowball)
            {
                // ✅ Создаём или обновляем интерфейс-проектайл (ThrowerInterface1)
                if (interfaceProj == null || !interfaceProj.active)
                {
                    int p = Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        Player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<ThrowerInterface1>(),
                        0, 0, Player.whoAmI
                    );
                    interfaceProj = Main.projectile[p];
                }

                if (interfaceProj != null && interfaceProj.active)
                {
                    // Обновляем позицию интерфейса
                    Vector2 targetPos = Player.Center + new Vector2(-30, 50);
                    interfaceProj.Center = targetPos;

                    // Обновляем кадр интерфейса
                    if (interfaceProj.ModProjectile is ThrowerInterface1 ui)
                    {
                        ui.SetFrame(comboCount);
                    }
                }
            }
        }

        public void AddCombo()
        {   
            if (doubleMode) return;
            comboTimer = 0;
            comboCount++;
  
            if (interfaceProj != null && interfaceProj.active && interfaceProj.ModProjectile is ThrowerInterface1 ui)
                ui.SetFrame(comboCount);

            if (comboCount >= 5)
            {
                comboCount = 5;
                doubleMode = true;
                comboTimer = 0;
                SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);

                if (interfaceProj != null && interfaceProj.active && interfaceProj.ModProjectile is ThrowerInterface1 uiFinal)
                    uiFinal.SetFrame(5);
            }
        }

        public void ResetCombo()
        {
            comboCount = 0;
            doubleMode = false;
            comboTimer = 0;

            if (interfaceProj != null && interfaceProj.active && interfaceProj.ModProjectile is ThrowerInterface1 ui)
            {
                ui.SetFrame(0);
                SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/ThrowingFail"));
            }
        }
    }
}
