using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class ProjectileReplacer : GlobalItem
    {

        
        public override void SetDefaults(Item item)
        {
            Mod vanillaMod = ModLoader.GetMod("Synergia");
            if (vanillaMod != null)
            {

                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "Avalon")
                {
                    switch (item.ModItem.Name)
                    {
                        case "DurataniumGlaive":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.DurataniumRework>();
                            break;
                        case "TroxiniumSpear":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TroxiniumRework>();
                            break;
                        case "NaquadahLance":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.NaquadahRework2>();
                            break;
                        case "CaesiumPike":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.CaesiumSpear>();
                            break;
                    }
                }
                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "ValhallaMod")
                {
                    switch (item.ModItem.Name)
                    {
                        case "ValhalliteSpear":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.ValhalliteRework>();
                            break;
                        case "LampSpear":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.LampRework>();
                            break;
                        case "ScaleBreaker":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.ScaleRework>();
                            break;
                        case "TerraSpear":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TerraRework>();
                            break;
                        case "PaintRoller":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.PaintRework>();
                            break;
                        case "TrueDarkLance":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TrueNightRework>();
                            break;
                        case "Pumpkill":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.SerialRework>();
                            break;
                        case "TrueGungnir":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TrueGungnirRework>();
                            break;
                        case "MarbleShieldBreaker":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.MarbleRework>();
                            break;
                        case "WoodenClub":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.WoodenClubRework>();
                            break;
                        case "FlamingBlade":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.FlamingBladeRework>();
                            break;
                         case "NebulaRod":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.NebulaPikeSpawner>();
                            break;      
                         case "MagicClaw":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Friendly.MagicClawRework>();
                            break; 
                         case "SunJavelin":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.Reworks2.SunJavelinRework>();
                            break;  
                         case "JadeSpear":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.Reworks2.JadeSpearRework>();
                            break;
                         case "FaceMace":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.Reworks2.FaceMace2>();
                            break;
                         case "AzraelsHeartstopper":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Thrower.AzraelsHeartstopper2>();
                            break;
                    }
                }
            
                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "RoA")
                {
                    switch (item.ModItem.Name)
                    {
                        case "OvergrownSpear":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.OvergrownRework>();
                            break;
                    }
                }
                if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "Bismuth")
                {
                    switch (item.ModItem.Name)
                    {
                        case "Typhoon":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TyphoonRework>();
                            break;
                    }
                    switch (item.ModItem.Name)
                    {
                        case "FuryOfWaters":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.Reworks2.FuryOfWatersRework>();
                            break;
                    }
                    switch (item.ModItem.Name)
                    {
                        case "BismuthumGlove":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.BismuthiumGloveRework>();
                            break;
                    }

                    switch (item.ModItem.Name)
                    {
                        case "JaguarsChakram":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.JaguarChakramRework>();
                            break;
                    }
                    switch (item.ModItem.Name)
                    {
                        case "WaveOfForce":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.PurpleCog>();
                            break;
                    }
                }
               if (item.ModItem != null &&
                    item.ModItem.Mod.Name == "NewHorizons")
                {
                    switch (item.ModItem.Name)
                    {
                        case "SpaceCowboy":
                            item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.Reworks2.SpaceCowboyRework>();
                            break;
                    }
                }
            

                switch (item.type)
                {
                    case ItemID.Spear: 
                        item.shoot = ProjectileID.Spear;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.SpearRework>();
                        break;
                    case ItemID.Trident: 
                        item.shoot = ProjectileID.Trident;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TridentRework>();
                        break;
                    case ItemID.ThunderSpear: 
                        item.shoot = ProjectileID.ThunderSpear;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.StormSpearRework>();
                        break;
                    case ItemID.CobaltNaginata: 
                        item.shoot = ProjectileID.CobaltNaginata;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.CobaltRework>();
                        break;
                    case ItemID.PalladiumPike: 
                        item.shoot = ProjectileID.PalladiumPike;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.PalladiumRework>();
                        break;  
                    case ItemID.OrichalcumHalberd: 
                        item.shoot = ProjectileID.OrichalcumHalberd;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.OrichalkumRework>();
                        break;
                    case ItemID.TitaniumTrident: 
                        item.shoot = ProjectileID.TitaniumTrident;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.TitaniumRework>();
                        break;      
                    case ItemID.MythrilHalberd: 
                        item.shoot = ProjectileID.MythrilHalberd;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.MythrilRework>();
                        break;  
                    case ItemID.AdamantiteGlaive: 
                        item.shoot = ProjectileID.AdamantiteGlaive;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.AdamantiteRework>();
                        break;
                    case ItemID.Swordfish: 
                        item.shoot = ProjectileID.Swordfish;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.FishRework>();
                        break; 
                    case ItemID.ObsidianSwordfish: 
                        item.shoot = ProjectileID.ObsidianSwordfish;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.ObsfishRework>();
                        break;  
                    case ItemID.ChlorophytePartisan: 
                        item.shoot = ProjectileID.ChlorophytePartisan;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.ChlorophyteRework>();
                        break;
                    case ItemID.DarkLance: 
                        item.shoot = ProjectileID.DarkLance;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.DarkRework>();
                        break;  
                    case ItemID.TheRottedFork: 
                        item.shoot = ProjectileID.TheRottedFork;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.ForkRework>();
                        break;  
                    case ItemID.MushroomSpear: 
                        item.shoot = ProjectileID.MushroomSpear;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.MushroomRework>();
                        break;  
                    case ItemID.NorthPole: 
                        item.shoot = ProjectileID.NorthPoleWeapon;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.FrozenRework>();
                        break;  
                    case ItemID.Gungnir: 
                        item.shoot = ProjectileID.Gungnir;
                        item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.GungnirRework>();
                        break;  
                }
            } 
        }
    }	
}