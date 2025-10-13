using Terraria;
using Terraria.ModLoader;
using Bismuth.Utilities;
using Synergia.Content.Buffs;

namespace Synergia.Common.GlobalPlayer
{
    public class BansheeHeadSynergyPlayer : ModPlayer
    {
        private int curseLockTimer = 0;

        public override void PostUpdate()
        {
            var bismuthPlayer = Player.GetModPlayer<BismuthPlayer>();

            if (bismuthPlayer.IsEquippedBansheesHead)
            {
                curseLockTimer = 60 * 90; 
                Player.AddBuff(ModContent.BuffType<BansheeCurse>(), curseLockTimer);
            }
            else if (curseLockTimer > 0)
            {
                bismuthPlayer.IsEquippedBansheesHead = true;
            }

            if (curseLockTimer > 0)
                curseLockTimer--;
        }
    }
}
