using Terraria;

namespace Synergia.Common.SynergiaCondition {
    public class RecipeCondition {
        static readonly bool c = false;
        public static Condition NULLitem(Item item) { 
            return new($"Check description item {item.Name}", () => c);
        }
    }
}