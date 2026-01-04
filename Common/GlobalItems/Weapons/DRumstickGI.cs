using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.AltUse;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class GreatDrumstickGI : GlobalItem
    {
        public override bool InstancePerEntity => true;
        private int swingDirection;
        
        private class DrumstickPlayer : ModPlayer
        {
            public bool holdingDrumstick = false;
            
            public override void ResetEffects()
            {
                holdingDrumstick = false;
            }
            
            public override void UpdateEquips()
            {
                if (holdingDrumstick && !Player.dead)
                {
                  
                    Player.AddBuff(BuffID.WellFed, 2); 
                }
            }
        }

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "Consolaria", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "GreatDrumstick", StringComparison.OrdinalIgnoreCase);
        }

        public override void SetDefaults(Item item)
        {
            item.DamageType = DamageClass.Melee;
            item.knockBack = 7f;
            item.useTime = 46;
            item.useAnimation = 46;
            item.useStyle = ItemUseStyleID.Shoot;
            item.useTurn = false;
            item.autoReuse = true;
            item.ArmorPenetration = 5;
            item.noMelee = true;
            item.UseSound = null;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<GreatDrumstickRework>();
        }

        public override void HoldItem(Item item, Player player)
        {
            player.GetModPlayer<DrumstickPlayer>().holdingDrumstick = true;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = Vector2.Zero;
            swingDirection = swingDirection == 1 ? -1 : 1;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 handPos = player.MountedCenter;
            float handleOffset = 8f;
            Vector2 offset = new Vector2(handleOffset * player.direction, -2f * player.gravDir);

            Projectile.NewProjectile(
                source,
                handPos + offset,
                Vector2.Zero,
                type,
                damage,
                knockback,
                player.whoAmI,
                ai0: swingDirection,
                ai1: player.MountedCenter.AngleTo(Main.MouseWorld),
                ai2: 0
            );

            return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
}