using Avalon;
using Avalon.Common.Players;
using Consolaria.Content.Items.Pets;
using Synergia.Common.GlobalPlayer;
using Synergia.Content.Items.Placeable.Banners;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

namespace Synergia.Content.NPCs.Contagion
{
    public class TaintedSlime : ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

        }

        public override void SetDefaults()
        {
            int width = 40; int height = 30;
            NPC.Size = new Vector2(width, height);

            NPC.lifeMax = 125;
            NPC.defense = 20;

            NPC.damage = 20;
            NPC.knockBackResist = 0f;
            NPC.rarity = 1;

            NPC.value = Item.buyPrice(silver: 5);
            NPC.alpha = 80;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            AnimationType = 81;
            NPC.aiStyle = 1;

            SpawnModBiomes = new int[] { ModContent.GetInstance<Avalon.Biomes.Contagion>().Type, ModContent.GetInstance<Avalon.Biomes.UndergroundContagion>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
            new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Avalon.Bestiary.Ickslime"))
            });
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Bleeding, 60 * 5);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TintableDust, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 24; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TintableDust, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetriDish>(), 25));
            npcLoot.Add(ItemDropRule.StatusImmunityItem(ItemID.Vitamins, 90));
            var slimeDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CorruptSlime, false);
            foreach (var slimeDropRule in slimeDropRules)
                npcLoot.Add(slimeDropRule);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.Player.GetModPlayer<AvalonBiomePlayer>().ZoneContagion || spawnInfo.Player.GetModPlayer<AvalonBiomePlayer>().ZoneUndergroundContagion) &&
                !spawnInfo.Player.InPillarZone() && Main.hardMode ? 0.7f : 0f;
        }
    }
}
