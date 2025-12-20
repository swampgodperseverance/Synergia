using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Melee {
    public class TizonaData : ModPlayer {
        public bool TizonaIsUsed { get; internal set; }

        public override void PostUpdate() {
            if (Player.inventory[Player.selectedItem].type == ModContent.ItemType<OrcishSword>() && Player.ItemAnimationJustStarted) {
                TizonaIsUsed = !TizonaIsUsed;
            }
        }
    }

    public class OrcishSword : ModItem {
        public override void SetStaticDefaults()
            => Item.ResearchUnlockCount = 1;

        public override void SetDefaults() {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 26;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shootsEveryUse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 18;
            Item.knockBack = 5;

            Item.value = Item.buyPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Content.Projectiles.Reworks.OrcishRework>();
            Item.shootSpeed = 7f;
        }
         public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            float adjustedItemScale = player.GetAdjustedItemScale(Item);
            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax * 2f, adjustedItemScale);
            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                
            
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
            => target.AddBuff(BuffID.ShadowFlame, 180);
    }
}