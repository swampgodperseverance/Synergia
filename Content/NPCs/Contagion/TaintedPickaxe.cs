using System;
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
    public class TaintedPickaxe : ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 0.8f,
                Position = new Vector2(20f, 10f),
                PortraitPositionXOverride = -5f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

        }

        public override void SetDefaults()
        {
            int width = 64; int height = width;
            NPC.Size = new Vector2(width, height);

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;

            NPC.damage = 80;
            NPC.defense = 20;

            NPC.lifeMax = 250;
            NPC.knockBackResist = 0.6f;
            NPC.rarity = 1;

            NPC.value = Item.buyPrice(silver: 15);

            NPC.noGravity = true;
            NPC.lavaImmune = true;

            NPC.aiStyle = 23;
            AIType = 83;
            AnimationType = 83;

            DrawOffsetY = 16;
            SpawnModBiomes = new int[] { ModContent.GetInstance<Avalon.Biomes.UndergroundContagion>().Type };

        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) =>
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
            new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Avalon.Bestiary.InfectedPickaxe"))
        });

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.CursedInferno, 60 * 5);
        }

        public override Color? GetAlpha(Color drawColor)
            => Color.White * 0.8f;

       

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => Main.hardMode && spawnInfo.Player.InModBiome<Avalon.Biomes.UndergroundContagion>()
        ? 0.2f : 0f;
        public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ItemDropRule.Common(ItemID.Nazar, 75));
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                int num161 = 0;
                while (num161 < hit.Damage / (double)NPC.lifeMax * 50.0)
                {
                    int num162 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                    Main.dust[num162].noGravity = true;
                    num161++;
                }
            }
            else if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
            {
                for (int num163 = 0; num163 < 20; num163++)
                {
                    int num164 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 0, default, 1.5f);
                    Main.dust[num164].velocity *= 2f;
                    Main.dust[num164].noGravity = true;
                }
                int num165 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y + NPC.height / 2 - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
                Main.gore[num165].velocity *= 0.5f;
                num165 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y + NPC.height / 2 - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
                Main.gore[num165].velocity *= 0.5f;
                num165 = Gore.NewGore(NPC.GetSource_FromThis(), new Vector2(NPC.position.X, NPC.position.Y + NPC.height / 2 - 10f), new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), 61, NPC.scale);
                Main.gore[num165].velocity *= 0.5f;
            }
        }
        // vanilla is STUPID!!!! the aistyle for the enchanted sword ALWAYS emits blue light if the npc type isn't cursed hammer or crimson axe!!!!!
        public override void AI()
        {
            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.2f, 0.25f, 0.05f);
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest();
            }
            if (NPC.ai[0] == 0f)
            {
                Vector2 distanceXY = Main.player[NPC.target].Center - NPC.Center;
                float num872 = 9f / MathF.Sqrt(MathF.Pow(distanceXY.X, 2) + MathF.Pow(distanceXY.Y, 2));
                NPC.velocity = distanceXY * num872;
                NPC.rotation = MathF.Atan2(NPC.velocity.Y, NPC.velocity.X) + 0.785f;
                NPC.ai[0] = 1f;
                NPC.ai[1] = 0f;
                NPC.netUpdate = true;
            }
            else if (NPC.ai[0] == 1f)
            {
                if (NPC.justHit)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                }
                NPC.velocity *= 0.99f;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 100f)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                    NPC.velocity.X = 0f;
                    NPC.velocity.Y = 0f;
                }
                else
                {
                    NPC.rotation = MathF.Atan2(NPC.velocity.Y, NPC.velocity.X) + 0.785f;
                }
            }
            else
            {
                if (NPC.justHit)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                }
                NPC.velocity *= 0.96f;
                NPC.ai[1] += 1f;
                float num875 = NPC.ai[1] / 120f;
                num875 = 0.1f + num875 * 0.4f;
                NPC.rotation += num875 * NPC.direction;
                if (NPC.ai[1] >= 120f)
                {
                    NPC.netUpdate = true;
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                }
            }
        }
    }
}