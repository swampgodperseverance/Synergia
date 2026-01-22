using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using NewHorizons.Globals;
using NewHorizons.Content.Projectiles.Throwing;
using NewHorizons.Content.Items.Weapons.Throwing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
	public class OrichalcumKama2 : ModProjectile
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00008E18 File Offset: 0x00007018
		public override void SetDefaults()
		{
			base.Projectile.width = 38;
			base.Projectile.height = 38;
			base.Projectile.friendly = true;
			base.Projectile.timeLeft = 600;
			base.Projectile.penetrate = 1;
			base.Projectile.DamageType = DamageClass.Throwing;
			base.Projectile.extraUpdates = 1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00008E9C File Offset: 0x0000709C
		public override void AI()
		{
			Player player = Main.player[base.Projectile.owner];
			base.Projectile.rotation += 0.188f * (float)base.Projectile.direction;
			base.Projectile.spriteDirection = base.Projectile.direction;
			Projectile projectile = base.Projectile;
			projectile.velocity.Y = projectile.velocity.Y + 0.15f;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003E4B File Offset: 0x0000204B
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(lightColor);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00008F10 File Offset: 0x00007110
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, new Vector2?(base.Projectile.position), null);
			if (Main.rand.NextBool(10))
			{
				Item.NewItem(base.Projectile.GetSource_DropAsItem(null), (int)base.Projectile.position.X, (int)base.Projectile.position.Y, base.Projectile.width, base.Projectile.height, ModContent.ItemType<OrichalcumKama>(), 1, false, 0, false, false);
				return;
			}
			for (int i = 0; i < 12; i++)
			{
				int num = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 145, 0f, 0f, 0, default(Color), 1.6f);
				Main.dust[num].velocity *= 3f;
				Main.dust[num].noGravity = true;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000073FC File Offset: 0x000055FC
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.myPlayer == Projectile.owner)
			{
				for (int i = 0; i < 3; i++)
				{
					float theta = Main.rand.NextFloat(MathHelper.TwoPi);
					float mag = 240f;

					Vector2 spawnPos = target.Center + new Vector2(
						(float)Math.Cos(theta),
						(float)Math.Sin(theta)
					) * mag;

					Vector2 velocity = -Vector2.Normalize(spawnPos - target.Center) * 8f;

					Projectile.NewProjectile(
						Projectile.InheritSource(Projectile),
						spawnPos,
						velocity,
						ModContent.ProjectileType<OrichalcumKamaProj2>(),
						Projectile.damage / 2,
						0f,
						Main.myPlayer
					);
				}
			}
		}

	}
}
