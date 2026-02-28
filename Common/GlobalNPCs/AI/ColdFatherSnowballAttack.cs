using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.IO;

public class ColdFatherSnowballAttack : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int snowballTimer = 0;
    private bool warningShown = false;
    private Vector2 warningPos;

    //Note from Not U.N. Owen: If you are using a GlobalNPC to modify just ONE entity, be sure to limit it with this.
    //It causes a lot of performance issues in multiplayer as it applies an instantiated GlobalNPC to all mobs instead of just the one being changed.
    public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == ModContent.NPCType<ValhallaMod.NPCs.Snowman.ColdFather>();

	public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
		if(Main.netMode == 0) return;
		bitWriter.WriteBit(warningShown);
        binaryWriter.Write(snowballTimer);
        binaryWriter.WriteVector2(warningPos);
	}
	public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
		if(Main.netMode == 0) return;
		warningShown = bitReader.ReadBit();
        snowballTimer = binaryReader.ReadInt32();
        warningPos = binaryReader.ReadVector2();
	}

    public override void AI(NPC npc)
    {
            if (npc.life < npc.lifeMax / 2)
            {
                snowballTimer++;

                if (snowballTimer == 1)
                {
                    Player target = Main.player[npc.target];
                    warningPos = target.Center + new Vector2(0, -100f);
                    warningShown = false;
                }

                if (snowballTimer == 60 && !warningShown)
                {
                    // First usinf mark
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int warning = Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            warningPos,
                            Vector2.Zero,
                            ModContent.ProjectileType<Synergia.Content.Projectiles.Hostile.StoneMark>(), 
                            0,
                            0f,
                            Main.myPlayer
                        );

                        if (Main.projectile.IndexInRange(warning))
                        {
                            Projectile p = Main.projectile[warning];
                            p.timeLeft = 60; 
                            p.aiStyle = 0;
                        }
                    }

                    warningShown = true;
                }

                if (snowballTimer >= 120)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 velocity = new Vector2(0f, 9f); 

                        int proj = Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            warningPos,
                            velocity,
                            ModContent.ProjectileType<ValhallaMod.Projectiles.Enemy.SnowmanIceBall>(),
                            40,
                            1f,
                            Main.myPlayer
                        );

                        if (Main.projectile.IndexInRange(proj))
                        {
                            Projectile p = Main.projectile[proj];
                            p.timeLeft = 120;
                            p.aiStyle = 0;
                        }
                    }

                    snowballTimer = 0;
                    warningShown = false;
                    npc.netUpdate = true;
                }
            }
        
    }
}
