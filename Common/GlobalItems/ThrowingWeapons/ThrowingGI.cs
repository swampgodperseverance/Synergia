using Synergia.Common.GlobalPlayer;
using System.Collections.Generic;
using Terraria;
using static Synergia.Common.SUtils.LocUtil;

namespace Synergia.Common.GlobalItems.ThrowingWeapons {
    public abstract class ThrowingGI : GlobalItem {
        public abstract int ItemType { get; }
        public abstract bool NewBehavior(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback);
        public virtual string AbilityInfo => "BaseText";
        public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemType;
        public sealed override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            ThrowingPlayer modPlayer = player.GetModPlayer<ThrowingPlayer>();
            if (modPlayer.DoubleMode) {
                return NewBehavior(item, player, source, position, velocity, type, damage, knockback);
            }
            return true;
        }
        public sealed override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            ThrowingPlayer throwing = player.GetModPlayer<ThrowingPlayer>();
            if (throwing.comboCount <= 5) {
                throwing.AddCombo();
            }
        }
        public sealed override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "ThrowingItem", ItemTooltip(WEP, "Info")));
            tooltips.Add(new TooltipLine(Mod, "Ability", ItemTooltip(WEP, AbilityInfo)));
        }
    }
}