using Synergia.Common.GlobalPlayer;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.GlobalNPCs {
    public class BaseBloodHit {
        static void BaseHit(Player player, NPC target) {
            BloodPlayer bPlayer = player.GetModPlayer<BloodPlayer>();
            Item item = PlayerHelpers.GetLocalItem(player);
            if (bPlayer != null) {
                if (Lists.Items.WeaponActiveBlood.Contains(item.type)) {
                    if (player.GetModPlayer<DebugPlayer>().DebugMod) {
                        if (bPlayer.currentHit != BloodPlayer.hitForActiveBloodBuff + 1) {
                            bPlayer.currentHit++;
                            bPlayer.timeLastHit = 0;
                        }
                        if (bPlayer.Tir10Buffs) {
                            int amount = Main.rand.Next(1, 6);
                            player.Heal(amount);
                        }
                    }
                    else {
                        if (target.type != NPCID.TargetDummy) {
                            if (bPlayer.currentHit != BloodPlayer.hitForActiveBloodBuff + 1) {
                                bPlayer.currentHit++;
                                bPlayer.timeLastHit = 0;
                            }
                            if (bPlayer.Tir10Buffs) {
                                int amount = Main.rand.Next(1, 6);
                                player.Heal(amount);
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