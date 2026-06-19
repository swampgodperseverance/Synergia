using System.Collections.Generic;
using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Accessories.PreHardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Ores;
using Avalon.Items.Material.Shards;
using Avalon.Items.Material.TomeMats;
using Avalon.Items.Weapons.Ranged.Hardmode.SunsShadow;
using Avalon.NPCs.Critters;
using Avalon.NPCs.Hardmode;
using Avalon.NPCs.PreHardmode;
using Bismuth.Content.NPCs;
using Consolaria.Content.NPCs;
using Microsoft.Xna.Framework.Input;
using NewHorizons.Content.Items.Materials;
using NewHorizons.Content.NPCs;
using Synergia.Common.SynergiaCondition;
using Synergia.Content.Items.Accessories;
using Synergia.Content.Items.ActiveAccessories;
using Synergia.Content.Items.Weapons.AuraStaff;
using Synergia.Content.Items.Weapons.Throwing;
using Synergia.Content.NPCs.Contagion;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Accessory.Active;
using ValhallaMod.Items.Accessory.Shield;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using ValhallaMod.Items.Weapons.Magic;
using ValhallaMod.Items.Weapons.Ranged.Bows;
using ValhallaMod.NPCs.Corruption;
using ValhallaMod.NPCs.Dungeon;
using ValhallaMod.NPCs.Frost;
using ValhallaMod.NPCs.Goblin;
using ValhallaMod.NPCs.Jungle;
using ValhallaMod.NPCs.Marble;
using ValhallaMod.NPCs.Snowman;
using ValhallaMod.NPCs.Tar;
using ValhallaMod.NPCs.Underground;
using ValhallaMod.NPCs.Underworld;
using ValhallaMod.NPCs.Zombies;
using static Synergia.ModList;
using static Terraria.ModLoader.ModContent;
using Gargoyle = ValhallaMod.NPCs.Granite.Gargoyle;

namespace Synergia.Common.GlobalNPCs.Drops {
    public class NPCLootsNew : GlobalNPC {

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

            if (npc.type == NPCID.Probe)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PostMechWings>(), 1000));
            }
            if (npc.type == NPCID.BloodNautilus)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodyNecklace>(), 10));
            }
            if (npc.type == NPCID.IlluminantBat)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IluminantBatbow>(), 20));
            }
            if (npc.type == ModContent.NPCType<ArchDemon>())
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<ForsakenRelic>(),  
                    20,                           
                    1,                              
                    1                             
                ));
            }
            if (npc.type == ModContent.NPCType<RadiantBones>() || npc.type == ModContent.NPCType<RadiantBones2>() || npc.type == ModContent.NPCType<RadiantBones3>() || npc.type == ModContent.NPCType<RadiantBones4>() || npc.type == ModContent.NPCType<Radiator>() || npc.type == ModContent.NPCType<Radiator2>())
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<ThunderShard>(),
                    chanceDenominator: 5,
                    minimumDropped: 1,
                    maximumDropped: 3
                ));
            }
            if (npc.type == ModContent.NPCType<SandWorm>())
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<BagOfSand>(),
                    chanceDenominator: 40,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.type == ModContent.NPCType<EvilNecromancer>())
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<CursedMirror>(),
                    chanceDenominator: 10,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
            if (npc.type == ModContent.NPCType<Ickslime>() || npc.type == ModContent.NPCType<TaintedSlime>())
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<StickyPad>(),
                    chanceDenominator: 15,
                    minimumDropped: 1,
                    maximumDropped: 1
                ));
            }
        }
    }
}