using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
	public class SnowballMega : ModProjectile
	{
		private int shootTimer;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			Projectile.width = 10;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 2; // стиль полёта как у метательного
			AIType = ProjectileID.SnowBallFriendly;
		}

		public override void AI()
		{
			// --- ЭМИТ СНЕЖКОВ ---
			shootTimer++;
			if (shootTimer >= 30) // каждые ~15 кадров
			{
				shootTimer = 0;
				if (Main.myPlayer == Projectile.owner)
				{
					int amount = Main.rand.Next(1, 2); // 2-3 снежка
					for (int i = 0; i < amount; i++)
					{
						Vector2 randomVel = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
						Projectile.NewProjectile(
							Projectile.GetSource_FromAI(),
							Projectile.Center,
							randomVel,
							ProjectileID.SnowBallFriendly,
							Projectile.damage / 5,
							Projectile.knockBack / 2,
							Projectile.owner
						);
					}
				}
			}

			// --- ТРЕЙЛ (эффект следа) ---
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
			dust.noGravity = true;
			dust.scale = 1.2f;
			dust.velocity *= 0.2f;
		}

		public override void OnKill(int timeLeft)
		{
			// --- ЭФФЕКТ ВЗРЫВА СНЕГА ---
			for (int i = 0; i < 20; i++)
			{
				Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow, velocity.X, velocity.Y, 150, default, 1.5f);
				Main.dust[dust].noGravity = true;
			}

			// --- ЗВУК ---
			SoundEngine.PlaySound(SoundID.Item30 with { PitchVariance = 0.2f, Volume = 1f }, Projectile.Center);
		}

		// --- ВИЗУАЛЬНЫЙ ТРЕЙЛ (2 копии) ---
		public override bool PreDraw(ref Color lightColor)
		{
			// рисуем 2 копии с прозрачностью
			Microsoft.Xna.Framework.Graphics.Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 2; k++)
			{
				Vector2 drawPos = Projectile.oldPos.Length > k ? Projectile.oldPos[k] + drawOrigin - Main.screenPosition : Projectile.position;
				Color color = Color.White * (0.6f - 0.3f * k);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
	}
}
