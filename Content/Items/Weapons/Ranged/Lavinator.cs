using Avalon.Common;
using Avalon.Common.Templates;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.RangedProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Weapons.Ranged.Darts;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class Lavinator : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.UseSound = new SoundStyle?(new SoundStyle("ValhallaMod/Sounds/DartShot", SoundType.Sound)
            {
                Volume = 0.9f,
                PitchVariance = 0.5f
            });
            Item.damage = 25;
            Item.knockBack = 3.5f;
            Item.shootSpeed = 12f;

            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Lime;

            Item.useAmmo = AmmoID.Dart;
            Item.shoot = ProjectileID.PoisonDartBlowgun;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Mod valhallaMod = ModLoader.GetMod("ValhallaMod");
            if (valhallaMod != null)
            {
                if (type == valhallaMod.Find<ModProjectile>("Dart").Type)
                {

                    type = ModContent.ProjectileType<LavinatorDart>();
                }

            }

            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(4));

                Projectile.NewProjectile(
                    source,
                    position,
                    perturbedSpeed,
                    type,
                    damage,
                    knockback,
                    player.whoAmI);
            }

            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (ModContent.GetInstance<AvalonClientConfig>().AdditionalScreenshakes)
                UseStyles.gunStyle(player, 0.05f, 5f, 3f);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, -1f);
        }
    }
}