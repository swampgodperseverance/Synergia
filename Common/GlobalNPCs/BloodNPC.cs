using Synergia.Common.GlobalPlayer;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs {
    public class BaseBloodHit {
        static void BaseHit(Player player, NPC target) {
            BloodPlayer bPlayer = player.GetModPlayer<BloodPlayer>();
            Item item = PlayerHelpers.GetLocalItem(player);
            if (bPlayer != null) {
                if (Lists.Items.WeaponActiveBlood.Contains(item.type)) {
                    if (target.type != NPCID.TargetDummy) {
                        if (!bPlayer.activeBloodBuff) {
                            if (bPlayer.currentHit != BloodPlayer.hitForActiveBloodBuff + 1) {
                                bPlayer.currentHit++;
                                bPlayer.timeLastHit = 0;
                            }
                        }
                    }
                }
            }
        }
        class GN : GlobalNPC {
            public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
                BaseHit(player, npc);
            }
        }
        class MP : ModPlayer {
            public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
                BaseHit(Player, target);
            }
        }
    }
}