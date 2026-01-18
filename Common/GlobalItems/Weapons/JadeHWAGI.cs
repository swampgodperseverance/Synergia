using System;
using Terraria;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks.Reworks2;

namespace Synergia.Content.Global
{
    public class HwachaGlobal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null)
                return false;
            var modName = entity.ModItem.Mod?.Name;
            var itemName = entity.ModItem?.Name;
            return string.Equals(modName, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(itemName, "JadeBalista", StringComparison.OrdinalIgnoreCase);
        }
        public override void SetDefaults(Item entity)
        {
            if (AppliesToEntity(entity, false))
            {
            }
        }
       public override bool Shoot(
            Item item,
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            if (!AppliesToEntity(item, false))
                return true;
    
        
            Mod valhallaMod = ModLoader.GetMod("ValhallaMod");
            if (valhallaMod != null)
            {
             
                if (type == valhallaMod.Find<ModProjectile>("Dart").Type)
                {
                    Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        ModContent.ProjectileType<JadeDart>(),
                        damage,
                        knockback,
                        player.whoAmI
                    );
                    return false;
                }
            }
    
            return true;
        }
    }
}