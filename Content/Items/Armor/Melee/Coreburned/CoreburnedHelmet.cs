using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material.Bar;

namespace Synergia.Content.Items.Armor.Melee.Coreburned;

[AutoloadEquip(EquipType.Head)]
public class CoreburnedHelmet : ModItem
{
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetDefaults()
    {
        int width = 26; int height = 20;
        Item.Size = new Vector2(width, height);
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 16;
        Item.value = Item.sellPrice(0, 4, 8, 50);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Melee) += 0.15f; 
        player.endurance += 0.10f; 
        player.buffImmune[BuffID.OnFire] = true;
        player.buffImmune[BuffID.Burning] = true;
        player.buffImmune[BuffID.CursedInferno] = true;
        player.buffImmune[BuffID.Frostburn] = true;
        player.buffImmune[BuffID.ShadowFlame] = true;
        player.fireWalk = true; 
    }
}