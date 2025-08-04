using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.Audio.SoundEngine;

namespace Vanilla.Content.Projectiles.Friendly
{
	public class OculithShardProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			var proj = Projectile;

			proj.width = 14;
			proj.height = 14;
			proj.aiStyle = 1;
			proj.friendly = true;
			proj.DamageType = DamageClass.Throwing;
			proj.penetrate = 2; 
			proj.timeLeft = 60 * 5; 
			proj.ignoreWater = true;
			proj.tileCollide = true;

			AIType = ProjectileID.BoneJavelin;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; 
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
				DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f,
				100, default, 1.2f);
			}

			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void OnKill(int timeLeft)
		{
			Dust.NewDustPerfect(
				Projectile.Center,
				DustID.Shadowflame,
				Main.rand.NextVector2Circular(7f, 7f),
				100, default, 2.5f
			);

			PlaySound(SoundID.Dig, Projectile.position);
		}

		public override void AI()
		{
			Projectile.velocity *= 1.01f;

			if (Projectile.velocity.Length() > 20f)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 20f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				float scale = Projectile.scale * (1f - i / (float)Projectile.oldPos.Length * 0.5f);

				Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}

			Vector2 currentDrawPos = Projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture, currentDrawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

			return false;
		}
	}
}