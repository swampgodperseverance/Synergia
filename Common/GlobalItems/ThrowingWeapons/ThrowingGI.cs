using Synergia.Common.GlobalPlayer;
using Synergia.Common.Rarities;
using System.Collections.Generic;
using Terraria;

namespace Synergia.Common.GlobalItems.ThrowingWeapons {
    public abstract class ThrowingGI : GlobalItem {
        public abstract int ItemType { get; }
        public abstract bool NewBehavior(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback);
        public virtual string AbilityInfo => "BaseText";
        public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemType;
        public sealed override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.GetModPlayer<ThrowingPlayer>().DoubleMode) { return NewBehavior(item, player, source, position, velocity, type, damage, knockback); }
            return true;
        }
        public sealed override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            if (player.GetModPlayer<ThrowingPlayer>().comboCount <= 5) { player.GetModPlayer<ThrowingPlayer>().AddCombo(); }
        }
        public sealed override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            Color color = BaseRarity.AnimatedColor([Color.Gray, Color.LightGray, Color.White], 45);
            tooltips.Add(new TooltipLine(Mod, "ThrowingItem", ItemTooltip(WEP, "Info")) { OverrideColor = color });
            tooltips.Add(new TooltipLine(Mod, "Ability", ItemTooltip(WEP, "BaseText")) { OverrideColor = color });
            tooltips.Add(new TooltipLine(Mod, "ActiveAbility", ItemTooltip(WEP, AbilityInfo)) { OverrideColor = color });
        }
    }
}