using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class SandAlbinoAuraProjectile : AuraAI
    {
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 0;

            // Цвет ауры — белый
            auraColor = new Color(255, 255, 255, 160);
            auraColor2 = new Color(220, 220, 220, 120);

            distanceMax = 120f; // уменьшенный радиус
            buffType = BuffID.Wrath;  
            orbCount = 6;
            orbSpeed = 1.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;

            spectreCut = false;
        }

        public override void PostAI()
        {
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.Distance(player.Center, Projectile.Center) < distanceMax)
                {
                    player.statDefense += 6;
                }
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Projectile.Center) < distanceMax)
                {
                    npc.AddBuff(BuffID.Slow, 30); 
                }
            }
        }
    }
}