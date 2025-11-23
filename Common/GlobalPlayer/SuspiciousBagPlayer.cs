using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ValhallaMod;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Items.Accessories
{
    public class SuspiciousBagPlayer : ModPlayer
    {
        public bool bagEquipped;
        private bool wasInAura = false;

        public override void ResetEffects()
        {
            bagEquipped = false;
        }

        public override void PostUpdate()
        {
            if (!bagEquipped)
                return;

            bool isInAura = IsInValhallaAura(Player);

            if (isInAura)
            {
                var auraPlayer = Player.GetModPlayer<AuraPlayer>();

                auraPlayer.bonusPlayerLinkedAuraRadius += 0.40f;

                Player.buffImmune[BuffID.Slow] = true;

                if (!wasInAura)
                {
                    SoundEngine.PlaySound(SoundID.Item16 with
                    {
                        Pitch = -0.4f,
                        Volume = 1.5f
                    }, Player.Center);
                }
            }

            wasInAura = isInAura;
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
