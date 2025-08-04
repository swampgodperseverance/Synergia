using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace Vanilla.Common.GlobalNPCs.Drops
{
    public sealed class SkinningNPC : GlobalNPC
    {
        private static readonly HashSet<string> AllowedModdedNPCs = new()
        {
            "Mods.Avalon.NPCs.Mime.DisplayName",
            "Mods.Avalon.NPCs.ContaminatedGhoul.DisplayName",
            "Mods.Avalon.NPCs.ContaminatedBunny.DisplayName",
            "Mods.Consolaria.NPCs.GiantAlbinoCharger.DisplayName",
            "Mods.Consolaria.NPCs.AlbinoCharger.DisplayName",
            "Mods.Consolaria.NPCs.AlbinoAntlion.DisplayName",
            "Mods.Consolaria.NPCs.ShadowMummy.DisplayName",
            "Mods.Consolaria.NPCs.FleshMummy.DisplayName",
            "Mods.Consolaria.NPCs.VampireMiner.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieUnicorn.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieUmbrella.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieUmbrella2.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieUmbrella3.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieTactical2.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieLibrarian.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieNinja.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieBucket.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieBalloon.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieBalloon2.DisplayName",
            "Mods.ValhallaMod.NPCs.ZombieBalloon3.DisplayName",
            "Mods.NewHorizons.NPCs.CarrotZombie.DisplayName",
            "Mods.ValhallaMod.NPCs.Croc.DisplayName",
            "Mods.ValhallaMod.NPCs.InfectedMan.DisplayName",
            "Mods.ValhallaMod.NPCs.GoblinBunny.DisplayName",
            "Mods.ValhallaMod.NPCs.VoodooCultistAlt.DisplayName",
            "Mods.ValhallaMod.NPCs.VoodooCultist.DisplayName",
            "Mods.ValhallaMod.NPCs.WitchDoctor.DisplayName",
            "Mods.ValhallaMod.NPCs.GoblinRaider.DisplayName",
            "Mods.ValhallaMod.NPCs.BlueGoblin.DisplayName",
            "Mods.ValhallaMod.NPCs.LihzahrdTrickster.DisplayName",
        };

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            string displayKey = npc.TypeName; // Use TypeName instead of GetFullNetName()
            bool isModdedAllowed = AllowedModdedNPCs.Contains(displayKey);

            bool invalidType =
                npc.SpawnedFromStatue || npc.value == 0 || npc.boss ||
                npc.HitSound == SoundID.NPCHit2 || npc.HitSound == SoundID.NPCHit4 ||
                npc.HitSound == SoundID.NPCHit5 || npc.HitSound == SoundID.NPCHit30 ||
                npc.HitSound == SoundID.NPCHit34 || npc.HitSound == SoundID.NPCHit36 ||
                npc.HitSound == SoundID.NPCHit39 || npc.HitSound == SoundID.NPCHit41 ||
                npc.HitSound == SoundID.NPCHit49 || npc.HitSound == SoundID.NPCHit54;

            invalidType |= npc.FullName.Contains("Slime") ||
                         npc.FullName.Contains("Elemental") ||
                         npc.FullName.Contains("Golem") ||
                         npc.FullName.Contains("Dandelion") ||
                         npc.FullName.Contains("Skeleton") ||
                         npc.FullName.Contains("Skull");

            if (!isModdedAllowed && invalidType)
                return;

            // Get mod instance first
            Mod roa = ModLoader.GetMod("RoA");
            if (roa == null) return;

            // Get items safely
            int roughLeather = roa.Find<ModItem>("RoughLeather")?.Type ?? 0;
            int animalLeather = roa.Find<ModItem>("AnimalLeather")?.Type ?? 0;
            if (roughLeather == 0 || animalLeather == 0) return;

            int itemType = npc.aiStyle == 3 ? roughLeather : animalLeather;

            var dropCondition = new SkinningDropCondition();
            var conditionalRule = new LeadingConditionRule(dropCondition);

            conditionalRule.OnSuccess(ItemDropRule.Common(itemType, 8));
            npcLoot.Add(conditionalRule);
        }
    }

    public class SkinningDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            Mod roa = ModLoader.GetMod("RoA");
            if (roa == null) return false;

            var skinningBuff = roa.Find<ModBuff>("Skinning");
            if (skinningBuff == null) return false;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead)
                    continue;

                if (info.npc.playerInteraction[i] && player.HasBuff(skinningBuff.Type))
                    return true;
            }
            return false;
        }

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Requires the Skinning buff";
    }
}