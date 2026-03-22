using Microsoft.Xna.Framework;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Other
{ //Code is from abandoned Victima Mod, devs allowed me to use their code
	public abstract class HeavyWeaponProj : ModProjectile
	{
		public float length = 40;
		public float speed = 1f;
		public int comboTimer = 25;
		public int width = 50;
		public int height = 50;

		public virtual void SetStats(ref float length, ref float speed, ref int comboTimer, ref int width, ref int height)
		{
		}

		public float SetSwingSpeed(float speed)
		{
			Player player = Main.player[Projectile.owner];
			return speed * player.GetAttackSpeed(DamageClass.Melee);
		}

		public override void SetDefaults()
		{
			SetStats(ref length, ref speed, ref comboTimer, ref width, ref height);
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.width = width;
			Projectile.height = height;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 0;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
		}

		public override bool ShouldUpdatePosition() => false;

		public bool right = false; 
		public Player player;
		public Vector2 startVector;
		public Vector2 vel;
		public float angle = 0;
		public float k = 0;
		public float combo = 0;
		public float bonusSpeed = 0;

		public override void AI()
		{
			player = Main.player[Projectile.owner];

			ExtraAI(player);

			player.itemTime = 2;
			player.itemAnimation = 2;
			if(Projectile.ai[1] <= 1)
				player.heldProj = Projectile.whoAmI;
			bonusSpeed = SetSwingSpeed(1);
			Projectile.localNPCHitCooldown = (int)(18 / speed / bonusSpeed);

			if (Main.myPlayer == Projectile.owner)
			{
				switch (Projectile.ai[0])
				{
					case 0:
						Projectile.Center = player.MountedCenter + (new Vector2(0, -1)).SafeNormalize(Vector2.UnitX).RotatedBy(angle) * length;
						startVector = (Projectile.Center - player.MountedCenter).SafeNormalize(Vector2.UnitX) * length;
						Projectile.ai[0] += 1;
						break;
					case 1:
						if (Projectile.ai[1] == 0)
						{
							SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
							Projectile.alpha = 0;
                            player.GetModPlayer<ScreenShakePlayer>().TriggerShake(10, 1.5f * speed * bonusSpeed);
                            k = 10f * speed * bonusSpeed;
							Projectile.ai[1] = 1;
						}
						vel = startVector.RotatedBy(MathHelper.ToRadians(angle));
						if (angle < 190f)
						{
							angle += k;
							if (angle >= 140 / speed / bonusSpeed)
								Projectile.alpha += 10;
							if(k > 2.5f && angle < 180)
								k *= 0.96f;
							else if ( k > 2f)
								k *= 0.95f;
							if(angle < 20)
								k += 5 * speed * bonusSpeed;
						}
						else
						{
							Projectile.ai[1]++;
							Projectile.alpha += 10;
							if (Projectile.ai[1] < comboTimer)
							{
								if ((Main.mouseLeftRelease && Main.mouseLeft || Main.mouseRightRelease && Main.mouseRight && right) && Projectile.ai[1] > comboTimer / 5)
								{
									if (Main.MouseWorld.X < player.MountedCenter.X && player.direction == 1)
										player.direction = -1;
									else if (Main.MouseWorld.X > player.MountedCenter.X && player.direction == -1)
                                        player.direction = 1;
                                    if (combo < 20)
										combo += 1;
									Projectile.ai[1] = 0;
									Projectile.ai[0] = 2;
                                    OnClick(player);
                                }
							}
							else
							{
								Projectile.Kill();
							}
						}
						break;
					case 2:
						if (Projectile.ai[1] == 0)
						{
							SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
							Projectile.alpha = 0;
                            player.GetModPlayer<ScreenShakePlayer>().TriggerShake(10, 1.5f * speed * bonusSpeed);
                            k = 10f * speed * bonusSpeed;
							Projectile.ai[1] = 1;
						}
						vel = startVector.RotatedBy(MathHelper.ToRadians(angle));
						if (angle > -10f)
						{
							angle -= k;
							if (angle <= 40 / speed / bonusSpeed)
								Projectile.alpha += 10;
							if(k > 2.5f && angle > 0)
								k *= 0.96f;
							else if ( k > 2f)
								k *= 0.95f;
							if(angle > 170)
								k += 5 * speed * bonusSpeed;
						}
						else
						{
							Projectile.ai[1]++;
							Projectile.alpha += 10;
							if (Projectile.ai[1] < comboTimer)
							{
								if ((Main.mouseLeftRelease && Main.mouseLeft || Main.mouseRightRelease && Main.mouseRight && right) && Projectile.ai[1] > comboTimer / 5)
								{
									if (combo < 20)
										combo += 1;
									Projectile.ai[1] = 0;
									Projectile.ai[0] = 1;
                                    OnClick(player);
                                }
							}
							else
							{
								Projectile.Kill();
							}
						}
						break;
				}
			}

			float i = angle < 90 ? MathHelper.Max(angle / 72.5f, 0.95f) : MathHelper.Max((190 - angle) / 72.5f, 0.95f);
			Projectile.spriteDirection = player.direction;
			Projectile.scale = i;
			Projectile.Center = player.MountedCenter + new Vector2(vel.X * player.direction, vel.Y);
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2);
			if (Projectile.spriteDirection == 1)
				Projectile.rotation = (Projectile.Center - player.Center).ToRotation() + MathHelper.PiOver4 - (Projectile.ai[0] == 2 ? 0 : MathHelper.PiOver2);
			else
				Projectile.rotation = (Projectile.Center - player.Center).ToRotation() - MathHelper.Pi - MathHelper.PiOver4 + (Projectile.ai[0] == 2 ? 0 : MathHelper.PiOver2);

			if (player.noItems || player.CCed || player.dead || !player.active
				|| (player.GetModPlayer<playerOther>().hurt == true && (Projectile.ai[0] == 1 || Projectile.ai[0] == 2)))
			{
				Projectile.Kill();
			}

			player.GetDamage(DamageClass.Generic) += combo / 50;
		}

		public virtual void ExtraAI(Player player)
		{
		}

		public virtual void OnClick(Player player)
		{
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            player.GetModPlayer<ScreenShakePlayer>().TriggerShake(10, 1.5f * speed * bonusSpeed);
        }

		public override bool? CanHitNPC(NPC target) => Projectile.alpha < 255 && !target.townNPC && ((angle < 170 && angle > 10) || (Projectile.ai[0] != 1 && Projectile.ai[0] != 2)) ? true : false;
	}
    public class playerOther : ModPlayer
    {
        public bool hurt;

        public override void ResetEffects()
        {
            hurt = false;
        }

        public override void PreUpdate()
        {
            hurt = false;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            hurt = true;
        }
    }
}