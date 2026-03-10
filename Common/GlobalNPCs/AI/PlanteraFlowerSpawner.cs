using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;
using Synergia.Content.Projectiles.Boss.PlanteraBuff;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class PlanteraFlowerSpawner : GlobalNPC
	{
		public override void Load() => On_NPC.AI += (orig, self) => {
			if(self.type == NPCID.PlanterasTentacle && NPC.plantBoss > -1) {
				if(Main.npc[NPC.plantBoss].ai[1] >= 900f) {
					if(Main.netMode != NetmodeID.MultiplayerClient) {
						Projectile.NewProjectile(self.GetSource_FromAI(), self.Center, self.rotation.ToRotationVector2() * 15f * self.spriteDirection, ModContent.ProjectileType<PlanterasTentacle>(), 40, 0f, Main.myPlayer);
						Projectile.NewProjectile(self.GetSource_FromAI(), self.Center, Vector2.Zero, ModContent.ProjectileType<PlanteraScream>(), 0, 0f, Main.myPlayer, 1f);
					}
					SoundEngine.PlaySound(self.DeathSound, self.Center);
					Main.instance.CameraModifiers.Add(new PunchCameraModifier(self.Center, self.rotation.ToRotationVector2() * self.spriteDirection, 4f, 10, 30, 480f, "Tentacle detach " + self.whoAmI));
					self.HitEffect();
					Vector2 distToProj = Main.npc[self.ai[3] > 0f ? (int)self.ai[3] - 1 : NPC.plantBoss].Center - self.Center;
					float distance = distToProj.Length();
					for(int i = 0; i < distance; i += 8) Dust.NewDust(self.Center + distToProj * (i / distance), 0, 0, 40);
					self.active = false;
					self.netUpdate = true;
					return;
				}
				if(self.velocity == Vector2.Zero && self.ai[3] > 0f && NPC.plantBoss != (int)self.ai[3] - 1) self.Center = Main.npc[(int)self.ai[3] - 1].Center;
				bool ftw = Main.getGoodWorld;
				Main.getGoodWorld = Main.npc[NPC.plantBoss].ai[1] > 780f;
				orig(self);
				self.rotation = (Main.npc[self.ai[3] > 0f ? (int)self.ai[3] - 1 : NPC.plantBoss].Center - self.Center).ToRotation() + MathHelper.PiOver2 * (self.spriteDirection + 1);
				Main.getGoodWorld = ftw;
			}
			else orig(self);
		};
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.Plantera;
		public override void AI(NPC npc) {
			Player target = Main.player[npc.target];
			Vector2 targetPos = target.Center;
			if(npc.life >= npc.lifeMax / 2) {
				if(++npc.ai[0] > 1200f) npc.ai[0] = (int)MathHelper.Lerp(900f, 0f, (float)npc.life / (float)npc.lifeMax);
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 shootDir = Vector2.Normalize(targetPos - npc.Center);
					if(npc.ai[0] == 480f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + shootDir * 80f, shootDir * 8f, ProjectileID.ThornBall, 40, 0f, Main.myPlayer);
					else if(npc.ai[0] > 120f && npc.ai[0] < 1080f && npc.ai[0] % 20 == 0) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + shootDir * 80f, shootDir * 15f, ProjectileID.SeedPlantera, 40, 0f, Main.myPlayer);
					else if(npc.ai[0] > 1080f && npc.ai[0] % 10f == 0) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Main.rand.NextVector2Circular(2f, 2f), ModContent.ProjectileType<FlowerPow>(), 40, 0f, Main.myPlayer, Main.rand.NextFloat(MathHelper.TwoPi));
					else if(npc.ai[0] == 1080f) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<PlanteraScream>(), 0, 0f, Main.myPlayer, 20f, 1f);
				}
				if(npc.ai[0] > 1080f) npc.velocity *= 1f - (npc.ai[0] - 1080f) / 120f;
				else npc.velocity += target.velocity * 0.01f;
				npc.localAI[1] = 0f;
			}
			else if(npc.life <= npc.lifeMax / 2) if(++npc.ai[1] > 900) npc.ai[1] = (int)MathHelper.Lerp(600f, 0f, (float)npc.life / (float)npc.lifeMax * 2);
			else if(npc.ai[1] > 780) {
				if(Main.getGoodWorld && Main.netMode != NetmodeID.MultiplayerClient && npc.ai[1] % 30 == 0) for(int i = -1; i <= 1; i++) {
					Vector2 shootDir = Vector2.Normalize(targetPos - npc.Center);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + shootDir * 32f, shootDir.RotatedBy(i * MathHelper.PiOver4) * 7f, ProjectileID.PoisonSeedPlantera, 40, 0f, Main.myPlayer);
				}
				npc.velocity *= 1f - (npc.ai[1] - 780f) / 120f;
				npc.localAI[1] = 0f;
			}
			else if(npc.ai[1] == 780f) {
				npc.ai[2] = 0f;
				npc.localAI[0] = 1f;
				SoundEngine.PlaySound(SoundID.Zombie125, npc.Center);
				if(Main.netMode != NetmodeID.MultiplayerClient) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<PlanteraScream>(), 0, 0f, Main.myPlayer, 20f, 2f);
			}
			else if(npc.ai[1] < 780f && npc.life < npc.lifeMax / 4) {
				if(npc.ai[2] >= 90f && npc.ai[1] == 779f) npc.ai[1]--;
				if(++npc.ai[2] > 150f) npc.ai[2] = (int)MathHelper.Lerp(75f, 0f, npc.life * 4f / npc.lifeMax);
				if(npc.ai[2] == 120) {
					npc.velocity = Vector2.Normalize(targetPos - npc.Center) * MathHelper.Lerp(24f, 12f, npc.life * 4f / npc.lifeMax);
					SoundEngine.PlaySound(SoundID.Item46, npc.Center);
				}
				if(npc.ai[2] >= 120) {
					for(int i = 0; i < 2; i++) Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 40, npc.velocity.X, npc.velocity.Y)].noGravity = true;
					if(npc.ai[2] >= 145f) npc.velocity = Vector2.Lerp(npc.velocity.SafeNormalize(npc.oldVelocity), Vector2.Normalize(targetPos - npc.Center), (npc.ai[2] - 145f) * 0.1f) * npc.velocity.Length() * 0.8f;
					else if(npc.ai[2] >= 140f) npc.velocity *= 0.9f;
					else npc.velocity += Vector2.Normalize(targetPos - npc.Center);
					npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver2;
				}
				else if(npc.ai[2] > 90) npc.velocity *= 0.9f;
				else if(npc.ai[2] % 25 == 0 && Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 shootDir = Vector2.Normalize(targetPos - npc.Center);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + shootDir * 32f, shootDir * 7f, ProjectileID.PoisonSeedPlantera, 40, 0f, Main.myPlayer);
				}
				npc.localAI[1] = 0f;
			}
			Lighting.AddLight(npc.Center, (npc.life >= npc.lifeMax / 2 ? Color.HotPink : Color.Lime).ToVector3());
		}
	}
}
