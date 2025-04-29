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
	public class PathogenicAura : AuraAI
	{
		private static Mod avalon = ModLoader.GetMod("Avalon");
		
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
			shootType = ProjectileType<PathogenicAuraSpawn>();
			shootCount = 6;
			shootSpeed = 7.5f;
			shootDirectionStyle = AuraShootDirection.SmartRandom;

			debuffType = BuffType<Avalon.Buffs.Debuffs.Lacerated>();

			buffType = BuffType<Avalon.Buffs.BacterialEndurance>();

			distanceMax = 320f;

			orbSpeed = 1f;
			orbCount = 8;
			orbTrailCount = 10;
			orbTrailGap = 0.03f;

			auraColor = new Color(171, 208, 60);
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}

