using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Vanilla.Content.Buffs;

public class EverwoodJavelinDebuffEffect : GlobalNPC
{
	public override bool InstancePerEntity => true;

	private int debuffTimer = 0;

	public override void AI(NPC npc)
	{
		if (npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()) && !npc.boss)
		{
			// Останавливаем движение
			npc.velocity = Vector2.Zero;
			npc.position = npc.oldPosition;

			// Сохраняем направление
			npc.direction = npc.oldDirection;
			npc.spriteDirection = npc.oldDirection;

			// Принудительно блокируем вращение, но мягко
			if (npc.rotation != 0f)
				npc.rotation = MathHelper.Lerp(npc.rotation, 0f, 0.25f);

			npc.netUpdate = true;
		}
	}

	public override void ResetEffects(NPC npc)
	{
		if (!npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()))
			debuffTimer = 0;
	}

	public override void PostAI(NPC npc)
	{
		if (npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()))
			debuffTimer++;
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()) && debuffTimer >= 1 && !npc.boss)
		{
			Texture2D texture = ModContent.Request<Texture2D>("Vanilla/Common/GlobalNPC/EverwoodJavelinDebuffEffect").Value;
			Vector2 position = new Vector2(npc.Center.X, npc.position.Y + npc.height - 14) - screenPos;

			// Плавный переход: после 10 тиков максимальная прозрачность = 0.8f
			float alpha = MathHelper.Clamp((debuffTimer - 3) / 7f, 0f, 0.8f);

			Color color = Color.White * alpha;

			spriteBatch.Draw(
				texture,
				position,
				null,
				color,
				0f,
				texture.Size() * 0.5f,
				1f,
				SpriteEffects.None,
				0f
			);
		}
	}
}
