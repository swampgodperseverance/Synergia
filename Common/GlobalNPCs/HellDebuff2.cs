using Terraria;
using Terraria.ModLoader;
using Synergia.Content.Buffs;

namespace Synergia.Common.Players
{
    public class HellDebuffPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (!NPC.downedBoss3 && Player.ZoneUnderworldHeight)
            {
                Player.AddBuff(ModContent.BuffType<HellishAir>(), 2);
            }
        }
    }
}