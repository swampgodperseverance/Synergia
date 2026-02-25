using System.Collections.Generic;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Garden;

namespace Synergia.Content.Items.Materials {
    public class CogBronze : ModItem {

            // Token: 0x06001434 RID: 5172 RVA: 0x000A3060 File Offset: 0x000A1260
            public override void SetDefaults()
            {
                base.Item.width = 18;
                base.Item.height = 26;
                base.Item.maxStack = 9999;
                base.Item.value = Item.sellPrice(0, 0, 2, 60);
                base.Item.rare = 1;
            }

            // Token: 0x06001435 RID: 5173 RVA: 0x000A30B8 File Offset: 0x000A12B8
            public override void AddRecipes()
            {
                base.CreateRecipe(2).AddIngredient<Sap>(2).AddIngredient<BronzeBar>(1).AddTile(16).Register();
            }
        
    }
}