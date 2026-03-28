using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Avalon.Projectiles.Hostile.BacteriumPrime;
using Avalon.NPCs.Bosses.PreHardmode;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class BacteriumPrimeBuff
    {
        public static bool Enabled = false;

        internal const float SpeedMultiplier = 1.3f;  
        internal const float DamageMultiplier = 1.5f; 
        internal const float AttackRateMultiplier = 1.9f; 
        internal const float MaxSpeed = 15f; 
        internal const float MaxDistance = 700f; 
        internal const int SporeAttackCooldownMin = 240; 
        internal const int SporeAttackCooldownMax = 420; 
    }
}
