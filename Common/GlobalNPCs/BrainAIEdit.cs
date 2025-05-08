using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class BrainAIEdit : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int fireTimer = 0;

    public override void AI(NPC npc)
    {
        // Проверяем, является ли этот NPC Creeper'ом (приспешником мозга Ктулху)
        if (npc.type == NPCID.Creeper)
        {
            // Только на стороне сервера (не в клиентской части)
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                fireTimer++;

                if (fireTimer >= 500) // 4 секунды * 60 тиков
                {
                    fireTimer = 0;

                    // Если цель существует и активна
                    Player target = Main.player[npc.target];
                    if (target != null && target.active && !target.dead)
                    {
                        Vector2 direction = target.Center - npc.Center;
                        direction.Normalize();
                        float speed = 6f;
                        Vector2 velocity = direction * speed;

                        // GreekFire1 — один из ванильных снарядов огня
                        int type = ProjectileID.GoldenShowerHostile;
                        int damage = 20;
                        float knockback = 1f;

                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, velocity, type, damage, knockback, Main.myPlayer);
                    }
                }
            }
        }
    }
}