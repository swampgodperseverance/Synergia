using System;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using System.IO;
using System.Linq;
using Synergia.Common.ModSystems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Helpers;
using Synergia.Trails;
using NewHorizons.Content.Projectiles.Throwing;


namespace Synergia.Content.Projectiles.Thrower
{
	public class NightGlaive2 : ModProjectile
	{
		private float flashIntensity;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.scale = 1f;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = 1;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation += 0.22f * Projectile.direction;

			if (Projectile.ai[0] == 0f)
			{
				Projectile.velocity *= 0.98f;
				Projectile.ai[1]++;
				if (Projectile.ai[1] > 40f)
				{
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
			}
			else if (Projectile.ai[0] == 1f)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					float speed = 6.5f;
					for (int i = 0; i < 4; i++)
					{
						Vector2 vel = Vector2.One.RotatedBy(MathHelper.PiOver2 * i).SafeNormalize(Vector2.UnitX) * speed;
						Projectile.NewProjectile(
							Projectile.GetSource_FromThis(),
							Projectile.Center,
							vel,
							ModContent.ProjectileType<NightGlaiveProj>(),
							Projectile.damage / 3,
							Projectile.knockBack,
							Projectile.owner
						);
					}
				}

				SoundEngine.PlaySound(SoundID.Item72, Projectile.Center);
				flashIntensity = 1f;
				Projectile.ai[0] = 2f;
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
			else
			{
				Player player = Main.player[Projectile.owner];
				Projectile.tileCollide = false;
				Vector2 vec = Vector2.Normalize(player.Center - Projectile.Center) * 8f;
				Projectile.velocity = (Projectile.velocity * 10f + vec) / 11f;

				if (Projectile.Hitbox.Intersects(player.Hitbox) && Main.myPlayer == Projectile.owner)
					Projectile.Kill();
			}

			if (flashIntensity > 0f)
				flashIntensity *= 0.88f;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 4;
			height = 4;
			fallThrough = true;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 origin = tex.Size() / 2f;

			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					continue;

				float fade = 1f - i / (float)Projectile.oldPos.Length;
				Main.EntitySpriteDraw(
					tex,
					Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition,
					null,
					new Color(170, 90, 255, 0) * fade * 0.55f,
					Projectile.rotation,
					origin,
					Projectile.scale * (1f - i * 0.04f),
					SpriteEffects.None,
					0
				);
			}

			BlendState oldBlend = Main.graphics.GraphicsDevice.BlendState;

			if (flashIntensity > 0.01f)
			{
				Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;

				Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
				float rot = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				float scale = 0.6f + flashIntensity * 1.6f;

				Color core = new Color(190, 90, 255) * flashIntensity;
				Color bloom = new Color(130, 60, 220) * flashIntensity * 0.6f;

				Main.EntitySpriteDraw(
					glowTex,
					Projectile.Center - Main.screenPosition,
					null,
					bloom,
					rot,
					glowTex.Size() / 2f,
					scale * 1.4f,
					SpriteEffects.None,
					0
				);

				Main.EntitySpriteDraw(
					glowTex,
					Projectile.Center - Main.screenPosition,
					null,
					core,
					rot,
					glowTex.Size() / 2f,
					scale,
					SpriteEffects.None,
					0
				);

				Main.graphics.GraphicsDevice.BlendState = oldBlend;
			}

			Main.EntitySpriteDraw(
				tex,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation,
				origin,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			return false;
		}
	}
	public class NightGlaiveProj : ModProjectile{
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults() {
            Projectile.width = 22; 
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.timeLeft = 150;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.alpha = 215;
            Projectile.extraUpdates = 1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = 10; 
            fallThrough = true; 
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) 
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) {
                Projectile.velocity.X += Projectile.direction * 2.5f;
                Projectile.velocity.Y = -oldVelocity.Y * 1.2f;
            }
            return false;
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item10.WithVolumeScale(0.9f).WithPitchOffset(0.25f), Projectile.Center);
            for (byte i = 0; i < 7; i++) {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, 0f, 0f, 100, Color.Purple, 1.4f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 2f;
            }
        }

        public override void AI() {
            if (Projectile.alpha > 90) Projectile.alpha -= 4;
            else Projectile.alpha = 90;

            if (Projectile.timeLeft > 135) Projectile.velocity *= 0.92f;
            else {
                Projectile.velocity.X *= 0.963f;
                if (Projectile.timeLeft > 120) Projectile.velocity.Y += 0.04f;
                else Projectile.velocity.Y = Utils.Clamp(Projectile.velocity.Y - 0.045f, -8f, 0f);
            }

            if (Projectile.velocity.Length() > 10f)
                Projectile.velocity /= 1.1f;

            Projectile.rotation += -Projectile.direction * 0.28f;

            if (Main.rand.NextBool(2)) {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, Projectile.velocity.X, Projectile.velocity.Y, 100, Color.Purple, 1.1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 0.25f;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            byte colValue = (byte)((Projectile.timeLeft + 75) * Projectile.Opacity);
            lightColor = new Color(colValue, colValue, colValue, colValue);
            MUtils.DrawSimpleAfterImage(lightColor, Projectile, TextureAssets.Projectile[Type].Value, 1f, 1f, 0.2f, 2f);
            return true;
        }
    }
} 