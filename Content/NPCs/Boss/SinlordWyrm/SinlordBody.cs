using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Synergia.Reassures;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using System;
using System.IO;
using System.Collections.Generic;

namespace Synergia.Content.NPCs.Boss.SinlordWyrm
{
	public class SinlordBody : ModNPC
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {Hide = true};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}
		public override void SetDefaults() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageNPC", NPC, true);
			NPC.lifeMax = 100000;
			NPC.damage = 35;
			NPC.defense = 125;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.HitSound = new SoundStyle($"{Mod.Name}/Assets/Sounds/CragwormHit2");
			NPC.knockBackResist = 0f;
			NPC.scale = 1.3f;
			NPC.npcSlots = 6f;
			NPC.Size = new Vector2(60f * NPC.scale);
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
		public override void OnKill() {
			if(Main.netMode != 1) Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Content.Projectiles.Boss.SinlordWyrm.BurningExplosion>(), 0, 0f, Main.myPlayer);
		}
		public override void AI() {
			if(NPC.ai[2] <= 0f) return;
			NPC body = Main.npc[(int)NPC.ai[2] - 1];
			if(!body.active) {
				NPC.velocity = NPC.rotation.ToRotationVector2() * (NPC.oldPosition - NPC.position).Length();
				NPC.ai[2] = 0f;
				return;
			}
			float distancing = 1f / NPC.scale * (body.ModNPC is Sinlord ? 94f / 68f * 0.4f : 0.45f) * 0.9f;
			Vector2 attachToBody = body.Center - body.rotation.ToRotationVector2() * body.height * distancing - NPC.Center;
			if(body.rotation != NPC.rotation) attachToBody = Utils.MoveTowards(Utils.RotatedBy(attachToBody, MathHelper.WrapAngle(body.rotation - NPC.rotation) * 0.02f, Vector2.Zero), (body.rotation - NPC.rotation).ToRotationVector2(), 1f);
			NPC.Center = body.Center - body.rotation.ToRotationVector2() * body.height * distancing - Utils.SafeNormalize(attachToBody, Vector2.Zero) * NPC.height * distancing;
			NPC.rotation = attachToBody.ToRotation();
			if(NPC.ai[3] <= 0f) return;
			NPC head = Main.npc[(int)NPC.ai[3] - 1];
			if(!head.active) {
				NPC.velocity = NPC.rotation.ToRotationVector2() * head.velocity.Length();
				NPC.ai[3] = 0f;
				return;
			}
			NPC.localAI[0] = head.localAI[0];
			NPC.dontTakeDamage = head.dontTakeDamage;
			NPC.target = head.target;
			if(body.whoAmI != head.whoAmI) body.localAI[3] = NPC.whoAmI + 1;
			NPC.realLife = head.whoAmI;
			NPC.immune = head.immune;
			if(NPC.life < head.life) head.life = NPC.life;
			else if(NPC.life > head.life) NPC.life = head.life;
		}
        public override void HitEffect(NPC.HitInfo hit)
        {

            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    var source = NPC.GetSource_Death();

                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore4").Type);
                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore5").Type);
                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore6").Type);
                }
            }
        }

        public override bool PreDraw(SpriteBatch sprite, Vector2 screenPosition, Color lightColor) {
			lightColor = NPC.GetNPCColorTintedByBuffs(lightColor);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			sprite.Draw(texture, NPC.Center - screenPosition, null, lightColor, NPC.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow");
			sprite.Draw(texture, NPC.Center - screenPosition, null, Color.White, NPC.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
			if(NPC.localAI[0] <= 0f) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_White");
			sprite.Draw(texture, NPC.Center - screenPosition, null, new Color(253, 32, 2, 0) * NPC.localAI[0], NPC.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
		public override bool CheckActive() => !NPC.active || NPC.ai[2] <= 0f || NPC.ai[3] <= 0f;
	}
}
