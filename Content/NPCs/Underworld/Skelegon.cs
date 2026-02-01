using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using static Synergia.Common.SUtils.LocUtil;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Content.NPCs.Underworld
{
    public class Skelegon : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 34;
            NPC.damage = 35;
            NPC.defense = 22;
            NPC.lifeMax = 900;
            NPC.knockBackResist = 0.05f;
            NPC.value = Item.buyPrice(0, 0, 80, 0);
            NPC.aiStyle = 3;
            AIType = NPCID.UndeadMiner;
            AnimationType = -1;
            NPC.scale = 1.4f;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = new SoundStyle("Synergia/Assets/Sounds/BrokenBone");
            NPC.lavaImmune = Main.expertMode;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            BiomePlayer modPlayer = spawnInfo.Player.GetModPlayer<BiomePlayer>();

            if (modPlayer.villageBiome && spawnInfo.Player.ZoneUnderworldHeight)
                return 0.6f;

            return 0f;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            if (Main.expertMode && NPC.lavaWet)
            {
                NPC.lifeRegen += 20;
                if (NPC.life < NPC.lifeMax)
                    NPC.life++;
            }

            float distance = Vector2.Distance(NPC.Center, player.Center);
            bool aggressive = distance <= 420f;

            NPC.direction = NPC.spriteDirection =
                NPC.Center.X < player.Center.X ? 1 : -1;

            float maxSpeed = aggressive ? 1.8f : 0.6f;
            float accel = aggressive ? 0.08f : 0.03f;

            NPC.velocity.X += accel * NPC.direction;
            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -maxSpeed, maxSpeed);
            NPC.velocity.X *= aggressive ? 0.96f : 0.90f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.12f;
            if (NPC.frameCounter >= 8)
                NPC.frameCounter = 0;
            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 3, 6));
        }

        public override void OnKill()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 center = NPC.Center;

            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Projectile.NewProjectile(
                    NPC.GetSource_Death(),
                    center,
                    Main.rand.NextVector2Circular(9f, 9f),
                    ModContent.ProjectileType<Projectiles.Hostile.SkelegonProj3>(),
                    25,
                    2f
                );
            }

            Projectile.NewProjectile(
                NPC.GetSource_Death(),
                center,
                Main.rand.NextVector2Circular(8f, 8f),
                ModContent.ProjectileType<Projectiles.Hostile.SkelegonProj1>(),
                30,
                2f
            );

            Projectile.NewProjectile(
                NPC.GetSource_Death(),
                center,
                Main.rand.NextVector2Circular(8f, 8f),
                ModContent.ProjectileType<Projectiles.Hostile.SkelegonProj2>(),
                30,
                2f
            );
        }
    }
}
