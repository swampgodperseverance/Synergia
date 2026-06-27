using Avalon.Items.Material;
using Avalon.Items.Other;
using Avalon.Items.Weapons.Melee.Hardmode.PossessedFlamesaw;
using Synergia.Content.Items.Weapons.Ranged;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Common.GlobalPlayer {
    public class FishingPlayer : ModPlayer {
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition) {
            if (attempt.inLava && attempt.CanFishInLava) {
                if (attempt.rare) {
                    if (NPC.downedPlantBoss) {
                        switch (Main.rand.Next(5)) {
                            case 0: itemDrop = ItemType<UnderworldKey>(); break;
                            case 1: itemDrop = ItemType<PossessedFlamesaw>(); break;
                            case 2: itemDrop = ItemType<Lavinator>(); break;
                            case 3: itemDrop = ItemType<InfamousFlame>(); break;
                        }
                        return;
                    }
                    if (Main.rand.Next(2) == 0) { itemDrop = ItemType<InfamousFlame>(); }
                    return;
                }
                else {
                    if (Main.rand.Next(4) == 0) { itemDrop = ItemType<BottledLava>(); }
                    return;
                }
            }
        }
    }
}