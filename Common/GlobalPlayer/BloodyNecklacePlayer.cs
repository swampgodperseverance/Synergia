using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Synergia.Content.Buffs;

namespace Synergia.Common.GlobalPlayer
{
    public class BloodyNecklacePlayer : ModPlayer
    {
        public bool bloodyNecklaceEquipped = false;
        private bool wasInAura = false;

        public override void ResetEffects()
        {
            bloodyNecklaceEquipped = false;
        }

        public override void PostUpdate()
        {
            if (!bloodyNecklaceEquipped)
                return;

            bool isInAura = IsInValhallaAura(Player);

            if (isInAura)
            {
                Player.AddBuff(ModContent.BuffType<BloodDodge>(), 2);
            }
            else if (wasInAura)
            {
                Player.ClearBuff(ModContent.BuffType<BloodDodge>());
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

        public override void OnHurt(Player.HurtInfo info)
        {
            if (!Player.HasBuff(ModContent.BuffType<BloodDodge>()))
                return;

            if (Main.rand.NextFloat() <= 0.10f) 
            {
                if (info.DamageSource.TryGetCausingEntity(out Entity attacker) && attacker is NPC npc)
                {
                    int damageToReturn = info.Damage * 5;
                    npc.SimpleStrikeNPC(damageToReturn, 0);

                    Player.AddBuff(BuffID.Bleeding, 300);

                    CombatText.NewText(Player.getRect(), Color.Red, "Blood Pact!");
                }
            }
        }
    }
}
