using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Thrower;
using Terraria.Audio;
using Terraria.ID;

namespace Synergia.Common.Players
{
    public class TrimarangPlayer : ModPlayer
    {
        public int comboCount;
        public int comboTimer;
        public bool doubleMode;
        public Projectile interfaceProj;
        

        private bool wasHoldingTrimarang;

        public override void Initialize()
        {
            comboCount = 0;
            comboTimer = 0;
            doubleMode = false;
            interfaceProj = null;
            wasHoldingTrimarang = false;
        }

        public override void ResetEffects()
        {
            // Сбрасываем комбо ТОЛЬКО когда игрок перестал держать Trimarang
            bool isHoldingTrimarang = Player.HeldItem.type == ItemID.Trimarang;
            
            if (wasHoldingTrimarang && !isHoldingTrimarang)
            {
                ResetCombo();
            }
            
            wasHoldingTrimarang = isHoldingTrimarang;
        }

        public override void PostUpdate()
        {
            if (doubleMode)
            {
                comboTimer++;
                if (comboTimer >= 120) 
                {
                    ResetCombo();
                }
            }

            if (Player.HeldItem.type == ItemID.Trimarang)
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
                    // Фиксированная позиция: ниже и левее игрока
                    Vector2 targetPos = Player.Center + new Vector2(-30, 50);
                    interfaceProj.Center = targetPos;
                    

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
            
            // Обновляем интерфейс сразу после увеличения комбо
            if (interfaceProj != null && interfaceProj.active && interfaceProj.ModProjectile is ThrowerInterface1 ui)
            {
                ui.SetFrame(comboCount);
            }
            
            if (comboCount >= 5)
            {
                comboCount = 5;
                doubleMode = true;
                comboTimer = 0;
                
                SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
                
                // Обновляем интерфейс для полного заряда
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