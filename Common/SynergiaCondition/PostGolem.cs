using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Synergia.Common.SynergiaCondition;

public class PostGolem : IItemDropRuleCondition
{
    public bool CanDrop(DropAttemptInfo info) => !info.IsInSimulation && Terraria.NPC.downedGolemBoss;
    public bool CanShowItemDropInUI() => true;
    public string GetConditionDescription() => Language.GetTextValue("Conditions.DownedGolem");
}