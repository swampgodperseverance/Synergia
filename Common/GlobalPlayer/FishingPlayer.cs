using Avalon.Items.Material;
using Avalon.Items.Other;
using Avalon.Items.Weapons.Melee.Hardmode.PossessedFlamesaw;
using Synergia.Content.Items.Weapons.Ranged;
using Synergia.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Common.GlobalPlayer
{
    public class FishingPlayer : ModPlayer
    {
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (attempt.inLava && attempt.CanFishInLava)
            {
                if (NPC.downedPlantBoss && attempt.rare)
                {
                    int rareChoice = Main.rand.Next(3);
                    switch (rareChoice)
                    {
                        case 0:
                            itemDrop = ItemType<UnderworldKey>();
                            break;
                        case 1:
                            itemDrop = ItemType<PossessedFlamesaw>();
                            break;
                        case 2:
                            itemDrop = ItemType<Lavinator>();
                            break;
                    }
                    return;
                }
                if (attempt.rare)
                {
                    itemDrop = ItemType<InfamousFlame>();
                    return;
                }
                itemDrop = ItemType<BottledLava>();
                return;
            }
        }
    }
}