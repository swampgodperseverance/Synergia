using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ValhallaMod;
using ValhallaMod.Projectiles.AI;
using static Terraria.ModLoader.ModContent;
using static ValhallaMod.Projectiles.AI.AuraAI;

namespace Synergia.Content.Items.Accessories
{
    public class DecahedronOfTheUnfortunate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Broken Dice");
            // Tooltip.SetDefault("Random effects in Valhalla auras\nChanges aura shape to Hexagon Horizontal");
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
            var modPlayer = player.GetModPlayer<DecahedronPlayer>();
            var auraPlayer = player.GetModPlayer<AuraPlayer>();

            modPlayer.diceEquipped = true;

            auraPlayer.auraShape = AuraShape.HexagonHorizontal;
            auraPlayer.bonusAuraRadius = 0.25f;
            auraPlayer.bonusPlayerLinkedAuraRadius = 0.30f;
            auraPlayer.maxAuras = 2;
        }
    }

    public class DecahedronPlayer : ModPlayer
    {
        public bool diceEquipped;
        private bool wasInAura = false;

        private readonly List<(string mod, string buff)> positiveBuffs = new()
        {
            ("RoA", "DryadBlood"),
            ("Avalon", "SpectrumBlur"),
            ("Avalon", "AdvWrath"),
            ("ValhallaMod", "LeafShieldBuff"),
            ("ValhallaMod", "HeliosFlameBuff"),
            ("Avalon", "AdvTitanskin"),
            ("Avalon", "AdvRegeneration"),
            ("Avalon", "Leaping"),
            ("Terraria", "Regeneration"),
            ("Terraria", "Swiftness"),
            ("Terraria", "Ironskin"),
            ("Terraria", "Endurance"),
            ("Terraria", "Lifeforce"),
            ("Terraria", "Wrath")
        };

        private readonly List<(string mod, string buff)> negativeBuffs = new()
        {
            ("RoA", "Hemorrhage"),
            ("RoA", "Infected"),
            ("RoA", "Weak"),
            ("RoA", "Root"),
            ("Avalon", "Sticky"),
            ("Avalon", "IcySlowdown"),
            ("Avalon", "Volted"),
            ("Avalon", "ShadowCurse"),
            ("Terraria", "Obstructed"),
            ("Terraria", "Confused"),
            ("Terraria", "Bleeding"),
            ("Terraria", "CursedInferno"),
            ("Terraria", "Stoned"),
            ("Terraria", "Darkness")
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

                CombatText.NewText(Player.getRect(),
                    isBuff ? Color.LimeGreen : Color.OrangeRed,
                    isBuff ? "Luck!" : "Unluck!");
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

            if (mod.TryFind(buffName, out ModBuff buff))
            {
                player.AddBuff(buff.Type, 300); // 5 
            }
        }
    }
}