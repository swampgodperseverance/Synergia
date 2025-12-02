using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.RangedProjectiles;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class Tribow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 38;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.reuseDelay = 0;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 7.5f;
            Item.useAmmo = AmmoID.Arrow;
            Item.autoReuse = false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.itemAnimation == player.itemAnimationMax;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2( -3f, 0f ); 
        }
     public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ModContent.ProjectileType<TriArrow>();

            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            float spread = 4f;
            float rad = MathHelper.ToRadians(spread);
            Vector2 v1 = velocity.RotatedBy(-rad);
            Vector2 v2 = velocity;
            Vector2 v3 = velocity.RotatedBy(+rad);

            Projectile.NewProjectile(source, position, v1, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, v2, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, v3, type, damage, knockback, player.whoAmI);


            SoundEngine.PlaySound(SoundID.Item5 with { Pitch = 0.1f }, position);

            return false;
        }

    }
}