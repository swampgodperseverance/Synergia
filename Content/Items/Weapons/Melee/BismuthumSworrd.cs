using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class BismuthumData : ModPlayer
    {
        public bool BismuthumIsUsed { get; internal set; }

        public override void PostUpdate()
        {
            if (Player.inventory[Player.selectedItem].type == ModContent.ItemType<BismuthumSword>() && Player.ItemAnimationJustStarted)
            {
                BismuthumIsUsed = !BismuthumIsUsed;
            }
        }
    }

    public class BismuthumSword : ModItem
    {
        public override void SetStaticDefaults()
            => Item.ResearchUnlockCount = 1;

        public override void SetDefaults()
        {
            int size = 30;
            Item.Size = new Vector2(size, size);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 16;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shootsEveryUse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 76;
            Item.knockBack = 5;

            Item.value = Item.buyPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;


            Item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.BismuthumRework>();
            Item.shootSpeed = 7f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float adjustedItemScale = player.GetAdjustedItemScale(Item);
            Projectile.NewProjectile(
                source,
                player.MountedCenter,
                new Vector2(player.direction, 0f),
                type,
                damage,
                knockback,
                player.whoAmI,
                player.direction * player.gravDir,
                player.itemAnimationMax * 2f,
                adjustedItemScale
            );

            Vector2 mouseWorld = Main.MouseWorld;


            Vector2 shootDirection = Vector2.UnitY * -1f;
            float shootSpeed = 20f;

            Projectile.NewProjectile(
                source,
                mouseWorld,
                shootDirection * shootSpeed,
                ModContent.ProjectileType<Content.Projectiles.Reworks.BismuthumSwordShoot>(),
                damage,
                knockback,
                player.whoAmI
            );
            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

            return false; 
        }


    }
}
