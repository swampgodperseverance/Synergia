using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.Common.SynergiaCondition;

// Код был взят из мода RoA
public class SkinningDropCondition : IItemDropRuleCondition
{
    public bool CanDrop(DropAttemptInfo info) {
        if (!info.IsInSimulation) {
            return info.player.FindBuffIndex(ModList.Roa.Find<ModBuff>("Skinning").Type) != -1;
        }
        return false;
    }
    public bool CanShowItemDropInUI() => true;
    public string GetConditionDescription() => Language.GetTextValue("Mods.RoA.Conditions.TanningRack");
}