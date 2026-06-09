using Avalon;
using Avalon.Common.Players;
using Avalon.Items.Accessories.Hardmode;
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
    public class TaintedMummy : ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 0.85f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);


        }

        public override void SetDefaults()
        {
            int width = 16; int height = 40;
            NPC.Size = new Vector2(width, height);

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;

            NPC.damage = 60;
            NPC.defense = 15;

            NPC.lifeMax = 350;
            NPC.knockBackResist = 0.1f;
            NPC.rarity = 1;

            NPC.value = Item.buyPrice(silver: 10);

            NPC.noGravity = false;
            NPC.lavaImmune = false;

            NPC.aiStyle = 3;
            AIType = NPCID.BloodMummy;
            AnimationType = NPCID.BloodMummy;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Avalon.Biomes.ContagionDesert>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
            new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Avalon.Bestiary.ViralMummy"))
            });
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(5))
                target.AddBuff(BuffID.Silenced, 60 * 8);
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Bleeding, 60 * 5);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int num730 = 0; num730 < hit.Damage / (double)NPC.lifeMax * 50.0; num730++)
                {
                    int num731 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 31, 0f, 0f, 0, default, 1.5f);
                    Dust dust = Main.dust[num731];
                    dust.velocity *= 2f;
                    Main.dust[num731].noGravity = true;
                }
                return;
            }
            for (int num732 = 0; num732 < 20; num732++)
            {
                int num733 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 31, 0f, 0f, 0, default, 1.5f);
                Dust dust = Main.dust[num733];
                dust.velocity *= 2f;
                Main.dust[num733].noGravity = true;
            }
            int num734 = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y - 10f), new Vector2(hit.HitDirection, 0f), 61, NPC.scale);
            Gore gore2 = Main.gore[num734];
            gore2.velocity *= 0.3f;
            num734 = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + NPC.height / 2 - 10f), new Vector2(hit.HitDirection, 0f), 62, NPC.scale);
            gore2 = Main.gore[num734];
            gore2.velocity *= 0.3f;
            num734 = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + NPC.height - 10f), new Vector2(hit.HitDirection, 0f), 63, NPC.scale);
            gore2 = Main.gore[num734];
            gore2.velocity *= 0.3f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.DarkShard, 10));
            npcLoot.Add(ItemDropRule.StatusImmunityItem(ItemID.Megaphone, 100));
            npcLoot.Add(ItemDropRule.StatusImmunityItem(ItemID.Blindfold, 100));
            npcLoot.Add(ItemDropRule.StatusImmunityItem(ModContent.ItemType<HiddenBlade>(), 100));
            npcLoot.Add(ItemDropRule.Common(ItemID.MummyMask, 75));
            npcLoot.Add(ItemDropRule.Common(ItemID.MummyShirt, 75));
            npcLoot.Add(ItemDropRule.Common(ItemID.MummyPants, 75));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.Player.GetModPlayer<AvalonBiomePlayer>().ZoneContagionDesert) &&
                !spawnInfo.Player.InPillarZone() && Main.hardMode ? 0.7f : 0f;
        }
    }
}