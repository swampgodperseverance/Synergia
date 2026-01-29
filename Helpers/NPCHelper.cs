using System.Collections.Generic;
using Terraria;

namespace Synergia.Helpers {
    public static partial class NPCHelper {
        public static void AddBanner(this ModNPC npc, int item) {
            npc.Banner = npc.Type;
            npc.BannerItem = item;
        }

        /// <summary>
        /// Tries to find closest NPC and returns index of found NPC in Main.npc.
        /// <para>If NPC is not found, returns null.</para>
        /// </summary>
        public static int? FindClosestNPC(Vector2 position, int type = 0, float maxDistance = -1f, bool ignoreTiles = false, bool shouldBeChaseable = true, List<int> ignoreThoseNPCs = null)
        {
            int? foundTarget = null;
            float maxDistanceSquared = maxDistance * maxDistance;
            for(int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                float distanceSquared = Vector2.DistanceSquared(position, npc.Center);

                if ((npc.type != type && type != 0) || !IsValidTarget(npc) || (!npc.CanBeChasedBy() && shouldBeChaseable))
                    continue;
                if (distanceSquared > maxDistanceSquared && maxDistance != 0)
                    continue;
                if (!ignoreTiles && !Collision.CanHit(position, 0, 0, npc.Center, 0, 0))
                    continue;
                if(ignoreThoseNPCs != null && ignoreThoseNPCs.Contains(i))
                    continue;

                maxDistanceSquared = distanceSquared;
                foundTarget = i;
            }
            return foundTarget;
        }

        public static bool IsValidTarget(NPC npc)
        {
            return
                npc != null &&
                npc.active &&
                npc.life > 0;
        }
    }
}
