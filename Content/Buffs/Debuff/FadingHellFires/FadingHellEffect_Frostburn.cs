using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Synergia.Common.GlobalItems.FadingHellData;

namespace Synergia.Content.Buffs.Debuff.FadingHellFires
{
    public sealed class FadingHellEffect_Frostburn : FadingHellEffect_Base
    {
        protected override FireType GetFireType() => FireType.Frostburn;
    }
}
