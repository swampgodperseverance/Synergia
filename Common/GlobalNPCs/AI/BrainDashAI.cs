using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.IO;
using Synergia.Content.Projectiles.Boss.BrainOfCthulhuBuff;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class BrainDashAI : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		private int dashCooldown = 0;

		private bool dashing = false;

		private bool spawnBrainWaves = false;

		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.BrainofCthulhu;

		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode == 0) return;
			bitWriter.WriteBit(spawnBrainWaves);
			bitWriter.WriteBit(dashing);
			binaryWriter.Write(dashCooldown);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode == 0) return;
			spawnBrainWaves = bitReader.ReadBit();
			dashing = bitReader.ReadBit();
			dashCooldown = binaryReader.ReadInt32();
		}
		public override bool PreAI(NPC npc) {
			if(spawnBrainWaves && npc.ai[0] == -1) {
				SoundEngine.PlaySound(SoundID.Item20 with { Pitch = -0.4f }, npc.Center);
				if(Main.netMode != 1) for(int i = -(int)npc.localAI[2]; i <= (int)npc.localAI[2]; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Normalize(Main.player[npc.target].Center - npc.Center).RotatedBy(i * MathHelper.PiOver4 * 0.8f) * 10f, ModContent.ProjectileType<BrainWaves>(), 25, 0f, Main.myPlayer);
				spawnBrainWaves = false;
			}
			if(npc.localAI[2] > 0f) if(++dashCooldown > 300 && (npc.alpha == 255 || dashing)) {
				if(!dashing) {
					dashing = true;
					dashCooldown = 300;
					npc.Center = Main.player[npc.target].Center + new Vector2(Main.rand.NextBool() ? 320 : -320, Main.rand.NextBool() ? 320 : -320);
					npc.netUpdate = true;
				}
				else if(dashCooldown < 315) {
					SoundEngine.PlaySound(SoundID.Item46 with { Pitch = -0.4f }, npc.Center);
					if(dashCooldown == 301 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<BrainofCthulhu>(), 25, 10f, Main.myPlayer);
					npc.velocity *= 0f;
				}
				else if(dashCooldown == 315) npc.velocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 16f;
				else if(dashCooldown > 360) {
					dashCooldown = 0;
					dashing = false;
					spawnBrainWaves = Main.masterMode;
					npc.netUpdate = true;
				}
				else if(dashCooldown > 345) npc.velocity *= 0.9f;
				return false;
			}
			return true;
		}
		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if(dashing) modifiers.Knockback *= 0f;
		}
	}
}
