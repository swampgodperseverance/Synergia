using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Items.Accessories
{
    public class CaesiumSkull : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Throwing) += 0.23f; 
            player.GetCritChance(DamageClass.Throwing) += 10f;
            player.GetAttackSpeed(DamageClass.Throwing) += 0.05f; 

            var modPlayer = player.GetModPlayer<CaesiumSkullPlayer>();
            modPlayer.hasCaesiumSkull = true;
            modPlayer.timeSinceLastGas++;

            if ((player.velocity.X != 0 || player.velocity.Y != 0) && modPlayer.timeSinceLastGas >= 30)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    int proj = Projectile.NewProjectile(
                        player.GetSource_Accessory(Item),
                        player.Center,
                        new Vector2(0, -1f),
                        ModContent.ProjectileType<CaesiumGas>(),
                        (int)(10f * player.GetDamage(DamageClass.Throwing).ApplyTo(10f)),
                        2f,
                        player.whoAmI
                    );
                    Main.projectile[proj].friendly = true;
                }
                modPlayer.timeSinceLastGas = 0;
            }


            if (modPlayer.throwSpeedBuffTimer > 0)
            {
                player.GetAttackSpeed(DamageClass.Throwing) += 0.20f; 
                modPlayer.throwSpeedBuffTimer--;
            }
        }
    }

    public class CaesiumSkullPlayer : ModPlayer
    {
        public bool hasCaesiumSkull = false;
        public int timeSinceLastGas = 0;
        public int throwSpeedBuffTimer = 0;

        public override void ResetEffects()
        {
            hasCaesiumSkull = false;
        }

        public override void PostUpdate()
        {
            if (hasCaesiumSkull)
            {
                timeSinceLastGas++;
            }
        }

        public override void OnRespawn()
        {
            if (hasCaesiumSkull)
            {
                throwSpeedBuffTimer = 60 * 60; 
            }
        }
    }

    public class CaesiumSecondaryGas : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 35;
            Projectile.height = 34;
            Projectile.scale = 0.8f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120; 
            Projectile.alpha = 150;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public float Timer;

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.X + Projectile.velocity.Y) * 0.1f * Projectile.direction;
            Projectile.velocity *= 0.980f;
            
            if (!Main.rand.NextBool(3))
            {
                Projectile.alpha += 2; 
            }
            
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
                return;
            }
            
            Timer++;
            if (Timer <= 30)
            {
                Projectile.scale *= 1.005f;
            }
            else if (Timer >= 30)
            {
                Projectile.scale *= 0.995f;
            }
            
            if (Timer >= 60)
            {
                Timer = 0;
                Projectile.scale = 0.8f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 120);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}