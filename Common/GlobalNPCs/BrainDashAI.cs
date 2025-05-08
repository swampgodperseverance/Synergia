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

        // Таймер между рывками
        dashCooldown--;
        if (dashCooldown <= 0)
        {
            dashCooldown = 90; // каждые 1.5 секунды (60 = 1 секунда)

            // Направление к игроку
            Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.Zero);
            float dashSpeed = 18f;

            // Делаем рывок
            npc.velocity = direction * dashSpeed;

            // Вспомогательная пыль и звук
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
            }

            SoundEngine.PlaySound(SoundID.Roar, npc.position);
        }
    }
}