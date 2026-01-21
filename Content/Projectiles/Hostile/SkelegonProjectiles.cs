using Avalon.Common;
using Avalon.Common.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile
{
    public class SkelegonProj3 : ModProjectile
    {
 		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 2;
			Projectile.scale = 1.2f;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;
			//Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 45)
			{
				Projectile.velocity.Y += 0.15f;
			}
			Projectile.velocity *= 0.99f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Rectangle frame = TextureAssets.Projectile[Type].Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 drawPos = Projectile.position - Main.screenPosition + frameOrigin;

			for (int i = 1; i < 4; i++)
			{
				Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos + new Vector2(Projectile.velocity.X * (-i * 2), Projectile.velocity.Y * (-i * 2)), frame, (lightColor * (1 - (i * 0.25f))) * 0.5f, Projectile.rotation + (i * -0.3f * Projectile.direction), frameOrigin, Projectile.scale * (1 - (i * 0.1f)), SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos, frame, lightColor, Projectile.rotation, frameOrigin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, -Projectile.velocity.X * 0.25f, -Projectile.velocity.Y * 0.25f, default, default, 0.9f);
			}
			SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);
		}
	}
    public class SkelegonProj1 : ModProjectile
    {
 		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 2;
			Projectile.scale = 1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;
			//Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 45)
			{
				Projectile.velocity.Y += 0.15f;
			}
			Projectile.velocity *= 0.99f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Rectangle frame = TextureAssets.Projectile[Type].Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 drawPos = Projectile.position - Main.screenPosition + frameOrigin;

			for (int i = 1; i < 4; i++)
			{
				Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos + new Vector2(Projectile.velocity.X * (-i * 2), Projectile.velocity.Y * (-i * 2)), frame, (lightColor * (1 - (i * 0.25f))) * 0.5f, Projectile.rotation + (i * -0.3f * Projectile.direction), frameOrigin, Projectile.scale * (1 - (i * 0.1f)), SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos, frame, lightColor, Projectile.rotation, frameOrigin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, -Projectile.velocity.X * 0.25f, -Projectile.velocity.Y * 0.25f, default, default, 0.9f);
			}
			SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);
		}
	}
    public class SkelegonProj2 : ModProjectile
    {
 		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 2;
			Projectile.scale = 1.1f;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;
			//Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0]++;
			if (Projectile.ai[0] > 45)
			{
				Projectile.velocity.Y += 0.15f;
			}
			Projectile.velocity *= 0.99f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Rectangle frame = TextureAssets.Projectile[Type].Frame();
			Vector2 frameOrigin = frame.Size() / 2f;
			Vector2 drawPos = Projectile.position - Main.screenPosition + frameOrigin;

			for (int i = 1; i < 4; i++)
			{
				Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos + new Vector2(Projectile.velocity.X * (-i * 2), Projectile.velocity.Y * (-i * 2)), frame, (lightColor * (1 - (i * 0.25f))) * 0.5f, Projectile.rotation + (i * -0.3f * Projectile.direction), frameOrigin, Projectile.scale * (1 - (i * 0.1f)), SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, drawPos, frame, lightColor, Projectile.rotation, frameOrigin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, -Projectile.velocity.X * 0.25f, -Projectile.velocity.Y * 0.25f, default, default, 0.9f);
			}
			SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.Center);
		}
	}
}