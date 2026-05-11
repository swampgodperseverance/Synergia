using System.Collections.Generic;
using Avalon.Items.Accessories.Hardmode;
using Bismuth.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using ValhallaMod.Items.Weapons.Ranged.Javelins;

namespace Synergia.Common.GlobalItems.Set {
    public class EditItem : GlobalItem {
        public override void SetDefaults(Item entity) {
            if (entity.type == ItemType<CarrotDagger>()) {
                entity.damage = 15;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
            if (item.type == ItemType<BacchusBoots>()) {
                player.GetDamage(DamageClass.Summon) += 0.10f;
            }
            if (item.type == ItemType<BerserksRing>()) {
                player.GetDamage(DamageClass.Generic) -= 0.20f;
            }
            if (item.type == ItemType<AriadnesTangle>())
            {
                player.yoyoString = true;
            }
            if (item.type == ItemType<SignOfUndead>())
            {
                player.statLifeMax2 += 140;
                player.lifeRegen = 0;
                player.lifeRegenCount = 0;
                player.lifeRegenTime = 0;
                player.lifeRegen = -player.lifeRegen;
                player.ClearBuff(BuffID.Regeneration);
                player.ClearBuff(BuffID.Honey);
                player.ClearBuff(BuffID.Campfire);
                player.ClearBuff(BuffID.HeartLamp);
                player.ClearBuff(BuffID.Sunflower);
                player.buffImmune[BuffID.Regeneration] = true;
                player.buffImmune[BuffID.Honey] = true;
                player.buffImmune[BuffID.Campfire] = true;
                player.buffImmune[BuffID.HeartLamp] = true;
                player.buffImmune[BuffID.Sunflower] = true;
            }
            if (item.type == ItemType<DraculasCover>())
            {

            }
            if (item.type == ItemType<MarbleMask>())
            {
                player.GetDamage(DamageClass.Ranged) += 0.05f;
            }
            if (item.type == ItemType<PendantOfBlood>())
            {
                player.endurance += 0.05f;
                player.lifeRegen += 1;
            }

        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            void Add(string modName, string itemName, string tooltipKey)
            {
                Mod m = ModLoader.GetMod(modName);
                if (m != null && item.type == m.Find<ModItem>(itemName).Type)
                    ReplaceTooltip(tooltips, "Tooltip0", Language.GetTextValue($"Mods.Synergia.{tooltipKey}"));
            }

            Add("Bismuth", "AriadnesTangle", "Items.AriadnesTangle.Tooltip");
            Add("Bismuth", "MarbleMask", "Items.MarbleMask.Tooltip");
            Add("Bismuth", "PendantOfBlood", "Items.PendantOfBlood.Tooltip");

            if (item.type == ModContent.Find<ModItem>("Bismuth/SignOfUndead").Type)
            {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name.StartsWith("Tooltip"));
                string newTooltip = Language.GetTextValue("Mods.Synergia.Items.SignOfUndead.Tooltip");
                tooltips.Add(new TooltipLine(Mod, "SignOfUndeadTooltip", newTooltip));
            }

            // DraculasCover
            if (item.type == ModContent.Find<ModItem>("Bismuth/DraculasCover").Type)
            {
                tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name.StartsWith("Tooltip"));
                string newTooltip = Language.GetTextValue("Mods.Synergia.Items.DraculasCover.Tooltip");
                tooltips.Add(new TooltipLine(Mod, "DraculasCoverTooltip", newTooltip));
            }
        }

        private void ReplaceTooltip(List<TooltipLine> tooltips, string tooltipName, string newText)
        {
            foreach (var line in tooltips)
            {
                if (line.Name == tooltipName && line.Mod == "Terraria")
                {
                    line.Text = newText;
                    break;
                }
            }
        }
    }
}
