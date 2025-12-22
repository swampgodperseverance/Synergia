using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class ThunderBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 72;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 46;

            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;

            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            Vector2 thunderVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(6f)) * 1.1f;

            Projectile.NewProjectile(
                source,
                position,
                thunderVelocity,
                ModContent.ProjectileType<ThunderSpike>(),
                (int)(damage * 0.7f),
                knockback,
                player.whoAmI
            );

            return false; 
        }
    }
}
