using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Boss.LunaticCultistBuff;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class CultistAI : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.type == NPCID.CultistBoss;
		public override void SetDefaults(NPC npc)  {
			npc.lifeMax = (int)(npc.lifeMax * 1.25f);
			npc.damage = (int)(npc.damage * 1.1f);
			npc.defDefense += 10;
		}
		public override void PostAI(NPC npc) {
			if(npc.ai[0] == 1f) npc.ai[2] = 0f;
			else if(npc.ai[0] == 2f) {
				if(npc.ai[2] == 0f) foreach(Projectile p in Main.ActiveProjectiles) if(p.type == 464) {
					p.Kill();
					npc.ai[2] = 1f;
					npc.netUpdate = true;
				}
				if(npc.ai[2] == 1f) {
					if(Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitX * npc.direction * 5f, ModContent.ProjectileType<Tornado>(), 45, 10f, Main.myPlayer);
					npc.ai[2] = 2f;
				}
				if(npc.ai[2] > 1f) if(++npc.ai[2] % (Main.expertMode ? 4 : 10) == 0 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.player[npc.target].Center - new Vector2(Main.rand.Next(-999, 1000), 1000f), new Vector2(12f * npc.direction, 12f), ModContent.ProjectileType<FallingStar>(), 45, 10f, Main.myPlayer);
			}
			else if(npc.ai[0] == 3f) {
				if(npc.ai[1] == 0f) {
					npc.ai[2] = (Main.player[npc.target].Center + Main.player[npc.target].velocity * 20 - npc.Center).ToRotation();
					npc.netUpdate = true;
				}
				npc.direction = npc.spriteDirection = System.Math.Sign(npc.ai[2].ToRotationVector2().X);
				if(npc.ai[1] > 0f && npc.ai[1] < 60f && npc.ai[1] % 5 == 0) {
					SoundEngine.PlaySound(SoundID.Item28, npc.Center);
					if(Main.netMode != 1) for(int i = -1; i <= 1; i += 2) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + new Vector2(npc.direction * 30f, 12f), Vector2.UnitY.RotatedBy(npc.ai[2]) * i * 2f, ModContent.ProjectileType<IceMist>(), 45, 10f, Main.myPlayer, 0f, 60f, npc.ai[2] + MathHelper.ToRadians(160f - npc.ai[1] * 2.6f) * i);
				}
				if(npc.ai[1] > (int)npc.ai[1]) foreach(Projectile p in Main.ActiveProjectiles) if(p.type == 467 && p.timeLeft == 3600) {
					p.velocity = npc.ai[2].ToRotationVector2() * Main.rand.Next(6, 8) + Main.rand.NextVector2Circular(1f, 1f);
					p.Center = npc.Center + new Vector2(npc.direction * 30f, 12f);
					p.tileCollide = false;
					p.netUpdate = true;
				}
				npc.ai[1]++;
			}
			else if(npc.ai[0] == 4f) {
				if(npc.ai[2] == 0f) foreach(Projectile p in Main.ActiveProjectiles) if(p.type == 465) {
					p.Kill();
					npc.ai[2] = 1f;
					npc.netUpdate = true;
				}
				if(npc.ai[2] == 1f) {
					if(Main.netMode != 1) for(int i = -10; i <= 10; i++) {
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - new Vector2(240 * i, 800f), Vector2.UnitY, ModContent.ProjectileType<SuperboltWarning>(), 45, 10f, Main.myPlayer, 240f);
						Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(Main.player[npc.target].Center.X, npc.Center.Y) - new Vector2(240 * i, 800f), Vector2.UnitY, ModContent.ProjectileType<SuperboltWarning>(), 45, 10f, Main.myPlayer, 240f, 60f);
					}
					npc.ai[2] = 2f;
				}
			}
			else if(npc.ai[0] == 5f && npc.ai[1] > 90f && Main.expertMode) {
				Projectile ritual = Main.projectile[(int)npc.ai[2]];
				if(!ritual.active) return;
				float spin = 1f - (float)npc.life / (float)npc.lifeMax;
				if(!Main.getGoodWorld) spin -= 0.25f;
				if(spin < 0f) return;
				foreach(NPC clone in Main.ActiveNPCs) if(clone.type == 440 && clone.ai[3] == npc.whoAmI) clone.Center = ritual.Center + (clone.Center - ritual.Center).RotatedBy(MathHelper.ToRadians(spin) * ritual.direction);
				npc.Center = ritual.Center + (npc.Center - ritual.Center).RotatedBy(MathHelper.ToRadians(spin) * ritual.direction);
			}
		}
	}
}
