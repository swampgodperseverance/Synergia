using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Common.GlobalPlayer;

namespace Synergia.Common.GlobalItems.ThrowingWeapons {
    public abstract class ThrowingGI : GlobalItem {
        public abstract int ItemType { get; }
        public abstract bool NewBehavior(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback);
        public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemType;
        public sealed override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            ThrowingPlayer modPlayer = player.GetModPlayer<ThrowingPlayer>();
            if (modPlayer.doubleMode) {
                return NewBehavior(item, player, source, position, velocity, type, damage, knockback);
            }
            return true;
        }
        public sealed override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) => player.GetModPlayer<ThrowingPlayer>().AddCombo();
    }
}