using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Reworks.Reworks2;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems.Weapons
{
    public class SBEAMGI : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (entity.ModItem == null) return false;
            return entity.ModItem.Mod?.Name == "ValhallaMod" && entity.ModItem.Name == "ShadowBolt";
        }

        public override void SetDefaults(Item item)
        {
            if (!AppliesToEntity(item, false)) return;

            item.DamageType = DamageClass.Magic;
            item.knockBack = 4f;
            item.useTime = 49;      
            item.useAnimation = 49;
            item.reuseDelay = 0;
            item.mana = 11;     
            item.shootSpeed = 9f;
            item.shoot = ModContent.ProjectileType<ShadowBeamRework>();
            item.noMelee = true;
            item.autoReuse = true;
        }


    }
}