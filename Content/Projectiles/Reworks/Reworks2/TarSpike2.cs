using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Synergia.Trails;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{ //id like to make something similar to the falling rocks from light and darkness mod, i didnt use any of their code, just was inspired but code mine
	public class TarSpike2 : ModProjectile
	{
		private int tickCounter;
		private Vector2 baseVelocity;
		private PrimDrawer visualTrail;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 6;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = false;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = false;
			Projectile.aiStyle = -1;
			Projectile.idStaticNPCHitCooldown = 15;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.netUpdate = true;
			Projectile.GetGlobalProjectile<TarHelper>().Cangoback = true;
		}

		public override void OnSpawn(IEntitySource source)
		{
			float randomOffset = Main.rand.NextFloat(-200f, 200f);
			Vector2 spawn = new Vector2(Main.MouseWorld.X + randomOffset, Main.screenPosition.Y + Main.screenHeight + 50f);
			Projectile.position = spawn - Projectile.Size / 2f;

			Vector2 targetDir = Main.MouseWorld - Projectile.Center;
			targetDir = targetDir.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10f, 10f)));
			baseVelocity = Vector2.Normalize(targetDir) * 16f;
			Projectile.velocity = baseVelocity;

			MiscShaderData shader = null;
			string[] keys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
			foreach (var k in keys)
				if (GameShaders.Misc.TryGetValue(k, out shader)) break;

			if (shader != null)
			{
				shader.UseImage1("Images/Misc/noise");
				shader.UseOpacity(0.8f);
				shader.UseColor(new Color(139, 69, 19));
				shader.UseSecondaryColor(new Color(160, 82, 45));
			}

			visualTrail = new PrimDrawer(
				widthFunc: t => MathHelper.Lerp(3f, 0.3f, t),
				colorFunc: t =>
				{
					Color start = new Color(139, 69, 19);
					Color end = new Color(160, 82, 45);
					Color c = Color.Lerp(start, end, t);
					c *= (1f - t);
					return c;
				},
				shader: shader
			);
		}

		public override void AI()
		{
			tickCounter++;
			Projectile.rotation -= 0.1f;

			if (tickCounter == 1 && Main.myPlayer == Projectile.owner)
				Projectile.velocity = baseVelocity;

			if (tickCounter is >= 50 and <= 68)
				Projectile.velocity *= 0.92f;

			if (tickCounter == 68)
				Projectile.velocity = Vector2.Zero;

			if (tickCounter > 68)
			{
				Projectile.velocity.Y += 1f;
				Projectile.velocity.X *= 0.5f;
				Projectile.tileCollide = true;
			}

			Lighting.AddLight(Projectile.Center, 0.3f, 0.15f, 0f);
			if (Main.rand.NextBool(3))
			{
				int d = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					DustID.Dirt,
					-Projectile.velocity.X * 0.2f,
					-Projectile.velocity.Y * 0.2f,
					100,
					default,
					1.1f
				);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0.3f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int d = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					DustID.Dirt,
					Main.rand.NextFloat(-2f, 2f),
					Main.rand.NextFloat(-2f, 2f),
					150,
					default,
					1.2f
				);
				Main.dust[d].noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (visualTrail != null)
			{
				List<Vector2> points = Projectile.oldPos
					.Where(v => v != Vector2.Zero)
					.Select(v => v + Projectile.Size / 2f)
					.ToList();

				if (points.Count > 1)
				{
					Vector2 offset = -Main.screenPosition;
					visualTrail.DrawPrims(points, offset, totalTrailPoints: 20);
				}
			}

			Texture2D tex = TextureAssets.Projectile[Type].Value;
			Vector2 pos = Projectile.Center - Main.screenPosition;
			Main.EntitySpriteDraw(tex, pos, null, lightColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
