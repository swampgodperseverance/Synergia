using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Aura;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.Weapons.AuraStaff
{
	[ExtendsFromMod("ValhallaMod")]
	public class PathogenicAuraStaff : ValhallaMod.Items.AI.ValhallaAuraItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			var item = Item;

			item.DamageType = DamageClass.Summon;
			item.width = 38;
			item.height = 38;
			item.useTime = 30;
			item.useAnimation = 30;
			item.UseSound = SoundID.Item82;
			item.noMelee = true;
			item.useTurn = true;
			item.useStyle = ItemUseStyleID.Shoot;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.mana = 60;
			item.damage = 40;
			item.shoot = ProjectileType<PathogenicAura>();
			item.shootSpeed = 1f;

			//typeScythe = ProjectileType<SuperAuraScytheCut>();
			//scytheDamageScale = 1f;
		}
	}
}