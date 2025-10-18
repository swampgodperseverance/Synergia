using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
	public class AngryParasiteProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();

			Projectile.WhipSettings.Segments = 20;
			Projectile.WhipSettings.RangeMultiplier = 1.1f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextFloat() < 0.5f)
			{
				target.AddBuff(BuffID.Poisoned, 60); 
			}
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			List<Vector2> segments = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, segments);
			int segmentHeight = 30; 
			int headHeight = 18;    
			//int totalSegments = 5;   
			for (int i = 0; i < segments.Count - 1; i++)
			{
				Vector2 segmentStart = segments[i];
				Vector2 segmentEnd = segments[i + 1];
				float rotation = (segmentEnd - segmentStart).ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(segmentStart.ToTileCoordinates());
				Rectangle frame;
				if (i == 0) 
				{
						frame = new Rectangle(0, 0, 18, segmentHeight);
				}
				else if (i == segments.Count - 2) 
				{
						frame = new Rectangle(0, texture.Height - headHeight, 18, headHeight);
				}
				else 
				{
						int bodyFrameY = segmentHeight + ((i - 1) % 3) * segmentHeight; 
						frame = new Rectangle(0, bodyFrameY, 18, segmentHeight);
				}
				Main.EntitySpriteDraw(
						texture,
						segmentStart - Main.screenPosition,
						frame,
						color,
						rotation,
						new Vector2(9, segmentHeight / 2), 
						Projectile.scale,
						SpriteEffects.None,
						0
				);
			}

			return false;
		}
	}
}