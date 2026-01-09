using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Content.NPCs
{
	public class Snowykaze : ModNPC
	{
		public override string LocalizationCategory => Category(CategoryName.NPC);

		private bool exploded;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.BelongsToInvasionFrostLegion[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 40;
			NPC.aiStyle = 38;

			NPC.damage = 50;
			NPC.defense = 40;
			NPC.lifeMax = 150;

			NPC.knockBackResist = 0f;
			NPC.value = 400f;

			NPC.HitSound = SoundID.NPCHit11;
			NPC.DeathSound = SoundID.Item14;

			NPC.velocity *= 1.6f;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.velocity.Y > 0f)
				NPC.frameCounter += 1.0;
			else if (NPC.velocity.Y < 0f)
				NPC.frameCounter -= 1.0;

			if (NPC.frameCounter < 6.0)
				NPC.frame.Y = 0;
			else if (NPC.frameCounter < 12.0)
				NPC.frame.Y = frameHeight;
			else if (NPC.frameCounter < 18.0)
				NPC.frame.Y = frameHeight * 2;

			if (NPC.frameCounter < 0.0)
				NPC.frameCounter = 0.0;
			if (NPC.frameCounter > 17.0)
				NPC.frameCounter = 17.0;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.FrostLegion,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				new FlavorTextBestiaryInfoElement("Explodes when getting too close. Running is advised.")
			});
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
				return;

			float distance = Vector2.Distance(NPC.Center, player.Center);

			if (distance < 110f && !exploded)
				Explode(player);
		}

		private void Explode(Player player)
		{
			exploded = true;

			SoundEngine.PlaySound(SoundID.Item14, NPC.Center);

			for (int i = 0; i < 70; i++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Snow);
				d.velocity = Main.rand.NextVector2Circular(6f, 6f);
				d.noGravity = true;
			}

			for (int i = 0; i < 40; i++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke);
				d.velocity = Main.rand.NextVector2Circular(7f, 7f);
			}

			if (Vector2.Distance(NPC.Center, player.Center) < 120f)
			{
			player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 60, NPC.direction);
			}

			if (Main.expertMode)
			{
				int count = Main.rand.Next(3, 6);

				for (int i = 0; i < count; i++)
				{
					Vector2 velocity = Main.rand.NextVector2CircularEdge(8f, 8f);
					Projectile.NewProjectile(
						NPC.GetSource_Death(),
						NPC.Center,
						velocity,
						ProjectileID.SnowBallFriendly,
						24,
						1f,
						Main.myPlayer
					);
				}
			}

			NPC.life = 0;
			NPC.checkDead();
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.SnowBlock, 1, 5, 10));
		}
	}
}