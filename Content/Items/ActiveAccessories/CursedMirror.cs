using ValhallaMod.Items.AI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using ValhallaMod.DamageClasses;
using Synergia.Content.Projectiles.ActiveAccessoriesProjectiles;
using static Terraria.ModLoader.ModContent;
using Synergia.Common; // Add this using directive

namespace Synergia.Content.Items.ActiveAccessories
{
    //[ExtendsFromMod("ValhallaMod")]
    public class CursedMirror : ValhallaMod.Items.AI.ActiveAccessoryItem
    {
        // Set this to true so item can be equiped
        public override void SetStaticDefaults()
        {
            ValhallaMod.Values.ActiveAccessoryItems[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.DamageType = GetInstance<AccessoryDamageClass>();
            Item.damage = 35;

            cooldown = 15 * 60; // Set Cooldown 60 is 1 sec
        }

        // Function that is triggered when Active Accessory is Used
        public override bool Use(Player player, ref int time, ref int damage, ref bool silent)
        {
            // Получаем позицию курсора в мире
                        Vector2 cursorPos = Main.MouseWorld + new Vector2(0, 200);


            // Количество снарядов в ряду
            int projectilesInRow = 3;
            float spacing = 30f; // расстояние между снарядами по горизонтали

            // Store damage in a local variable to avoid ref parameter issues
            int currentDamage = damage;

            // Spawn the first row immediately
            SpawnRow(cursorPos, projectilesInRow, spacing, player, currentDamage);

            // Second row after 1.5 seconds (90 ticks) using the new TimerSystem
            int delay = 90;
            TimerSystem.DelayAction(delay, () => 
            {
                SpawnRow(cursorPos, projectilesInRow, spacing, player, currentDamage);
            });

            return true;
        }

        // Separate method to spawn projectiles in a row
        private void SpawnRow(Vector2 cursorPos, int projectilesInRow, float spacing, Player player, int damage)
        {
            for (int i = 0; i < projectilesInRow; i++)
            {
                // Сместим снаряд в ряд относительно курсора
                Vector2 spawnPos = cursorPos + new Vector2((i - 1) * spacing, 0f);
        
                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    spawnPos,
                    Vector2.Zero, // без начальной скорости
                    ModContent.ProjectileType<NecroKnifeFriendly>(),
                    damage,
                    0f,
                    player.whoAmI
                );
            }
        }

        // Passive accessory buffs
        public override void SafeUpdateAccessory(Player player, bool hideVisual)
        {
            // Add any passive effects here
        }
    }
}