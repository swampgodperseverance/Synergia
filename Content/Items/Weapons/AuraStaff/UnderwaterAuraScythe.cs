using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Aura;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.Weapons.AuraStaff
{
    [ExtendsFromMod("ValhallaMod")]
    public class UnderwaterAuraScythe : ValhallaMod.Items.AI.ValhallaAuraItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            ValhallaMod.Values.AuraScythe[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Summon;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item85;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.mana = 20;
            Item.damage = 30;
            Item.shoot = ProjectileType<UnderwaterAura>();
            Item.shootSpeed = 1f;

            typeScythe = ProjectileType<UnderwaterAuraScytheCut>();
            scytheDamageScale = 1.1f;
        }
    }
}
