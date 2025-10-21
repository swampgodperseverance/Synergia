using Synergia.Content.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Other
{
    public class BookProj : TransformProj
    {
        public override int FirstTypeOfDust => DustID.CrimsonTorch;
        public override int SecondTypeOfDust => DustID.HallowedTorch;
        public override int TransformsInItemType => ModContent.ItemType<OldTales2>();
    }
}