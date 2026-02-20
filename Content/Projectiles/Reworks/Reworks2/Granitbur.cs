using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class GranitburRework : ModProjectile
    {
        private float scaleMultiplier;
        private float startPoint;
        private bool spawnedProjectile = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 2;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if (startPoint == 0f)
            {
                startPoint = Projectile.ai[1];
            }

            float swingTime = 0f;
            Player player = Main.player[Projectile.owner];
            scaleMultiplier = Projectile.scale * player.GetAdjustedItemScale(player.HeldItem);

            if (Projectile.ai[1] <= 0f || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = (int)Projectile.ai[1];
            player.itemTime = (int)Projectile.ai[1];
            player.itemAnimation = (int)Projectile.ai[1];
            player.itemLocation = Vector2.Zero;

            swingTime = Projectile.ai[1] / startPoint;
            for (int e = 0; e < 3; e++)
            {
                swingTime = MathHelper.SmoothStep(0f, 1f, swingTime);
            }

            if (Projectile.ai[1] > startPoint)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
                }
                swingTime = 1f;
            }

            if (Projectile.ai[1] < startPoint * 0.5f && !spawnedProjectile)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 velDir = Vector2.Normalize(Projectile.velocity);
                    float rotOffset = MathHelper.PiOver4 * (Projectile.ai[0] > 0f ? -1.2f : 1.2f) * player.direction;
                    float velRotOffset = MathHelper.PiOver4 * (Projectile.ai[0] > 0f ? -0.2f : 0.2f) * player.direction;

                    int p = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        player.MountedCenter + velDir * 8f + velDir.RotatedBy(rotOffset) * 32f,
                        velDir.RotatedBy(velRotOffset) * 8f,
                        ModContent.ProjectileType<GranitburProjectile>(),
                        Projectile.damage / 2,
                        Projectile.knockBack,
                        player.whoAmI
                    );
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
                }
                spawnedProjectile = true;
            }

            player.heldProj = Projectile.whoAmI;
            player.compositeFrontArm.enabled = true;

            if (Projectile.velocity.X != 0f)
            {
                player.ChangeDir(Projectile.velocity.X > 0f ? 1 : -1);
            }

            float num = Projectile.ai[0];
            float swing, armSwing;

            Vector2 mouse = Main.MouseWorld;
            bool attackDownwards = mouse.Y > player.Center.Y + 20f;

            if (num != 0f && num == 1f)
            {
                swing = MathHelper.Lerp(MathHelper.ToRadians(135f) * -player.direction, MathHelper.ToRadians(135f) * player.direction, swingTime) + Projectile.velocity.ToRotation();
                armSwing = MathHelper.Lerp(1.5707964f * -player.direction, 1.5707964f * player.direction, swingTime) + Projectile.velocity.ToRotation();

                if (attackDownwards)
                {
                    Projectile.spriteDirection = player.direction;
                }
                else
                {
                    Projectile.spriteDirection = -player.direction;
                }
            }
            else
            {
                swing = MathHelper.Lerp(MathHelper.ToRadians(135f) * player.direction, MathHelper.ToRadians(135f) * -player.direction, swingTime) + Projectile.velocity.ToRotation();
                armSwing = MathHelper.Lerp(1.5707964f * player.direction, 1.5707964f * -player.direction, swingTime) + Projectile.velocity.ToRotation();

                if (attackDownwards)
                {
                    Projectile.spriteDirection = -player.direction;
                }
                else
                {
                    Projectile.spriteDirection = player.direction;
                }
            }

            Projectile.Center = swing.ToRotationVector2() * 30f * scaleMultiplier;
            Projectile.rotation = swing;

            player.compositeFrontArm.rotation = armSwing - MathHelper.PiOver2 - (player.gravDir - 1f) * MathHelper.PiOver2;

            if (scaleMultiplier > 1.125f)
                player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Full;
            else if (scaleMultiplier > 1.0625f)
                player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
            else
                player.compositeFrontArm.stretch = Player.CompositeArmStretchAmount.Quarter;

            Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation) + Projectile.Center;

            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] -= 1f;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(startPoint);
            writer.Write(scaleMultiplier);
            writer.Write(Projectile.direction);
            writer.Write(spawnedProjectile);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            startPoint = reader.ReadSingle();
            scaleMultiplier = reader.ReadSingle();
            Projectile.direction = reader.ReadInt32();
            spawnedProjectile = reader.ReadBoolean();
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Vector2 dir = Vector2.Normalize(Projectile.velocity);
            hitbox = new Rectangle(
                (int)(Projectile.Center.X + dir.X * 4f - Projectile.width / 2f * scaleMultiplier),
                (int)(Projectile.Center.Y + dir.Y * 4f - Projectile.height / 2f * scaleMultiplier),
                (int)(Projectile.width * scaleMultiplier),
                (int)(Projectile.height * scaleMultiplier)
            );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var mainTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/GranitburRework", AssetRequestMode.ImmediateLoad).Value;
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(
                mainTex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection,
                mainTex.Size() / 2f,
                scaleMultiplier,
                effects,
                0
            );

            var glowTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/GranitburGlow", AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(
                glowTex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White * 0.6f,
                Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection,
                glowTex.Size() / 2f,
                scaleMultiplier,
                effects,
                0
            );

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            float progress = Projectile.ai[1] / startPoint;
            if (progress >= 0.5f)
                progress = MathHelper.Lerp(1f, 0f, progress);
            else
                progress *= 2f;

            for (int i = 0; i < 2; i++)
                progress *= progress;

            float trailOpacity = MathF.Sin(progress * MathHelper.Pi) - 0.1f;
            if (trailOpacity <= 0f) return;

            Color trailColor = Color.Lerp(new Color(100, 140, 180), new Color(60, 100, 140), trailOpacity);
            var trailTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/GraniteTrail", AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(
                trailTex,
                player.MountedCenter - new Vector2(2f, 4f) * player.direction - Main.screenPosition,
                null,
                trailColor * trailOpacity * 0.7f,
                Projectile.rotation - (MathHelper.Pi / 8f) * Projectile.spriteDirection,
                trailTex.Size() * 0.5f,
                scaleMultiplier * 1.5f,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );
        }

        public override bool ShouldUpdatePosition() => false;
    }
}