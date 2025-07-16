using Terraria;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Microsoft.Xna.Framework;

namespace Vanilla.Common.GlobalPlayer
{
    public class MirrorOfTheLostPlayer : ModPlayer
    {
        public bool mirrorEquipped = false;

        public override void ResetEffects()
        {
            mirrorEquipped = false;
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (!mirrorEquipped)
                return;

            bool isInAura = IsInValhallaAura(Player);

            if (isInAura)
                damage += 0.20f; // +20
            else
                damage -= 0.25f; // -25% 
        }

        private bool IsInValhallaAura(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI &&
                    proj.ModProjectile is AuraAI aura &&
                    Vector2.Distance(player.Center, proj.Center) <= aura.distanceMax)
                {
                    return true;
                }
            }
            return false;
        }
    }
}