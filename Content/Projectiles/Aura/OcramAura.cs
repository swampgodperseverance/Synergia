using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Projectiles;
using ValhallaMod.Projectiles.AI;
using Avalon.Buffs.Debuffs;
using Avalon.Buffs.AdvancedBuffs;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
	[ExtendsFromMod("ValhallaMod")]
	public class OcramAura : AuraAI
	{
		private static Mod avalon = ModLoader.GetMod("Avalon");
		
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;

			//Special
			shootSpawnStyle = AuraShootStyles.Periodically;
			shootCooldown = 120;
			shootType = ProjectileType<OcramAuraSpawn>();
			shootCount = 4;
			shootSpeed = 5f;
			shootDirectionStyle = AuraShootDirection.SmartRandom;

			debuffType = 70;
			buffType = 176;

			distanceMax = 400f;

			orbSpeed = 1.5f;
			orbCount = 5;
			orbTrailCount = 10;
			orbTrailGap = 0.03f;

			auraColor = new Color(147, 29, 194);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}

