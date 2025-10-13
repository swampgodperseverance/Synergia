using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.NPCs
{
    public class SkeletronPrimeArmlessAttack : GlobalNPC
    {
        private int nextThreshold = 95;
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
            => npc.type == NPCID.SkeletronPrime;

        public override void OnSpawn(NPC npc, Terraria.DataStructures.IEntitySource source)
        {
            if (npc.type == NPCID.SkeletronPrime)
                nextThreshold = 95;
        }

        public override void PostAI(NPC npc)
        {
            if (npc.type != NPCID.SkeletronPrime || npc.lifeMax <= 0 || npc.life <= 0)
                return;

            bool hasHands = false;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && (
                    Main.npc[i].type == NPCID.PrimeCannon ||
                    Main.npc[i].type == NPCID.PrimeLaser ||
                    Main.npc[i].type == NPCID.PrimeSaw ||
                    Main.npc[i].type == NPCID.PrimeVice))
                {
                    hasHands = true;
                    break;
                }
            }

            if (hasHands)
                return; 

            int hpPct = (int)System.Math.Ceiling(npc.life * 100f / npc.lifeMax);

            while (nextThreshold >= 0 && hpPct <= nextThreshold)
            {
                FireSkulls(npc);
                nextThreshold -= 5;
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

            for (int i = -1; i <= 1; i++)
            {
                Vector2 vel = baseDir.RotatedBy(MathHelper.ToRadians(10 * i)) * speed;
                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    vel,
                    type,
                    damage,
                    knockback
                );
            }

            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.NPCHit8, npc.Center);
        }
    }
}
