using System;
using Avalon;
using Avalon.Common.Players;
using Bismuth.Content.Items.Armor;
using Synergia.Common.GlobalPlayer;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems.Hooks.Ons
{
    public class HookForOrcSetBonus : HookForArmorSetBonus
    {
        public override Type Armor => typeof(OrcishHelmet);
        public override int ArmorType => ItemType<OrcishHelmet>();

        public override void NewLogicForSetBonus(Orig_SetBonus orig, ModItem item, Player player)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.GetModPlayer<AvalonStaminaPlayer>().StatStamMax2 += 30;
            player.moveSpeed += 1f;
            player.lifeRegen += 1;
            player.setBonus = ItemTooltip(ARM, "OrcishSetBonus");
            AddEssenceDrainOnHit(player);
        }

        private void AddEssenceDrainOnHit(Player player)
        {
            player.GetModPlayer<OrcSetBonusPlayer>().hasEssenceDrain = true;
        }
    }

    public class OrcSetBonusPlayer : ModPlayer
    {
        public bool hasEssenceDrain = false;
        private int essenceDrainType = -1;

        public override void ResetEffects()
        {
            hasEssenceDrain = false;
        }

        private int GetEssenceDrainType()
        {
            if (essenceDrainType == -1)
            {
                if (ModContent.TryFind("RoA/EssenceDrain", out ModBuff buff))
                {
                    essenceDrainType = buff.Type;
                }
            }
            return essenceDrainType;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (hasEssenceDrain)
            {
                int buffType = GetEssenceDrainType();
                if (buffType > 0)
                {
                    npc.AddBuff(buffType, 120);
                }
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (hasEssenceDrain)
            {
                int buffType = GetEssenceDrainType();
                if (buffType > 0 && proj.owner >= 0 && Main.npc[proj.owner].active)
                {
                    Main.npc[proj.owner].AddBuff(buffType, 120);
                }
            }
        }
    }
}