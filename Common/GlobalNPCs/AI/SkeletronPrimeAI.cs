using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;

namespace Synergia.Content.NPCs
{
    public class SkeletronPrimeArmlessAttack : GlobalNPC
    {
        private int nextThreshold = 100;
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
            => npc.type == NPCID.SkeletronPrime;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode > 0) binaryWriter.Write(nextThreshold);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode > 0) nextThreshold = binaryReader.ReadInt32();
        }

        public override void OnSpawn(NPC npc, Terraria.DataStructures.IEntitySource source)
        {
            nextThreshold = 100;
        }

        public override void PostAI(NPC npc)
        {
            if (npc.type != NPCID.SkeletronPrime || npc.lifeMax <= 0 || npc.life <= 0)
                return;

            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && (
                    Main.npc[i].type == NPCID.PrimeCannon ||
                    Main.npc[i].type == NPCID.PrimeLaser ||
                    Main.npc[i].type == NPCID.PrimeSaw ||
                    Main.npc[i].type == NPCID.PrimeVice ||
                    Main.npc[i].ModNPC?.Name == "PrimeLauncher" ||
                    Main.npc[i].ModNPC?.Name == "PrimeMace" ||
                    Main.npc[i].ModNPC?.Name == "PrimeRail"))
                {
                    return;
                }
            }

            int hpPct = (int)System.Math.Ceiling(npc.life * 100f / npc.lifeMax);

            if(npc.ai[1] == 0f) if(--nextThreshold <= 0) {
                FireSkulls(npc);
                nextThreshold = hpPct * 3;
            }
        }

        private void FireSkulls(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                target = Main.player[Player.FindClosest(npc.Center, 1, 1)];

            Vector2 baseDir = npc.DirectionTo(target.Center);
            if (baseDir == Vector2.Zero) baseDir = new Vector2(0f, -1f);

            float speed = 10f;
            int damage = 30;
            int knockback = 2;
            int type = ModContent.ProjectileType<Content.Projectiles.Hostile.PrimeSkull>();

            Projectile.NewProjectile(
                npc.GetSource_FromAI(),
                npc.Center,
                baseDir * speed,
                type,
                damage,
                knockback
            );
            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.NPCHit8, npc.Center);
        }
    }
}
