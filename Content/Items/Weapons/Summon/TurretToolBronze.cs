using System.Collections.Generic;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Ores;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Items.Garden;
using ValhallaMod.Items.Material;
using Synergia.Content.Projectiles.Summon;
using Synergia.Content.Items.Materials;


namespace Synergia.Content.Items.Weapons.Summon {
    public class TurretToolBronze : ModItem
    {
        // Token: 0x06000C35 RID: 3125 RVA: 0x0007B630 File Offset: 0x00079830
        public override void SetDefaults()
        {
            base.Item.damage = 13;
            base.Item.DamageType = DamageClass.Summon;
            base.Item.mana = 20;
            base.Item.width = 26;
            base.Item.height = 28;
            base.Item.autoReuse = true;
            base.Item.useTime = 40;
            base.Item.useAnimation = 40;
            base.Item.useStyle = 1;
            base.Item.noMelee = true;
            base.Item.knockBack = 3f;
            base.Item.value = Item.sellPrice(0, 0, 7, 0);
            base.Item.rare = 0;
            base.Item.UseSound = new SoundStyle?(SoundID.Item78);
            base.Item.shoot = ModContent.ProjectileType<TurretBronze>();
            base.Item.shootSpeed = 1f;
            base.Item.sentry = true;
        }

        // Token: 0x06000C36 RID: 3126 RVA: 0x00003673 File Offset: 0x00001873
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        // Token: 0x06000C37 RID: 3127 RVA: 0x0007B730 File Offset: 0x00079930
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Vector2 vector = ValhallaHelper.GetMousePositionInWorld(player, false) / 16f;
                int X = (int)vector.X;
                int Y = (int)vector.Y;
                while (Y < Main.maxTilesY - 10 && Main.tile[X, Y] != null && !WorldGen.SolidTile2(X, Y) && Main.tile[X - 1, Y] != null && !WorldGen.SolidTile2(X - 1, Y) && Main.tile[X + 1, Y] != null && !WorldGen.SolidTile2(X + 1, Y))
                {
                    Y++;
                }
                Y--;
                Projectile.NewProjectile(source, (float)Main.mouseX + Main.screenPosition.X, (float)(Y * 16 - 24), 0f, 15f, type, damage, knockback, player.whoAmI, 0f, 0f, 0f);
                player.UpdateMaxTurrets();
            }
            return false;
        }

        // Token: 0x06000C38 RID: 3128 RVA: 0x0007B829 File Offset: 0x00079A29
        public override void AddRecipes()
        {
            base.CreateRecipe(1).AddIngredient<CogBronze>(1).AddRecipeGroup(RecipeGroupID.Wood, 8).AddTile(16).Register();
        }
    }
}
