using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Avalon.Items.Accessories.Hardmode;
using Terraria.Localization;
using Avalon.Items.Material.Shards;//if mod item
using static Terraria.ModLoader.ModContent;
using static Terraria.Localization.Language;

namespace Synergia.Common.GlobalNPCs.Drops {
    public class AvalonAccessories : GlobalNPC {
        private static Mod avalon = ModLoader.GetMod("Avalon");
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ArchImp.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ArchImp.DisplayName"))
            //Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
            {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,//Item that player will receive
                chanceDenominator: 100,//drop chance 100/4=25
                minimumDropped: 1,//min amount
                maximumDropped: 1//max amount
                                 //if u need exact amount, set identical	numbers
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Cacodemon.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Cacodemon.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 20,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Firebird.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Firebird.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Heater.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Heater.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.HeaterWinged.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.HeaterWinged.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Ifrit.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Ifrit.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 10,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.LavaJellyfish.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.LavaJellyfish.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SporeSlinger.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SporeSlinger.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SporeSmasher.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SporeSmasher.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("Vortex").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ChargingSpearbones.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ChargingSpearbones.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.CenturionShieldless.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.CenturionShieldless.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2Shieldless.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2Shieldless.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Centurion2.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Geomancer.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ShaftSentinel.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ShaftSentinel.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonTrapper.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SkeletonTrapper.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.DragonSkull.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.DragonSkull.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.VampireMiner.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.VampireMiner.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Archdruid.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.Archdruid.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUnicorn.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUnicorn.DisplayName"))
            //Big text after if ( if so, we change it on npc.type == NPCType<MODBOSSNAME>() if npc code not private
            {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella3.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella3.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella2.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieUmbrella.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical2.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieLibrarian.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieLibrarian.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieNinja.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieNinja.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBucket.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBucket.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon2.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon3.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieBalloon3.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.NewHorizons.NPCs.CarrotZombie.DisplayName") || npc.FullName == GetTextValue("Mods.NewHorizons.NPCs.CarrotZombie.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SoullessLocket").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.RoA.NPCs.Hunter.DisplayName") || npc.FullName == GetTextValue("Mods.NewHorizons.NPCs.Hunter.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("HiddenBlade").Type,
                chanceDenominator: 10,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshMummy.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("HiddenBlade").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowMummy.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("HiddenBlade").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshSlime.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshSlime.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("GoldenShield").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.FleshAxe.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.FleshAxe.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("GoldenShield").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.ZombieTactical.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("AmmoMagazine").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Coldmando.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Coldmando.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("AmmoMagazine").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Coldmando.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Coldmando.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("AmmoMagazine").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.MisterShotty.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.MisterShotty.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("AmmoMagazine").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.SnowmanTrasher.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.SnowmanTrasher.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("AmmoMagazine").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.OrbOfCorruption.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.OrbOfCorruption.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("GreekExtinguisher").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowSlime.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowSlime.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("GreekExtinguisher").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.ShadowHammer.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.ShadowHammer.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("GreekExtinguisher").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator2.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator2.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SixHundredWattLightbulb").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Radiator.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("SixHundredWattLightbulb").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.Spectropod.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.Spectropod.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("RubberBoot").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.Consolaria.NPCs.SpectralMummy.DisplayName") || npc.FullName == GetTextValue("Mods.Consolaria.NPCs.SpectralMummy.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("RubberBoot").Type,
                chanceDenominator: 100,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
            if (npc.TypeName == GetTextValue("Mods.ValhallaMod.NPCs.Cacodemon.DisplayName") || npc.FullName == GetTextValue("Mods.ValhallaMod.NPCs.Cacodemon.DisplayName")) {
                npcLoot.Add(
                ItemDropRule.Common(
                avalon.Find<ModItem>("BloodyWhetstone").Type,
                chanceDenominator: 33,
                minimumDropped: 1,
                maximumDropped: 1
                )
                );
            }
        }
    }
}
