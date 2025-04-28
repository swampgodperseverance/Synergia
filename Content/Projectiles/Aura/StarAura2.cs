using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Projectiles;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
	[ExtendsFromMod("ValhallaMod")]
	public class StarAura2 : AuraAI
	{
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			//Projectile.damage = 50;

			//Special
			shootSpawnStyle = AuraShootStyles.Periodically;
			shootCooldown = 120;
			shootType = ProjectileType<ValhallaMod.Projectiles.Aura.Damage.StarAuraDamage>();
			shootCount = 5;
			shootSpeed = 7.5f;
			shootDirectionStyle = AuraShootDirection.SmartRandom;

			buffType = BuffType<ValhallaMod.Buffs.Aura.LowGravityBuff>();


			distanceMax = 240f;

			orbSpeed = 2f;
			orbCount = 4;
			orbTrailCount = 10;
			orbTrailGap = 0.03f;

			auraColor = new Color(100, 100, 0);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}

