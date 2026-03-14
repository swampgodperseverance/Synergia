using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.Common.ModSystems.RecipeSystem {
    public struct RecipeGroupStruct(List<int> target, List<int> type, string recipeGroupName, int stack = 1) {
        public List<int> Target = target;
        public List<int> ItemType = type;
        public string RecipeGroupName = recipeGroupName;
        public int Stack = stack;
    }
}
