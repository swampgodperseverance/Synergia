using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Vanilla.Content.Projectiles
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
				target.AddBuff(BuffID.Poisoned, 60); // Отравление на 1 секунду
			}
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			List<Vector2> segments = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, segments);

			// Размеры каждого сегмента текстуры
			int segmentHeight = 30; // Высота хвоста/тела (18x30)
			int headHeight = 18;    // Высота головы (18x18)
			int totalSegments = 5;   // Всего частей в текстуре

			// Отрисовка каждого сегмента хлыста
			for (int i = 0; i < segments.Count - 1; i++)
			{
				Vector2 segmentStart = segments[i];
				Vector2 segmentEnd = segments[i + 1];
				float rotation = (segmentEnd - segmentStart).ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(segmentStart.ToTileCoordinates());

				// Выбираем часть текстуры в зависимости от позиции
				Rectangle frame;
				if (i == 0) 
				{
						// Хвост (первый сегмент)
						frame = new Rectangle(0, 0, 18, segmentHeight);
				}
				else if (i == segments.Count - 2) 
				{
						// Голова (последний сегмент)
						frame = new Rectangle(0, texture.Height - headHeight, 18, headHeight);
				}
				else 
				{
						// Тело (середина)
						int bodyFrameY = segmentHeight + ((i - 1) % 3) * segmentHeight; // Цикл по 3 сегментам тела
						frame = new Rectangle(0, bodyFrameY, 18, segmentHeight);
				}

				// Отрисовка
				Main.EntitySpriteDraw(
						texture,
						segmentStart - Main.screenPosition,
						frame,
						color,
						rotation,
						new Vector2(9, segmentHeight / 2), // Центр вращения
						Projectile.scale,
						SpriteEffects.None,
						0
				);
			}

			return false;
		}
	}
}