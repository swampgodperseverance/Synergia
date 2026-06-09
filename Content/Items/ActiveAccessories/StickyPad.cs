using ValhallaMod.Items.AI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using ValhallaMod.DamageClasses;
using Synergia.Content.Projectiles.ActiveAccessoriesProjectiles;
using static Terraria.ModLoader.ModContent;
using Synergia.Common;
using Terraria.Audio;

namespace Synergia.Content.Items.ActiveAccessories
{
    public class StickyPad : ValhallaMod.Items.AI.ActiveAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            ValhallaMod.Values.ActiveAccessoryItems[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.DamageType = GetInstance<AccessoryDamageClass>();

            cooldown = 10 * 60;
        }

        public override bool Use(Player player, ref int time, ref int damage, ref bool silent)
        {
            player.velocity.Y = -24f;

            SoundEngine.PlaySound(SoundID.Item16, player.Center);
            SoundEngine.PlaySound(SoundID.Item52, player.Center);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(player.Bottom - new Vector2(15, 0), 30, 8,
                    DustID.SlimeBunny, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -1f),
                    100, Color.LightGray, 1.2f);
                dust.noGravity = false;

                Dust dust2 = Dust.NewDustDirect(player.position, player.width, player.height,
                    DustID.SlimeBunny, Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-3f, -1f),
                    80, Color.White, 1f);
            }

            for (int i = 0; i < 6; i++)
            {
                Dust dust = Dust.NewDustDirect(player.Center - new Vector2(20, 20), 40, 40,
                    DustID.Smoke, Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-2.5f, -0.5f),
                    120, Color.Gray, 1.5f);
            }

            for (int i = 0; i < 360; i += 30)
            {
                Vector2 offset = new Vector2(0, 20).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(player.Bottom + offset - new Vector2(3, 3), 6, 6,
                    DustID.SlimeBunny, offset.X * 0.15f, offset.Y * 0.15f - 2.5f, 100, Color.LightGray, 0.8f);
                dust.noGravity = false;
            }

            return true;
        }

        public override void SafeUpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;

            if (player.velocity.Y > 0)
            {
                if (Main.rand.NextBool(8))
                {
                    Dust dust = Dust.NewDustDirect(player.Bottom - new Vector2(12, 0), 24, 4,
                        DustID.Cloud, Main.rand.NextFloat(-0.8f, 0.8f), Main.rand.NextFloat(0.3f, 1.5f),
                        60, Color.LightGray, 0.6f);
                    dust.noGravity = false;
                }
            }

            if (!hideVisual)
            {
                Lighting.AddLight(player.Center, 0.3f, 0.3f, 0.4f);
            }
        }
    }
}