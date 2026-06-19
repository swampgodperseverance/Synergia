using System;
using Bismuth.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Materials;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Accessories
{
    public class MortalStones : ModItem
    {
        public const int ArmorPenetration = 10;

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.GetDamage(DamageClass.Melee) += 0.10f;
            player.GetArmorPenetration(DamageClass.Generic) += ArmorPenetration;
        }
    }
}