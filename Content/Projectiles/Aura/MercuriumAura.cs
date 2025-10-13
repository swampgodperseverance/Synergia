using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Synergia.Content.Dusts;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class MercuriumAura : AuraAI
    { 
        public override void SetDefaults()
        {
            base.SetDefaults(); // Don't forget to call base.SetDefaults()

            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;

            distanceMax = 160f;

            // Buffs for allies (Team)
            buffTypes[AuraEffectTarget.Team].Add(BuffID.ManaRegeneration);

            // Debuffs for enemies
            buffTypes[AuraEffectTarget.Enemy].Add(BuffID.Electrified);
            // If you want to add another debuff, just add another line:
            // buffTypes[AuraEffectTarget.Enemy].Add(ModContent.BuffType<YourCustomDebuff>());

            // Disable shooting
            shootSpawnStyle = AuraShootStyles.None;
            shootType = 0;
            shootCount = 0;
            shootSpeed = 0f;
            shootCooldown = 0;
            shootDirectionStyle = AuraShootDirection.Random;
            shootStep = 0f;

            // Visual settings
            orbCount = 12;
            orbSpeed = 0.2f;
            orbTrailCount = 10;
            orbTrailGap = 0.01f;

            auraColor = new Color(20, 40, 255, 150); // dark blue
            auraColor2 = new Color(0, 0, 100, 150);

            spectreCut = true;
            spectreCutDust = ModContent.DustType<MercuriumSparkDust>();
            spectreCutDustScale = 4.0f;
            spectreCutCooldown = 20;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        // Optional: Override ApplyEffectNPC if you need custom behavior for NPC debuffs
        public override void ApplyEffectNPC(NPC target)
        {
            base.ApplyEffectNPC(target); // This will apply the buffs/debuffs from buffTypes automatically
            
            // Add any additional custom NPC effects here
        }

        // Optional: Override ApplyEffectPlayer if you need custom behavior for player buffs
        public override void ApplyEffectPlayer(Player target)
        {
            base.ApplyEffectPlayer(target); // This will apply the buffs from buffTypes automatically
            
            // Add any additional custom player effects here
        }
    }
}