using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace Vanilla.Common.GlobalItems
{
    public class VanillaThrowingConverter : GlobalItem
    {
        private static readonly HashSet<int> VanillaBoomerangs = new()
        {
            ItemID.EnchantedBoomerang,
            ItemID.WoodenBoomerang,
            ItemID.IceBoomerang,
            ItemID.FruitcakeChakram,
            ItemID.ThornChakram,
            ItemID.BloodyMachete,
            ItemID.Flamarang,
            ItemID.LightDisc,
            ItemID.Bananarang,
            ItemID.PossessedHatchet,
            ItemID.ScourgeoftheCorruptor,
            ItemID.Anchor,
            ItemID.Trimarang,
            ItemID.Shroomerang
        };

        public override void SetDefaults(Item item)
        {
            if (VanillaBoomerangs.Contains(item.type))
            {
                item.DamageType = DamageClass.Throwing;
            }
        }
    }
}
    