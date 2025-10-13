using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Synergia.Content.Items.Accessories
{
    public class CaesiumRing : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Throwing) += 0.15f;

            var modPlayer = player.GetModPlayer<CaesiumRingPlayer>();
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
        }
    }

    public class CaesiumRingPlayer : ModPlayer
    {
        public int timeSinceLastGas = 0;

        public override void ResetEffects()
        {
        }

        public override void PostUpdate()
        {
            timeSinceLastGas++; 
        }
    }

    public class CaesiumGas : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 42;
            Projectile.scale = 1.15f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180; 
            Projectile.alpha = 150;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.tileCollide = false;
        }

        public float Timer;

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.X + Projectile.velocity.Y) * 0.1f * Projectile.direction;
            Projectile.velocity *= 0.980f;
            
            if (!Main.rand.NextBool(3))
            {
                Projectile.alpha++;
            }
            
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
                return;
            }
            
            Timer++;
            if (Timer <= 40)
            {
                Projectile.scale *= 1.007f;
            }
            else if (Timer >= 40)
            {
                Projectile.scale *= 0.993f;
            }
            
            if (Timer >= 80)
            {
                Timer = 0;
                Projectile.scale = 1.15f;
            }

        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);

            if (Main.myPlayer == Projectile.owner)
            {
                int secondaryProj = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -1f)),
                    ModContent.ProjectileType<CaesiumSecondaryGas>(), 
                    (int)(Projectile.damage * 0.5f), 
                    Projectile.knockBack * 0.5f,
                    Projectile.owner
                );
            }
        }
    }
}