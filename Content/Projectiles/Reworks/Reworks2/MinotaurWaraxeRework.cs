using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;                      // ← Add this for AssetRequestMode
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class MinotaursWaraxeRework : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Type] = 2;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 68;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 2;
            Projectile.extraUpdates = 2;
            Projectile.netImportant = true;
            Projectile.manualDirectionChange = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        private float scaleMultiplier;
        private float startPoint;

        public override void AI()
        {
            if (startPoint == 0f)
            {
                Projectile.ai[1] *= Projectile.MaxUpdates;
                startPoint = Projectile.ai[1];
            }

            float swingTime = 0f;
            Player player = Main.player[Projectile.owner];
            scaleMultiplier = Projectile.scale * player.GetAdjustedItemScale(player.HeldItem);

            if (Projectile.ai[1] <= 0f || player.dead)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.timeLeft = (int)Projectile.ai[1];
                player.itemTime = (int)Projectile.ai[1];
                player.itemAnimation = (int)Projectile.ai[1];

                if (Projectile.ai[1] > startPoint * 0.5f)
                {
                    swingTime = MathHelper.SmoothStep(0f, 2f, Projectile.ai[1] / startPoint) - 1f;
                }
                else
                {
                    swingTime = MathHelper.SmoothStep(0f, 1f,
                        MathHelper.SmoothStep(0f, 1f,
                        MathHelper.SmoothStep(0f, 1f,
                        MathHelper.SmoothStep(0f, 1f,
                        MathHelper.SmoothStep(0f, 1f, Projectile.ai[1] * 2f / startPoint)))));
                }

                if (Projectile.ai[1] > startPoint)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
                    }
                    swingTime = 1f;
                }

                player.heldProj = Projectile.whoAmI;
                player.compositeFrontArm.enabled = true;

                if (Projectile.velocity.X != 0f)
                {
                    player.ChangeDir(Projectile.velocity.X > 0f ? 1 : -1);
                }
            }

            float swing = MathHelper.Lerp(0f, -2.3561945f * player.direction, swingTime) + Projectile.velocity.ToRotation();
            float armSwing = MathHelper.Lerp(0.7853982f * player.direction, 1.5707964f * -player.direction, swingTime) + Projectile.velocity.ToRotation();

            if (Projectile.ai[1] > startPoint * 0.5f)
            {
                swing = MathHelper.Lerp(-2.3561945f * player.direction, 1.5707964f * -player.direction, swingTime) + Projectile.velocity.ToRotation();
                armSwing = MathHelper.Lerp(1.5707964f * -player.direction, 0.7853982f * -player.direction, swingTime) + Projectile.velocity.ToRotation();
            }

            Projectile.Center = player.MountedCenter
                - new Vector2(2f, 4f) * player.direction   // assuming Directions is typo / old name → use direction
                + armSwing.ToRotationVector2() * 12f
                + swing.ToRotationVector2() * 28f * scaleMultiplier;

            Projectile.rotation = swing;
            player.compositeFrontArm.rotation = armSwing - MathHelper.PiOver2 - (player.gravDir - 1f) * MathHelper.PiOver2;
            player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Full;

            Projectile.spriteDirection = player.direction;

            // ────────────────────────────────────────────────────────────────
            // Ground slam / flash logic (unchanged except syntax)
            // ────────────────────────────────────────────────────────────────

            if (Collision.SolidCollision(
                Projectile.Center + Projectile.rotation.ToRotationVector2() * scaleMultiplier * 24f - new Vector2(16f) * scaleMultiplier,
                (int)(32f * scaleMultiplier), (int)(32f * scaleMultiplier))
                && Projectile.ai[1] < startPoint * 0.5f
                && Projectile.ai[0] == 0f)
            {
                if (Math.Abs(Projectile.oldRot[0] - Projectile.oldRot[1]) < MathHelper.ToRadians(1f))
                {
                    Projectile.ai[0] += 1f;
                }

                if (Main.myPlayer == Projectile.owner)
                {
                    int p = Projectile.NewProjectile(
                        Entity.GetSource_FromThis(),
                        Projectile.Center + (Projectile.rotation - 0.7853982f * Projectile.direction).ToRotationVector2() * scaleMultiplier * 24f,
                        Vector2.Zero,
                        ModContent.ProjectileType<GlacierFlash>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner,
                        (6f + Projectile.ai[0] * 4f) * scaleMultiplier);

                    NetMessage.SendData(MessageID.SyncProjectile, number: p);
                }

                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.UnitY, 8f, 12f, 60, 160f, "Ground Pound"));
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }

            if (player.channel ? Projectile.ai[1] != startPoint * 0.5f : Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] -= 1f;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(startPoint);
            writer.Write(scaleMultiplier);
            writer.Write(Projectile.direction);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            startPoint = reader.ReadSingle();
            scaleMultiplier = reader.ReadSingle();
            Projectile.direction = reader.ReadInt32();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] < startPoint * 0.5f)
            {
                if (Math.Abs(Projectile.oldRot[0] - Projectile.oldRot[1]) < MathHelper.ToRadians(1f))
                {
                    Projectile.ai[0] += 1f;
                    NetMessage.SendData(MessageID.SyncProjectile, number: Projectile.whoAmI);
                }

                if (Main.myPlayer == Projectile.owner)
                {
                    int p = Projectile.NewProjectile(
                        Entity.GetSource_FromThis(),
                        Projectile.Center + (Projectile.rotation - 0.7853982f * Projectile.direction).ToRotationVector2() * scaleMultiplier * 24f,
                        Vector2.Zero,
                        ModContent.ProjectileType<GlacierFlash>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner,
                        (3f + Projectile.ai[0] * 2f) * scaleMultiplier);

                    NetMessage.SendData(MessageID.SyncProjectile, number: p);
                }

                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.UnitY, 8f, 12f, 60, 160f, "Ground Pound"));
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Vector2 dir = Vector2.Normalize(Projectile.velocity);
            hitbox = new Rectangle(
                (int)(Projectile.Center.X + dir.X * 4f - Projectile.width / 2f * scaleMultiplier),
                (int)(Projectile.Center.Y + dir.Y * 4f - Projectile.height / 2f * scaleMultiplier),
                (int)(Projectile.width * scaleMultiplier),
                (int)(Projectile.height * scaleMultiplier));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Trail / afterimage effect (Extra_98 is usually the generic afterimage texture)
            Texture2D texTrail = ModContent.Request<Texture2D>("Terraria/Images/Extra_98", AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(
                texTrail,
                Projectile.Center
                    + new Vector2(4f, 25 * -Projectile.spriteDirection)
                        .RotatedBy(Projectile.rotation + 0.7853982f * Projectile.spriteDirection)
                    * scaleMultiplier
                    - Main.screenPosition,
                new Rectangle(0, texTrail.Height / 2, texTrail.Width, texTrail.Height / 2),
                new Color(255, 0, 0, 0),
                Projectile.rotation + MathHelper.PiOver2 * (Projectile.spriteDirection + 1),
                new Vector2(texTrail.Width / 2f, 0f),
                scaleMultiplier * new Vector2(0.5f, Math.Abs(Projectile.oldRot[0] - Projectile.oldRot[1]) * 12f),
                SpriteEffects.None, 0);

            // Main texture
            Texture2D texMain = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(
                texMain,
                Projectile.Center - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation + 0.7853982f * Projectile.spriteDirection,
                texMain.Size() / 2f,
                scaleMultiplier,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0);

            // Glowmask (assuming you have a GlowTexture field/property defined somewhere)
            Texture2D texGlow = ModContent.Request<Texture2D>(GlowTexture, AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(
                texGlow,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation + 0.7853982f * Projectile.spriteDirection,
                texGlow.Size() / 2f,
                scaleMultiplier,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0);

            return false;
        }

        public override bool? CanDamage() => Projectile.ai[1] < startPoint * 0.5f;

        public override bool ShouldUpdatePosition() => false;
    }
}