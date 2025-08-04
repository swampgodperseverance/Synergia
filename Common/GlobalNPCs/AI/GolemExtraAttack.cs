using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class GolemExtraAttack : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int attackTimer = 0;

    public override void AI(NPC npc)
    {
        if (npc.type == NPCID.Golem || npc.type == NPCID.GolemHead)
        {
            attackTimer++;

            if (attackTimer >= 300)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Player target = Main.player[npc.target];

                    Vector2 direction = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX);

                    // 8 projs
                    int numberOfProjectiles = 8;
                    float spread = MathHelper.ToRadians(45); 
                    float baseSpeed = 4f;

                    for (int i = 0; i < numberOfProjectiles; i++)
                    {
                        float rotation = MathHelper.Lerp(-spread / 2, spread / 2, i / (float)(numberOfProjectiles - 1));
                        Vector2 perturbedSpeed = direction.RotatedBy(rotation) * baseSpeed;

                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, perturbedSpeed,
                            ProjectileID.Fireball, 20, 1f, Main.myPlayer);
                    }
                }

                attackTimer = 0;
            }
        }
    }
}