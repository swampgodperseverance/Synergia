using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.Summon.Sentries;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Summon
{
    // Token: 0x0200005E RID: 94
    public class TurretBronze : ValhallaSentryTower
    {
        // Token: 0x060001D4 RID: 468 RVA: 0x0001529B File Offset: 0x0001349B
        public override void SetStaticDefaults()
        {
            Main.projFrames[base.Projectile.type] = 6;
            ProjectileID.Sets.MinionTargettingFeature[base.Projectile.type] = true;
        }

        // Token: 0x060001D5 RID: 469 RVA: 0x00015770 File Offset: 0x00013970
        public override void SetDefaults()
        {
            base.Projectile.netImportant = true;
            base.Projectile.width = 60;
            base.Projectile.height = 30;
            base.Projectile.penetrate = -1;
            base.Projectile.ignoreWater = true;
            base.Projectile.sentry = true;
            base.Projectile.aiStyle = 0;
            base.Projectile.timeLeft = 36000;
            base.Projectile.DamageType = DamageClass.Summon;
            this.angleLimit = 0f;
            this.angleShootPermission = 0.1f;
            this.shootHorizontal = true;
            this.collisionHeight = 38;
            this.collisionWidth = 30;
            this.textureExtraHeight = 25;
            this.overWidth = 10;
            this.overHeight = 0;
            this.shootCooldown = 20f;
            this.shootTimerReset = -30f;
            this.shootSpeed = 5.5f;
            this.bulletType = ModContent.ProjectileType<TurretStone>();
            this.soundFire = new SoundStyle?(SoundID.Item11);
            this.bulletFireSize = new Vector2(0f, 0f);
        }

        // Token: 0x060001D6 RID: 470 RVA: 0x00002050 File Offset: 0x00000250
        public override void Behavior()
        {
        }

        // Token: 0x060001D7 RID: 471 RVA: 0x00015888 File Offset: 0x00013A88
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects direction = SpriteEffects.None;

            Asset<Texture2D> textureExtra = ModContent.Request<Texture2D>(
                base.Texture + "_Extra",
                AssetRequestMode.ImmediateLoad);

            Main.spriteBatch.Draw(
                textureExtra.Value,
                new Vector2(
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
                    Projectile.position.Y - Main.screenPosition.Y + textureExtraHeight),
                null,
                lightColor,
                0f,
                textureExtra.Size() * 0.5f,
                Projectile.scale,
                direction,
                0f);

            int headFrame = 0;

            if (Math.Abs(Projectile.rotation) < 0.5236f)
            {
                if (ShootTimer < -20f)
                    headFrame = 5;
                else if (ShootTimer < -10f)
                    headFrame = 4;
                else if (ShootTimer < 0f)
                    headFrame = 3;
                else
                    headFrame = 0;
            }
            else if (Math.Abs(Projectile.rotation) < 1.0472f)
                headFrame = 1;
            else if (Math.Abs(Projectile.rotation) < 1.5708f)
                headFrame = 2;

            Asset<Texture2D> texture = ModContent.Request<Texture2D>(
                base.Texture,
                AssetRequestMode.ImmediateLoad);

            int frameHeight = texture.Height() / Main.projFrames[Projectile.type];

            Main.spriteBatch.Draw(
                texture.Value,
                Projectile.Center - Main.screenPosition,
                new Rectangle(0, frameHeight * headFrame, texture.Width(), frameHeight),
                lightColor,
                0f,
                new Vector2(texture.Width() / 2f, frameHeight / 2f),
                Projectile.scale,
                Projectile.spriteDirection == 1
                    ? SpriteEffects.None
                    : SpriteEffects.FlipHorizontally,
                0f);

            return false;
        }

        // Token: 0x060001D8 RID: 472 RVA: 0x000107EB File Offset: 0x0000E9EB
        public override void OnKill(int timeLeft)
        {
            base.Projectile.alpha = 255;
        }
    }
}
