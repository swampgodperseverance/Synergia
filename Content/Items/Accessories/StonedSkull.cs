using Avalon.Items.Material.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material.Bar;

namespace Synergia.Content.Items.Accessories
{
    public class StonedSkull : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 20)
                .AddIngredient(ModContent.ItemType<Peridot>(), 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Throwing) += 0.05f;
            player.GetModPlayer<StonedSkullPlayer>().hasStonedSkull = true;
        }
    }

    public class StonedSkullPlayer : ModPlayer
    {
        public bool hasStonedSkull;
        private int throwSpeedBuffTimer = 0;

        public override void ResetEffects()
        {
            hasStonedSkull = false;
        }

        public override void OnRespawn()
        {
            if (hasStonedSkull)
            {
                throwSpeedBuffTimer = 60 * 60; 
            }
        }

        public override void PostUpdate()
        {
            if (throwSpeedBuffTimer > 0)
            {
                Player.GetAttackSpeed(DamageClass.Throwing) += 0.10f;
                throwSpeedBuffTimer--;
            }
        }
    }
}
