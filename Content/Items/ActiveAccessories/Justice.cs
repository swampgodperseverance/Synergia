using System;
using Avalon.Items.Material.Shards;
using Microsoft.Xna.Framework;
using Synergia.Common;
using Synergia.Content.Projectiles.ActiveAccessoriesProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.DamageClasses;
using ValhallaMod.Items.AI;
using ValhallaMod.Items.Material.Bar;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.ActiveAccessories
{
    public class Justice : ValhallaMod.Items.AI.ActiveAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            ValhallaMod.Values.ActiveAccessoryItems[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 3, 25, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.DamageType = GetInstance<AccessoryDamageClass>();
            Item.damage = 0;

            cooldown = 30 * 60;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SacredShard>(), 5)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool Use(Player player, ref int time, ref int damage, ref bool silent)
        {
            player.immune = true;
            player.immuneTime = 90;

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height,
                    DustID.GoldFlame, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, -0.5f));
            }

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height,
                    DustID.HallowedWeapons, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-3f, 0f));
            }


            SoundEngine.PlaySound(SoundID.Item29, player.Center);

            return true;
        }

        public override void SafeUpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.05f;
        }
    }
}