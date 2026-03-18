using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.GlowMasks;
using Synergia.Content.Buffs;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Common.Rarities;

namespace Synergia.Content.Items.Weapons.Cogworm;

[AutoloadGlowMask] // Code from Rise of Ages: Raven's Eye
sealed class Menace : ModItem {
    public override void SetStaticDefaults() {
        // DisplayName.SetDefault("Menace");
        // Tooltip.SetDefault("Fires magical shards from above");

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
        Item.damage = 42;
        Item.knockBack = 2f;

        Item.noMelee = true;
        Item.channel = true;
        Item.mana = 10;

        Item.rare = ModContent.RarityType<LavaGradientRarity>();

        Item.UseSound = SoundID.Item105;

        Item.shoot = ModContent.ProjectileType<MagicStalactite>();
        Item.shootSpeed = 12f;

        Item.value = Item.sellPrice(0, 1, 50, 0);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        Vector2 newVelocity = velocity.SafeNormalize(Vector2.Zero);
        position += newVelocity * 40;
        position += new Vector2(-newVelocity.Y, newVelocity.X) * (-10f * player.direction);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 funnyOffset = Vector2.Normalize(velocity) * 5f;
        position += funnyOffset - new Vector2(player.direction == -1 ? 0f : 8f, -2f * player.direction).RotatedBy(funnyOffset.ToRotation());

        for (int i = 0; i < 5; i++)
        {
            if (Main.rand.NextBool())
            {
                bool flag = Main.rand.NextBool();
                int dust = Dust.NewDust(position, 0, 0, flag ? 60 : 96, 0, 0.5f, 0, default, (!flag ? 1.5f : 2.3f) + 0.5f * Main.rand.NextFloat());
                Main.dust[dust].noGravity = true;
            }
        }

        bool hasHellborn = player.HasBuff(ModContent.BuffType<Hellborn>());
        if (!hasHellborn)
        {
            int count = Main.rand.Next(2, 5);

            for (int i = 0; i < count; i++)
            {
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
        }
        else
        {
            int bursts = 3; // Количество выстрелов в очереди
            int burstDelay = 5; // Задержка между выстрелами в тиках (1/60 секунды)
            int shotsPerBurst = 3; // Количество снарядов за выстрел

            for (int burst = 0; burst < bursts; burst++)
            {
                for (int shot = 0; shot < shotsPerBurst; shot++)
                {
                    // Небольшое отклонение для снарядов в очереди
                    Vector2 burstVelocity = velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.9f, 1.1f);

                    Vector2 spawnPos = new Vector2(
                        player.Center.X + Main.rand.NextFloat(-80f, 80f),
                        player.Center.Y + Main.rand.NextFloat(-40f, 30f)
                    );

                    // Создаем проектiles с задержкой
                    Projectile.NewProjectile(
                        source,
                        spawnPos,
                        burstVelocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI,
                        ai0: burst * burstDelay // Используем ai0 для синхронизации задержки
                    );
                }
            }
        }

        return false;
    }
}
