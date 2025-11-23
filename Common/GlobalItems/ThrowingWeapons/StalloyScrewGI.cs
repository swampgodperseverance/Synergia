using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Synergia.Common.Players;
using Synergia.Content.Projectiles.Thrower;

namespace Synergia.Common.GlobalItems
{
    public class StalloyScrewGlobalItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            // ✅ ТОЛЬКО ЭТО для модовых предметов!
            if (entity.ModItem == null)
                return false;

            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;

            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "StalloyScrew", StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.GetModPlayer<StalloyScrewPlayer>();

            if (modPlayer.doubleMode)
            {
                int newType = ModContent.ProjectileType<StalloyMegascrew>();
                velocity *= 1.5f;
                damage = (int)(damage * 2.3f);
                knockback *= 1.2f;

                Projectile.NewProjectile(source, position, velocity, newType, damage, knockback, player.whoAmI);
                return false; // Не стреляем оригинал
            }

            return true;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<StalloyScrewPlayer>().AddCombo();
        }

        public override void HoldItem(Item item, Player player)
        {
            var modPlayer = player.GetModPlayer<StalloyScrewPlayer>();
            if (modPlayer.interfaceProj != null && modPlayer.interfaceProj.active &&
                modPlayer.interfaceProj.ModProjectile is ThrowerInterface1 ui)
            {
                ui.SetFrame(modPlayer.comboCount);
            }
        }
    }
}