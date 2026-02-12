using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Items.Misc;
using Synergia.Content.Items.Placeable;
using Synergia.Content.Items.Weapons.Cogworm;
using Synergia.Content.Projectiles.Boss.SinlordWyrm;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Placeable.Blocks;

namespace Synergia.Content.NPCs.Boss.SinlordWyrm
{
	[AutoloadBossHead]
	public class Sinlord : ModNPC
	{
		private bool openMouth = false;
		private bool playScreenshake = false;
		private Vector2 storedPos = Vector2.Zero;
		private List<int> segments = new List<int>();
		public override void SetStaticDefaults() {
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageNPC", NPC, true);
			NPC.lifeMax = 100000;
			NPC.damage = 85;
			NPC.defense = 30;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.knockBackResist = 0f;
			NPC.boss = true;
			NPC.npcSlots = 6f;
            NPC.HitSound = new SoundStyle($"{Mod.Name}/Assets/Sounds/CragwormHit");
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.Size = new Vector2(80f * NPC.scale);
			NPC.aiStyle = -1;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod Calamity)) if((bool)Calamity.Call("GetDifficultyActive", "BossRush")) {
				NPC.lifeMax *= 40;
				NPC.defense += 40;
			}
			else if((bool)Calamity.Call("GetDifficultyActive", "Death")) {
				NPC.lifeMax += NPC.lifeMax / 5;
				NPC.defense += 12;
			}
			else if((bool)Calamity.Call("GetDifficultyActive", "Revengeance")) {
				NPC.lifeMax += NPC.lifeMax / 10;
				NPC.defense += 6;
			}
			NPC.lifeMax = (int)(balance * bossAdjustment * NPC.lifeMax * 0.5f);
		}
		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;
		public override bool CheckDead() {
			if(NPC.ai[0] == -1f) return true;
			NPC.dontTakeDamage = true;
			NPC.ai[0] = -1f;
			NPC.ai[1] = 0f;
			NPC.life = 1;
			NPC.netUpdate = true;
			return false;
		}
		public override void OnKill() {
			if(Main.netMode != 1) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), 0, 0f, Main.myPlayer);
				foreach(int i in segments) if(Main.npc[i].active) Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.npc[i].Center, Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), 0, 0f, Main.myPlayer);
			}
		}
		public override void AI() {
			bool masterMode = Main.masterMode;
			bool legendaryMode = Main.getGoodWorld;
			if(ModLoader.TryGetMod("InfernumMode", out Mod Infernum) && Infernum.Call("GetInfernumActive") is bool im && im) legendaryMode = masterMode = true;
			if(ModLoader.TryGetMod("FargowiltasSouls", out Mod FargoSouls)) {
				masterMode |= FargoSouls.Call("EternityMode") is bool em && em;
			}
			bool calamity = false;
			if(ModLoader.TryGetMod("CalamityMod", out Mod Calamity)) {
				calamity = (Calamity.Call("GetDifficultyActive", "Revengeance") is bool rm && rm) || (Calamity.Call("GetDifficultyActive", "Death") is bool dm && dm) || (Calamity.Call("GetDifficultyActive", "Bossrush") is bool br && br);
				masterMode |= calamity;
			}
			if(NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active) {
				NPC.TargetClosest();
				if(NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active) {
					if(NPC.timeLeft > 30) NPC.timeLeft = 30;
					NPC.velocity = NPC.velocity.RotatedBy(NPC.velocity.X > 0 ? MathHelper.PiOver4 : -MathHelper.PiOver4);
					NPC.alpha = (int)MathHelper.Lerp(255f, 0f, (float)NPC.timeLeft * 0.05f);
					NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
					return;
				}
				NPC.netUpdate = true;
			}
			bool phase2 = NPC.life < NPC.lifeMax * (Main.expertMode ? 0.75 : 0.5);
			Vector2 targetPos = Main.player[NPC.target].Center;
			Vector2 shootDir = targetPos - NPC.Center;
			int projectileDamage = NPC.damage;
			if(Main.masterMode) projectileDamage /= 3;
			else if(Main.expertMode) projectileDamage /= 2;
			bool playScreenshake = Collision.SolidCollision(NPC.position, NPC.width, NPC.height);
			if(Main.expertMode && NPC.life < NPC.lifeMax * 0.25 && NPC.ai[0] < 5f && NPC.ai[0] != -1f && NPC.ai[1] == 0f) {
				NPC.ai[0] = 5f;
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
				NPC.ai[3] = 0f;
				NPC.dontTakeDamage = true;
				NPC.netUpdate = true;
			}
			else switch(NPC.ai[0]) {
				case -1:
					NPC.dontTakeDamage = true;
					NPC.ai[1]++;
					NPC.localAI[0] = NPC.ai[1] / 120f;
					if(Main.netMode != 1 && NPC.ai[1] % 4 == 0) {
						Vector2 spawnPos = Main.npc[Main.rand.Next(segments.ToArray())].Center;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos + Main.rand.NextVector2Circular(80f, 80f), Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), projectileDamage / 3, 0f, Main.myPlayer);
					}
					if(NPC.ai[1] < 120f) {
						foreach(int i in segments) if(Main.npc[i].active) {
							Main.npc[i].life = 1;
							Main.npc[i].localAI[0] = NPC.localAI[0];
							if(Main.npc[i].ai[2] == 0f) Main.npc[i].velocity = Main.npc[i].rotation.ToRotationVector2() * NPC.velocity.Length();
						}
						NPC.life = 1;
					}
					else {
						NPC.StrikeInstantKill();
						foreach(int i in segments) if(Main.npc[i].active) Main.npc[i].StrikeInstantKill();
					}
					NPC.velocity *= 0.95f;
				break;
				case 0:
					NPC.Center = targetPos + Vector2.UnitY * 1600f;
					NPC.velocity = -Vector2.UnitY * 16f;
					if(Main.netMode != 1) {
						int segments = 10;
						if(Main.getGoodWorld) segments *= 2;
						segments--;
						int attachTo = NPC.whoAmI;
						for(int i = 0; i <= segments; i++) {
							int s = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, i == segments ? ModContent.NPCType<SinlordTail>() : ModContent.NPCType<SinlordBody>(), NPC.whoAmI);
							Main.npc[s].ai[2] = attachTo + 1;
							Main.npc[s].position.Y += NPC.width + i * Main.npc[s].height;
							Main.npc[s].ai[3] = NPC.whoAmI + 1;
							if(Main.netMode == 2 && s < 200) NetMessage.SendData(23, -1, -1, null, s);
							attachTo = s;
							this.segments.Add(s);
						}
					}
					NPC.ai[0]++;
					NPC.dontTakeDamage = true;
					NPC.netUpdate = true;
				break;
				case 1:
					if(NPC.Center.Y < targetPos.Y || NPC.ai[1] > 0f) {
						NPC.velocity *= 0.95f;
						NPC.ai[1]++;
					}
					if(NPC.ai[1] > 180f) {
						NPC.dontTakeDamage = false;
						NPC.ai[0]++;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.localAI[0] = 0f;
						NPC.netUpdate = true;
						openMouth = false;
						NPC.TargetClosest();
					}
					else if(NPC.ai[1] > 60f) {
						NPC.localAI[0] = (float)Math.Abs(Math.Sin(NPC.ai[1] / 30f * MathHelper.TwoPi)) * 0.2f;
						openMouth = true;
						if(Main.netMode != 1 && NPC.ai[1] % 10 == 0) Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, Vector2.Zero, ModContent.ProjectileType<BurningScream>(), 0, 0f, Main.myPlayer, 20f);
						NPC.velocity += shootDir.SafeNormalize(NPC.velocity) * 0.01f;
						if(NPC.ai[1] % 10 == 0) Main.instance.CameraModifiers.Add(new PunchCameraModifier(NPC.Center, Vector2.Normalize(NPC.velocity), NPC.velocity.Length(), 10, 60, 2400f, "Sinlord Screenshake"));
						NPC.velocity *= 0.95f;
					}
					else if(NPC.ai[1] > 30f) {
						if(NPC.ai[1] == 60f) SoundEngine.PlaySound(SoundID.NPCDeath10 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.4f }, NPC.Center);
						NPC.velocity += shootDir.SafeNormalize(NPC.velocity) * (1f - (NPC.ai[1] - 30f) / 30f);
						NPC.velocity *= 0.95f;
					}
				break;
				case 2:
					NPC.dontTakeDamage = false;
					if(NPC.ai[2] == 0f && Main.netMode != 1) {
						NPC.ai[2] = Main.rand.NextBool() ? 1f : -1f;
						NPC.netUpdate = true;
					}
					float actualAttackTime = masterMode ? 90f : 120f;
					if(NPC.ai[1] < actualAttackTime) {
						if(NPC.ai[1] < 30f && phase2 && NPC.ai[3] == 2f) NPC.ai[3]++;
						float distance = shootDir.Length();
						float offset = MathHelper.Clamp(distance - 320f, -20f, 20f);
						NPC.velocity = Vector2.Normalize(shootDir).RotatedBy(MathHelper.ToRadians(MathHelper.Min(90f, NPC.ai[1] * 3f) - MathHelper.Min(1f, NPC.ai[1] / 30f) * offset) * NPC.ai[2]) * MathHelper.Min(NPC.ai[1] / 4f + 4f, 16f);
					}
					else if(NPC.ai[1] < actualAttackTime + 30f) {
						openMouth = (int)(NPC.ai[1] / 6) % 2 == 0;
						if(NPC.ai[3] < 3f) NPC.localAI[0] = 0f;
						else NPC.localAI[0] = (NPC.ai[1] - actualAttackTime) / 30f;
						NPC.velocity = Vector2.Normalize(shootDir).RotatedBy(MathHelper.ToRadians(MathHelper.SmoothStep(90f - MathHelper.Clamp(shootDir.Length() - 480f, -10f, 10f), 0f, MathHelper.Min(1f, (NPC.ai[1] - actualAttackTime) / 30f))) * NPC.ai[2]) * (16f - MathHelper.Min((NPC.ai[1] - actualAttackTime) / 2f, 12f));
					}
					else if(NPC.ai[1] < actualAttackTime + 90f) {
						if(NPC.ai[1] == actualAttackTime + 30f) SoundEngine.PlaySound(SoundID.Item46 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.4f, Volume = 4f }, NPC.Center);
						openMouth = true;
						NPC.localAI[0] = (float)Math.Sin((NPC.ai[1] - actualAttackTime - 30f) * MathHelper.Pi / 60f);
						NPC.velocity = Vector2.Normalize(NPC.velocity) * (NPC.localAI[0] * 28f + 4f);
						if(NPC.ai[3] < 3f) NPC.localAI[0] *= 0.25f;
						else NPC.localAI[0] = 1f;
						int l = Dust.NewDust(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * NPC.width / 3 - Vector2.One * 2f, 0, 0, 6);
						Main.dust[l].noGravity = true;
						Main.dust[l].scale *= 2.1f;
						Main.dust[l].velocity = NPC.velocity.RotatedBy(MathHelper.PiOver2) * 0.2f;
						l = Dust.NewDust(NPC.Center - Vector2.UnitY.RotatedBy(NPC.rotation) * NPC.width / 3 - Vector2.One * 2f, 0, 0, 6);
						Main.dust[l].noGravity = true;
						Main.dust[l].scale *= 2.1f;
						Main.dust[l].velocity = NPC.velocity.RotatedBy(-MathHelper.PiOver2) * 0.2f;
					}
					else {
						if(NPC.ai[1] == actualAttackTime + 90f && Main.netMode != 1) switch(NPC.ai[3]) {
							case 0:
								for(int i = -10; i <= 10; i++) Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos - new Vector2(i * 128, Main.rand.Next(32) + 640), Main.rand.NextVector2Circular(1f, 3f), ModContent.ProjectileType<LavaStalactite>(), projectileDamage / 3, 0f, Main.myPlayer);
							break;
							case 1:
								for(int i = 0; i < 18; i++) Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootDir.SafeNormalize(Vector2.Zero).RotatedBy(i / 18f * MathHelper.TwoPi) * Main.rand.Next(8, 16), ModContent.ProjectileType<HellMeteor2>(), projectileDamage / 3, 0f, Main.myPlayer);
							break;
							case 3:
								for(int i = 0; i < 18; i++) Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos + Main.rand.NextVector2Circular(120f * (i + 1), (i + 1) * 90f), Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), projectileDamage / 3, 0f, Main.myPlayer, 30f + i * 5f);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, Vector2.Zero, ModContent.ProjectileType<BurningScream>(), 0, 0f, Main.myPlayer, 20f);
							break;
						}
						if(NPC.ai[3] < 3f) NPC.localAI[0] = 0f;
						else NPC.localAI[0] = 1f - (NPC.ai[1] - actualAttackTime - 90f) / 30f;
						openMouth = false;
						NPC.velocity = Vector2.Normalize(Vector2.Lerp(NPC.velocity, shootDir, (NPC.ai[1] - actualAttackTime - 90f) / 30f)).RotatedBy(MathHelper.ToRadians(90f - MathHelper.Clamp(shootDir.Length() - 480f, -10f, 10f) * (NPC.ai[1] - actualAttackTime - 90f) / 30f) * NPC.ai[2]) * 4f;
					}
					if(++NPC.ai[1] > actualAttackTime + 120f) {
						if(NPC.ai[3] >= 2f) {
							NPC.ai[0]++;
							NPC.ai[3] = 0f;
						}
						else NPC.ai[3]++;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.netUpdate = true;
						openMouth = false;
						NPC.TargetClosest();
					}
				break;
				case 3:
				case 8:
					openMouth = NPC.ai[1] > 90f && NPC.ai[1] < 150f;
					if(!openMouth) {
						if(phase2 && NPC.ai[3] == 2f) if(NPC.ai[1] <= 90f) NPC.localAI[0] = NPC.ai[1] / 90f;
						else if(NPC.ai[1] >= 150f && NPC.ai[1] <= 180f) NPC.localAI[0] = 1f - (NPC.ai[1] - 150f) / 30f;
						if(storedPos != Vector2.Zero) {
							NPC.velocity += (storedPos - NPC.Center).SafeNormalize(NPC.velocity);
							if(NPC.Distance(storedPos) < 80f + NPC.velocity.Length()) storedPos = Vector2.Zero;
						}
						else NPC.velocity += shootDir.SafeNormalize(NPC.velocity) * (phase2 && NPC.ai[3] == 2f ? 0.75f : 0.35f);
						if(NPC.ai[0] == 8f && NPC.ai[3] == 1f) NPC.ai[1] += 2f;
						if(NPC.ai[0] == 8f && NPC.ai[3] == 2f && NPC.ai[1] < 90f) NPC.ai[1]++;
					}
					else {
						if(phase2 && NPC.ai[3] < 2f) NPC.velocity += shootDir.SafeNormalize(NPC.velocity) * 0.05f;
						if(Main.netMode != 1) if(phase2 && NPC.ai[3] == 2f) {
							for(int i = -4; i <= 4; i++) Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, NPC.velocity.SafeNormalize(NPC.rotation.ToRotationVector2()).RotatedBy(MathHelper.ToRadians(i * 3f)) * 9f, ModContent.ProjectileType<SinlordFireBreath>(), projectileDamage / 4, 0f, Main.myPlayer);
							if(NPC.ai[1] == 91f) Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos, Vector2.Zero, ModContent.ProjectileType<BurningAura>(), 0, 0f, Main.myPlayer, 75f);
							else if(NPC.ai[0] == 8f && NPC.ai[1] == 120f) foreach(int i in segments) if(Main.rand.NextBool(2) || masterMode) Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.npc[i].Center + NPC.velocity, Vector2.UnitY.RotatedBy(Main.npc[i].rotation) * (Main.rand.NextBool(2) ? -4f : 4f), ModContent.ProjectileType<HellMeteor3>(), projectileDamage / 3, 0f, Main.myPlayer, NPC.target + 1, Main.rand.Next(30, 91));
						}
						else if(NPC.ai[1] % 4 == 0) Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, NPC.velocity.SafeNormalize(NPC.rotation.ToRotationVector2()) * 9f + Main.rand.NextVector2Circular(6f, 6f), ModContent.ProjectileType<LavaBone>(), projectileDamage / 4, 0f, Main.myPlayer);
						if(NPC.ai[1] % 4 == 0) SoundEngine.PlaySound((phase2 && NPC.ai[3] == 2f ? SoundID.DD2_FlameburstTowerShot : SoundID.Item88) with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.8f, Volume = 4f }, NPC.Center);
						Gore gore = Main.gore[Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3 - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64))];
						gore.velocity = NPC.velocity.SafeNormalize(NPC.rotation.ToRotationVector2()) * 9f + Main.rand.NextVector2Circular(6f, 6f);
						gore.scale *= 0.8f;
						int l = Dust.NewDust(NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3 + Vector2.One * 4, 0, 0, 6);
						Main.dust[l].noGravity = true;
						Main.dust[l].scale *= 2.1f;
						Main.dust[l].velocity = NPC.velocity.SafeNormalize(NPC.rotation.ToRotationVector2()) * 9f + Main.rand.NextVector2Circular(6f, 6f);
					}
					NPC.velocity *= 0.95f;
					if(++NPC.ai[1] > (NPC.ai[3] < (phase2 ? 2f : 1f) ? 150f : 240f)) {
						if(NPC.ai[3] >= (phase2 ? 2f : 1f)) {
							if(NPC.ai[0] == 3f) NPC.ai[0]++;
							else NPC.ai[0] -= 2f;
							NPC.ai[3] = 0f;
						}
						else {
							NPC.ai[3]++;
							storedPos = targetPos;
						}
						NPC.localAI[0] = 0f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.netUpdate = true;
						openMouth = false;
						NPC.TargetClosest();
					}
				break;
				case 4:
					if(shootDir.Length() > 800) {
						NPC.ai[2] = 0f;
						NPC.velocity += shootDir.SafeNormalize(NPC.oldVelocity);
						NPC.velocity *= 0.95f;
					}
					else if(NPC.velocity.Length() < 16f) NPC.velocity *= 1.05f;
					else {
						NPC.velocity = NPC.velocity.RotatedBy((float)Math.Sin(NPC.ai[2] * 0.025f * MathHelper.TwoPi) * MathHelper.ToRadians(2.5f));
						if(NPC.ai[1] > 570f && NPC.velocity.Length() > 4f) NPC.velocity *= 0.96f;
						if(++NPC.ai[2] > 40f) NPC.ai[2] = 0f;
					}
					if(NPC.ai[1] % (phase2 && legendaryMode ? 100 : phase2 || legendaryMode ? 150 : 200) == 0 && NPC.ai[1] < 600f) {
						if(Main.netMode != 1) {
							foreach(int i in segments) if(Main.rand.NextBool(2) || masterMode) Projectile.NewProjectile(NPC.GetSource_FromAI(), Main.npc[i].Center + NPC.velocity, Vector2.UnitY.RotatedBy(Main.npc[i].rotation) * (Main.rand.NextBool(2) ? -4f : 4f), ModContent.ProjectileType<HellMeteor3>(), projectileDamage / 3, 0f, Main.myPlayer, NPC.target + 1, Main.rand.Next(30, 91));
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, Vector2.Zero, ModContent.ProjectileType<BurningScream>(), 0, 0f, Main.myPlayer, 20f);
						}
						SoundEngine.PlaySound(SoundID.Item20 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.8f, Volume = 4f }, NPC.Center);
					}
					if(++NPC.ai[1] > 600f) {
						NPC.ai[0] -= 2;
						NPC.localAI[0] = 0f;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.netUpdate = true;
						openMouth = false;
						NPC.TargetClosest();
					}
				break;
				case 5:

                        playScreenshake = false;
					if(++NPC.ai[1] > 180f) {
						NPC.dontTakeDamage = false;
						NPC.ai[0]++;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.localAI[0] = 0f;
						NPC.netUpdate = true;
						openMouth = false;
						NPC.TargetClosest();
					}
					else if(NPC.ai[1] > 60f) {
						NPC.localAI[0] = (float)Math.Abs(Math.Sin(NPC.ai[1] / 30f * MathHelper.TwoPi)) * 0.2f;
						openMouth = true;
						if(Main.netMode != 1 && NPC.ai[1] % 10 == 0) Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, Vector2.Zero, ModContent.ProjectileType<BurningScream>(), 0, 0f, Main.myPlayer, 20f);
						NPC.velocity += shootDir.SafeNormalize(NPC.velocity) * 0.01f;
						if(NPC.ai[1] % 10 == 0) Main.instance.CameraModifiers.Add(new PunchCameraModifier(NPC.Center, Vector2.Normalize(NPC.velocity), NPC.velocity.Length(), 10, 60, 2400f, "Sinlord Screenshake"));
						NPC.velocity *= 0.95f;
					}
					else if(NPC.ai[1] > 30f) {
						if(NPC.ai[1] == 60f) SoundEngine.PlaySound(SoundID.NPCDeath10 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.4f }, NPC.Center);
						NPC.velocity += shootDir.SafeNormalize(NPC.velocity) * (1f - (NPC.ai[1] - 30f) / 30f);
						NPC.velocity *= 0.95f;
					}
				break;
				case 6:
					playScreenshake = false;
					if(NPC.ai[2] == 0f && Main.netMode != 1) {
						NPC.ai[2] = Main.rand.NextBool() ? 1f : -1f;
						NPC.netUpdate = true;
					}
					if(++NPC.ai[1] > 480f) {
						NPC.ai[0]++;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.netUpdate = true;
						NPC.TargetClosest();
						break;
					}
					else {
						float distance = shootDir.Length();
						float offset = MathHelper.Clamp(distance - 320f, -40f, 40f);
						NPC.velocity = Vector2.Normalize(shootDir).RotatedBy(MathHelper.ToRadians(MathHelper.Min(90f, NPC.ai[1] * 3f) - MathHelper.Min(1f, NPC.ai[1] / 30f) * offset) * NPC.ai[2]) * MathHelper.Min(NPC.ai[1] / 4f + 4f, 24f);
					}
					if(Main.netMode != 1 && NPC.ai[1] % (masterMode ? 60 : 80) == 0 && NPC.ai[1] < 480f) {
						Vector2 spawnPos = Main.npc[Main.rand.Next(segments.ToArray())].Center;
						shootDir = Vector2.Normalize(targetPos - spawnPos);
						for(int i = -1; i <= 1; i++) Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, shootDir.RotatedBy(i * MathHelper.PiOver4) * 12f, ModContent.ProjectileType<HellMeteor1>(), projectileDamage / 3, 0f, Main.myPlayer, shootDir.ToRotation());
						Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), projectileDamage / 3, 0f, Main.myPlayer);
					}
				break;
				case 7:
					if(NPC.ai[1] <= 0f && NPC.ai[1] > -120f && shootDir.Length() < 800f) {
						if(storedPos == Vector2.Zero) storedPos = targetPos;
						NPC.velocity += Vector2.Normalize(NPC.Center - storedPos) * 0.65f;
						NPC.velocity *= 0.95f;
						NPC.ai[1]--;
						break;
					}
					if(NPC.ai[1] < 0f) NPC.ai[1] = 0f;
					if(NPC.ai[1] < 30f) {
						openMouth = (int)(NPC.ai[1] / 6) % 2 == 0;
						NPC.localAI[0] = NPC.ai[1] / 30f;
						NPC.velocity = Vector2.Normalize(shootDir).RotatedBy(MathHelper.ToRadians(MathHelper.SmoothStep(90f - MathHelper.Clamp(shootDir.Length() - 480f, -10f, 10f), 0f, MathHelper.Min(1f, NPC.ai[1] / 30f))) * NPC.ai[2]) * (16f - MathHelper.Min(NPC.ai[1] / 2f, 12f));
					}
					else if(NPC.ai[1] < 90f) {
						if(NPC.ai[1] == 30f) SoundEngine.PlaySound(SoundID.Item46 with { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Pitch = -0.4f, Volume = 4f }, NPC.Center);
						openMouth = true;
						NPC.velocity = Vector2.Normalize(NPC.velocity) * ((float)Math.Sin((NPC.ai[1] - 30f) * MathHelper.Pi / 60f) * 28f + 4f);
						NPC.localAI[0] = 1f;
						int l = Dust.NewDust(NPC.Center + Vector2.UnitY.RotatedBy(NPC.rotation) * NPC.width / 3 - Vector2.One * 2f, 0, 0, 6);
						Main.dust[l].noGravity = true;
						Main.dust[l].scale *= 2.1f;
						Main.dust[l].velocity = NPC.velocity.RotatedBy(MathHelper.PiOver2) * 0.2f;
						l = Dust.NewDust(NPC.Center - Vector2.UnitY.RotatedBy(NPC.rotation) * NPC.width / 3 - Vector2.One * 2f, 0, 0, 6);
						Main.dust[l].noGravity = true;
						Main.dust[l].scale *= 2.1f;
						Main.dust[l].velocity = NPC.velocity.RotatedBy(-MathHelper.PiOver2) * 0.2f;
					}
					else {
						if(NPC.ai[1] == 90f && Main.netMode != 1) {
							for(int i = -10; i <= 10; i++) Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos - new Vector2(i * 128, Main.rand.Next(32) + 640), Main.rand.NextVector2Circular(1f, 3f), ModContent.ProjectileType<LavaStalactite>(), projectileDamage / 3, 0f, Main.myPlayer);
							for(int i = 0; i < 18; i++) {
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootDir.SafeNormalize(Vector2.Zero).RotatedBy(i / 18f * MathHelper.TwoPi) * Main.rand.Next(8, 16), ModContent.ProjectileType<HellMeteor2>(), projectileDamage / 3, 0f, Main.myPlayer);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), targetPos + Main.rand.NextVector2Circular(120f * (i + 1), (i + 1) * 90f), Vector2.Zero, ModContent.ProjectileType<BurningExplosion>(), projectileDamage / 3, 0f, Main.myPlayer, 30f + i * 5f);
							}
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.rotation.ToRotationVector2() * NPC.height / 3, Vector2.Zero, ModContent.ProjectileType<BurningScream>(), 0, 0f, Main.myPlayer, 20f);
						}
						NPC.localAI[0] = 1f - (NPC.ai[1] - 90f) / 30f;
						openMouth = false;
						NPC.velocity = Vector2.Normalize(Vector2.Lerp(NPC.velocity, shootDir, (NPC.ai[1] - 90f) / 30f)).RotatedBy(MathHelper.ToRadians(90f - MathHelper.Clamp(shootDir.Length() - 480f, -10f, 10f) * (NPC.ai[1] - 90f) / 30f) * NPC.ai[2]) * 4f;
					}
					if(++NPC.ai[1] > 120f) {
						NPC.ai[0]++;
						NPC.ai[1] = 0f;
						NPC.ai[2] = 0f;
						NPC.netUpdate = true;
						NPC.TargetClosest();
					}
				break;
			}
			if(this.playScreenshake != playScreenshake) {
				if(NPC.velocity != Vector2.Zero) Main.instance.CameraModifiers.Add(new PunchCameraModifier(NPC.Center, Vector2.Normalize(NPC.velocity), NPC.velocity.Length(), 10, 60, 2400f, "Sinlord Screenshake"));
				this.playScreenshake = playScreenshake;
				SoundEngine.PlaySound(SoundID.DeerclopsRubbleAttack, NPC.position);
			}
			if(NPC.velocity != Vector2.Zero) NPC.rotation = NPC.velocity.ToRotation();
		}
        public override void HitEffect(NPC.HitInfo hit)
        {

            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    var source = NPC.GetSource_Death();

                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore3").Type);
                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore2").Type);
                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore1").Type);
                }
            }
        }
        public override bool PreDraw(SpriteBatch sprite, Vector2 screenPosition, Color lightColor) {
			lightColor = NPC.GetNPCColorTintedByBuffs(lightColor);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			sprite.Draw(texture, NPC.Center - screenPosition, new Rectangle(0, texture.Height / 2 * (openMouth ? 1 : 0), texture.Width, texture.Height / 2), lightColor, NPC.rotation + MathHelper.PiOver2, texture.Size() * new Vector2(0.5f, 0.25f), NPC.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow");
			sprite.Draw(texture, NPC.Center - screenPosition, new Rectangle(0, texture.Height / 2 * (openMouth ? 1 : 0), texture.Width, texture.Height / 2), Color.White, NPC.rotation + MathHelper.PiOver2, texture.Size() * new Vector2(0.5f, 0.25f), NPC.scale, SpriteEffects.None, 0);
			if(NPC.localAI[0] <= 0f) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_White");
			sprite.Draw(texture, NPC.Center - screenPosition, new Rectangle(0, texture.Height / 2 * (openMouth ? 1 : 0), texture.Width, texture.Height / 2), Color.DarkOrange with {A = 0} * NPC.localAI[0], NPC.rotation + MathHelper.PiOver2, texture.Size() * new Vector2(0.5f, 0.25f), NPC.scale, SpriteEffects.None, 0);
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer) {
			if(Main.netMode == 0) return;
			writer.Write(openMouth);
			writer.WriteVector2(storedPos);
			for(int i = 0; i < NPC.localAI.Length; i++) writer.Write(NPC.localAI[i]);
		}
		public override void ReceiveExtraAI(BinaryReader reader) {
			if(Main.netMode == 0) return;
			openMouth = reader.ReadBoolean();
			storedPos = reader.ReadVector2();
			for(int i = 0; i < NPC.localAI.Length; i++) NPC.localAI[i] = reader.ReadSingle();
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CogwormTrophy>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sinstone>(), 1, 38, 82));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SinstoneMagma>(), 1, 13, 34));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 10, 18));

            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Cleavage>(), ModContent.ItemType<Menace>(), ModContent.ItemType<Pyroclast>(), ModContent.ItemType<HellgateAuraScythe>(), ModContent.ItemType<Impact>()));
            npcLoot.Add(notExpertRule);

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CogwormBag>()));

            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<CogwormRelicItem>()));
        }
        public override void BossHeadRotation(ref float rotation) => rotation = NPC.rotation;
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
		public override bool CheckActive() => !NPC.active || NPC.target < 0 || Main.player[NPC.target].dead;
	}
}