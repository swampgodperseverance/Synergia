using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace Synergia.Content.Projectiles.Thrower
{
	public class BlackScarletGungnir : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 26;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 3;
			AIType = 14;
			Projectile.aiStyle = 1;
		}

		public override void AI()
		{
		
			if (Main.rand.NextBool(3)) 
			{
				int index = Dust.NewDust(
					Projectile.position + Projectile.velocity,
					42, 42,
					219,
					Projectile.oldVelocity.X * 0.25f,
					Projectile.oldVelocity.Y * 0.25f
				);

				Main.dust[index].scale = 0.45f; 
				Main.dust[index].noGravity = true;
			}

			if (Main.bloodMoon)
				Projectile.extraUpdates = 4;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 160);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D trailTex = ModContent.Request<Texture2D>(
				"Synergia/Assets/Textures/Trails/BlackScarletGungnir_Trail"
			).Value;

			Vector2 origin = trailTex.Size() / 2f;

			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				float progress = 1f - i / (float)Projectile.oldPos.Length;
				Color color = new Color(120, 20, 20, 0) * progress;

				Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

				Main.spriteBatch.Draw(
					trailTex,
					drawPos,
					null,
					color,
					Projectile.rotation,
					origin,
					Projectile.scale,
					SpriteEffects.None,
					0f
				);
			}

			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Main.spriteBatch.Draw(
				texture,
				Projectile.Center - Main.screenPosition,
				null,
				Projectile.GetAlpha(lightColor),
				Projectile.rotation,
				texture.Size() / 2f,
				Projectile.scale,
				SpriteEffects.None,
				0f
			);

			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					219
				);

				Main.dust[num].velocity *= 6f;
				Main.dust[num].noGravity = true;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			fallThrough = true;
			return true;
		}
	}
}
