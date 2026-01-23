using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.Audio;
using Terraria.ModLoader;
using Avalon.Dusts;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
	public class Sludge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 3; // 3 кадра
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
		}

		private Player player => Main.player[Projectile.owner];

		public override void AI()
		{
			// Темно-зелёный свет
			Lighting.AddLight(Projectile.Center, 0.05f, 0.18f, 0.05f);

			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0]++;

			if (Projectile.ai[0] == 4)
			{
				Projectile.alpha = 0;

				for (int i = 0; i < 12; i++)
				{
					int dust = Dust.NewDust(
						Projectile.position,
						Projectile.width,
						Projectile.height,
						ModContent.DustType<ContagionWaterSplash>(),
						Projectile.oldVelocity.X * 0.4f,
						Projectile.oldVelocity.Y * 0.4f,
						0,
						new Color(30, 90, 40),
						1.2f
					);

					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.4f;
				}
			}

			if (Projectile.ai[0] > 10)
				Projectile.velocity.Y += Projectile.ai[0] / 100f;

			if (Projectile.velocity.Length() >= 20f)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 20f;

			if (Projectile.ai[0] >= 4)
			{
				int dust = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					ModContent.DustType<ContagionWaterSplash>(),
					Projectile.oldVelocity.X * 0.1f,
					Projectile.oldVelocity.Y * 0.1f,
					0,
					new Color(20, 80, 35),
					0.9f
				);

				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.7f;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}

			if (Projectile.frame >= 3)
				Projectile.frame = 0;
		}



		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];

			Rectangle frame = new Rectangle(
				0,
				frameHeight * Projectile.frame,
				texture.Width,
				frameHeight
			);

			Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;

			// Тёмно-зелёное свечение
			Main.EntitySpriteDraw(
				texture,
				drawPos,
				frame,
				new Color(40, 120, 60, 180),
				Projectile.rotation,
				origin,
				Projectile.scale * 1.15f,
				Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
			);

			// Основной слой
			Main.EntitySpriteDraw(
				texture,
				drawPos,
				frame,
				Color.Lerp(lightColor, new Color(60, 160, 80), 0.6f),
				Projectile.rotation,
				origin,
				Projectile.scale,
				Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
			);

			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int size = Projectile.width * 2;
			hitbox.Inflate(size / 2, size / 2);
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath9, Projectile.position);

			for (int i = 0; i < 40; i++)
			{
				int dust = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					ModContent.DustType<ContagionWaterSplash>(),
					Main.rand.NextFloat(-2f, 2f),
					Main.rand.NextFloat(-2f, 2f),
					0,
					new Color(25, 100, 50),
					Main.rand.NextFloat(1f, 1.6f)
				);

				Main.dust[dust].noGravity = false;
			}
		}
	}
}
