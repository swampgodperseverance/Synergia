using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class ColdFatherSnowballAttack : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int snowballTimer = 0;
    private bool warningShown = false;
    private Vector2 warningPos;

    public override void AI(NPC npc)
    {
        if (npc.type == ModContent.NPCType<ValhallaMod.NPCs.Snowman.ColdFather>())
        {
            if (npc.life < npc.lifeMax / 2)
            {
                snowballTimer++;

                if (snowballTimer == 1)
                {
                    // Target player
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
                            ModContent.ProjectileType<Vanilla.Content.Projectiles.StoneMark>(), 
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
                }
            }
        }
    }
}