using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class UnderwaterAura : AuraAI
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;

            distanceMax = 240f; 

     
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Gills);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Flipper);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Calm);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Sonar);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Fishing);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.ObsidianSkin);

          
            buffTypes[AuraEffectTarget.Enemy].Add(BuffID.Wet);
            buffTypes[AuraEffectTarget.Enemy].Add(BuffID.Slow);
            buffTypes[AuraEffectTarget.Enemy].Add(BuffID.Weak);
            buffTypes[AuraEffectTarget.Enemy].Add(ModContent.BuffType<Buffs.DeepPressureDebuff>());

            shootSpawnStyle = AuraShootStyles.None;

            orbCount = 10;
            orbSpeed = 0.25f;
            orbTrailCount = 12;
            orbTrailGap = 0.01f;

            auraColor = new Color(0, 80, 180, 120);  
            auraColor2 = new Color(0, 180, 220, 80);  

            spectreCut = true;
            spectreCutDust = DustID.Water;
            spectreCutDustScale = 2.5f;
            spectreCutCooldown = 6;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void AI()
        {
            base.AI();

            float radius = distanceMax;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                if (!Projectile.WithinRange(npc.Center, radius))
                    continue;

                // Если на NPC висит кастомный дебафф — эффект пузырьков
                if (npc.HasBuff(ModContent.BuffType<Buffs.DeepPressureDebuff>()))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDustPerfect(
                            Projectile.Center + Main.rand.NextVector2Circular(radius, radius),
                            DustID.Water,
                            Main.rand.NextVector2Circular(0.5f, 0.5f),
                            100,
                            default,
                            1.1f
                        ).noGravity = true;
                    }
                }
            }
        }
    }
}

