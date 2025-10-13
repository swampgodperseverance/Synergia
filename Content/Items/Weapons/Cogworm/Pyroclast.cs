using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Synergia.Common;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Content.Buffs; 

namespace Synergia.Content.Items.Weapons.Cogworm
{
    public class Pyroclast : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Firestorm Bow");
            // Tooltip.SetDefault("Shoots three fireballs at once\nSets the wielder on fire");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 56;
            Item.damage = 110;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.rare = ModContent.RarityType<LavaGradientRarity>();

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<FireballProjectile>();
            Item.shootSpeed = 10f;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f); 
        }

        public override void HoldItem(Player player)
        {
            player.AddBuff(BuffID.OnFire, 30);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity *= 1.2f;
            position += new Vector2(-2f, 0f);

            if (player.HasBuff(ModContent.BuffType<Hellborn>()))
            {
                damage = (int)(damage * 1.2f); 
                knockback += 1f;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool hellborn = player.HasBuff(ModContent.BuffType<Hellborn>());
            int fireballCount = hellborn ? 5 : 3;
            float spread = hellborn ? 0.45f : 0.3f;

            for (int i = 0; i < fireballCount; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(spread);
                newVelocity *= 1f - Main.rand.NextFloat(0.1f);

                Projectile.NewProjectile(
                    source,
                    position,
                    newVelocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI);
            }

            for (int i = 0; i < 5; i++)
            {
                Vector2 dustVel = velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.5f, 1.5f);
                Dust.NewDustPerfect(position, DustID.Torch, dustVel, 100, default, 1.5f).noGravity = true;
            }

            if (hellborn)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 sparkVel = Main.rand.NextVector2Circular(3f, 3f);
                    int spark = Dust.NewDust(position, 0, 0, DustID.Flare, sparkVel.X, sparkVel.Y, 150, default, 1.3f);
                    Main.dust[spark].noGravity = true;
                }

                SoundEngine.PlaySound(SoundID.Item74, position); 
            }

            return false;
        }

    }
}
