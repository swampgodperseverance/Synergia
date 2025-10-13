using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ValhallaMod.NPCs.Pirate;
using Synergia.Content.Projectiles.Hostile; 

namespace Synergia.Content.GlobalNPCs.AI
{
    public class CarnageAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private bool spawnedAt95 = false;
        private bool spawnedAt85 = false;
        private bool spawnedAt75 = false;

        private bool spawnedAt63 = false;
        private bool spawnedAt52 = false;
        private bool spawnedAt41 = false;

        private bool spawnedAt30 = false;
        private bool spawnedAt20 = false;
        private bool spawnedAt8 = false;

        private int PirateSquidType => ModContent.NPCType<PirateSquid>();

        private int CarnageChest1Type => ModContent.ProjectileType<CarnageChest1>();
        private int CarnageChest2Type => ModContent.ProjectileType<CarnageChest2>();
        private int CarnageChest22Type => ModContent.ProjectileType<CarnageChest22>();
        private int CarnageChest3Type => ModContent.ProjectileType<CarnageChest3>();
        private int CarnageChest33Type => ModContent.ProjectileType<CarnageChest33>();
        private int CarnageChest333Type => ModContent.ProjectileType<CarnageChest333>();

        public override void AI(NPC npc)
        {
            if (npc.type != PirateSquidType)
                return;

            float hpPercent = 100f * npc.life / npc.lifeMax;

            if (hpPercent <= 95 && !spawnedAt95)
            {
                SpawnChest(npc, CarnageChest1Type);
                spawnedAt95 = true;
            }
            if (hpPercent <= 85 && !spawnedAt85)
            {
                SpawnChest(npc, CarnageChest1Type);
                spawnedAt85 = true;
            }
            if (hpPercent <= 75 && !spawnedAt75)
            {
                SpawnChest(npc, CarnageChest1Type);
                spawnedAt75 = true;
            }

            if (hpPercent <= 63 && !spawnedAt63)
            {
                SpawnChest(npc, Main.rand.NextBool() ? CarnageChest2Type : CarnageChest22Type);
                spawnedAt63 = true;
            }
            if (hpPercent <= 52 && !spawnedAt52)
            {
                SpawnChest(npc, Main.rand.NextBool() ? CarnageChest2Type : CarnageChest22Type);
                spawnedAt52 = true;
            }
            if (hpPercent <= 41 && !spawnedAt41)
            {
                SpawnChest(npc, Main.rand.NextBool() ? CarnageChest2Type : CarnageChest22Type);
                spawnedAt41 = true;
            }

            if (hpPercent <= 30 && !spawnedAt30)
            {
                SpawnChest(npc, RandomChest3Type());
                spawnedAt30 = true;
            }
            if (hpPercent <= 20 && !spawnedAt20)
            {
                SpawnChest(npc, RandomChest3Type());
                spawnedAt20 = true;
            }
            if (hpPercent <= 8 && !spawnedAt8)
            {
                SpawnChest(npc, RandomChest3Type());
                spawnedAt8 = true;
            }
        }

        private int RandomChest3Type()
        {
            int choice = Main.rand.Next(3);
            return choice switch
            {
                0 => CarnageChest3Type,
                1 => CarnageChest33Type,
                _ => CarnageChest333Type,
            };
        }

        private void SpawnChest(NPC npc, int projType)
        {
            if (projType == 0) return;

            Vector2 spawnPos = npc.Center + new Vector2(0, npc.height / 2);

            Projectile.NewProjectile(
                npc.GetSource_FromAI(),
                spawnPos,
                Vector2.Zero,
                projType,
                10,
                0f,
                Main.myPlayer,
                ai0: npc.whoAmI
            );
        }
    }
}
