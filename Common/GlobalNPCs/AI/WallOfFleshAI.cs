using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Synergia.Content.Projectiles.Hostile;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class WallOfFleshGlobal : GlobalNPC
    {
        private int attackTimer = 0;

        public override bool InstancePerEntity => true;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                attackTimer++;

                if (attackTimer >= 300) 
                {
                    attackTimer = 0;

                    Player target = Main.player[npc.target];
                    if (target.active && !target.dead)
                    {
                        float hpPercent = (float)npc.life / npc.lifeMax;

                        Vector2 direction = target.Center - npc.Center;
                        direction.Normalize();

                        float speed = MathHelper.Lerp(20f, 10f, hpPercent);

                        if (hpPercent <= 0.2f)
                        {
                            Vector2 dir1 = direction.RotatedBy(MathHelper.ToRadians(-10)) * speed;
                            Vector2 dir2 = direction.RotatedBy(MathHelper.ToRadians(10)) * speed;

                            Projectile.NewProjectile(
                                npc.GetSource_FromAI(),
                                npc.Center,
                                dir1,
                                ModContent.ProjectileType<StonedBlood>(),
                                35, 
                                3f
                            );

                            Projectile.NewProjectile(
                                npc.GetSource_FromAI(),
                                npc.Center,
                                dir2,
                                ModContent.ProjectileType<StonedBlood>(),
                                35,
                                3f
                            );

                            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit8, npc.Center);
                        }
                        else 
                        {
                            Vector2 velocity = direction * speed;

                            Projectile.NewProjectile(
                                npc.GetSource_FromAI(),
                                npc.Center,
                                velocity,
                                ModContent.ProjectileType<StonedBlood>(),
                                30,
                                2f
                            );

                            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit8, npc.Center);
                        }
                    }
                }
            }
        }
    }
}