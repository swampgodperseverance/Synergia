using System.Linq;
using Avalon.Items.Weapons.Magic.Hardmode.MagicGrenade;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.BloodAxe;
using ValhallaMod.Items.Weapons.Magic.Music;
using ValhallaMod.Items.Weapons.Ranged.Launchers;
using ValhallaMod.Items.Weapons.Summon;
using ValhallaMod.Items.Weapons.Summon.Auras;
using ValhallaMod.NPCs.Pirate;

namespace Synergia.Common.GlobalNPCs.Drops
{
    public class DropRemover : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<BootyMimic>())
            {
                var allRules = npcLoot.Get().ToList();
                foreach (var rule in allRules)
                {
                    npcLoot.Remove(rule);
                }

                int[] lootTable = new int[]
                {
                    ModContent.ItemType<MagicGrenade>(),
                    ModContent.ItemType<CoconutGun>(),
                    ModContent.ItemType<Blunderbass>(),
                    ModContent.ItemType<DeathmansCutter>(),
                    ModContent.ItemType<GreedAuraStaff>()
                };

                npcLoot.Add(ItemDropRule.OneFromOptions(1, lootTable));
            }
        }
    }
}