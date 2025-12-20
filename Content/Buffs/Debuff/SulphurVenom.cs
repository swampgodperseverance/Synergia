using Terraria;
using Terraria.ID;

namespace Synergia.Content.Buffs.Debuff {
    public class SulphurVenom : DebuffBase {
        public override string Texture => Synergia.Blank;
        public override void SetStaticDefaults() {
            base.SetStaticDefaults();
        }
        public override void Update(NPC npc, ref int buffIndex) {
            if (Main.rand.NextBool(2)) {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Copper);
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust.velocity *= 0.3f;
            }
            if (Main.rand.NextBool(3)) {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Copper);
                dust.noGravity = true;
                dust.scale = 1.4f;
                dust.velocity *= 0.3f;
            }
            npc.lifeRegen -= 10;
        }
    }
}