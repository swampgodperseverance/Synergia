using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.Main;

namespace Vanilla.Content.Buffs
{
	public class EverwoodJavelinDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			debuff[Type] = true;
			buffNoSave[Type] = true;
			buffNoTimeDisplay[Type] = false;
			pvpBuff[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{

			if (npc.buffTime[buffIndex] % 10 == 0)
			{
				int damage = 5;

				npc.life -= damage;

				CombatText.NewText(npc.Hitbox, CombatText.DamagedHostile, damage);

				SoundEngine.PlaySound(SoundID.NPCHit1 with { Pitch = -0.2f }, npc.Center);

				for (int i = 0; i < 3; i++)
				{
					Dust.NewDust(
						npc.position,
						npc.width,
						npc.height,
						DustID.Blood,
						Main.rand.NextFloat(-1.5f, 1.5f),
						Main.rand.NextFloat(-1.5f, 1.5f),
						100,
						default,
						1.2f
					);
				}

				if (npc.life <= 0 && !npc.SpawnedFromStatue)
				{
					npc.HitEffect();
					npc.active = false;
				}
			}
		}
	}
}
