using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Summon;

namespace Synergia.Content.Items.Weapons.Summon
{
    public class Triwhip : ModItem
    {
        public override void SetDefaults()
        {
       
            Item.damage = 21;
            Item.knockBack = 2f;
            Item.useTime = 30;        
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;        
            Item.shoot = ModContent.ProjectileType<TriwhipProjTriple>();
            Item.shootSpeed = 4f;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 40, 0);

           
            Item.UseSound = SoundID.Item116; 
        }

        public override bool MeleePrefix() => true;

       
        public class TriwhipPlayer : ModPlayer
        {
            public byte TriwhipCounter = 0;
            public override void PostUpdateRunSpeeds()
            {
            
                if (Player.itemAnimation == 0)
                    TriwhipCounter = 0;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<TriwhipPlayer>();

            bool tripleSwing = modPlayer.TriwhipCounter % 2 == 1;

            if (tripleSwing)
            {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TriwhipProjTriple>(), damage, knockback, player.whoAmI);
            Vector2 left = velocity.RotatedBy(MathHelper.ToRadians(-15));
            Vector2 right = velocity.RotatedBy(MathHelper.ToRadians(15));
            Projectile.NewProjectile(source, position, left, ModContent.ProjectileType<TriwhipProjTriple>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, right, ModContent.ProjectileType<TriwhipProjTriple>(), damage, knockback, player.whoAmI);

            }
            else
            {
             
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            }

            modPlayer.TriwhipCounter++;
            return false;
        }
    }
}
