using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Synergia.Content.Buffs;

public class EverwoodJavelinDebuffEffect : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int debuffTimer = 0;

    private int frameCounter = 0;
    private int currentFrame = 0;
    private const int MaxFrames = 4; 
    private const int FrameSpeed = 18;   

    public override void AI(NPC npc)
    {
        if (npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()) && !npc.boss)
        {
            npc.velocity = Vector2.Zero;
            npc.position = npc.oldPosition;

            npc.direction = npc.oldDirection;
            npc.spriteDirection = npc.oldDirection;

            if (npc.rotation != 0f)
                npc.rotation = MathHelper.Lerp(npc.rotation, 0f, 0.25f);

            npc.netUpdate = true;
        }
    }

    public override void ResetEffects(NPC npc)
    {
        if (!npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()))
        {
            debuffTimer = 0;
            frameCounter = 0;
            currentFrame = 0;
        }
    }

    public override void PostAI(NPC npc)
    {
        if (npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()))
        {
            debuffTimer++;

            frameCounter++;
            if (frameCounter >= FrameSpeed)
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % MaxFrames;
            }
        }
    }

    public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (npc.HasBuff(ModContent.BuffType<EverwoodJavelinDebuff>()) && debuffTimer >= 1 && !npc.boss)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Synergia/Common/GlobalNPCs/EverwoodJavelinDebuffEffect").Value;
            int frameHeight = texture.Height / MaxFrames;
            Rectangle sourceRect = new Rectangle(0, frameHeight * currentFrame, texture.Width, frameHeight);

            Vector2 position = new Vector2(npc.Center.X, npc.position.Y + npc.height - 14) - screenPos;

            float alpha = MathHelper.Clamp((debuffTimer - 3) / 7f, 0f, 0.8f);
            Color color = Color.White * alpha;

            spriteBatch.Draw(
                texture,
                position,
                sourceRect,
                color,
                0f,
                new Vector2(texture.Width / 2f, frameHeight / 2f),
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}