using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Synergia.Content.Projectiles.Thrower;
using Terraria.Audio;

namespace Synergia.Common.Players
{
    public class StalloyScrewPlayer : ModPlayer
    {
        public int comboCount;
        public int comboTimer;
        public bool doubleMode;
        public Projectile interfaceProj;

        private bool wasHoldingScrew;

        public override void Initialize()
        {
            comboCount = 0;
            comboTimer = 0;
            doubleMode = false;
            interfaceProj = null;
            wasHoldingScrew = false;
        }

        public override void ResetEffects()
        {
            bool isHoldingScrew = Player.HeldItem.ModItem != null &&
                                  Player.HeldItem.ModItem.Mod?.Name == "ValhallaMod" &&
                                  Player.HeldItem.ModItem.Name == "StalloyScrew";

            if (wasHoldingScrew && !isHoldingScrew)
            {
                ResetCombo();
            }

            wasHoldingScrew = isHoldingScrew;
        }

        public override void PostUpdate()
        {
            if (doubleMode)
            {
                comboTimer++;
                if (comboTimer >= 120)
                    ResetCombo();
            }

            bool isHoldingScrew = Player.HeldItem.ModItem != null &&
                                  Player.HeldItem.ModItem.Mod?.Name == "ValhallaMod" &&
                                  Player.HeldItem.ModItem.Name == "StalloyScrew";

            if (isHoldingScrew)
            {
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
                    // Обновляем позицию
                    Vector2 targetPos = Player.Center + new Vector2(-30, 50);
                    interfaceProj.Center = targetPos;

                    // Обновляем кадр интерфейса **каждый тик**
                    if (interfaceProj.ModProjectile is ThrowerInterface1 ui)
                    {
                        ui.SetFrame(comboCount);
                    }
                }
            }

        }
        public void AddCombo()
        {
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
