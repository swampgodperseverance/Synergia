using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vanilla.Content.Projectiles.Aura;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Items.Weapons.AuraStaff
{
    [ExtendsFromMod("ValhallaMod")]
    public class SandAlbinoAuraStaff : ValhallaMod.Items.AI.ValhallaAuraItem
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
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.mana = 20;
            Item.damage = 0;
            Item.shoot = ProjectileType<SandAlbinoAuraProjectile>();
            Item.shootSpeed = 1f;
        }
    }
}
