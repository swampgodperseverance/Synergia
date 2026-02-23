using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using ValhallaMod.Projectiles.Ranged.Thrown;
using ValhallaMod;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
	public class Garlic2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (target.type == 159 || target.type == 158)
			{
				modifiers.SourceDamage *= 100f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundStyle soundStyle = new SoundStyle("ValhallaMod/Sounds/pvz_melonimpact");
			soundStyle.Volume = 0.9f;
			soundStyle.PitchVariance = 0.3f;
			SoundEngine.PlaySound(soundStyle, Projectile.position);

			if (Projectile.owner == Main.myPlayer)
			{
				int count = Main.rand.Next(7, 11);
				float gasSpeedMultiplier = 2.5f;

				for (int i = 0; i < count; i++)
				{
					Vector2 vector = new Vector2(
						Main.rand.Next(-100, 101),
						Main.rand.Next(-100, 101)
					);

					vector.Normalize();
					vector *= Main.rand.NextFloat(2.5f, 5.5f) * gasSpeedMultiplier;

					Projectile.NewProjectile(
						Projectile.GetSource_FromThis(),
						Projectile.Center,
						vector,
						Values.Clouds[Main.rand.Next(3)],
						Projectile.damage,
						1f,
						Projectile.owner
					);
				}
			}
		}
	}
}
