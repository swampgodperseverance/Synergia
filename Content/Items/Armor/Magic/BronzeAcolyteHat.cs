using Microsoft.Xna.Framework;
using Terraria;
using RoA.Content.Items.Equipables.Armor.Magic;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Armor.Magic
{
        [AutoloadEquip(EquipType.Head)]
        public class BronzeAcolyteHat : ModItem {
        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Acolyte Hat");
            // Tooltip.SetDefault("6% reduced mana usage");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 28; int height = 16;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;

            Item.value = Item.sellPrice(0, 0, 4, 50);
        }

        public override void UpdateEquip(Player player) => player.manaCost -= 0.06f;
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            // --- Получаем RoA, если есть ---
            Mod roa = null;
            ModLoader.TryGetMod("RoA", out roa);

            // --- Типы брони ---

            // Bronze — твой мод
            int bronzeJacket = ModContent.ItemType<BronzeAcolyteJacket>();
            int bronzeLegs   = ModContent.ItemType<BronzeAcolyteLeggings>();

            // Copper/Tin — из RoA (безопасно)
            int copperJacket = roa?.TryFind("CopperAcolyteJacket", out ModItem cj) == true ? cj.Type : -1;
            int tinJacket    = roa?.TryFind("TinAcolyteJacket", out ModItem tj) == true ? tj.Type : -1;

            int copperLegs = roa?.TryFind("CopperAcolyteLeggings", out ModItem cl) == true ? cl.Type : -1;
            int tinLegs    = roa?.TryFind("TinAcolyteLeggings", out ModItem tl) == true ? tl.Type : -1;

            // --- Проверка на жилет ---
            bool bodyMatch =
                body.type == bronzeJacket ||
                body.type == copperJacket ||
                body.type == tinJacket;

            // --- Проверка на штаны ---
            bool legsMatch =
                legs.type == bronzeLegs ||
                legs.type == copperLegs ||
                legs.type == tinLegs;

            return bodyMatch && legsMatch;
        }

        public override void UpdateArmorSet(Player player) {
            player.setBonus = Language.GetText("Mods.RoA.Items.Tooltips.AcolyteSetBonus").Value;

            if (player.statMana <= 40) {
                player.GetDamage(DamageClass.Magic) += 0.4f;
            }
        }
    }
}