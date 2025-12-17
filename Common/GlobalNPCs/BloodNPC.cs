using Synergia.Common.GlobalPlayer;
using Synergia.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs {
    public class BloodGN : GlobalNPC {
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
            BaseHit(player, npc);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) { // убери єто если снаряды не нужны
            if (projectile.owner >= 0 && projectile.owner < Main.maxPlayers) {
                Player owner = Main.player[projectile.owner];
                if (owner != null) {
                    BaseHit(owner, npc);
                }
            }
        }
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
    }
}