using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ValhallaMod.Projectiles.AI;

namespace Vanilla.Content.Items.Accessories
{
    public class BrokenDice : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Broken Dice");
            // Tooltip.SetDefault("Random effects in Valhalla auras");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BrokenDicePlayer>().diceEquipped = true;
        }
    }

    public class BrokenDicePlayer : ModPlayer
    {
        public bool diceEquipped;
        private bool wasInAura = false;

        private readonly List<(string mod, string buff)> positiveBuffs = new()
        {
            ("RoA", "DryadBlood"),
            ("Avalon", "SpectrumBlur"),
            ("Avalon", "AdvWrath"),
            ("ValhallaMod", "HeliosFlameBuff"),
            ("Avalon", "AdvRegeneration")
        };

        private readonly List<(string mod, string buff)> negativeBuffs = new()
        {
            ("RoA", "Hemorrhage"),
            ("RoA", "Infected"),
            ("Avalon", "Sticky"),
            ("Avalon", "IcySlowdown"),
            ("Avalon", "ShadowCurse")
        };

       public override void ResetEffects()
        {
            diceEquipped = false;
        }

        public override void PostUpdate()
        {
            if (!diceEquipped)
                return;

            bool isInAura = IsInValhallaAura(Player);

            if (isInAura && !wasInAura)
            {
                bool isBuff = Main.rand.NextBool();
                TryApplyRandomEffect(Player, isBuff);
                
                // Show floating text above player's head
                if (isBuff)
                    CombatText.NewText(Player.getRect(), Color.LimeGreen, "Luck!");
                else
                    CombatText.NewText(Player.getRect(), Color.OrangeRed, "Unluck!");
            }

            wasInAura = isInAura;
        }

        private bool IsInValhallaAura(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && 
                    proj.ModProjectile is AuraAI aura &&
                    Vector2.Distance(player.Center, proj.Center) <= aura.distanceMax)
                {
                    return true;
                }
            }
            return false;
        }

        private void TryApplyRandomEffect(Player player, bool isPositive)
        {
            var list = isPositive ? positiveBuffs : negativeBuffs;
            if (list.Count == 0) return;

            var (modName, buffName) = list[Main.rand.Next(list.Count)];
            Mod mod = ModLoader.GetMod(modName);
            if (mod == null) return;

            ModBuff buff = mod.Find<ModBuff>(buffName);
            if (buff != null)
            {
                player.AddBuff(buff.Type, 300); // 5 seconds
            }
        }
    }
}