using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Synergia.Content.Buffs
{
    public class Ashen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }

    public class AshenGlobalNPC : GlobalNPC
    {
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<Ashen>()))
            {
                modifiers.SetCrit();
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<Ashen>()))
            {
                modifiers.SetCrit();
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(ModContent.BuffType<Ashen>()))
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegenExpectedLossPerSecond += 480; 

                if (damage < 80)
                {
                    damage = 80;
                }
            }
        }
    }
}
