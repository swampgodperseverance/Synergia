using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Mage
{
    public class OrcishSwungo : ModItem
    {
        private int noteCooldown = 0;

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 16;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.mana = 24;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 40;
            Item.shoot = ModContent.ProjectileType<OrcishSwungoP>();
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
        }
           public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Vector2 offset = Vector2.Normalize(velocity) * 30f; 
            
            if (Collision.CanHit(position, 0, 0, position + offset, 0, 0))
            {
                position += offset;
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false; 
        }
    }
}
