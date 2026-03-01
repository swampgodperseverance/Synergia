using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using System;
using System.IO;
using System.Collections.Generic;

namespace Synergia.Content.NPCs.Boss.SinlordWyrm
{
	public class SinlordTail : SinlordBody
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {Hide = true};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (!Main.dedServ)
                {
                    var source = NPC.GetSource_Death();

                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore7").Type);
                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore8").Type);
                    Gore.NewGore(source, NPC.position, NPC.velocity, Mod.Find<ModGore>("SinlordGore9").Type);
                }
            }
        }
        public override bool PreDraw(SpriteBatch sprite, Vector2 screenPosition, Color lightColor) {
			lightColor = NPC.GetNPCColorTintedByBuffs(lightColor);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			sprite.Draw(texture, NPC.Center - screenPosition, null, lightColor, NPC.rotation + MathHelper.PiOver2, new Vector2(texture.Width) * 0.5f, NPC.scale, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow");
			sprite.Draw(texture, NPC.Center - screenPosition, null, Color.White, NPC.rotation + MathHelper.PiOver2, new Vector2(texture.Width) * 0.5f, NPC.scale, SpriteEffects.None, 0);
			if(NPC.localAI[0] <= 0f) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "_White");
			sprite.Draw(texture, NPC.Center - screenPosition, null, new Color(253, 32, 2, 0) * NPC.localAI[0], NPC.rotation + MathHelper.PiOver2, new Vector2(texture.Width) * 0.5f, NPC.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
