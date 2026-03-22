using Avalon.Items.Weapons.Magic.Hardmode.DevilsScythe;
using Avalon.Items.Weapons.Magic.Hardmode.FreezeBolt;
using Avalon.Items.Weapons.Magic.Hardmode.Outbreak;
using Avalon.Items.Weapons.Magic.PreHardmode.ChaosTome;
using Avalon.Items.Weapons.Magic.PreHardmode.FrozenLyre;
using Avalon.Items.Weapons.Magic.PreHardmode.GlacierStaff;
using Consolaria.Content.Items.Weapons.Magic;
using NewHorizons.Content.Items.Weapons.Magic;
using NVorbis;
using Synergia.Content.Buffs.Debuff.FadingHellFires;
using Synergia.Content.Items.Weapons.Cogworm;
using Synergia.Content.Items.Weapons.Mage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using ValhallaMod.Items.Weapons.Magic.Gloves;
using ValhallaMod.Items.Weapons.Magic.Lanterns;
using ValhallaMod.Items.Weapons.Magic.Staffs;
using ValhallaMod.Items.Weapons.Magic.Tomes;

namespace Synergia.Common.GlobalItems
{
    public class FadingHellData
    {
        public enum FireType
        {
            None,
            OnFire,
            CursedInferno,
            Frostburn,
            Shadowflame
        }
        public struct FadingHellFireData
        {
            public int DustID;
            public int VanillaDebuffID;
            public int ModdedDebuffID;
            public Color Color;
        }

        internal static FadingHellFireData[] data = new FadingHellFireData[] {
            new FadingHellFireData { DustID = DustID.Torch, VanillaDebuffID = BuffID.OnFire, ModdedDebuffID = ModContent.BuffType<FadingHellEffect_OnFire>(),   Color = new Color(250, 95, 5, 127)},
            new FadingHellFireData { DustID = DustID.CursedTorch, VanillaDebuffID = BuffID.CursedInferno, ModdedDebuffID = ModContent.BuffType<FadingHellEffect_CursedInferno>(),   Color = new Color(58, 250, 5, 127)},
            new FadingHellFireData { DustID = DustID.IceTorch, VanillaDebuffID = BuffID.Frostburn2, ModdedDebuffID = ModContent.BuffType<FadingHellEffect_Frostburn>(),   Color = new Color(5, 217, 250, 127)},
            new FadingHellFireData { DustID = DustID.Shadowflame, VanillaDebuffID = BuffID.ShadowFlame, ModdedDebuffID = ModContent.BuffType<FadingHellEffect_Shadowflame>(),   Color = new Color(181, 69, 255, 127)}
        };

        internal static readonly List<List<int>> SuitableItem = new List<List<int>>()
        {
            new(){ ItemID.WandofSparking, ItemID.FlowerofFire, ItemID.Flamelash, ItemID.InfernoFork, ModContent.ItemType<DevilsScythe>(), ModContent.ItemType<ScorcherRequiem>(), ModContent.ItemType<Menace>(), ModContent.ItemType<Scorcher>() },
            new(){ ItemID.CursedFlames, ItemID.ClingerStaff, ModContent.ItemType<Outbreak>(), ModContent.ItemType<OcramsEye>()  },
            new(){ ItemID.FlowerofFrost, ItemID.WandofFrosting, ItemID.FrostStaff, ModContent.ItemType<HandCooler>(), ModContent.ItemType<FrostBowlingStaff>(),ModContent.ItemType<FreezeBolt>(), ModContent.ItemType<GlacierStaff>(),ModContent.ItemType<FrozenLyre>()  },
            new(){ ItemID.UnholyTrident, ItemID.ShadowFlameHexDoll, ModContent.ItemType<Unlighter>(), ModContent.ItemType<DarkVolley>(), ModContent.ItemType<ShadowBolt>(), ModContent.ItemType<ChaosTome>() }
        };
        public static FadingHellFireData? GetFireData(FireType fireType)
        {
            if (fireType == FireType.None)
                return null;
            return data[(int)fireType - 1];
        }
        public static bool IsSuitableItem(Item item, ref FireType type)
        {
            for(int i = 0; i < SuitableItem.Count; i++)
            {
                if (!SuitableItem[i].Contains(item.type))
                    continue;
                type = (FireType)(i + 1);
                return true;
            }
            type = FireType.None;
            return false;
        }
    }
}
