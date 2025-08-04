using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

public class BrainDashAI : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int dashCooldown = 0;

    public override void AI(NPC npc)
    {
        if (npc.type != NPCID.BrainofCthulhu || npc.life >= npc.lifeMax * 0.75)
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
        }
    }
}