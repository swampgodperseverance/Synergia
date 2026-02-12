using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Items.Misc;
using Terraria.DataStructures;

namespace Synergia.Content.Items.Misc
{
    public class HeliosSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = 6;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight((int)(((double)this.Item.position.X + (double)(this.Item.width / 2)) / 16.0), (int)(((double)this.Item.position.Y + (double)(this.Item.height / 2)) / 16.0), 0.6f, 0.78f, 0.75f);
        }
    }
}