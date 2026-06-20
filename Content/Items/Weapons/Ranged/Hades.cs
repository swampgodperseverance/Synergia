using System;
using Synergia.Common.Rarities;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;


namespace Synergia.Content.Items.Weapons.Ranged
{
    public class Hades : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 36;

            Item.damage = 110;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 6f;
            Item.crit = 8;

            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.reuseDelay = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HadesHeldItemProj>();
            Item.noUseGraphic = true;
            Item.shootSpeed = 16f;
            Item.autoReuse = true;
            Item.channel = true;

            Item.rare = ModContent.RarityType<CoreburnedRarity>();
            Item.value = Item.sellPrice(0, 10, 0, 0);

            Item.useAmmo = AmmoID.Dart;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<HadesHeldItemProj>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {

        }
    }

    public class HadesHeldItemProj : ModProjectile
    {
        private const float HoldOffset = 3f;
        private const float ChargeTime = 60f;
        private const float RotationOffset = 45f / 180f * MathHelper.Pi;
        private const float RotationOffsetSpeed = 12f;

        public ref float ChargeTimer => ref Projectile.ai[0];
        public ref float RotationOffsetTimer => ref Projectile.ai[1];
        public ref float CurrentCharge => ref Projectile.ai[2];

        private Item heldItem;
        private int defaultShootType;

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 36;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3;
        }

        public override bool? CanDamage() => false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (heldItem == null)
            {
                heldItem = player.HeldItem;
                defaultShootType = heldItem?.shoot ?? ProjectileID.PoisonDartBlowgun;
            }

            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

            if (RotationOffsetTimer > 0)
                RotationOffsetTimer--;

            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || !player.CheckMana(player.GetManaCost(heldItem), true))
                {
                    if (ChargeTimer > 0 && CurrentCharge > 0)
                    {
                        ReleaseShot(player, playerCenter);
                    }
                    Projectile.Kill();
                    return;
                }

                Vector2 direction = HoldOffset * Vector2.Normalize(Main.MouseWorld - playerCenter);
                if (direction.X != Projectile.velocity.X || direction.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = direction;

                if (ChargeTimer < ChargeTime)
                {
                    ChargeTimer++;
                    CurrentCharge = ChargeTimer / ChargeTime;

                    if (ChargeTimer % 5 == 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust dust = Dust.NewDustDirect(
                                Projectile.Center + Projectile.velocity,
                                20, 20,
                                DustID.Smoke,
                                0, 0,
                                100,
                                default,
                                0.8f + CurrentCharge
                            );
                            dust.velocity = Projectile.velocity.RotatedByRandom(0.5f) * 0.3f;
                            dust.noGravity = true;
                        }

                        if (CurrentCharge > 0.3f)
                        {
                            Dust fireDust = Dust.NewDustDirect(
                                Projectile.Center + Projectile.velocity,
                                10, 10,
                                DustID.Torch,
                                0, 0,
                                100,
                                default,
                                1.2f
                            );
                            fireDust.velocity = Projectile.velocity.RotatedByRandom(0.3f) * 0.2f;
                            fireDust.noGravity = true;
                        }
                    }
                }

                if (CurrentCharge >= 0.95f && ChargeTimer % 2 == 0)
                {
                    RotationOffsetTimer = 3;
                }
            }

            Projectile.direction = Projectile.velocity.X < 0 ? -1 : 1;
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            Projectile.Center = playerCenter;
            Projectile.timeLeft = 2;

            float rotationOffset = Projectile.spriteDirection == -1 ? MathHelper.Pi : 0;
            Projectile.rotation = Projectile.velocity.ToRotation() + rotationOffset;
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        private void ReleaseShot(Player player, Vector2 playerCenter)
        {
            Mod valhallaMod = ModLoader.GetMod("ValhallaMod");

            float chargePower = Math.Max(0.3f, CurrentCharge);
            int dartCount = (int)MathHelper.Lerp(6, 14, chargePower);
            float spread = MathHelper.Lerp(14, 6, chargePower);
            float damageMultiplier = MathHelper.Lerp(0.8f, 1.5f, chargePower);
            int damage = (int)(Projectile.damage * damageMultiplier);

            float shootSpeed = heldItem?.shootSpeed ?? 16f;
            Vector2 shootVelocity = Vector2.Normalize(Projectile.velocity) * shootSpeed;

            if (Main.netMode != NetmodeID.Server)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    playerCenter + Projectile.velocity,
                    Vector2.Zero,
                    ModContent.ProjectileType<HadesMuzzleFlash>(),
                    0,
                    0f,
                    player.whoAmI
                );

                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        playerCenter + Projectile.velocity - new Vector2(15, 15),
                        40, 40,
                        DustID.Smoke,
                        -shootVelocity.X * 0.2f,
                        -shootVelocity.Y * 0.2f,
                        100,
                        default,
                        1.3f + chargePower
                    );
                    dust.velocity *= Main.rand.NextFloat(0.5f, 1.5f);
                    dust.noGravity = true;
                }

                for (int i = 0; i < 8; i++)
                {
                    Dust fireDust = Dust.NewDustDirect(
                        playerCenter + Projectile.velocity - new Vector2(10, 10),
                        30, 30,
                        DustID.Torch,
                        -shootVelocity.X * 0.15f,
                        -shootVelocity.Y * 0.15f,
                        100,
                        default,
                        1.0f + chargePower * 0.4f
                    );
                    fireDust.noGravity = true;
                }
            }

            for (int i = 0; i < dartCount; i++)
            {
                int currentType = defaultShootType;

                if (valhallaMod != null)
                {
                    float randomChance = Main.rand.NextFloat();
                    if (randomChance < 0.35f)
                    {
                        var dartExplode = valhallaMod.Find<ModProjectile>("DartExplode");
                        if (dartExplode != null)
                            currentType = dartExplode.Type;
                    }
                    else if (randomChance < 0.7f)
                    {
                        var dartFire = valhallaMod.Find<ModProjectile>("DartFire");
                        if (dartFire != null)
                            currentType = dartFire.Type;
                    }
                }

                Vector2 perturbedSpeed = shootVelocity.RotatedByRandom(MathHelper.ToRadians(spread));
                perturbedSpeed *= Main.rand.NextFloat(0.85f, 1.15f);

                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(),
                    playerCenter + Projectile.velocity,
                    perturbedSpeed,
                    currentType,
                    damage,
                    Projectile.knockBack,
                    player.whoAmI);
            }

            if (player.whoAmI == Main.myPlayer)
            {
                var shakePlayer = player.GetModPlayer<Common.GlobalPlayer.ScreenShakePlayer>();
                shakePlayer.TriggerShake(25, 0.8f + chargePower * 0.7f);
            }

            Vector2 recoil = -shootVelocity.SafeNormalize(Vector2.Zero) * (5f + chargePower * 3f);
            player.velocity += recoil;

            SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.5f, Pitch = -0.3f - chargePower * 0.2f }, player.Center);
            SoundEngine.PlaySound(new SoundStyle("ValhallaMod/Sounds/DartShot") with { Volume = 0.6f, Pitch = 0.2f }, player.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            float chargeProgress = CurrentCharge;

            Vector2 shakeOffset = Vector2.Zero;
            if (chargeProgress > 0.7f && ChargeTimer % 3 == 0)
            {
                shakeOffset = Main.rand.NextVector2Unit() * 2f * chargeProgress;
            }

            Vector2 position = Projectile.Center - Main.screenPosition + shakeOffset;

            float rotationOffset = RotationOffsetTimer > 0 ?
                RotationOffset - RotationOffset * EaseFunctions.EaseOutBack((RotationOffsetSpeed - RotationOffsetTimer) / RotationOffsetSpeed) : 0f;
            float rotation = Projectile.rotation - rotationOffset * Projectile.direction;
            Vector2 origin = texture.Size() / 2;

            Color drawColor = Color.Lerp(lightColor, new Color(255, 100, 50), chargeProgress * 1.5f);

            if (chargeProgress > 0)
            {
                float glowIntensity = 0.3f + chargeProgress * 0.7f;
                for (int i = 0; i < 3; i++)
                {
                    Main.EntitySpriteDraw(
                        texture,
                        position + Main.rand.NextVector2Circular(2, 2),
                        null,
                        new Color(255, 80, 30, 0) * 0.3f * glowIntensity,
                        rotation + Main.rand.NextFloat(-0.1f, 0.1f),
                        origin,
                        Projectile.scale + (chargeProgress * 0.1f),
                        Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0
                    );
                }
            }

            Main.EntitySpriteDraw(
                texture,
                position,
                null,
                drawColor,
                rotation,
                origin,
                Projectile.scale,
                Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
            );

            return false;
        }
    }

    public class HadesMuzzleFlash : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/LightTrail_2";

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 20;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 12)
            {
                Projectile.scale = 1.3f + Main.rand.NextFloat(-0.2f, 0.3f);
                Projectile.rotation = Main.rand.NextFloat(-0.5f, 0.5f);
            }

            Projectile.alpha += 15;
            if (Projectile.alpha > 255) Projectile.alpha = 255;

            Projectile.scale *= 0.7f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Color mainColor = new Color(255, 120, 40, 240) * (1f - Projectile.alpha / 255f);
            Color glowColor = new Color(255, 60, 0, 200) * (1f - Projectile.alpha / 255f);

            float baseScale = Projectile.scale;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                texture.Size() / 2f,
                baseScale * 1.5f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor,
                Projectile.rotation + 0.3f,
                texture.Size() / 2f,
                baseScale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}