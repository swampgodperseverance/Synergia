using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Aura;
using Synergia.Content.Buffs;

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
