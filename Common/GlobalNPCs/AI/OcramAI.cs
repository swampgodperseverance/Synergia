using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Reflection;
using Synergia.Content.Projectiles.Boss.OcramBuff
using Consolaria.Content.NPCs.Bosses.Ocram;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class OcramAI : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.ModNPC is Ocram;
		public override bool PreAI(NPC npc) {
			Ocram o = npc.ModNPC as Ocram;
			if(npc.life < npc.lifeMax * (Main.expertMode ? 0.65 : 0.5) && npc.ai[0] != 3f) return true;
			else if(npc.ai[0] == 0f && npc.ai[1] == 0f && npc.ai[2] == 0f) try {
				if(o.GetType().GetField("_spawnCheck", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o) is float f && f < 100f) return true;
			}
			catch {
			}
			if(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active) {
				npc.TargetClosest();
				if((npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active) && !npc.despawnEncouraged) {
					npc.EncourageDespawn(30);
					npc.velocity.Y--;
				}
			}
			Vector2 targetPos = Main.player[npc.target].Center;
			Vector2 shootDir = targetPos - npc.Center;
			float targetRot = shootDir.ToRotation() - MathHelper.PiOver2;
			float turnSpeed = 0.1f;
			bool? trail = null;
			if(npc.ai[0] == 3f) switch(npc.ai[1]) {
				case 0:
					if(npc.ai[2] > 90f) {
						npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation) * 0.1f;
						turnSpeed = 0.05f;
						if(npc.ai[2] % 10 == 0f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - npc.rotation.ToRotationVector2() * npc.width / 3f * (npc.ai[2] % 20 == 0 ? -1 : 1), (npc.rotation + MathHelper.PiOver4 * 0.6f * (npc.ai[2] % 20 == 0 ? -1 : 1) + MathHelper.PiOver2).ToRotationVector2() * 2f, o.Mod.Find<ModProjectile>("OcramSkull").Type, 30, 4f, Main.myPlayer);
					}
					else {
						if(npc.ai[2] == 90f) try {
							o.GetType().GetMethod("AddGlow", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, new object[] {20f, 0.95f, Color.BlueViolet});
						}
						catch {
						}
						npc.velocity += (targetPos - Vector2.UnitY * 320f - npc.Center) * 0.018f * npc.ai[2] / 90f;
						turnSpeed *= npc.ai[2] / 45f;
					}
					if(++npc.ai[2] > 180f) {
						if(++npc.ai[3] > 2f) {
							npc.ai[1]++;
							npc.ai[3] = 0f;
							npc.ai[2] = -30f;
						}
						else npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
					}
					npc.velocity *= 0.92f;
				break;
				case 1:
					if(npc.ai[2] < 0f) {
						if(npc.ai[2] == -15f) try {
							o.GetType().GetMethod("AddGlow", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, new object[] {20f, 0.85f, Color.BlueViolet});
						}
						catch {
						}
						npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation);
						npc.velocity *= 0.92f;
					}
					else if(npc.ai[2] < 60f) {
						trail = true;
						if(npc.ai[2] == 0f) npc.velocity = Vector2.Normalize(shootDir) * 24f;
						else npc.velocity += shootDir * 0.00024f;
						targetRot = npc.velocity.ToRotation() - MathHelper.PiOver2;
					}
					else {
						trail = false;
						turnSpeed = 0.05f;
						npc.velocity = Vector2.UnitY.RotatedBy(npc.rotation) * npc.velocity.Length() * 0.96f;
					}
					if(++npc.ai[2] > 90f) {
						if(++npc.ai[3] > 2f) {
							npc.ai[1]++;
							npc.ai[3] = 0f;
							npc.ai[2] = -60f;
						}
						npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
						trail = false;
					}
				break;
				case 2:
					if(npc.ai[2] >= 0f && npc.ai[2] <= 40f) try {
						Type type = o.GetType();
						type.GetField("predictScale", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(o, (float)Math.Sin(npc.ai[2] / 40f * MathHelper.Pi));
						type.GetField("scytheSpawnRotation", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(o, npc.velocity.ToRotation() + (npc.ai[3] % 2 == 0 ? MathHelper.PiOver4 : -MathHelper.PiOver4));
						if(npc.ai[2] == 40f && Main.netMode != 1 && type.GetField("scytheSpawnRotation", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o) is float scytheSpawnRotation && type.GetField("scytheSpawnOffset", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o) is Vector2[] scytheSpawnOffset) foreach(Vector2 spawnOffset in scytheSpawnOffset) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + spawnOffset.RotatedBy(scytheSpawnRotation), Vector2.Normalize(spawnOffset).RotatedBy(scytheSpawnRotation), o.Mod.Find<ModProjectile>("OcramScythe").Type, 36, 4f, Main.myPlayer);
					}
					catch {
					}
					npc.velocity += shootDir * 0.003f;
					if(++npc.ai[2] > 90f) {
						if(Main.netMode != 1) for(int i = 0; i < 4; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Main.rand.NextVector2Circular(4f, 4f), ModContent.ProjectileType<OcramBomb>(), 32, 1f, Main.myPlayer, npc.target + 1);
						if(++npc.ai[3] > 3f) {
							npc.ai[1]++;
							npc.ai[3] = 0f;
						}
						npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
						trail = false;
					}
					npc.velocity *= 0.9f;
				break;
				case 3:
					if(npc.ai[2] > 0f) {
						if(Main.netMode != 1 && npc.ai[2] % 20f == 0) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<OcramKnife>(), 32, 1f, Main.myPlayer, npc.target + 1);
						targetRot = npc.velocity.ToRotation() - MathHelper.PiOver2;
						targetPos -= Vector2.Normalize(targetPos - npc.Center).RotatedBy(npc.ai[3]) * 320f;
						npc.velocity += (targetPos - npc.Center) * 0.01f;
					}
					else {
						trail = true;
						if(trail ?? false) npc.ai[3] = Math.Sign(shootDir.X);
						npc.velocity += shootDir * 0.003f;
					}
					if(++npc.ai[2] > 240f) {
						npc.ai[1]++;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						npc.velocity *= 0.5f;
						npc.netUpdate = true;
						trail = false;
					}
					npc.velocity *= 0.9f;
				break;
				case 4:
					if(npc.ai[2] > 90f) {
						npc.velocity += (targetPos - Vector2.UnitY * 420f - npc.Center) * 0.018f - Vector2.UnitY.RotatedBy(npc.rotation) * 0.1f;
						turnSpeed = 0.05f;
						if(npc.ai[2] % 12 == 0f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - npc.rotation.ToRotationVector2() * npc.width / 3f * (npc.ai[2] % 24 == 0 ? -1 : 1), (npc.rotation + MathHelper.PiOver2 * 1.2f * (npc.ai[2] % 24 == 0 ? -1 : 1) + MathHelper.PiOver2).ToRotationVector2() * 2f, ModContent.ProjectileType<OcramSkull>(), 30, 4f, Main.myPlayer);
					}
					else {
						if(npc.ai[2] == 90f) try {
							o.GetType().GetMethod("AddGlow", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, new object[] {20f, 0.95f, Color.BlueViolet});
						}
						catch {
						}
						npc.velocity += (targetPos - Vector2.UnitY * 320f - npc.Center) * 0.018f * npc.ai[2] / 90f;
						turnSpeed *= npc.ai[2] / 45f;
					}
					if(++npc.ai[2] > 120f) {
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
					}
					npc.velocity *= 0.92f;
				break;
			}
			if(npc.ai[0] == 0f) switch(npc.ai[1]) {
				case 0:
					if(npc.ai[2] > 90f) {
						npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation) * 0.1f;
						turnSpeed = 0.05f;
						if(npc.ai[2] % 6 == 0f && Main.netMode != 1) if(npc.ai[3] % 2 == 0) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(npc.rotation) * 12f, o.Mod.Find<ModProjectile>("OcramLaser1").Type, 35, 0f, Main.myPlayer);
						else for(int i = -1; i <= 1; i += 2) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - npc.rotation.ToRotationVector2() * npc.width / 5f * i, (npc.rotation + MathHelper.PiOver4 * 0.4f * i + i * MathHelper.Lerp(MathHelper.PiOver4 * 0.6f, MathHelper.PiOver4 * -0.4f, (npc.ai[2] - 90f) / 30f) + MathHelper.PiOver2).ToRotationVector2() * 12f, o.Mod.Find<ModProjectile>("OcramLaser2").Type, 33, 1f, Main.myPlayer);
					}
					else {
						if(npc.ai[2] == 90f) try {
							o.GetType().GetMethod("AddGlow", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, new object[] {30f, 0.65f, npc.ai[3] % 2 == 0 ? Color.Red : Color.BlueViolet});
						}
						catch {
						}
						npc.velocity += (targetPos - Vector2.UnitY * 320f - npc.Center) * 0.018f * npc.ai[2] / 90f;
						turnSpeed *= npc.ai[2] / 45f;
					}
					if(++npc.ai[2] > 120f) {
						if(++npc.ai[3] > 3f) {
							npc.ai[1]++;
							npc.ai[3] = 0f;
							npc.ai[2] = -60f;
						}
						else npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
					}
					npc.velocity *= 0.92f;
				break;
				case 1:
					if(npc.ai[2] < 0f) {
						if(npc.ai[2] == -15f) try {
							o.GetType().GetMethod("AddGlow", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, new object[] {20f, 0.85f, Color.Red});
						}
						catch {
						}
						npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation);
						npc.velocity *= 0.92f;
					}
					else if(npc.ai[2] < 60f) {
						trail = true;
						if(npc.ai[2] == 0f) npc.velocity = Vector2.Normalize(shootDir) * 24f;
						else npc.velocity += shootDir * 0.00024f;
						targetRot = npc.velocity.ToRotation() - MathHelper.PiOver2;
					}
					else {
						trail = false;
						turnSpeed = 0.05f;
						npc.velocity = Vector2.UnitY.RotatedBy(npc.rotation) * npc.velocity.Length() * 0.98f;
					}
					if(++npc.ai[2] > 120f) {
						if(++npc.ai[3] > 3f) {
							npc.ai[1]++;
							npc.ai[3] = 0f;
						}
						npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
						trail = false;
					}
				break;
				case 2:
					if(npc.ai[2] > 120f) {
						if(Main.netMode != 1 && npc.ai[2] % 20f == 0) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Normalize(shootDir), o.Mod.Find<ModProjectile>("OcramScythe").Type, 32, 1f, Main.myPlayer);
						targetRot = npc.velocity.ToRotation() - MathHelper.PiOver2;
						targetPos -= Vector2.Normalize(targetPos - npc.Center).RotatedBy(npc.ai[3]) * 320f;
						npc.velocity += (targetPos - npc.Center) * 0.01f;
					}
					else {
						trail = npc.ai[2] == 120f;
						if(trail ?? false) npc.ai[3] = Math.Sign(shootDir.X);
						npc.velocity += shootDir * 0.003f;
					}
					if(++npc.ai[2] > 480f) {
						npc.ai[1]++;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						npc.velocity *= 0.5f;
						npc.netUpdate = true;
						trail = false;
					}
					npc.velocity *= 0.9f;
				break;
				case 3:
					if(npc.ai[2] > 90f) {
						npc.velocity -= Vector2.UnitY.RotatedBy(npc.rotation) * 0.1f;
						turnSpeed = 0.05f;
						if(npc.ai[2] % 15 == 0f && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - npc.rotation.ToRotationVector2() * npc.width / 3f * (npc.ai[2] % 30 == 0 ? -1 : 1), (npc.rotation + MathHelper.PiOver4 * 0.4f * (npc.ai[2] % 30 == 0 ? -1 : 1) + MathHelper.PiOver2).ToRotationVector2() * 2f, o.Mod.Find<ModProjectile>("OcramSkull").Type, 30, 4f, Main.myPlayer);
					}
					else {
						if(npc.ai[2] == 90f) try {
							o.GetType().GetMethod("AddGlow", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, new object[] {20f, 0.95f, Color.BlueViolet});
						}
						catch {
						}
						npc.velocity += (targetPos - Vector2.UnitY * 320f - npc.Center) * 0.018f * npc.ai[2] / 90f;
						turnSpeed *= npc.ai[2] / 45f;
					}
					if(++npc.ai[2] > 180f) {
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
						npc.TargetClosest();
					}
					npc.velocity *= 0.92f;
				break;
			}
			if(turnSpeed > 0) {
				if(targetRot < 0f) targetRot += MathHelper.TwoPi;
				else if(targetRot > MathHelper.TwoPi) targetRot -= MathHelper.TwoPi;
				if(npc.rotation < targetRot) if(targetRot - npc.rotation > MathHelper.Pi) npc.rotation -= turnSpeed;
				else npc.rotation += turnSpeed;
				else if(npc.rotation > targetRot) if(npc.rotation - targetRot > MathHelper.Pi) npc.rotation += turnSpeed;
				else npc.rotation -= turnSpeed;
				if(npc.rotation > targetRot - turnSpeed && npc.rotation < targetRot + turnSpeed && npc.rotation != targetRot) npc.rotation = targetRot;
				if(npc.rotation < 0f) npc.rotation += MathHelper.TwoPi;
				else if(npc.rotation > MathHelper.TwoPi) npc.rotation -= MathHelper.TwoPi;
				if(npc.rotation > targetRot - turnSpeed && npc.rotation < targetRot + turnSpeed && npc.rotation != targetRot) npc.rotation = targetRot;
			}
			if(trail != null) try {
				o.GetType().GetField("drawTrail", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(o, trail ?? false);
			}
			catch {
			}
			return false;
		}
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			if(Main.netMode == 0) return;
			binaryWriter.Write(npc.rotation);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			if(Main.netMode == 0) return;
			npc.rotation = binaryReader.ReadSingle();
		}
	}
}
