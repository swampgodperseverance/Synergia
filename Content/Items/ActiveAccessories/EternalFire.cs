using System;
using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Synergia.Common;
using Synergia.Content.Items.Misc;
using Synergia.Content.Projectiles.ActiveAccessoriesProjectiles;
using Synergia.Content.Projectiles.RangedProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.DamageClasses;
using ValhallaMod.Items.AI;
using ValhallaMod.Items.Material.Bar;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.ActiveAccessories
{
    public class EternalFire : ValhallaMod.Items.AI.ActiveAccessoryItem
    {
        private int dashTimer = 0;
        private bool isDashing = false;
        private Vector2 dashTarget;
        private Vector2 dashStart;
        private int bombSpawnCounter = 0;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Justice>(), 1)
                .AddIngredient(ModContent.ItemType<RunicWard>(), 1)
                .AddIngredient(ItemID.Torch, 10)
                .AddIngredient(ModContent.ItemType<SoulofBlight>(), 10)
                .AddTile(ModContent.TileType<CaesiumHeavyAnvilTile>())
                .Register();
        }
        public override void SetStaticDefaults()
        {
            ValhallaMod.Values.ActiveAccessoryItems[Item.type] = true;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 3, 25, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.DamageType = GetInstance<AccessoryDamageClass>();
            Item.damage = 300;

            cooldown = 18 * 60;
        }

        public override bool Use(Player player, ref int time, ref int damage, ref bool silent)
        {
            if (!isDashing)
            {
                Vector2 cursorPosition = Main.MouseWorld;
                Vector2 playerPosition = player.Center;
                Vector2 direction = cursorPosition - playerPosition;
                float maxDistance = 600f;

                if (direction.Length() > maxDistance)
                {
                    direction = Vector2.Normalize(direction) * maxDistance;
                }

                dashStart = playerPosition;
                dashTarget = playerPosition + direction;
                isDashing = true;
                dashTimer = 0;
                bombSpawnCounter = 0;

                player.velocity = Vector2.Zero;
                player.immune = true;
                player.immuneTime = 15;

                SoundEngine.PlaySound(SoundID.Item20, player.Center);
                SoundEngine.PlaySound(SoundID.Item34, player.Center);
            }
            return true;
        }

        public override void SafeUpdateAccessory(Player player, bool hideVisual)
        {
            if (isDashing)
            {
                dashTimer++;

                float progress = dashTimer / 4f;
                if (progress >= 1f)
                {
                    player.position = dashTarget - player.Size / 2;
                    isDashing = false;
                    player.velocity = Vector2.Zero;

                    player.immune = true;
                    player.immuneTime = 90;

                    for (int i = 0; i < 30; i++)
                    {
                        Dust burstDust = Dust.NewDustDirect(player.position, player.width, player.height,
                            DustID.SolarFlare, 0f, 0f, 100, default, 1.8f);
                        burstDust.noGravity = true;
                        burstDust.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        Dust burstDust = Dust.NewDustDirect(player.position, player.width, player.height,
                            DustID.GoldFlame, 0f, 0f, 100, default, 1.5f);
                        burstDust.noGravity = true;
                        burstDust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                    }
                }
                else
                {
                    player.position = Vector2.Lerp(dashStart, dashTarget, progress) - player.Size / 2;

                    for (int i = 0; i < 6; i++)
                    {
                        Dust dust = Dust.NewDustDirect(player.position, player.width, player.height,
                            DustID.SolarFlare, 0f, 0f, 80, default, 1.2f);
                        dust.noGravity = true;
                        dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                    }

                    bombSpawnCounter++;
                    if (bombSpawnCounter == 1 || bombSpawnCounter == 2 || bombSpawnCounter == 3 ||
                        bombSpawnCounter == 4 || bombSpawnCounter == 5 || bombSpawnCounter == 6)
                    {
                        int bombsToSpawn = Main.rand.Next(2, 4);
                        for (int j = 0; j < bombsToSpawn; j++)
                        {
                            Vector2 bombOffset = new Vector2(
                                Main.rand.NextFloat(-100f, 100f),
                                Main.rand.NextFloat(-60f, 60f));

                            Vector2 velocity = new Vector2(
                                Main.rand.NextFloat(-1f, 1f),
                                Main.rand.NextFloat(-1f, -0.2f));

                            Projectile.NewProjectile(player.GetSource_Accessory(Item),
                                player.Center + bombOffset,
                                velocity,
                                ProjectileType<SunBomb>(),
                                Item.damage,
                                3f,
                                player.whoAmI);
                        }
                    }
                }
            }
        }
    }

    public class SunBomb : ModProjectile
    {
        private int explosionDelay = 55;
        private bool hasExploded = false;
        private float growth = 0.3f;
        private float targetScale = 0.7f;
        private float rotationSpeed;
        private float rotationOffset;
        private float spawnFade = 0f;

        private float[] rayRotations;
        private float[] rayScaleX;
        private float[] rayScaleY;
        private float[] rayPulseSpeed;
        private float rayRandomSeed;
        private int rayCount;
        private static Asset<Texture2D> rayTexture;

        public override void Load()
        {
            rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
        }

        public override void Unload()
        {
            rayTexture = null;
        }

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = explosionDelay + 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.alpha = 255;
            Projectile.scale = 0.1f;

            rotationSpeed = Main.rand.NextFloat(-0.08f, 0.08f);
            rotationOffset = Main.rand.NextFloat(0f, MathHelper.TwoPi);

            rayCount = Main.rand.Next(2, 4);
            rayRandomSeed = Main.rand.NextFloat(100f);
            rayRotations = new float[rayCount];
            rayScaleX = new float[rayCount];
            rayScaleY = new float[rayCount];
            rayPulseSpeed = new float[rayCount];

            for (int i = 0; i < rayCount; i++)
            {
                rayRotations[i] = Main.rand.NextFloat(-0.3f, 0.3f);
                rayScaleX[i] = Main.rand.NextFloat(0.3f, 0.5f);
                rayScaleY[i] = Main.rand.NextFloat(0.5f, 0.8f);
                rayPulseSpeed[i] = Main.rand.NextFloat(1.5f, 2.5f);
            }
        }

        public override void AI()
        {
            if (spawnFade < 1f)
            {
                spawnFade += 0.05f;
                Projectile.alpha = (int)(255 * (1f - spawnFade));
                Projectile.scale = 0.1f + growth * spawnFade;
            }

            if (growth < targetScale && spawnFade >= 1f)
            {
                growth += 0.02f;
                Projectile.scale = growth;
            }
            else if (spawnFade < 1f)
            {
                growth += 0.02f;
            }

            if (Projectile.alpha > 50 && spawnFade >= 1f)
            {
                Projectile.alpha -= 3;
            }

            Projectile.rotation += rotationSpeed;

            if (spawnFade >= 0.5f && Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.SolarFlare, 0f, 0f, 50, default, Projectile.scale * 0.4f);
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.5f, -0.1f));
            }

            if (spawnFade >= 0.7f && Main.rand.NextBool(6))
            {
                Vector2 glowPos = Projectile.Center + new Vector2(
                    (float)Math.Sin(Main.GameUpdateCount * 0.1f + rotationOffset) * Projectile.width * 0.3f * Projectile.scale,
                    (float)Math.Cos(Main.GameUpdateCount * 0.15f + rotationOffset) * Projectile.height * 0.3f * Projectile.scale);

                Dust glowDust = Dust.NewDustPerfect(glowPos, DustID.GoldFlame,
                    Vector2.Zero, 60, default, 0.5f);
                glowDust.noGravity = true;
            }

            explosionDelay--;

            if (explosionDelay <= 0 && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }

            if (explosionDelay < 12 && explosionDelay > 0 && !hasExploded && spawnFade >= 1f)
            {
                Projectile.scale = growth + (float)Math.Sin(Main.GameUpdateCount * 0.5f) * 0.05f;

                if (Main.rand.NextBool(3))
                {
                    Vector2 warningPos = Projectile.Center + new Vector2(
                        Main.rand.NextFloat(-Projectile.width / 2f * Projectile.scale, Projectile.width / 2f * Projectile.scale),
                        Main.rand.NextFloat(-Projectile.height / 2f * Projectile.scale, Projectile.height / 2f * Projectile.scale));

                    Dust warningDust = Dust.NewDustPerfect(warningPos, DustID.Torch,
                        Vector2.Normalize(warningPos - Projectile.Center) * Main.rand.NextFloat(0.5f, 1f),
                        60, Color.OrangeRed, 0.6f);
                    warningDust.noGravity = true;
                }
            }
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item73, Projectile.Center);

            for (int i = 0; i < 12; i++)
            {
                float speed = Main.rand.NextFloat(2f, 6f);
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-1f, 1f));
                velocity.Normalize();
                velocity *= speed;

                Dust explosionDust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare,
                    velocity, 70, default, Main.rand.NextFloat(1f, 1.4f));
                explosionDust.noGravity = true;
            }

            for (int i = 0; i < 6; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f));

                Dust sparkDust = Dust.NewDustPerfect(Projectile.Center, DustID.InfernoFork,
                    velocity, 50, default, Main.rand.NextFloat(0.7f, 1f));
                sparkDust.noGravity = true;
            }

            float explosionRadius = 90f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) <= explosionRadius)
                {
                    int damage = Projectile.damage;
                    float knockback = 5f;
                    int direction = npc.Center.X > Projectile.Center.X ? 1 : -1;

                    npc.SimpleStrikeNPC(damage, direction, false, knockback);

                    for (int i = 0; i < 1; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height,
                            DustID.SolarFlare, 0f, 0f, 70, default, 0.7f);
                    }
                }
            }

            Lighting.AddLight(Projectile.Center, 0.7f, 0.4f, 0.1f);

            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            if (!hasExploded)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                    ProjectileType<LavinatorMicroBurst>(), Projectile.damage / 2, 2f, Projectile.owner);

                for (int i = 0; i < 3; i++)
                {
                    Vector2 velocity = new Vector2(
                        Main.rand.NextFloat(-1f, 1f),
                        Main.rand.NextFloat(-1.5f, 0f));

                    Dust disappearDust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare,
                        velocity, 80, default, Main.rand.NextFloat(0.5f, 0.7f));
                    disappearDust.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;

            float glowIntensity = 0.6f + (float)Math.Sin(Main.GameUpdateCount * 0.12f) * 0.3f;
            float pulseScale = 1f + (float)Math.Sin(Main.GameUpdateCount * 0.2f) * 0.03f;

            float spawnMultiplier = Math.Min(1f, spawnFade * 1.5f);

            Texture2D ringTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Vector2 ringOrigin = ringTexture.Size() / 2f;
            float ringScale = (0.45f + (float)Math.Sin(Main.GameUpdateCount * 0.08f) * 0.05f) * spawnMultiplier;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Color ringColor = new Color(255, 100, 0) * (0.4f * spawnMultiplier);
            Main.EntitySpriteDraw(ringTexture, drawPosition, null, ringColor,
                Projectile.rotation * 0.5f, ringOrigin, ringScale, SpriteEffects.None, 0);

            Color ringColor2 = new Color(255, 60, 0) * (0.3f * spawnMultiplier);
            Main.EntitySpriteDraw(ringTexture, drawPosition, null, ringColor2,
                -Projectile.rotation * 0.3f, ringOrigin, ringScale * 0.8f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Color drawColor = Projectile.GetAlpha(new Color(255, 140, 0));
            drawColor.A = (byte)(60 * spawnMultiplier);

            Color glowColor = new Color(255, 100, 0);

            for (int i = 0; i < 4; i++)
            {
                float angle = Main.GameUpdateCount * 0.08f + i * MathHelper.TwoPi / 4f;
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (2f + (float)Math.Sin(Main.GameUpdateCount * 0.15f + i) * 0.5f);
                Color offsetColor = glowColor * (0.3f * glowIntensity * spawnMultiplier);
                offsetColor.A = (byte)(40 * spawnMultiplier);

                Main.EntitySpriteDraw(texture, drawPosition + offset, null, offsetColor,
                    Projectile.rotation, texture.Size() / 2f, (Projectile.scale * pulseScale + 0.03f) * spawnMultiplier, SpriteEffects.None, 0);
            }

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2 offset = new Vector2(i, j) * 1f;
                    Color outlineColor = glowColor * (0.4f * spawnMultiplier);
                    outlineColor.A = (byte)(45 * spawnMultiplier);

                    Main.EntitySpriteDraw(texture, drawPosition + offset, null, outlineColor,
                        Projectile.rotation, texture.Size() / 2f, Projectile.scale * pulseScale * spawnMultiplier, SpriteEffects.None, 0);
                }
            }

            Main.EntitySpriteDraw(texture, drawPosition, null, drawColor,
                Projectile.rotation, texture.Size() / 2f, Projectile.scale * pulseScale * spawnMultiplier, SpriteEffects.None, 0);

            if (rayTexture != null && rayTexture.IsLoaded && explosionDelay > 0 && explosionDelay < 40 && spawnFade >= 1f)
            {
                float lifeProgress = 1f - (explosionDelay / 55f);
                float globalRotation = Main.GlobalTimeWrappedHourly * 0.3f;

                Texture2D rayTex = rayTexture.Value;
                Vector2 rayOrigin = new Vector2(rayTex.Width / 2f, rayTex.Height);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                for (int i = 0; i < rayCount; i++)
                {
                    float rayIntensity = (1f - lifeProgress) * 0.6f;
                    if (rayIntensity <= 0.05f) continue;

                    float angle = MathHelper.TwoPi / rayCount * i + globalRotation + rayRotations[i];
                    float pulseScaleRay = 0.7f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * rayPulseSpeed[i] + i) * 0.15f;

                    Color rayColor = new Color(255, 120, 0) * (rayIntensity * 0.5f);

                    float scaleX = rayScaleX[i] * pulseScaleRay * 0.6f;
                    float scaleY = rayScaleY[i] * 0.5f;

                    Main.EntitySpriteDraw(rayTex, drawPosition, null, rayColor,
                        angle, rayOrigin,
                        new Vector2(scaleX, scaleY),
                        SpriteEffects.None, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }

            return false;
        }
    }
}