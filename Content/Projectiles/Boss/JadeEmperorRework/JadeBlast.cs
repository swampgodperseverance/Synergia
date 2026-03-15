using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Projectiles.Boss.JadeEmperorRework
{
	public class JadeBlast : ModProjectile
	{
		private int accelerationDelay; 
		private float accelerationAmount; 
		private float maxSpeed;
		public override string GlowTexture => "Synergia/Assets/Textures/LightTrail_1";
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			accelerationDelay = 30;
			accelerationAmount = 0.05f;
			maxSpeed = 12f;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
		}

		public override void AI()
		{
			Projectile.ai[0]++;

			if (Projectile.velocity.LengthSquared() > 0.01f)
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (Projectile.ai[0] == 1)
			{
				Projectile.velocity *= 0.4f;
			}
			else if (Projectile.ai[0] >= accelerationDelay)
			{
				float targetSpeed = maxSpeed;
				float newSpeed = MathHelper.Lerp(Projectile.velocity.Length(), targetSpeed, accelerationAmount);
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * newSpeed;
				if(Projectile.ai[0] > accelerationDelay * 3f && Projectile.ai[0] < accelerationDelay * 5f) 
				{
					double rotOff = (Main.player[Player.FindClosest(Projectile.Center, 0, 0)].Center - Projectile.Center).ToRotation() - Projectile.velocity.ToRotation();
					if(rotOff > MathHelper.Pi) rotOff -= MathHelper.TwoPi;
					if(rotOff < -MathHelper.Pi) rotOff += MathHelper.TwoPi;
					Projectile.velocity = Projectile.velocity.RotatedBy(rotOff * MathHelper.ToRadians(2));
				}
				if (Main.rand.NextBool(2))
				{
					Dust.NewDust(
						Projectile.position,
						Projectile.width,
						Projectile.height,
						ModContent.DustType<JadeDust>(),
						Projectile.velocity.X * 0.3f,
						Projectile.velocity.Y * 0.3f,
						200,
						default,
						1.2f
					);
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = texture.Size() / 2f;
			float fadeOut = MathHelper.Min(Projectile.timeLeft, 10) * 0.1f;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					continue;

				Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
				float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

				float rotation;
				if (i > 0 && Projectile.oldPos[i - 1] != Vector2.Zero)
					rotation = (Projectile.oldPos[i - 1] - Projectile.oldPos[i]).ToRotation() + MathHelper.PiOver2;
				else
					rotation = Projectile.rotation;

				// color
				Color turquoise = new Color(0, 255, 210);

				Color trailColor = Projectile.ai[0] > accelerationDelay
					? turquoise * fade * 1.2f 
					: turquoise * fade * 0.7f;
				trailColor.A = 0;
				Main.EntitySpriteDraw(
					texture,
					drawPos,
					null,
					trailColor * 0.5f * fadeOut,
					rotation,
					origin,
					Projectile.scale,
					SpriteEffects.None,
					0
				);
			}

			Main.EntitySpriteDraw(
				texture,
				Projectile.Center - Main.screenPosition,
				null,
				Color.White * fadeOut,
				Projectile.rotation,
				origin,
				Projectile.scale,
				SpriteEffects.None,
				0
			);
			texture = ModContent.Request<Texture2D>(GlowTexture).Value;
			origin = texture.Size() / 2f;
			if(Projectile.ai[0] < accelerationDelay || Projectile.ai[0] > accelerationDelay * 3f) for(int i = 0; i < 2; i++) for(int j = 0; j < 2; j++) Main.EntitySpriteDraw(
				texture,
				Projectile.Center - Vector2.UnitY.RotatedBy(Projectile.rotation) * 8f - Main.screenPosition,
				null,
				(i == 0 ? new Color(0, 255, 210, 0) : Color.White with {A = 0}) * fadeOut,
				Projectile.rotation + MathHelper.PiOver2 * j,
				origin,
				(Projectile.scale - 0.2f * i) * (Projectile.ai[0] > accelerationDelay * 3f ? MathHelper.Min(1f, (Projectile.ai[0] - accelerationDelay * 3f) / accelerationDelay) * 0.4f : (float)System.Math.Sin(Projectile.ai[0] / accelerationDelay * MathHelper.Pi)),
				SpriteEffects.None,
				0
			);

			return false;
		}


	}
}
