using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Buffs
{
    public class BansheeCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {

            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }

    public class BansheeCursePlayer : ModPlayer
    {
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Player.HasBuff(ModContent.BuffType<BansheeCurse>()))
            {
                modifiers.FinalDamage *= 3f;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (Player.HasBuff(ModContent.BuffType<BansheeCurse>()))
            {
                modifiers.FinalDamage *= 3f;
            }
        }
    }
}
