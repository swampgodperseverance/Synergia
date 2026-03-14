using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Global
{
    public class BismuthToMeleeGlobal : GlobalItem
    {
        private static readonly HashSet<string> ToMelee = new(StringComparer.OrdinalIgnoreCase)
        {
            "SoulEater",
            "Baselard",
            "Sting",
            "SnakesFang",
            "Angrist",
            "Misericorde",
            "Breakwater",
            "ManGosh",
            "SacrificialDagger",
            "Stiletto",
            "Whirlpool",
            "AluminumBlade",
                        "Parazonium",
        };

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem != null
                && entity.ModItem.Mod.Name == "Bismuth"  
                && ToMelee.Contains(entity.ModItem.Name);
        }

        public override void SetDefaults(Item item)
        {
            item.DamageType = DamageClass.Melee;
        }
    }
}