using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader; using Synergia.Common;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Content.Buffs; 

namespace Synergia.Content.Items.Weapons.Cogworm
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
            Item.damage = 22;
            Item.width = 50;
            Item.height = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 1, 1, 29);
            Item.rare = ModContent.RarityType<LavaGradientRarity>();

            Item.shootSpeed = 17;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.useAnimation = 20;
            Item.useTime = 4; // one third of useAnimation
            Item.reuseDelay = 70;
            Item.consumeAmmoOnLastShotOnly = true;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0f);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Заменяем деревянные стрелы на кастомные
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<PyroclastShoot>();
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numProjectiles = Main.rand.Next(1, 6);
            for (int p = 0; p < numProjectiles; p++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);
                
                // Создаем снаряд с правильным типом
                Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            return false;
        }
    }
}