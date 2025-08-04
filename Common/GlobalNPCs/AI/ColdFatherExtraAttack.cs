using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ValhallaMod.NPCs;

public class ColdFatherExtraAttack : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int jumpTimer = 0;

    public override void AI(NPC npc)
    {
        if (npc.type == ModContent.NPCType<ValhallaMod.NPCs.Snowman.ColdFather>())
        {
            jumpTimer++;

            if (jumpTimer >= 170)
            {
                npc.velocity.Y = -22f; 
                jumpTimer = 0;
            }
        }
    }
}