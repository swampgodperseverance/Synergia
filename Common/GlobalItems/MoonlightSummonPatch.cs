using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Synergia.Content.Projectiles.Reworks;
using Synergia.Content.Buffs;

namespace Synergia.Common.GlobalItems
{
    public class MoonlightSummonPatch : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem?.GetType().FullName == "Bismuth.Content.Items.Weapons.Magical.MoonlightStaff";
        }

        public override void SetDefaults(Item item)
        {
            item.DamageType = DamageClass.Summon;
            item.noMelee = true;
            item.mana = 10;
            item.UseSound = SoundID.Item44; 
            item.buffType = ModContent.BuffType<RunicLegacy>();
            item.shoot = ModContent.ProjectileType<RuneMinion>();
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<RunicLegacy>(), 3600);

            Projectile.NewProjectile(
                source,
                position,
                Vector2.Zero,
                ModContent.ProjectileType<RuneMinion>(),
                damage,
                knockback,
                player.whoAmI);

            return false; 
        }
    }
}
