using Synergia.Content.Projectiles.Aura;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Common.GlobalItems
{
    public class GalvornSummonPatch : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem?.GetType().FullName == "Bismuth.Content.Items.Weapons.Magical.GalvornStaff";
        }

        public override void SetDefaults(Item item)
        {
            item.DamageType = DamageClass.Summon;
            item.noMelee = true;
            item.mana = 40;
            item.UseSound = SoundID.Item44; 
            item.shoot = ModContent.ProjectileType<GalvornAura>();
        }
    }
}
