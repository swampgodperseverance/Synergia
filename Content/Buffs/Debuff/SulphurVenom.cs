using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ID;

namespace Synergia.Content.Buffs.Debuff {
    public class SulphurVenom : BaseDebuff {
        public override string Texture => Reassures.Reassures.Blank;
        public override void Update(NPC npc, ref int buffIndex) {
            ArmorPlayers.SpawnBurst(npc.Center, DustID.Copper);
            npc.lifeRegen -= 10;
        }
    }
}