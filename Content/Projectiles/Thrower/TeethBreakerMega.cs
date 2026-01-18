using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using ValhallaMod;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Thrower
{
	// Token: 0x020001F4 RID: 500
	public class TeethBreakerMega : ValhallaGlaive
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x00012DA5 File Offset: 0x00010FA5
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0005B780 File Offset: 0x00059980
		public override void SetDefaults()
		{
			base.Projectile.width = 22;
			base.Projectile.height = 22;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = DamageClass.Throwing;
			base.Projectile.alpha = 0;
			base.Projectile.penetrate = 103;
			base.Projectile.scale = 1f;
			base.Projectile.tileCollide = true;
			base.Projectile.extraUpdates = 1;
			this.glaive = true;
			this.bounces = 0;
			this.timeFlying = 30;
			this.speedHoming = 9f;
			this.speedFlying = 10f;
			this.speedComingBack = 24f;
			this.homingDistanceMax = 200f;
			this.homingStyle = 2;
			this.homingStart = false;
			this.tileBounce = true;
			this.rotationSpeed = 0.20f;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0003DD39 File Offset: 0x0003BF39
		public override bool PreDraw(ref Color lightColor)
		{
			return DrawHelper.DrawTrail(base.Projectile, Main.spriteBatch, lightColor * 0.25f, 2, true, false);
		}
	    public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Point tilePos = Projectile.Center.ToTileCoordinates();

			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					int i = tilePos.X + x;
					int j = tilePos.Y + y;

					if (WorldGen.InWorld(i, j))
					{
						Tile tile = Main.tile[i, j];
						if (tile.HasTile && Main.tileSolid[tile.TileType])
						{
							WorldGen.KillTile(i, j, false, false, true);
						}
					}
				}
			}

			return false;
		}

	}
}
