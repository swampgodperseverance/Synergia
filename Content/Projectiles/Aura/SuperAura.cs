using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Projectiles;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
	[ExtendsFromMod("ValhallaMod")]
	public class SuperAura : AuraAI
	{
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			//Projectile.damage = 50;

			//********** Radius **********//
			// Base aura radius
			distanceMax = 320f;


			//********** Buffs **********//

			// BuffID applied to owner, teammates and town NPC within aura's range
			//Default: 0
			buffType = BuffID.Ironskin;

			// Second BuffID applied to owner, teammates and town NPC within aura's range
			//Default: 0
			buffType = BuffID.Shine;

			// BuffID applied to enemies
			//Default: 0
			debuffType = BuffID.Poisoned;

			// Second BuffID applied to enemies
			//Default: 0
			debuffType2 = BuffID.OnFire;


			//********** Shoot Projectiles **********//
			// Shoot Style:
			// None - No Projectile will be spawned;
			// OnCreate - One Projectile will be spawned on creation of aura
			// Periodically - Projectiles will be spawned every 'shootCooldown' frames
			//Default: 0
			shootSpawnStyle = AuraShootStyles.Periodically;

			// ProjectileID of projectile that this aura will shoot
			//Default: 0
			shootType = ProjectileType<PathogenicAuraSpawn>();
			// The number of projectiles that this aura will shoot
			//Default: 1
			shootCount = 10;
			// The speed of the projectile (measured in pixels per frame)
			//Default: 5f
			shootSpeed = 5f;
			// Cooldown for projectile shot (60 ticks == 1 second)
			//Default: 0
			shootCooldown = 120;
			// Direction to shoot projectile 
			// Random - Every shot is in random direction
			// Step - Every shot is moved by angle (shootStep * orbSpeed / 100)
			// SmartRandom - Shots are directed to closest enemy within aura's range, if none found shoots like Random
			// SmartStep - Shots are directed to closest enemy within aura's range, if none found shoots like Step
			//Default: AuraShootDirection.Random
			shootDirectionStyle = AuraShootDirection.SmartStep;
			// Value changing shoot angle of projectile shot by AuraShootDirection.Step
			//Default: 0
			shootStep = 2f;


			//********** Draw **********//
			// Count of orbs that circle around aura border
			//Default: 4
			orbCount = 12;
			// Circle speed of orbs
			//Default: 1f
			orbSpeed = 0.2f;
			// Trail of orbs
			//Default: 10
			orbTrailCount = 10;
			// Distance between trail of orbs
			//Default: 0.03f
			orbTrailGap = 0.01f;

			// Color of aura
			//Default: new Color(255, 255, 255, 150)
			auraColor = new Color(0, 0, 255, 150);
			// Second color of aura
			//Default: null
			auraColor2 = new Color(255, 0, 0, 150);


			//********** Spectre Cuts **********//
			// If 'true' aura will cut enemies within its range
			//Default: false
			spectreCut = true;
			// DustID used by spectre cuts
			//Default: -1
			spectreCutDust = DustID.Torch;
			// Scale of spectre cut dust
			//Default: 1.0f
			// Scale of spectre cut dust
			spectreCutDustScale = 4.0f;
			// Cooldown for spectre cut to attack (60 ticks == 1 second)
			//Default: 0
			spectreCutCooldown = 20;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}

