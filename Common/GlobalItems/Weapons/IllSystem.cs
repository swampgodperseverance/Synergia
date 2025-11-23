using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Global
{
    public class BatbowShotScheduler : ModSystem
    {
        private static readonly List<ScheduledShot> scheduledShots = new();

        public override void PostUpdatePlayers()
        {
            for (int i = scheduledShots.Count - 1; i >= 0; i--)
            {
                var shot = scheduledShots[i];
                shot.Timer--;

                if (shot.Timer <= 0)
                {
                    if (shot.Player.active && !shot.Player.dead)
                    {
                        Projectile.NewProjectile(
                            shot.Source,
                            shot.Position,
                            shot.Velocity,
                            shot.Type,
                            shot.Damage,
                            shot.Knockback,
                            shot.Player.whoAmI
                        );
                    }

                    scheduledShots.RemoveAt(i);
                }
            }
        }

        public static void AddDelayedShot(
            Player player,
            float delayTicks,
            IEntitySource source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            scheduledShots.Add(new ScheduledShot
            {
                Player = player,
                Timer = (int)delayTicks,
                Source = source,
                Position = position,
                Velocity = velocity,
                Type = type,
                Damage = damage,
                Knockback = knockback
            });
        }

        private class ScheduledShot
        {
            public Player Player;
            public int Timer;
            public IEntitySource Source;
            public Vector2 Position;
            public Vector2 Velocity;
            public int Type;
            public int Damage;
            public float Knockback;
        }
    }
}
