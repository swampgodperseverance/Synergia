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
			if(self.type == NPCID.PlanterasTentacle && Main.npc[NPC.plantBoss].ai[1] > 780f) {
				if(Main.npc[NPC.plantBoss].ai[1] >= 900f) {
					if(Main.netMode != NetmodeID.MultiplayerClient) {
						Projectile.NewProjectile(self.GetSource_FromAI(), self.Center, self.rotation.ToRotationVector2() * 15f * self.spriteDirection, ModContent.ProjectileType<PlanterasTentacle>(), 40, 0f, Main.myPlayer);
						Projectile.NewProjectile(self.GetSource_FromAI(), self.Center, Vector2.Zero, ModContent.ProjectileType<PlanteraScream>(), 0, 0f, Main.myPlayer, 1f);
					}
					SoundEngine.PlaySound(self.DeathSound, self.Center);
					Main.instance.CameraModifiers.Add(new PunchCameraModifier(self.Center, self.rotation.ToRotationVector2() * self.spriteDirection, 4f, 10, 30, 480f, "Tentacle detach " + self.whoAmI));
					self.HitEffect();
					self.active = false;
					self.netUpdate = true;
					return;
				}
				bool ftw = Main.getGoodWorld;
				Main.getGoodWorld = true;
				orig(self);
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
				npc.localAI[1] = 0f;
			}
			else if(npc.life <= npc.lifeMax / 2) if(++npc.ai[1] > 900) npc.ai[1] = (int)MathHelper.Lerp(600f, 0f, (float)npc.life / (float)npc.lifeMax * 2);
			else if(npc.ai[1] > 780) {
				if(Main.masterMode && Main.netMode != NetmodeID.MultiplayerClient && npc.ai[1] % 30 == 0) for(int i = -1; i <= 1; i++) {
					Vector2 shootDir = Vector2.Normalize(targetPos - npc.Center);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + shootDir * 32f, shootDir.RotatedBy(i * MathHelper.PiOver4) * 7f, ProjectileID.PoisonSeedPlantera, 40, 0f, Main.myPlayer);
				}
				npc.velocity *= 1f - (npc.ai[1] - 780f) / 120f;
				npc.localAI[1] = 0f;
			}
			else if(npc.ai[1] == 780f) {
				npc.localAI[0] = 1f;
				SoundEngine.PlaySound(SoundID.Zombie125, npc.Center);
				if(Main.netMode != NetmodeID.MultiplayerClient) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<PlanteraScream>(), 0, 0f, Main.myPlayer, 20f, 2f);
			}
			Lighting.AddLight(npc.Center, (npc.life >= npc.lifeMax / 2 ? Color.HotPink : Color.Lime).ToVector3());
		}
	}
}
