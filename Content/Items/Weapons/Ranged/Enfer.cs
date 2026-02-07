using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.RangedProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Ranged
{
    public class Enfer : ModItem
    {
        private static Texture2D glowTexture;

        private int shotCounter = 0;
        private int cooldownTimer = 0;

        private bool spawnedRune1 = false;
        private bool spawnedRune2 = false;
        private bool spawnedRune3 = false;
        private bool spawnedRune4 = false;

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 64;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 8, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<EnferArrow>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.HeldItem.type != Item.type)
            {
                shotCounter = 0;
                cooldownTimer = 0;
                spawnedRune1 = false;
                spawnedRune2 = false;
                spawnedRune3 = false;
                spawnedRune4 = false;

                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.owner == player.whoAmI &&
                        (proj.type == ModContent.ProjectileType<LavaRune1>() ||
                         proj.type == ModContent.ProjectileType<LavaRune2>() ||
                         proj.type == ModContent.ProjectileType<LavaRune3>() ||
                         proj.type == ModContent.ProjectileType<LavaRune4>()))
                    {
                        proj.Kill();
                    }
                }

                return;
            }

            if (cooldownTimer > 0)
            {
                cooldownTimer--;
                if (cooldownTimer % 6 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 pos = player.Center + new Vector2(Main.rand.NextFloat(-16, 16), Main.rand.NextFloat(-24, 0));
                        int d = Dust.NewDust(pos, 0, 0, DustID.Torch, 0f, -1.2f, 80, default, 1.3f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 0.4f;
                    }
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return cooldownTimer <= 0 && base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shotCounter++;

            if (shotCounter >= 1 && shotCounter <= 3 && !spawnedRune1)
            {
                SpawnRune(player, source, ModContent.ProjectileType<LavaRune1>());
                spawnedRune1 = true;
            }
            else if (shotCounter >= 4 && shotCounter <= 6 && !spawnedRune2)
            {
                SpawnRune(player, source, ModContent.ProjectileType<LavaRune2>());
                spawnedRune2 = true;
            }
            else if (shotCounter >= 7 && shotCounter <= 9 && !spawnedRune3)
            {
                SpawnRune(player, source, ModContent.ProjectileType<LavaRune3>());
                spawnedRune3 = true;
            }
            else if (shotCounter >= 10 && shotCounter <= 12 && !spawnedRune4)
            {
                SpawnRune(player, source, ModContent.ProjectileType<LavaRune4>());
                spawnedRune4 = true;
            }

            int numberProjectiles = Main.rand.Next(2, 5);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                Projectile.NewProjectile(
                    source,
                    position,
                    perturbedSpeed,
                    ModContent.ProjectileType<EnferArrow>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
            }

            if (shotCounter >= 12)
            {
                shotCounter = 0;
                cooldownTimer = 180;
                spawnedRune1 = false;
                spawnedRune2 = false;
                spawnedRune3 = false;
                spawnedRune4 = false;

                SoundEngine.PlaySound(SoundID.Item103, player.Center);
            }

            return false;
        }

        private void SpawnRune(Player player, EntitySource_ItemUse_WithAmmo source, int runeType)
        {
            Vector2 runePos = player.Center + new Vector2(0, -20);
            Projectile.NewProjectile(
                source,
                runePos,
                Vector2.Zero,
                runeType,
                (int)(player.GetWeaponDamage(Item) * 10f),  
                Item.knockBack * 1.5f,
                player.whoAmI
                        );
        }

        public override Vector2? HoldoutOffset() => new Vector2(-4f, 0f);

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
            Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.2f + 0.8f);
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            if (glowTexture != null)
            {
                Color glowColor = new Color(255, 120, 40, 255) * (0.5f + pulse * 0.5f);
                spriteBatch.Draw(glowTexture, position, frame, glowColor, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Vector2 position = Item.position - Main.screenPosition + new Vector2(Item.width / 2f, Item.height - texture.Height / 2f);
            Rectangle? frame = texture.Frame();
            Vector2 origin = frame.Value.Size() / 2f;
            spriteBatch.Draw(texture, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
            if (glowTexture != null)
            {
                float pulse = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 5f) * 0.2f + 0.8f);
                Color glowColor = new Color(255, 100, 20) * (0.6f + pulse * 0.4f);
                spriteBatch.Draw(glowTexture, position, frame, glowColor, rotation, origin, scale * 1.05f, SpriteEffects.None, 0f);
            }
            if (Main.rand.NextFloat() < 0.1f)
            {
                int dust = Dust.NewDust(Item.position, Item.width, Item.height, DustID.Torch, 0f, -0.5f, 150, Color.Orange, 1.2f);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].noGravity = true;
            }
            return false;
        }
    }
}