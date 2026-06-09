using System;
using Avalon.Dusts;
using Avalon.Items.Material.Bars;
using Avalon.Items.Material.Shards;
using Bismuth.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Synergia.Common;
using Synergia.Content.Projectiles.ActiveAccessoriesProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.DamageClasses;
using ValhallaMod.Items.AI;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.ActiveAccessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class RunicWard : ValhallaMod.Items.AI.ActiveAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            ValhallaMod.Values.ActiveAccessoryItems[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.DamageType = GetInstance<AccessoryDamageClass>();
            Item.damage = 225;

            cooldown = 30 * 60;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RuneEssence>(), 8)
                .AddIngredient(ItemID.MythrilBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RuneEssence>(), 8)
                .AddIngredient(ItemID.OrichalcumBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RuneEssence>(), 8)
                .AddIngredient(ModContent.ItemType<NaquadahBar>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool Use(Player player, ref int time, ref int damage, ref bool silent)
        {
            Vector2 cursorPosition = Main.MouseWorld;
            Vector2 playerPosition = player.Center;
            Vector2 direction = cursorPosition - playerPosition;
            float maxDistance = 300f;

            if (direction.Length() > maxDistance)
            {
                direction = Vector2.Normalize(direction) * maxDistance;
            }

            Vector2 newPosition = playerPosition + direction;

            if (!Collision.SolidCollision(newPosition - player.Size / 2, player.width, player.height))
            {
                SpellCloneProjectile(player, playerPosition);
                player.Teleport(newPosition, 1, 0);
                SoundEngine.PlaySound(SoundID.Item8, player.Center);

                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(playerPosition - player.Size / 2, player.width, player.height,
                        DustID.MagicMirror, 0f, 0f, 100, default, 1.5f);
                }

                return true;
            }
            else
            {
                CombatText.NewText(player.Hitbox, Color.Red, "!");
                return false;
            }
        }

        private void SpellCloneProjectile(Player player, Vector2 position)
        {
            Projectile.NewProjectile(player.GetSource_Accessory(Item),
                position,
                Vector2.Zero,
                ProjectileType<SpellClone>(),
                Item.damage,
                3f,
                player.whoAmI);
        }

        public override void SafeUpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }
    }

    public class SpellClone : ModProjectile
    {
        private int explosionDelay = 90;
        private bool hasExploded = false;
        private float fadeInProgress = 0f;

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = explosionDelay + 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
            Projectile.Opacity = 0.6f;
        }

        public override void AI()
        {
            if (fadeInProgress < 1f)
            {
                fadeInProgress += 0.05f;
                Projectile.alpha = (int)(255 * (1f - fadeInProgress));
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    ModContent.DustType<TourmalineDust>(), 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.5f, -0.2f));
            }

            explosionDelay--;

            if (explosionDelay <= 0 && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }

            if (explosionDelay < 20 && explosionDelay > 0 && !hasExploded)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 warningPos = Projectile.Center + new Vector2(
                        Main.rand.NextFloat(-Projectile.width / 2f, Projectile.width / 2f),
                        Main.rand.NextFloat(-Projectile.height / 2f, Projectile.height / 2f));

                    Dust warningDust = Dust.NewDustPerfect(warningPos, DustID.Torch,
                        Vector2.Normalize(warningPos - Projectile.Center) * Main.rand.NextFloat(1f, 2f),
                        100, Color.Red, 0.8f);
                    warningDust.noGravity = true;
                }
            }
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 25; i++)
            {
                float speed = Main.rand.NextFloat(3f, 8f);
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-1f, 1f));
                velocity.Normalize();
                velocity *= speed;

                Dust explosionDust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<TourmalineDust>(),
                    velocity, 100, default, Main.rand.NextFloat(1.2f, 2f));
                explosionDust.noGravity = true;
            }

            for (int i = 0; i < 12; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-6f, 6f),
                    Main.rand.NextFloat(-6f, 6f));

                Dust sparkDust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<TourmalineDust>(),
                    velocity, 80, default, Main.rand.NextFloat(0.8f, 1.5f));
                sparkDust.noGravity = true;
            }

            float explosionRadius = 110f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) <= explosionRadius)
                {
                    int damage = Projectile.damage;
                    float knockback = 5f;
                    int direction = npc.Center.X > Projectile.Center.X ? 1 : -1;

                    npc.SimpleStrikeNPC(damage, direction, false, knockback);

                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height,
                            ModContent.DustType<TourmalineDust>(), 0f, 0f, 100, default, 0.8f);
                    }
                }
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.7f, 0.9f);

            for (int i = 0; i < 24; i++)
            {
                float angle = MathHelper.ToRadians(i * 15);
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * explosionRadius;
                Dust ringDust = Dust.NewDustPerfect(Projectile.Center + offset, ModContent.DustType<TourmalineDust>(),
                    Vector2.Zero, 80, default, 0.9f);
                ringDust.noGravity = true;
                ringDust.velocity = Vector2.Normalize(offset) * 1.5f;
            }

            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            if (!hasExploded)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 velocity = new Vector2(
                        Main.rand.NextFloat(-1.5f, 1.5f),
                        Main.rand.NextFloat(-2f, 0f));

                    Dust disappearDust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<TourmalineDust>(),
                        velocity, 150, default, Main.rand.NextFloat(0.6f, 1f));
                    disappearDust.noGravity = true;
                }

                SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;

            float glowIntensity = 1f;
            if (explosionDelay < 20 && explosionDelay > 0 && !hasExploded)
            {
                glowIntensity = 1.5f;
            }

            Color drawColor = Projectile.GetAlpha(new Color(100, 255, 200));
            drawColor.A = (byte)(80);

            Color glowColor = new Color(0, 200, 255);

            for (int i = 0; i < 6; i++)
            {
                float angle = Main.GameUpdateCount * 0.02f + i * MathHelper.TwoPi / 6f;
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 2f;
                Color offsetColor = glowColor * (0.3f);
                offsetColor.A = (byte)(40 * glowIntensity);

                Main.EntitySpriteDraw(texture, drawPosition + offset, null, offsetColor,
                    Projectile.rotation, texture.Size() / 2f, Projectile.scale + 0.03f, SpriteEffects.None, 0);
            }

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2 offset = new Vector2(i, j) * 1.5f;
                    Color outlineColor = glowColor * 0.5f;
                    outlineColor.A = (byte)(50 * glowIntensity);

                    Main.EntitySpriteDraw(texture, drawPosition + offset, null, outlineColor,
                        Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
                }
            }

            Main.EntitySpriteDraw(texture, drawPosition, null, drawColor,
                Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}