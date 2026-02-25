using Synergia.Common.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;
using Synergia.Content.Projectiles.Tools;
using Synergia.Content.Items.Materials;

namespace Synergia.Content.Items.Tools
{
    public class BronzeDrill : ModItem
    {
        // Token: 0x0600124B RID: 4683 RVA: 0x00099043 File Offset: 0x00097243
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsDrill[base.Type] = true;
        }

        // Token: 0x0600124C RID: 4684 RVA: 0x0009AE0C File Offset: 0x0009900C
        public override void SetDefaults()
        {
            base.Item.damage = 4;
            base.Item.DamageType = DamageClass.Melee;
            base.Item.width = 20;
            base.Item.height = 12;
            base.Item.useAnimation = 21;
            base.Item.useTime = 7;
            base.Item.channel = true;
            base.Item.noUseGraphic = true;
            base.Item.noMelee = true;
            base.Item.pick = 35;
            base.Item.tileBoost = -1;
            base.Item.useStyle = 5;
            base.Item.knockBack = 1f;
            base.Item.value = Item.sellPrice(0, 0, 35, 0);
            base.Item.rare = 0;
            base.Item.UseSound = new SoundStyle?(SoundID.Item23);
            base.Item.autoReuse = true;
            base.Item.shoot = ModContent.ProjectileType<BronzeDrillProj>();
            base.Item.shootSpeed = 24f;
            base.Item.ArmorPenetration = 5;
        }

        // Token: 0x0600124D RID: 4685 RVA: 0x0009AF2F File Offset: 0x0009912F
        public override void AddRecipes()
        {
            base.CreateRecipe(1).AddIngredient<CogBronze>(4).AddIngredient(85, 6).AddTile(16).Register();
        }
    }
}
