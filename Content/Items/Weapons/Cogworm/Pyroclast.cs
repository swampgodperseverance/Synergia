using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles.Friendly;

namespace Vanilla.Content.Items.Weapons.Cogworm
{
    public class Pyroclast : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Firestorm Bow");
            // Tooltip.SetDefault("Shoots three fireballs at once\nSets the wielder on fire");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 56;
            Item.damage = 110;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<FireballProjectile>();
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.None;
            
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = false;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f); 
        }

        public override void HoldItem(Player player)
        {
            player.AddBuff(BuffID.OnFire, 2);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<FireballProjectile>();
            velocity *= 1.2f;
            
            position += new Vector2(-2f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spread = 0.3f;
            
            for (int i = 0; i < 3; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(spread);
                newVelocity *= 1f - Main.rand.NextFloat(0.1f);
                
                Projectile.NewProjectile(
                    source, 
                    position, 
                    newVelocity, 
                    type, 
                    damage, 
                    knockback, 
                    player.whoAmI);
            }
            
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(
                    position, 
                    DustID.Torch, 
                    velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.5f, 1.5f),
                    100, 
                    default, 
                    1.5f);
            }
            
            return false;
        }
    }
}