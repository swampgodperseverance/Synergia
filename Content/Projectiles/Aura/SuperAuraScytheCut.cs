using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Vanilla.Content.Projectiles.Aura
{
	[ExtendsFromMod("ValhallaMod")]
	public class SuperAuraScytheCut : ModProjectile
	{
		public override void SetDefaults()
		{
			var proj = Projectile;

			proj.width = 6;
			proj.height = 6;
			proj.aiStyle = 0;
			proj.friendly = true;
			proj.penetrate = 5;
			proj.tileCollide = false;
			proj.timeLeft = 30;
			proj.DamageType = DamageClass.Summon;
			proj.alpha = 255;
			proj.extraUpdates = 4;
		}

		public override void AI()
		{
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] > 0f)
			{
					for (int i = 0; i < 5; i++)
					{
						float num93 = Projectile.velocity.X / 3f * i;
						float num94 = Projectile.velocity.Y / 3f * i;
						int m = 4;
						Vector2 pos = new Vector2(Projectile.Center.X + m, Projectile.Center.Y + m);
						int w = Projectile.width - m * 2;
						int h = Projectile.height - m * 2;
						int dust = Dust.NewDust(pos, w, h, DustID.BlueTorch, 0f, 0f, 100, default, 1.2f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0f;
						Main.dust[dust].position.X = Main.dust[dust].position.X - num93;
						Main.dust[dust].position.Y = Main.dust[dust].position.Y - num94;
					}
			}
		}
	}
}