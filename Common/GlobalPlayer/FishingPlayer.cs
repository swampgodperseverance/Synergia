using Synergia.Content.Items.Weapons.Ranged;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Common.GlobalPlayer {
    public class FishingPlayer : ModPlayer {
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition) {
            if (Player.GetModPlayer<BiomePlayer>().lakeBiome && NPC.downedPlantBoss && attempt.inLava) {
                if (!attempt.CanFishInLava) { return; }
                if (attempt.rare) { itemDrop = ItemType<Lavinator>(); } // rare? => veryrare
            }
        }
    }
}