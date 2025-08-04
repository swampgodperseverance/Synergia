using Microsoft.Xna.Framework;

using Vanilla.Common.GlowMasks;
using Vanilla.Content.Projectiles.Friendly;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Vanilla.Content.Items.Weapons.Cogworm;

[AutoloadGlowMask]//Code from rise of ages ravens eye
sealed class Menace : ModItem {
    public override void SetStaticDefaults() {
        // DisplayName.SetDefault("Raven's Eye");
        // Tooltip.SetDefault("");

        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults() {
        Item.staff[Item.type] = true;

        int width = 38; int height = 40;
        Item.Size = new Vector2(width, height);

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = Item.useAnimation = 20;
        Item.autoReuse = true;

        Item.DamageType = DamageClass.Magic;
        Item.damage = 60;
        Item.knockBack = 2f;

        Item.noMelee = true;
        Item.channel = true;
        Item.mana = 10;

        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item105;

        Item.shoot = ModContent.ProjectileType<MagicStalactite>();
        Item.shootSpeed = 12f;

        Item.value = Item.sellPrice(0, 1, 50, 0);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        Vector2 newVelocity = new Vector2(velocity.X, velocity.Y).SafeNormalize(Vector2.Zero);
        position += newVelocity * 40;
        position += new Vector2(-newVelocity.Y, newVelocity.X) * (-10f * player.direction);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        Vector2 funnyOffset = Vector2.Normalize(velocity) * 5f;
        position += funnyOffset - new Vector2(player.direction == -1 ? 0f : 8f, -2f * player.direction).RotatedBy(funnyOffset.ToRotation());

        for (int i = 0; i < 5; i++) {
            if (Main.rand.NextBool()) {
                bool flag = Main.rand.NextBool();
                int dust = Dust.NewDust(position, 0, 0, flag ? 60 : 96, 0, 0.5f, 0, default, (!flag ? 1.5f : 2.3f) + 0.5f * Main.rand.NextFloat());
                Main.dust[dust].noGravity = true;
            }
        }

        int count = Main.rand.Next(2, 5);
        for (int i = 0; i < count; i++) {
            Vector2 spawnPos = new Vector2(
                player.Center.X + Main.rand.NextFloat(-60f, 60f),
                player.Center.Y + Main.rand.NextFloat(-30f, 20f)
            );

            Projectile.NewProjectile(
                source,
                spawnPos,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );
        }

        return false;
    }

}