using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using System;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class EyeAI : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.EyeofCthulhu;
		public override bool PreAI(NPC npc) {
			if(!Main.masterMode || npc.despawnEncouraged) return true;
			bool autoSnap = true;
			bool suppressAI = false;
			float rotation = npc.rotation;
			float rotationSpeed = 0.1f;
			if(npc.ai[0] >= 3 && npc.life < npc.lifeMax * 0.4) switch(npc.ai[1]) {
				case 3:
					if(npc.ai[3] > 3) {
						npc.ai[1] = npc.life < npc.lifeMax * 0.12 || Main.player[npc.target].Center.Y - 14 * 16 > npc.Center.Y ? 6 : Main.player[npc.target].Center.Y + 12 * 16 < npc.Center.Y ? 7 : 8;
						npc.ai[2] = 0;
						npc.netUpdate = true;
						suppressAI = true;
					}
				break;
				case 6:
					if(npc.ai[2] == 0) {
						npc.ai[3] = -Math.Sign(Main.player[npc.target].Center.X - npc.Center.X);
						npc.netUpdate = true;
					}
					if(npc.ai[2] < 60) {
						Vector2 targetPos = Main.player[npc.target].Center;
						targetPos.Y -= 420f;
						targetPos.X += npc.ai[3] * 480f + (float)Math.Sin(npc.ai[2] / 60f * MathHelper.Pi) * 320f * npc.ai[3];
						npc.velocity += (targetPos - npc.Center) * 0.01f;
						npc.velocity *= 0.9f;
						rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathHelper.PiOver2;
					}
					else if(npc.ai[2] == 60f) {
						npc.ai[3] *= -1;
						npc.velocity = Vector2.UnitX * npc.ai[3] * 24f;
						npc.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Roar, npc.position);
					}
					else if(npc.ai[2] < 120) {
						rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
						Vector2 shootDir = Vector2.Normalize(npc.velocity);
						if(Main.netMode != 1 && npc.ai[2] % 6f == 0f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + shootDir * npc.width * 0.4f, shootDir * 4f, 811, 15, 0f, Main.myPlayer, 1000f);
					}
					else if(npc.ai[2] < 180) {
						Vector2 targetPos = Main.player[npc.target].Center;
						targetPos.X += npc.ai[3] * 640f + (float)Math.Sin((npc.ai[2] - 180) / 60f * MathHelper.Pi) * 240f * npc.ai[3];
						npc.velocity += (targetPos - npc.Center) * 0.01f;
						npc.velocity *= 0.9f;
						rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathHelper.PiOver2;
					}
					else if(npc.ai[2] == 180) {
						npc.ai[3] *= -1;
						npc.velocity = Vector2.UnitX * npc.ai[3] * 28f;
						npc.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Roar, npc.position);
					}
					else if(npc.ai[2] < 210) rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
					else {
						rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathHelper.PiOver2;
						npc.velocity *= 0.92f;
					}
					if(npc.ai[2] > 240) {
						npc.ai[1] = npc.life < npc.lifeMax * 0.12 ? 7 : 0;
						npc.ai[2] = 0;
						npc.ai[3] = 0;
						npc.netUpdate = true;
					}
					else npc.ai[2]++;
					suppressAI = true;
				break;
				case 7:
					if(npc.ai[2] == 0) {
						npc.ai[3] = npc.velocity.X != 0 ? -Math.Sign(npc.velocity.X) : Main.rand.NextBool() ? -1 : 1;
						npc.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Roar, npc.position);
					}
					if(npc.ai[2] < 120) {
						Vector2 targetPos = Main.player[npc.target].Center;
						targetPos -= Vector2.Normalize(targetPos - npc.Center).RotatedBy(npc.ai[3] * (Math.Sign(Main.player[npc.target].velocity.X) == Math.Sign(npc.velocity.X) ? 0.55f : 0.45f)) * 320f;
						npc.velocity += (targetPos - npc.Center) * 0.01f;
						npc.velocity *= 0.9f;
						rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
					}
					else {
						autoSnap = false;
						rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathHelper.PiOver2;
						rotationSpeed = 0.03f;
						Vector2 shootDir = (npc.rotation + MathHelper.PiOver2).ToRotationVector2();
						if(Main.netMode != 1 && npc.ai[2] % 6f == 0f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + shootDir * npc.width * 0.4f, shootDir * 16f, 811, 15, 0f, Main.myPlayer);
						if(npc.ai[2] % 6f == 0f) npc.velocity -= shootDir * 10f;
						npc.velocity *= 0.8f;
					}
					if(npc.ai[2] > 180) {
						npc.ai[1] = 0;
						npc.ai[2] = 0;
						npc.ai[3] = 0;
						npc.netUpdate = true;
					}
					else npc.ai[2]++;
					suppressAI = true;
				break;
				case 8:
					if(npc.ai[2] == 0) {
						npc.velocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 16f;
						npc.netUpdate = true;
						SoundEngine.PlaySound(SoundID.Roar, npc.position);
					}
					rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
					if(npc.ai[2] > 56f) npc.velocity *= 0.9f;
					else if(Main.netMode != 1 && npc.ai[2] > 12f && npc.ai[2] % 3f == 0f) Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 4f + Vector2.Normalize(npc.velocity) * npc.width * 0.4f, Vector2.Normalize(npc.velocity).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.1f) * 16f, 811, 15, 0f, Main.myPlayer).velocity *= 1.5f;
					if(npc.ai[2] > 60) {
						npc.ai[1] = npc.life < npc.lifeMax * 0.12 || Main.player[npc.target].Center.Y - 14 * 16 > npc.Center.Y ? 6 : Main.player[npc.target].Center.Y + 12 * 16 < npc.Center.Y ? 7 : 0;
						npc.ai[2] = 0;
						npc.netUpdate = true;
					}
					else npc.ai[2]++;
					suppressAI = true;
				break;
			}
			if(npc.ai[0] == 1 || npc.ai[0] == 2) npc.dontTakeDamage = true;
			else if(npc.dontTakeDamage) npc.dontTakeDamage = false;
			if(suppressAI) {
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				else if(rotation > MathHelper.TwoPi) rotation -= MathHelper.TwoPi;
				if(npc.rotation < rotation) {
					if(rotation - npc.rotation > MathHelper.Pi) npc.rotation -= rotationSpeed;
					else npc.rotation += rotationSpeed;
				}
				else if(npc.rotation > rotation) {
					if(npc.rotation - rotation > MathHelper.Pi) npc.rotation += rotationSpeed;
					else npc.rotation -= rotationSpeed;
				}
				if(autoSnap && npc.rotation > rotation - rotationSpeed && npc.rotation < rotation + rotationSpeed && npc.rotation != rotation) npc.rotation = rotation;
				if(npc.rotation < 0f) npc.rotation += MathHelper.TwoPi;
				else if(npc.rotation > MathHelper.TwoPi) npc.rotation -= MathHelper.TwoPi;
				if(autoSnap && npc.rotation > rotation - rotationSpeed && npc.rotation < rotation + rotationSpeed && npc.rotation != rotation) npc.rotation = rotation;
			}
			return !suppressAI;
		}
	}
}
