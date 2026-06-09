using Avalon.Items.Accessories.Hardmode;
using Avalon.Items.Material;
using Avalon.Items.Material.Bars;
using Synergia.Content.Buffs;
using Synergia.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material.Bar;

namespace Synergia.Content.Items.Accessories
{[AutoloadEquip(EquipType.Shield)]
    public class CorrodeCrossShield : ModItem
    {
        public override void SetStaticDefaults()
        {

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DurataniumOmegaShield>(), 1)
                .AddIngredient(ModContent.ItemType<CorrodeBar>(), 10)
                .AddIngredient(ModContent.ItemType<LifeDew>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CobaltOmegaShield>(), 1)
    .AddIngredient(ModContent.ItemType<CorrodeBar>(), 10)
    .AddIngredient(ModContent.ItemType<LifeDew>(), 1)
    .AddTile(TileID.MythrilAnvil)
    .Register();
            CreateRecipe()
    .AddIngredient(ModContent.ItemType<PalladiumOmegaShield>(), 1)
    .AddIngredient(ModContent.ItemType<CorrodeBar>(), 10)
    .AddIngredient(ModContent.ItemType<LifeDew>(), 1)
    .AddTile(TileID.MythrilAnvil)
    .Register();
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 12, 50, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CorrodeCrossShieldPlayer>().hasShield = true;
        }
    }

    public class CorrodeCrossShieldPlayer : ModPlayer
    {
        public bool hasShield;
        private int buffTimer;

        public override void ResetEffects()
        {
            hasShield = false;
        }

        public override void UpdateDead()
        {
            buffTimer = 0;
        }

        public override void PostUpdate()
        {
            if (hasShield)
            {
                Player.GetDamage(DamageClass.Throwing) += 0.12f;

                if (buffTimer > 0)
                {
                    buffTimer--;
                    Player.AddBuff(ModContent.BuffType<CorrodeCrossShieldBuff>(), 30);
                }
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (hasShield && info.Damage > 0)
            {
                buffTimer = 60 * 10; 
            }
        }
    }
}
