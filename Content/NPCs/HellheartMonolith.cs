using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using static Synergia.Common.SUtils.LocUtil;
using System;

namespace Synergia.Content.NPCs
{
    public class HellheartMonolith : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);

        private float breathTimer;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 95;
            NPC.height = 170;
            NPC.alpha = 45;

            NPC.lifeMax = 25000;
            NPC.damage = 0;
            NPC.defense = 18;

            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;

            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath14;

            NPC.value = 0f;
        }

        public override void AI()
        {
     
            breathTimer += 0.025f;
            float pulse = (float)Math.Sin(breathTimer) * 0.04f;
            NPC.scale = 1f + pulse;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = NPC.Center - screenPos;

            Color crystalColor = Color.White * 0.92f; 
            spriteBatch.Draw(
                texture,
                drawPos,
                null,
                crystalColor,
                0f,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            return false; 
        }


        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>(
                Texture + "_Glow"
            ).Value;

            Vector2 origin = glow.Size() / 2f;

            float glowPulse =
                0.65f +
                (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2.2f) * 0.25f;

            spriteBatch.Draw(
                glow,
                NPC.Center - screenPos,
                null,
                Color.White * glowPulse,
                0f,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
        }
    }
}
