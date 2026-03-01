using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ValhallaMod.NPCs;
using System.IO;

public class ColdFatherExtraAttack : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int jumpTimer = 0;

    //Note from Not U.N. Owen: If you are using a GlobalNPC to modify just ONE entity, be sure to limit it with this.
    //It causes a lot of performance issues in multiplayer as it applies an instantiated GlobalNPC to all mobs instead of just the one being changed.
    public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == ModContent.NPCType<ValhallaMod.NPCs.Snowman.ColdFather>();

    public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
        if(Main.netMode > 0) binaryWriter.Write(jumpTimer);
    }
    public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
        if(Main.netMode > 0) jumpTimer = binaryReader.ReadInt32();
	}

    public override void AI(NPC npc)
    {
        jumpTimer++;
        if (jumpTimer >= 170)
        {
             npc.velocity.Y = -22f; 
             jumpTimer = 0;
        }
    }
}
