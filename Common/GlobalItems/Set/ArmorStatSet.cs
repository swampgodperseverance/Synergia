using System.Collections.Generic;
using Avalon.Buffs;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ValhallaMod; // AuraPlayer
using ValhallaMod.Projectiles.AI;
using static ValhallaMod.Projectiles.AI.AuraAI;

namespace Synergia.Common.GlobalItems.Set
{
	public class ArmorStatSet : GlobalItem
	{
		private static Mod consolaria = ModLoader.GetMod("Consolaria");
		private static Mod roa = ModLoader.GetMod("RoA");
		private static Mod horizons = ModLoader.GetMod("NewHorizons");
		private static Mod valhalla = ModLoader.GetMod("ValhallaMod");
		private static Mod avalon = ModLoader.GetMod("Avalon");
		private static Mod bismuth = ModLoader.GetMod("Bismuth");

		public override void UpdateEquip(Item item, Player player)
		{
			var auraPlayer = player.GetModPlayer<AuraPlayer>();

			// Consolaria
			if (consolaria != null)
			{
				if (item.type == consolaria.Find<ModItem>("OstaraHat").Type)
				{
					player.GetDamage(DamageClass.Throwing) += 0.11f;
					player.GetCritChance(DamageClass.Throwing) += 5;
				}
				if (item.type == consolaria.Find<ModItem>("OstaraJacket").Type)
				{
					player.GetAttackSpeed(DamageClass.Throwing) += 0.13f;
				}
				if (item.type == consolaria.Find<ModItem>("OstaraBoots").Type)
				{
					player.jumpSpeedBoost += 2.4f;
				}
				if (item.type == consolaria.Find<ModItem>("WarlockHood").Type)
				{
					auraPlayer.bonusAuraRadius += 0.25f;
					auraPlayer.maxAuras = 1;
				}
				if (item.type == consolaria.Find<ModItem>("WarlockRobe").Type)
				{
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.35f;
				}
				if (item.type == consolaria.Find<ModItem>("WarlockLeggings").Type)
				{
					auraPlayer.bonusAuraRadius += 0.10f;
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.10f;
				}
				if (item.type == consolaria.Find<ModItem>("AncientWarlockHood").Type)
				{
					auraPlayer.bonusAuraRadius += 0.25f;
					auraPlayer.maxAuras = 1;
				}
				if (item.type == consolaria.Find<ModItem>("AncientWarlockRobe").Type)
				{
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.35f;
				}
				if (item.type == consolaria.Find<ModItem>("AncientWarlockLeggings").Type)
				{
					auraPlayer.bonusAuraRadius += 0.10f;
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.10f;
				}
			}

			// RoA
			if (roa != null)
			{
				if (item.type == roa.Find<ModItem>("SentinelHelmet").Type)
				{
					player.GetDamage(DamageClass.Throwing) += 0.15f;
					player.GetCritChance(DamageClass.Throwing) += 5;
				}
				if (item.type == roa.Find<ModItem>("SentinelBreastplate").Type)
				{
					player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
				}
				if (item.type == roa.Find<ModItem>("SentinelLeggings").Type)
				{
					player.moveSpeed += 0.2f;
				}
			}

			// Valhalla
			if (valhalla != null)
			{
				if (item.type == valhalla.Find<ModItem>("GreediestHead").Type)
				{
					player.endurance += 0.13f;
					player.GetDamage(DamageClass.Throwing) += 0.25f;
				}
				if (item.type == valhalla.Find<ModItem>("GreediestBody").Type)
				{
					player.endurance += 0.14f;
					player.GetCritChance(DamageClass.Throwing) += 8;
				}
				if (item.type == valhalla.Find<ModItem>("GreediestLegs").Type)
				{
					player.GetAttackSpeed(DamageClass.Throwing) += 0.09f;
				}
				if (item.type == valhalla.Find<ModItem>("EvilHead").Type)
				{
					player.maxMinions += 2;
					player.GetDamage(DamageClass.Summon) += 0.20f;
				}
				if (item.type == valhalla.Find<ModItem>("SniperBody").Type)
				{
					player.GetDamage(DamageClass.Ranged) += 0.11f;
					player.GetCritChance(DamageClass.Ranged) += 20;
				}
				if (item.type == valhalla.Find<ModItem>("SniperHead").Type)
				{
					player.GetDamage(DamageClass.Ranged) += 0.06f;
					player.GetCritChance(DamageClass.Generic) += 0.06f;
				}
				if (item.type == valhalla.Find<ModItem>("SniperLegs").Type)
				{
					player.GetDamage(DamageClass.Ranged) += 0.05f;
					player.GetDamage(DamageClass.Melee) -= 0.11f;
				}
				
			}

			// Avalon
			if (avalon != null)
			{
				if (item.type == avalon.Find<ModItem>("EarthsplitterChestpiece").Type)
				{
					player.GetDamage(DamageClass.Throwing) += 0.16f;
					player.GetCritChance(DamageClass.Throwing) += 15;
				}
				if (item.type == avalon.Find<ModItem>("EarthsplitterHelm").Type)
				{
					player.GetAttackSpeed(DamageClass.Throwing) += 0.18f;
				}
				if (item.type == avalon.Find<ModItem>("EarthsplitterLeggings").Type)
				{
					player.GetCritChance(DamageClass.Throwing) += 8;
				}
				if (item.type == avalon.Find<ModItem>("FleshCap").Type)
				{
					auraPlayer.bonusAuraRadius += 0.15f;
				}
				if (item.type == avalon.Find<ModItem>("FleshWrappings").Type)
				{
					auraPlayer.buffTypes[AuraEffectTarget.Enemy].Add(BuffID.Bleeding);
					auraPlayer.buffTypes[AuraEffectTarget.Team].Add(ModContent.BuffType<Heartsick>());
				}
				if (item.type == avalon.Find<ModItem>("FleshPants").Type)
						{
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.15f;
					auraPlayer.maxAuras = 1;
				}
				if (item.type == avalon.Find<ModItem>("AeroforceGuardia").Type)
				{
					auraPlayer.bonusAuraRadius += 0.25f;
					auraPlayer.maxAuras = 1;	
					player.GetDamage(DamageClass.Summon) += 0.10f;
				}
				if (item.type == avalon.Find<ModItem>("AeroforceProtector").Type)
				{
					auraPlayer.bonusAuraRadius += 0.05f;
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.13f;
					player.wingTimeMax = (int)(player.wingTimeMax * 1.3f);
				}
				if (item.type == avalon.Find<ModItem>("AeroforceLeggings").Type)
				{
					auraPlayer.bonusPlayerLinkedAuraRadius += 0.17f;
				}
			}

			// Horizons
			if (horizons != null)
			{
				if (item.type == horizons.Find<ModItem>("LivingWoodCrown").Type)
				{
					auraPlayer.bonusAuraRadius += 0.06f;
				}
				if (item.type == horizons.Find<ModItem>("NightMageHat").Type)
				{
					player.GetDamage(DamageClass.Magic) += 0.08f;
					player.manaCost -= 0.13f;
				}
				if (item.type == horizons.Find<ModItem>("NightMageCape").Type)
				{
					player.GetDamage(DamageClass.Magic) += 0.03f;
					player.GetCritChance(DamageClass.Magic) += 5;
					player.GetAttackSpeed(DamageClass.Magic) += 0.09f;
				}
				if (item.type == horizons.Find<ModItem>("NightMagePants").Type)
				{
					player.GetDamage(DamageClass.Magic) += 0.03f;
				}
			}
			
			//Bismuth
			if (bismuth != null) 
			{
				if (item.type == bismuth.Find<ModItem>("BismuthumBreastplate").Type)
				{
					player.GetDamage(DamageClass.Generic) -= 0.10f;
				}
				if (item.type == bismuth.Find<ModItem>("BismuthumLeggings").Type)
				{
					player.moveSpeed -= 0.1f;
				}
				if (item.type == bismuth.Find<ModItem>("BismuthumHeadgear").Type)
				{
					player.GetDamage(DamageClass.Generic) -= 0.10f;
				}
				if (item.type == bismuth.Find<ModItem>("KillersHood").Type)
				{
                    player.GetCritChance(DamageClass.Magic) += 8;
                }
				if (item.type == bismuth.Find<ModItem>("KillersJacket").Type)
				{
                    player.GetDamage(DamageClass.Magic) += 0.10f;
                }
                if (item.type == bismuth.Find<ModItem>("RivetedHood").Type)
                {
                    player.GetCritChance(DamageClass.Ranged) += 8;
                }
                if (item.type == bismuth.Find<ModItem>("RivetedJacket").Type)
                {
                    player.GetDamage(DamageClass.Ranged) += 0.08f;
                }
                if (item.type == bismuth.Find<ModItem>("NomadsHood").Type)
                {
                    player.GetDamage(DamageClass.Throwing) += 0.07f;
                }
                if (item.type == bismuth.Find<ModItem>("NomadsJacket").Type)
                {
                    player.GetDamage(DamageClass.Throwing) += 0.11f;
                }
                if (item.type == bismuth.Find<ModItem>("NomadsBoots").Type)
                {
                    player.GetDamage(DamageClass.Throwing) += 0.07f;
                }
                if (item.type == bismuth.Find<ModItem>("FeralLeggings").Type)
                {
                    player.GetCritChance(DamageClass.Throwing) += 0.06f;
                    player.GetCritChance(DamageClass.Melee) -= 0.06f;
                }
                if (item.type == bismuth.Find<ModItem>("FeralBreastplate").Type)
                {
                    player.GetDamage(DamageClass.Throwing) += 0.07f;
                    player.GetDamage(DamageClass.Melee) -= 0.07f;
                }
                if (item.type == bismuth.Find<ModItem>("FeralHelmet").Type)
                {
                    player.GetDamage(DamageClass.Throwing) += 0.05f;
                    player.GetDamage(DamageClass.Ranged) += 0.05f;
                    player.GetDamage(DamageClass.Melee) -= 0.05f;
                }
                if (item.type == bismuth.Find<ModItem>("PikemansHelmet").Type)
                {
                    item.defense += 2;
                }
                if (item.type == bismuth.Find<ModItem>("PikemansBreastplate").Type)//Add localization 
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.12f;
                    player.GetDamage(DamageClass.Melee) += 0.10f;
                    player.moveSpeed -= 0.12f;
                    item.defense += 5;
                }
                if (item.type == bismuth.Find<ModItem>("PikemansLeggings").Type)
                {

                    player.GetCritChance(DamageClass.Melee) += 0.06f;
                    player.moveSpeed -= 0.08f;
                    item.defense += 3;
                }
                if (item.type == bismuth.Find<ModItem>("WatersHelmet").Type)
                {
                    player.GetDamage(DamageClass.Magic) += 0.05f;
                    player.manaRegenBonus += 8;
                    item.defense += 5;
                }
                if (item.type == bismuth.Find<ModItem>("WatersBreastplate").Type)
                {
                    player.GetModPlayer<WaterPlayer>().waterAccessoryEquipped = true;
                    player.GetCritChance(DamageClass.Magic) += 10f;
                    item.defense += 13;
                }
                if (item.type == bismuth.Find<ModItem>("WatersLeggings").Type)
                {
                    player.GetDamage(DamageClass.Magic) += 0.08f;
                    player.manaRegenBonus += 10;
                    item.defense += 7;
                }
                if (item.type == bismuth.Find<ModItem>("NecromancersRobe").Type)
                {
                    player.maxMinions -= 1;

                }

            }
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			void Add(string modName, string itemName, string tooltipKey)
			{
				Mod m = ModLoader.GetMod(modName);
				if (m != null && item.type == m.Find<ModItem>(itemName).Type)
					ReplaceTooltip(tooltips, "Tooltip0", Language.GetTextValue($"Mods.Synergia.{tooltipKey}"));
			}

			Add("Consolaria", "OstaraHat", "Items.OstaraHat.Tooltip");
			Add("Consolaria", "OstaraJacket", "Items.OstaraJacket.Tooltip");
			Add("Consolaria", "OstaraBoots", "Items.OstaraBoots.Tooltip");
			Add("Consolaria", "WarlockHood", "Items.WarlockHood.Tooltip");
			Add("Consolaria", "WarlockRobe", "Items.WarlockRobe.Tooltip");	
			Add("Consolaria", "WarlockLeggings", "Items.WarlockLeggings.Tooltip");
			Add("Consolaria", "AncientWarlockHood", "Items.AncientWarlockHood.Tooltip");
			Add("Consolaria", "AncientWarlockRobe", "Items.AncientWarlockRobe.Tooltip");
			Add("Consolaria", "AncientWarlockLeggings", "Items.AncientWarlockLeggings.Tooltip");

			Add("RoA", "SentinelHelmet", "Items.SentinelHelmet.Tooltip");
			Add("RoA", "SentinelBreastplate", "Items.SentinelBreastplate.Tooltip");
			Add("RoA", "SentinelLeggings", "Items.SentinelLeggings.Tooltip");

			Add("ValhallaMod", "GreediestHead", "Items.GreediestHead.Tooltip");
			Add("ValhallaMod", "GreediestBody", "Items.GreediestBody.Tooltip");
			Add("ValhallaMod", "GreediestLegs", "Items.GreediestLegs.Tooltip");
			Add("ValhallaMod", "EvilHead", "Items.EvilHead.Tooltip");
			Add("ValhallaMod", "SniperBody", "Items.SniperBody.Tooltip");
			Add("ValhallaMod", "SniperHead", "Items.SniperHead.Tooltip");
			Add("ValhallaMod", "SniperLegs", "Items.SniperLegs.Tooltip");

			Add("Avalon", "EarthsplitterChestpiece", "Items.EarthsplitterChestpiece.Tooltip");
			Add("Avalon", "EarthsplitterHelm", "Items.EarthsplitterHelm.Tooltip");
			Add("Avalon", "EarthsplitterLeggings", "Items.EarthsplitterLeggings.Tooltip");
			Add("Avalon", "FleshCap", "Items.FleshCap.Tooltip");
			Add("Avalon", "FleshWrappings", "Items.FleshWrappings.Tooltip");
			Add("Avalon", "FleshPants", "Items.FleshPants.Tooltip");
			Add("Avalon", "AeroforceGuardia", "Items.AeroforceGuardia.Tooltip");
			Add("Avalon", "AeroforceProtector", "Items.AeroforceProtector.Tooltip");
			Add("Avalon", "AeroforceLeggings", "Items.AeroforceLeggings.Tooltip");

			Add("NewHorizons", "LivingWoodCrown", "Items.LivingWoodCrown.Tooltip");
			Add("NewHorizons", "NightMageHat", "Items.NightMageHat.Tooltip");
			Add("NewHorizons", "NightMageCape", "Items.NightMageCape.Tooltip");
			Add("NewHorizons", "NightMagePants", "Items.NightMagePants.Tooltip");

			Add("Bismuth", "BismuthumLeggings", "Items.BismuthumLeggings.Tooltip");
			Add("Bismuth", "BismuthumBreastplate", "Items.BismuthumBreastplate.Tooltip");

            Add("Bismuth", "KillersHood", "Items.KillersHood.Tooltip");
            Add("Bismuth", "KillersJacket", "Items.KillersJacket.Tooltip");

            Add("Bismuth", "RivetedHood", "Items.RivetedHood.Tooltip");
            Add("Bismuth", "RivetedJacket", "Items.RivetedJacket.Tooltip");

            Add("Bismuth", "NomadsHood", "Items.NomadsHood.Tooltip");
            Add("Bismuth", "NomadsJacket", "Items.NomadsJacket.Tooltip");

            Add("Bismuth", "FeralLeggings", "Items.FeralLeggings.Tooltip");
            Add("Bismuth", "FeralBreastplate", "Items.FeralBreastplate.Tooltip");
            Add("Bismuth", "FeralHelmet", "Items.FeralHelmet.Tooltip");


            Add("Bismuth", "PikemansLeggings", "Items.PikemansLeggings.Tooltip");
            Add("Bismuth", "PikemansBreastplate", "Items.PikemansBreastplate.Tooltip");

            Add("Bismuth", "WatersLeggings", "Items.WatersLeggings.Tooltip");
            Add("Bismuth", "WatersBreastplate", "Items.WatersBreastplate.Tooltip");
            Add("Bismuth", "WatersHelmet", "Items.WatersHelmet.Tooltip");

            if (item.type != ModContent.Find<ModItem>("Bismuth/NomadsBoots").Type)
                return;

            tooltips.RemoveAll(line =>
                line.Mod == "Terraria" &&
                line.Name.StartsWith("Tooltip")
            );
            string newTooltip = Language.GetTextValue("Mods.Synergia.Items.NomadsBoots.Tooltip");
            tooltips.Add(new TooltipLine(Mod, "NomadsBootsTooltip", newTooltip));
        }

		private void ReplaceTooltip(List<TooltipLine> tooltips, string tooltipName, string newText)
		{
			foreach (var line in tooltips)
			{
				if (line.Name == tooltipName && line.Mod == "Terraria")
				{
					line.Text = newText;
					break;
				}
			}
		}
	}
}