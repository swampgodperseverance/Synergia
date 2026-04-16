using System;
using System.Collections.Generic;
using Bismuth.Content.Buffs;
using Bismuth.Content.Items.Accessories;
using Synergia.Common.GlobalPlayer;
using Terraria;
namespace Synergia.Common.ModSystems.Hooks.Ons.EditAcc
{
    public class HookForHerosBoots
    {
        readonly static Type type = typeof(HerosBoots);
        readonly static int itemT = ItemType<HerosBoots>();
        static int level;
        public class BonusHerosBoots : HookForEditAcc
        {
            public override Type HookType => type;
            public override int HookItem => itemT;
            public override void EditAcc(Orig_UpdateAccessory orig, ModItem item, Player player, bool hideVisual)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                var sp = player.GetModPlayer<SynergiaPlayer>();

                player.waterWalk = true;
                player.waterWalk2 = true;

                if (level >= 4) player.lavaImmune = true;

                float extraSpeed = 0f;
                float extraJump = 0f;
                float extraFall = 0f;

                if (level == 1)
                {
                    extraFall = 8f;      
                    extraJump = 1.5f;
                }
                if (level == 2)
                {
                    extraSpeed = 0.08f;
                    extraJump = 2f;
                    extraFall = -1f;     
                }
                if (level == 3)
                {
                    extraSpeed = 0.08f;
                    extraJump = 2.5f;
                    extraFall = -1f;
                    sp.herosAirDashChance = 0.3f;
                }
                if (level == 4)
                {
                    extraSpeed = 0.12f;
                    extraJump = 3f;
                    extraFall = -1f;
                    sp.herosAirDashChance = 0.5f;
                    sp.herosRunningRegen = 1;
                }
                if (level == 5)
                {
                    extraSpeed = 0.16f;
                    extraJump = 3.5f;
                    extraFall = -1f;
                    sp.herosAirDashChance = 0.7f;
                    sp.herosRunningRegen = 2;
                }

                player.moveSpeed += extraSpeed;
                player.jumpSpeedBoost += extraJump;
                player.extraFall += (int)extraFall;
            }
        }
        public class TooltipsHerosBoots : HookForTooltips
        {
            public override Type ItemType => type;
            public override int Target => itemT;
            public override void EditTooltips(origTooltips orig, ModItem item, List<TooltipLine> tooltips)
            {
                level = GetInstance<BismuthArtifactsLevels>().GetLevel();
                tooltips.Add(new TooltipLine(Mod, "ItemName", ItemTooltip(ACC + ".BalanceBismuth", "DivineEquipment")) { OverrideColor = new Color(0, 239, 239) });
                TooltipLine tooltip = new(Mod, "Info", ItemTooltip(ACC + ".BalanceBismuth." + "HerosBoots", "Level" + level.ToString()));
                tooltips.Add(tooltip);
            }
        }
    }
}