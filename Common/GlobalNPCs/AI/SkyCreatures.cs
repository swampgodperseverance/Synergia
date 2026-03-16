using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class Harpy : GlobalNPC 
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.Harpy;
		public override void AI(NPC npc) {
			bool shoot = false;
			if(npc.localAI[3] != npc.frame.Y) {
				shoot = npc.frame.Y == 258 && npc.velocity.Length() < 0.5f;
				npc.localAI[3] = npc.frame.Y;
			}
			if(npc.ai[0] < 120f) {
				if(npc.justHit) npc.ai[0] = 120f;
				else npc.velocity *= 1f - npc.ai[0] / 120f;
				if(shoot && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + new Vector2(npc.direction * 24f, 32f), Vector2.Normalize(npc.Center + new Vector2(npc.direction * 24f, 32f) - Main.player[npc.target].Center) * -8f, ProjectileID.HarpyFeather, npc.damage / (Main.masterMode ? 3 : Main.expertMode ? 2 : 1) / 2, 0f, Main.myPlayer);
			}
			if(npc.ai[0] == (int)npc.ai[0]) npc.ai[0] += 0.5f;
		}
	}
	public class Wyvern : GlobalNPC 
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type >= NPCID.WyvernHead && npc.type <= NPCID.WyvernTail;
		public override void AI(NPC npc) {
			if(npc.type != NPCID.WyvernHead) {
				NPC body = Main.npc[(int)npc.ai[1]];
				npc.target = body.target;
				float distancing = 1f / npc.scale * 0.675f;
				Vector2 attachToBody = body.Center - Vector2.UnitY.RotatedBy(body.localAI[3]) * body.height * distancing - npc.Center;
				if(body.localAI[3] != npc.rotation) attachToBody = Utils.MoveTowards(Utils.RotatedBy(attachToBody, MathHelper.WrapAngle(body.localAI[3] - npc.rotation) * 0.2f, Vector2.Zero), (body.localAI[3] - npc.rotation).ToRotationVector2(), 1f);
				npc.Center = body.Center + Vector2.UnitY.RotatedBy(body.rotation) * body.height * distancing - Utils.SafeNormalize(attachToBody, Vector2.Zero) * npc.height * distancing;
				npc.rotation = attachToBody.ToRotation() + MathHelper.PiOver2;
				body.localAI[3] = body.localAI[2];
				body.localAI[2] = body.localAI[1];
				body.localAI[1] = body.type == NPCID.WyvernHead ? body.rotation : body.localAI[0];
				if(body.type != NPCID.WyvernHead) body.localAI[0] = body.rotation;
				return;
			}
			if(npc.target < -1) return;
			if(npc.ai[2] == 0f && npc.Distance(Main.player[npc.target].Center) < 480f && (Vector2.Normalize(Main.player[npc.target].Center - npc.Center) - Vector2.Normalize(npc.velocity)).Length() < 0.05f) npc.ai[2] = 60f;
			if(npc.ai[2] > 0f) {
				npc.velocity = npc.oldVelocity;
				npc.ai[2]--;
			}
			npc.oldVelocity = npc.velocity;
		}
	}
}
