// Code by 𝒜𝑒𝓇𝒾𝓈
using Synergia.Common.GlobalItems.Changes;
using Terraria;

namespace Synergia.Common.ModSystems.RecipeSystem.ChangesRecipe.AvalonsChanges {
    public partial class Avalons {
        public class Avalon_Item : BaseItem {
            public override void EditArmor(Item entity) {
                EditArmor(entity, EarthsplitterChestpiece, 12);
                EditArmor(entity, EarthsplitterHelm, 10);
            }
        }
    }
}