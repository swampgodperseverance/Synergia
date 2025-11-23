using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
	public class GiantSnow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(24);
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 210;
		}
		public override void AI()
		{   

			Projectile.rotation += Projectile.direction * Projectile.velocity.Length() * 0.014f;

			if (Projectile.ai[1] == 0)
			{
				SoundEngine.PlaySound(SoundID.Item28, Projectile.position);
			}

			Lighting.AddLight(Projectile.Center, 0, 0.2f, 0.2f);
			if (Main.rand.NextBool(3))
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
				d.noGravity = true;
				d.velocity = Projectile.velocity * 0.4f;
			}
			if (Main.rand.NextBool(5))
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostStaff);
				d.noGravity = true;
				d.velocity = Projectile.velocity * 0.4f;
			}

			Projectile.ai[1]++;

			if (Projectile.ai[1] > 60)
			{
				Projectile.velocity *= 0.98f; 
			}
		}
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			for (int i = 0; i < 7; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
				d.noGravity = true;
				d.velocity = Projectile.oldVelocity.RotatedByRandom(1) * Main.rand.NextFloat(-1, 0);
				d.fadeIn = Main.rand.NextFloat(0, 1);
			}
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch);
				d.noGravity = false;
				d.velocity = Projectile.oldVelocity.RotatedByRandom(1) * Main.rand.NextFloat(-0.3f, 0);
				d.fadeIn = 1;
				d.customData = 0;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[0] == 0)
				target.AddBuff(BuffID.Frostburn, 60 * 3);
			else
				target.AddBuff(BuffID.Frostburn2, 60 * 3);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Projectile.ai[0] == 0)
				target.AddBuff(BuffID.Frostburn, 60 * 3);
			else
				target.AddBuff(BuffID.Frostburn2, 60 * 3);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			int frameHeight = TextureAssets.Projectile[Type].Value.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, TextureAssets.Projectile[Type].Value.Width, frameHeight);
			Vector2 frameOrigin = frame.Size() / 2f;
			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
			{
				float shrink = (float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
				Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[i] + frameOrigin - Main.screenPosition, frame, Color.White * shrink * 0.35f, Projectile.rotation, frameOrigin, Projectile.scale - (i * 0.03f), SpriteEffects.None);
			}
			Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.position + frameOrigin - Main.screenPosition, frame, Color.White * 0.7f, Projectile.rotation, frameOrigin, Projectile.scale, SpriteEffects.None);
			return false;
		}
	}
}