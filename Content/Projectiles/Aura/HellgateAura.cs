using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using ValhallaMod.Projectiles.AI;
using Synergia.Common.ModSystems;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Projectiles.Aura
{
    [ExtendsFromMod("ValhallaMod")]
    public class HellgateAura : AuraAI
    {
        private static int mode = 0; // 0 - Support, 1 - Attack
        private static int modeSwitchCooldown = 0;
        private int attackCooldown = 0;

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.damage = 10;

            // Initial aura colors
            auraColor = new Color(255, 100, 0, 160);
            auraColor2 = new Color(200, 80, 0, 120);

            distanceMax = 200f;

            orbCount = 12;
            orbSpeed = 1.1f;
            orbTrailCount = 6;
            orbTrailGap = 0.02f;

            shootSpawnStyle = AuraShootStyles.None;

            spectreCut = true;
            spectreCutDust = DustID.Lava;
            spectreCutDustScale = 4.0f;
            spectreCutCooldown = 20;
        }
            public override void AI()
            {
                if (modeSwitchCooldown <= 0 && VanillaKeybinds.ToggleAuraModeKeybind.JustPressed)
                {
                    mode = (mode + 1) % 2;
                    modeSwitchCooldown = 15;
                    CombatText.NewText(Projectile.Hitbox, Color.OrangeRed, $"Mode {(mode == 0 ? "Support" : "Attack")}", true);
                }
                else if (modeSwitchCooldown > 0)
                {
                    modeSwitchCooldown--;
                }
                auraColor = mode == 0
                    ? new Color(255, 80, 10, 180)    
                    : new Color(150, 20, 20, 180);   
                auraColor2 = mode == 0
                    ? new Color(180, 40, 10, 120)
                    : new Color(255, 120, 120, 160);

                if (mode == 1)
                {
                    int ashenBuffID = BuffType<Buffs.Ashen>();

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                        {
                            float distanceToNPC = Vector2.Distance(npc.Center, Projectile.Center);
                            if (distanceToNPC <= distanceMax) 
                            {
                                npc.AddBuff(ashenBuffID, 20); 
                            }
                        }
                    }
                }

                base.AI();
            }

        public override void ApplyEffectPlayer(Player target)
        {
            if (mode == 0)
            {
                int hellbornBuffID = BuffType<Buffs.Hellborn>();
                target.AddBuff(hellbornBuffID, 10);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (mode == 1)
            {
                int ashenBuffID = BuffType<Buffs.Ashen>();
                target.AddBuff(ashenBuffID, 10);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            for (int i = 0; i < 2; i++) 
            {
                int dust = Dust.NewDust(Projectile.position, 8, 8, DustID.Lava);
                Main.dust[dust].scale = 0.8f;
                Main.dust[dust].velocity *= 0.4f;
                Main.dust[dust].noGravity = true;
            }
        }
    }
}