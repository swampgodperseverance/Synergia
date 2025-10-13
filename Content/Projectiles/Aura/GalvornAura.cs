using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ValhallaMod.Projectiles.AI;
using System;

namespace Synergia.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class GalvornAura : AuraAI
    {
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 0;

            auraColor = new Color(20, 50, 20, 180);   
            auraColor2 = new Color(0, 30, 0, 120);     

            distanceMax = 250f;
            orbCount = 4;
            orbSpeed = 1.8f;
            orbTrailCount = 10;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;
            spectreCut = false;

            buffTypes[AuraEffectTarget.Team].Add(BuffID.Wrath);
            buffTypes[AuraEffectTarget.Team].Add(BuffID.Mining); 
        }

        public override void AI()
        {
            base.AI();

            float pulse = (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 20f;
            auraColor = new Color(20, (int)(50 + pulse), 20, (int)(160 + pulse * 0.5f));
        }

        public override void ApplyEffectPlayer(Player target)
        {
            target.GetDamage(DamageClass.Generic) += 0.12f;

            target.pickSpeed -= 0.25f; 

            target.statDefense += 6;

            target.moveSpeed += 0.1f;

        }

        public override void ApplyEffectNPC(NPC target)
        {

            target.AddBuff(BuffID.Poisoned, 120);

        }
    }
}
