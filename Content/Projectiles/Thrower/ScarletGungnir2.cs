using NewHorizons.Content.Projectiles.Throwing;
using Synergia.Helpers;
using Synergia.Trails;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Thrower
{
	public class ScarletGungnir2 : ModProjectile
	{
		private int timer;

		public override void SetDefaults()
		{
			Projectile.tileCollide = true;
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 3;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
		}

		public override void AI()
		{
            int d = Dust.NewDust(Projectile.position, 42, 42, DustID.Fireworks, Projectile.oldVelocity.X * 0.4f, Projectile.oldVelocity.Y * 0.4f);
			Main.dust[d].scale = 0.6f;
			Main.dust[d].noGravity = true;

			timer++;

			if (timer % 20 == 0)
			{
				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					Projectile.velocity,
					ModContent.ProjectileType<ScarletGungnirFhantomProj>(),
					Projectile.damage,
					Projectile.knockBack / 2f,
					Projectile.owner
				);
			}

			if (timer % 10 == 0)
			{
				Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.UnitX);
				Vector2 left = dir.RotatedBy(-MathHelper.PiOver2);
				Vector2 right = dir.RotatedBy(MathHelper.PiOver2);

				int dmg = Projectile.damage / 2;

				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					left * 4f,
					ModContent.ProjectileType<BloodySpike>(),
					dmg,
					0f,
					Projectile.owner
				);

				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					right * 4f,
					ModContent.ProjectileType<BloodySpike>(),
					dmg,
					0f,
					Projectile.owner
				);
			}
		}
	}
public class BloodySpike : ModProjectile
    {
        private float accelTimer;
        private const float MaxSpeed = 18f;
        private PrimDrawer trailDrawer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;   
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void OnSpawn(IEntitySource source)
        {
            MiscShaderData shader = null;

            if (GameShaders.Misc.TryGetValue("MagicMissileTrail", out shader) ||
                GameShaders.Misc.TryGetValue("FlameLashTrailColorGradient", out shader))
            {
                shader.UseImage1("Images/Misc/noise");
                shader.UseOpacity(0.85f);
                shader.UseColor(new Color(140, 10, 10));   
                shader.UseSecondaryColor(new Color(180, 20, 20)); 
            }

            trailDrawer = new PrimDrawer(
                widthFunc: t => MathHelper.Lerp(3.5f, 0.8f, t),  
                colorFunc: t =>
                {
                    Color c = new Color(140, 10, 10);           
                    c *= (1f - t) * 1.1f;                       
                    return c;
                },
                shader: shader
            );
        }

        public override void AI()
        {
            for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
            }
            Projectile.oldPos[0] = Projectile.position;
            Projectile.oldRot[0] = Projectile.rotation;

            accelTimer++;
            float t = MathHelper.Clamp(accelTimer / 20f, 0f, 1f);
            float speed = MathHelper.Lerp(2f, MaxSpeed, EaseFunctions.EaseOutBack(t));

            Vector2 targetVel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * speed;

            NPC target = FindTarget(500f);
            if (target != null)
            {
                Vector2 toTarget = Projectile.DirectionTo(target.Center);
                targetVel = Vector2.Lerp(targetVel, toTarget * speed, 0.08f);
            }

            Projectile.velocity = targetVel;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.25f, 0.04f, 0.04f); 
        }

        private NPC FindTarget(float range)
        {
            NPC target = null;
            float dist = range;
            foreach (NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy(this)) continue;
                float d = Vector2.Distance(Projectile.Center, npc.Center);
                if (d < dist)
                {
                    dist = d;
                    target = npc;
                }
            }
            return target;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (trailDrawer != null)
            {
                List<Vector2> points = Projectile.oldPos
                    .Where(v => v != Vector2.Zero)
                    .Select(v => v + Projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
                    trailDrawer.DrawPrims(points, offset, totalTrailPoints: 28);
                }
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit5, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
                Main.dust[d].velocity *= 2.8f;
                Main.dust[d].noGravity = true;
                Main.dust[d].scale *= Main.rand.NextFloat(0.9f, 1.35f);
            }
        }
    }
} 