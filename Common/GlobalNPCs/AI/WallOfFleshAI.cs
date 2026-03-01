using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Synergia.Content.Projectiles.Hostile;
using System.IO;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class WallOfFleshGlobal : GlobalNPC
    {
        private int attackTimer = 0;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.WallofFlesh;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode > 0) binaryWriter.Write(attackTimer);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode > 0) attackTimer = binaryReader.ReadInt32();
        }
        public override void AI(NPC npc)
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
