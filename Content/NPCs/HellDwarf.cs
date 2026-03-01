// Code by SerNik
using System.Collections.Generic;
using Bismuth.Content.Items.Materials;
using Bismuth.Content.Items.Other;
using Synergia.Common.ModSystems;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;

namespace Synergia.Content.NPCs {
    [AutoloadHead]
    public class HellDwarf : ModNPC {
        public override string LocalizationCategory => Category(CategoryName.NPC);
        public override List<string> SetNPCNameList() => [this.GetLocalizedValue("Name.Skyzephire"), this.GetLocalizedValue("Name.Thorin"), this.GetLocalizedValue("Name.Belegar"), this.GetLocalizedValue("Name.Dragan"), this.GetLocalizedValue("Name.Wulfrik")];
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(
            [
                new FlavorTextBestiaryInfoElement("Mods.Synergia.NPCs.HellDwarf.BestiaryInfo"),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
            ]);
        }
        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 5;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 550;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
        }
        public override void SetDefaults() {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 30;
            NPC.height = 44;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 100;
            NPC.defense = 20;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.GoblinTinkerer;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom) {
            bool hellStruct = WorldHelper.CheckBiomeTile(left, top, 237 + SynergiaGenVars.HellArenaPositionX - SynergiaGenVars.HellLakeX, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 119);
            return hellStruct;
        }
        public override void OnSpawn(IEntitySource source) {
            if (source is EntitySource_SpawnNPC) {
                SynergiaWorld.SpawnDwarf = true;
            }
        }
        public override bool CanTownNPCSpawn(int numTownNPCs) {
            if (SynergiaWorld.SpawnDwarf) {
                return true;
            }
            if (!SynergiaWorld.SpawnDwarf && SynergiaWorld.FirstEnterInHellVillage && NPC.downedPlantBoss) {
                return true;
            }
            else { return false; }
        }
        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
        public override string GetChat() {
            return Main.rand.Next(maxValue: 9) switch {
                0 => GetText("Text"),
                1 => GetText("Text1"),
                2 => GetText("Text2"),
                3 => GetText("Text3"),
                4 => GetText("Text4"),
                5 => GetText("Text5"),
                6 => GetText("Text6"),
                7 => GetText("Text7"),
                8 => GetText("Text8"),
                _ => Language.GetTextValue("tModLoader.DefaultTownNPCChat"),
            };
        }
        // TODO: add item for shop
        public override void AddShops()
        {
            NPCShop shop = new(Type, "Shop");

            Condition wOF = Condition.Hardmode;
            Condition plant = Condition.DownedPlantera;
            Condition sinlord = new("sdadd", () => DownedBossSystem.DownedSinlordBoss);

            shop.Add(new Item(ItemID.AshBlock) { shopCustomPrice = Item.buyPrice(silver: 3) });
            shop.Add(new Item(ItemID.Obsidian) { shopCustomPrice = Item.buyPrice(silver: 30) });
            shop.Add(new Item(ItemID.LavaBomb) { shopCustomPrice = Item.buyPrice(silver: 20, copper: 50) });

            shop.Add(new Item(ItemID.HellfireArrow) { shopCustomPrice = Item.buyPrice(silver: 4, copper: 80) });

            shop.Add(new Item(ModContent.ItemType<ValhallaMod.Items.Placeable.Painting.MemoriesOfFire>())
            {
                shopCustomPrice = Item.buyPrice(gold: 10)
            });

            var condHardmode = new Condition("Mods.Avalon.HardmodeCondition", () => Main.hardMode);

            shop.Add(new Item(ModContent.ItemType<Avalon.Items.Material.Shards.FireShard>())
            {
                shopCustomPrice = Item.buyPrice(silver: 35)
            }, condHardmode);

            shop.Add(new Item(ItemID.LavaAbsorbantSponge)
            {
                shopCustomPrice = Item.buyPrice(platinum: 1, gold: 50)
            }, condHardmode);

            shop.Add(new Item(ItemID.HellfireArrow) { shopCustomPrice = Item.buyPrice(silver: 8, copper: 80) });
            shop.Add(new Item(ItemType<ValhallaMod.Items.Placeable.Painting.MemoriesOfFire>()) { shopCustomPrice = Item.buyPrice(gold: 10) });

            shop.Add(new Item(ItemType<Avalon.Items.Material.Shards.FireShard>()) { shopCustomPrice = Item.buyPrice(silver: 35) }, wOF);
            shop.Add(new Item(ItemID.LavaAbsorbantSponge) { shopCustomPrice = Item.buyPrice(platinum: 1, gold: 50) }, wOF);
            shop.Add(new Item(ItemID.PottedLavaPlantBulb) { shopCustomPrice = Item.buyPrice(gold: 4) }, wOF);

            shop.Add(new Item(ItemType<Avalon.Items.Weapons.Ranged.PreHardmode.Boompipe.Boompipe>()) { shopCustomPrice = Item.buyPrice(gold: 16, silver: 90) }, plant);
            shop.Add(new Item(ItemID.LavaRocket) { shopCustomPrice = Item.buyPrice(silver: 30) }, plant);

            shop.Add(new Item(ItemType<ValhallaMod.Items.Placeable.Painting.HellUnderEarth>()) { shopCustomPrice = Item.buyPrice(platinum: 1) }, sinlord);


            shop.Add(new Item(ItemID.PottedLavaPlantBulb)
            {
                shopCustomPrice = Item.buyPrice(gold: 4)
            }, condHardmode);

            var condPostPlant = new Condition("Mods.Avalon.DownedPlantBoss", () => NPC.downedPlantBoss);

            shop.Add(new Item(ModContent.ItemType<Avalon.Items.Weapons.Ranged.PreHardmode.Boompipe.Boompipe>())
            {
                shopCustomPrice = Item.buyPrice(gold: 16, silver: 90)
            }, condPostPlant);

            shop.Add(new Item(ItemID.LavaRocket)
            {
                shopCustomPrice = Item.buyPrice(silver: 30)
            }, condPostPlant);

            var condSinlord = new Condition("Mods.Valhalla.DownedSinlord", () => DownedBossSystem.DownedSinlordBoss);

            shop.Add(new Item(ModContent.ItemType<ValhallaMod.Items.Placeable.Painting.HellUnderEarth>())
            {
                shopCustomPrice = Item.buyPrice(platinum: 1)
            }, condSinlord);

         
            shop.Register();
        }
        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) {
            itemWidth = 32;
            itemHeight = 32;
        }
        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset) {
            Main.GetItemDrawFrame(ItemID.GoldHammer, out item, out itemFrame);
            if (NPC.ai[1] > (float)NPCID.Sets.AttackTime[NPC.type] * 0.66f) {
                offset = new Vector2(offset.X - 7, offset.Y + 24);
            }
            else {
                offset = new Vector2(offset.X, offset.Y + 7);
            }
        }
        string GetText(string key) => LocNPCKey(Name, "Chat." + key);
        // TODO: Add custom text
        public override bool ModifyDeathMessage(ref NetworkText customText, ref Color color) {
            return base.ModifyDeathMessage(ref customText, ref color);
        }
        // TODO: Add gore
        public override void HitEffect(NPC.HitInfo hit) {
            base.HitEffect(hit);
        }
    }
    public class DwarfEmote : ModEmoteBubble {
        public override string Texture => (GetType().Namespace + "." + "Emote").Replace('.', '/');
        public override void SetStaticDefaults() {
            AddToCategory(EmoteID.Category.Town);
        }
    }
}