using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Synergia.Content.Buffs;

namespace Synergia.Content.Items.Accessories
{[AutoloadEquip(EquipType.Shield)]
    public class CorrodeCrossShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Corrode Cross Shield");
            /* Tooltip.SetDefault("+12% Throwing damage\n" +
                               "When struck, increases defense and life regeneration for 10s\n" +
                               "Also reduces the effect of damage-over-time debuffs"); */
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 12, 50, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CorrodeCrossShieldPlayer>().hasShield = true;
        }
    }

    public class CorrodeCrossShieldPlayer : ModPlayer
    {
        public bool hasShield;
        private int buffTimer;

        public override void ResetEffects()
        {
            hasShield = false;
        }

        public override void UpdateDead()
        {
            buffTimer = 0;
        }

        public override void PostUpdate()
        {
            if (hasShield)
            {
                Player.GetDamage(DamageClass.Throwing) += 0.12f;

                if (buffTimer > 0)
                {
                    buffTimer--;
                    Player.AddBuff(ModContent.BuffType<CorrodeCrossShieldBuff>(), 30);
                }
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (hasShield && info.Damage > 0)
            {
                buffTimer = 60 * 10; 
            }
        }
    }
}
