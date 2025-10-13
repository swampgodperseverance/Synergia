using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Aura;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.Weapons.AuraStaff
{
    [ExtendsFromMod("ValhallaMod")]
    public class UnderwaterAuraStaff : ValhallaMod.Items.AI.ValhallaAuraItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item82;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.mana = 20;
            Item.damage = 10;
            Item.shoot = ProjectileType<UnderwaterAuraProjectile>();
            Item.shootSpeed = 1f;
        }
    }
}