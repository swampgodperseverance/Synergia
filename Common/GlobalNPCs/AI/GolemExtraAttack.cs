using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Common.GlobalNPCs.AI
{
	public class GolemExtraAttacks : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type >= NPCID.Golem && npc.type <= NPCID.GolemHeadFree;
		public override void SetDefaults(NPC npc) => npc.trapImmune = true;
		public override void AI(NPC npc) {
			if(NPC.golemBoss < 0) return;
			if(npc.type == NPCID.GolemHead) {
				if(npc.ai[3] > 0f) npc.ai[3]--;
				if(npc.ai[2] == 0f) npc.ai[3] = 10f;
				if(npc.whoAmI < NPC.golemBoss) npc.position += Main.npc[NPC.golemBoss].velocity;
			}
			else if(npc.type > NPCID.GolemHead && npc.type < NPCID.GolemHeadFree) {
				npc.dontTakeDamage = Main.npc[NPC.golemBoss].dontTakeDamage;
				if(npc.ai[0] == 0f && Main.npc[NPC.golemBoss].ai[0] == 0f && Main.npc[NPC.golemBoss].ai[1] < -20f) npc.ai[1] = 0f;
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch sprite, Vector2 screenPos, Color lightColor) {
			if(npc.type != NPCID.GolemHead) return;
			int frameHeight = Terraria.GameContent.TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type];
			if(npc.ai[3] <= 0f || npc.ai[3] > 10f) return;
			float glowTime = (float)System.Math.Sin(npc.ai[3] * 0.1f * MathHelper.Pi);
			if(npc.localAI[1] == 0f) for(int k = -1; k <= 1; k += 2) {
				Vector2 center = npc.Bottom - screenPos;
				center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
				center += -new Vector2(k * 15f + 1f, 3f).RotatedBy(npc.rotation) * npc.scale;
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, Color.Lerp(Color.DarkOrange, Color.Gold, npc.ai[3] * 0.1f) with { A = 25 } * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
				}
			}
			else {
				Vector2 center = npc.Bottom - screenPos;
				center.Y += -frameHeight * npc.scale + 4f + frameHeight / 2 * npc.scale;
				center += new Vector2(npc.localAI[1] * 29f + 1f, -4f).RotatedBy(npc.rotation) * npc.scale;
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, Color.Lerp(Color.DarkOrange, Color.Gold, npc.ai[3] * 0.1f) with { A = 25 } * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 1.2f, SpriteEffects.None, 0);
				}
				for(int i = 1; i < 3; i++) {
					Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_" + i);
					sprite.Draw(texture, center, null, new Color(200, 200, 200, 0) * glowTime, npc.rotation, texture.Size() * 0.5f, new Vector2(glowTime * (i == 1 ? 1.1f : 0.8f), (i != 1 ? 1.1f : 0.8f) * glowTime) * 0.8f, SpriteEffects.None, 0);
				}
			}
		}
	}
}
