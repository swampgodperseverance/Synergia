using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Synergia.Content.Projectiles.Hostile;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class LothorAttackSystem : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		private int _attackCooldown = 0;
		// DefensiveRing
		private int _defensiveRingID = -1;
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC != null && npc.ModNPC.Mod.Name == "RoA" && npc.ModNPC.Name == "Lothor";
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode == 0) return;
			binaryWriter.Write(_attackCooldown);
			binaryWriter.Write(_defensiveRingID);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode == 0) return;
			_attackCooldown = binaryReader.ReadInt32();
			_defensiveRingID = binaryReader.ReadInt32();
		}
		public override void AI(NPC npc) {
			if(npc.life < npc.lifeMax * 0.25f && _defensiveRingID == -1) {
				for(int i = 0; i < 50; i++) {
					Dust.NewDustPerfect(
						npc.Center,
						DustID.LifeDrain,
						Main.rand.NextVector2Circular(10, 10),
						0,
						Color.DarkRed,
						2.5f
					).noGravity = true;
				}
				SpawnDefensiveRing(npc);
			}
			if(_defensiveRingID != -1 && !Main.projectile[_defensiveRingID].active) _defensiveRingID = -1;
			if(npc.life < npc.lifeMax * 0.75 && (npc.ai[3] == 3f || npc.ai[3] == 7f) && Main.netMode != 1) foreach(Projectile projectile in Main.ActiveProjectiles) if(projectile.ModProjectile?.Mod == npc.ModNPC?.Mod && projectile.ai[0] == 1f && projectile.ModProjectile?.Name == "LothorScream") {
				Vector2 spreadDirection = Vector2.Normalize(Main.player[npc.target].Center - projectile.Center).RotatedByRandom(MathHelper.ToRadians(15));
				if(++_attackCooldown > 2) Projectile.NewProjectile(
					npc.GetSource_FromAI(),
					projectile.Center + npc.velocity,
					spreadDirection * (6f + Main.rand.NextFloat(-1f, 1f)),
					ModContent.ProjectileType<PrimordialBlood>(),
					npc.damage / 4,
					2f,
					Main.myPlayer
				);
				if(_attackCooldown > 2) _attackCooldown = 0;
			}
		}
		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if(_defensiveRingID != -1 && npc.life < npc.lifeMax * 0.25f) modifiers.FinalDamage *= 0.80f; 
		}
		private void SpawnDefensiveRing(NPC boss) {
			_defensiveRingID = Projectile.NewProjectile(
				boss.GetSource_FromAI(),
				boss.Center,
				Vector2.Zero,
				ModContent.ProjectileType<DefensiveRing>(),
				1,
				0f,
				Main.myPlayer,
				boss.whoAmI
			);
			Projectile ring = Main.projectile[_defensiveRingID];
			ring.timeLeft = int.MaxValue;
			ring.scale = 1.3f;
			ring.hostile = false;
			ring.friendly = false;
		}
	}
}
