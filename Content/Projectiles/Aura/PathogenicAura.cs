using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;
using Avalon.Buffs.Debuffs;
using Avalon.Buffs.AdvancedBuffs;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class PathogenicAura : AuraAI
    {
        private static Mod avalon = ModLoader.GetMod("Avalon");

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;

            shootCooldown = 120;
            shootType = ProjectileType<PathogenicAuraSpawn>();
            shootCount = 6;
            shootSpeed = 7.5f;
            shootDirectionStyle = AuraShootDirection.SmartRandom;
            shootSpawnStyle = AuraShootStyles.Periodically;

            if (avalon != null)
            {
                buffTypes[AuraEffectTarget.Team].Add(avalon.Find<ModBuff>("BacterialEndurance")?.Type ?? 0);
                // Add debuffs to the appropriate buffTypes dictionary entries
                buffTypes[AuraEffectTarget.Enemy].Add(avalon.Find<ModBuff>("Lacerated")?.Type ?? 0);
            }

            distanceMax = 320f;
            orbSpeed = 1f;
            orbCount = 8;
            orbTrailCount = 10;
            orbTrailGap = 0.03f;

            auraColor = new Color(171, 208, 60);
        }

        public override void ApplyEffectPlayer(Player target)
        {
            // Apply team buffs
            if (CheckPlayer(Projectile, target, AuraEffectTarget.Team))
            {
                foreach (var buff in buffTypes[AuraEffectTarget.Team])
                {
                    if (buff != 0)
                        target.AddBuff(buff, BuffTime);
                }
            }
        }

        public override void ApplyEffectNPC(NPC target)
        {
            // Apply enemy debuffs
            if (CheckNPC(Projectile, target, AuraEffectTarget.Enemy))
            {
                foreach (var debuff in buffTypes[AuraEffectTarget.Enemy])
                {
                    if (debuff != 0)
                        target.AddBuff(debuff, BuffTime);
                }
            }
        }
    }
}