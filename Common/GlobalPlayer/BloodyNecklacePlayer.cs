using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Vanilla.Content.Buffs;

namespace Vanilla.Common.GlobalPlayer
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
                // Бесконечно продлеваем бафф, пока в ауре
                Player.AddBuff(ModContent.BuffType<BloodDodge>(), 2); // 2 тика (обновляется каждый кадр)
            }
            else if (wasInAura) // Если только что вышел из ауры
            {
                // Убираем бафф при выходе
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
            if (Player.HasBuff(ModContent.BuffType<BloodDodge>()))
            {
                if (Main.rand.NextFloat() <= 0.10f) // 10% шанс контрудара
                {
                    if (info.DamageSource.TryGetCausingEntity(out Entity attacker) && attacker is NPC npc)
                    {
                        int damageToReturn = info.Damage * 5;
                        npc.SimpleStrikeNPC(damageToReturn, 0);

                        // Кровотечение только после контрудара (на 5 секунд)
                        Player.AddBuff(BuffID.Bleeding, 300); // 300 тиков = 5 секунд

                        // Сообщение при срабатывании
                        CombatText.NewText(Player.getRect(), Color.Red, "Blood Pact!");
                    }
                }
            }
        }
    }
}