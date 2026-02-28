using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.IO;

public class BrainDashAI : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int dashCooldown = 0;

    //Note from Not U.N. Owen: If you are using a GlobalNPC to modify just ONE entity, be sure to limit it with this.
    //It causes a lot of performance issues in multiplayer as it applies an instantiated GlobalNPC to all mobs instead of just the one being changed.
    public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.BrainofCthulhu;

    public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
        if(Main.netMode > 0) binaryWriter.Write(dashCooldown);
    }
    public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
        if(Main.netMode > 0) dashCooldown = binaryReader.ReadInt32();
	}

    public override void AI(NPC npc)
    {
        if (npc.life >= npc.lifeMax * 0.75)
            return;

        Player target = Main.player[npc.target];
        if (!target.active || target.dead)
            return;

        dashCooldown--;
        if (dashCooldown <= 0)
        {
            dashCooldown = 90; 

            Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.Zero);
            float dashSpeed = 18f;

            npc.velocity = direction * dashSpeed;

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
            }

            SoundEngine.PlaySound(SoundID.Roar, npc.position);
            npc.netUpdate = true;
        }
    }
}
