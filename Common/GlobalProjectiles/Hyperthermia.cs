using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Drawing;
using System.IO;

namespace Synergia.Content.GlobalProjectiles
{
	public class Hyperthermia : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => projectile.ModProjectile != null &&
			  projectile.ModProjectile.Mod?.Name == "ValhallaMod" &&
			  projectile.ModProjectile.Name == "HyperthermiaRay";

		public override void SetStaticDefaults() => ProjectileID.Sets.DrawScreenCheckFluff[ModLoader.GetMod("ValhallaMod").Find<ModProjectile>("HyperthermiaRay").Type] = 2400;
		public override void SetDefaults(Projectile projectile) {
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.DamageType = DamageClass.Ranged;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.MaxUpdates = projectile.timeLeft = 180;
		}
		public override bool PreAI(Projectile projectile) {
			if(projectile.localAI[0] == 0f && projectile.localAI[1] == 0f) {
				projectile.velocity = Vector2.Normalize(projectile.velocity) * 8f;
				projectile.localAI[0] = projectile.Center.X;
				projectile.localAI[1] = projectile.Center.Y;
			}
			if(projectile.ai[0] > 0f || (projectile.timeLeft <= 2 && projectile.ai[0] == 0f)) {
				projectile.extraUpdates = 0;
				projectile.timeLeft = 2;
				projectile.velocity = Vector2.Zero;
				if(projectile.ai[0] < 5f * projectile.MaxUpdates && projectile.ai[0] % projectile.MaxUpdates == 0) {
					Vector2 beamDir = Vector2.Normalize(new Vector2(projectile.localAI[0], projectile.localAI[1]) - projectile.Center) * -5f;
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = projectile.Center, MovementVector = beamDir.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.4f), UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.OrangeRed).X * 255f)});
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = new Vector2(projectile.localAI[0], projectile.localAI[1]) + beamDir * 12f, MovementVector = beamDir.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2) * 0.8f), UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.OrangeRed).X * 255f)});
				}
				if(projectile.ai[0] < 10f * projectile.MaxUpdates) projectile.ai[0]++;
				else projectile.active = false;
			}
			if(projectile.velocity != Vector2.Zero) projectile.rotation = projectile.velocity.ToRotation();
			return false;
		}
		public override bool PreDraw(Projectile projectile, ref Color lightColor) {
			Texture2D texture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray").Value;
			Vector2 endPos = new Vector2(projectile.localAI[0], projectile.localAI[1]);
			Vector2 startPos = projectile.Center;
			Vector2 length = endPos - startPos;
			float rotation = length.ToRotation();
			float distance = length.Length() - texture.Height;
			int h = 0;
			while(distance < 0f && h < 1000) {
				distance++;
				h++;
			}
			endPos -= Vector2.Normalize(length) * texture.Height;
			if(h > 0) startPos -= Vector2.Normalize(length) * h;
			for(int i = 0; i < 2; i++) {
				float laserSize = Vector2.UnitX.RotatedBy(projectile.ai[0] / (10f * projectile.MaxUpdates) * MathHelper.Pi).Y * projectile.scale;
				if(i > 0) lightColor = new Color(200, 200, 200, 0);
				else lightColor = Color.Lerp(Color.OrangeRed, Color.DarkOrange, laserSize);
				laserSize *= 0.075f * new Vector2(projectile.width, projectile.height).Length();
				if(i > 0) laserSize *= 0.5f;
				Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * laserSize * (i + 1), rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(projectile.scale, projectile.localAI[2] * 0.5f + 0.25f) * laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, texture.Height - 1, texture.Width, 1), lightColor * laserSize * (i + 1), rotation - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, 0f), new Vector2(projectile.scale * laserSize, distance), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, endPos - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height - 1), lightColor * laserSize * (i + 1), rotation + MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, texture.Height - 1), new Vector2(projectile.scale, 0.75f) * laserSize, SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
			projectile.tileCollide = false;
			if(Main.myPlayer == projectile.owner) {
				projectile.position += projectile.velocity;
				projectile.velocity = Vector2.Zero;
				NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
			}
			return false;
		}
		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
			projectile.localAI[2] = 1f;
			projectile.tileCollide = false;
			projectile.velocity = Vector2.Zero;
			NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
		}
		public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter writer) {
			bitWriter.WriteBit(projectile.tileCollide);
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}
		public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader reader) {
			projectile.tileCollide = bitReader.ReadBit();
			projectile.localAI[0] = reader.ReadSingle();
			projectile.localAI[1] = reader.ReadSingle();
		}
	}
}
