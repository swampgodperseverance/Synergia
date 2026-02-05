using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using NewHorizons.Content.Projectiles.Ranged;
using NewHorizons.Content.Items.Weapons.Throwing;

namespace Synergia.Content.Projectiles.Thrower
{
    public class CrystalDagger2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            base.Projectile.aiStyle = 113;
            base.AIType = 48;
            base.Projectile.width = 18;
            base.Projectile.height = 18;
            base.Projectile.friendly = true;
            base.Projectile.timeLeft = 30;
            base.Projectile.penetrate = 2;
            base.Projectile.DamageType = DamageClass.Throwing;
            base.Projectile.extraUpdates = 1;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = -1;
        }

        // Token: 0x060000AA RID: 170 RVA: 0x00005F20 File Offset: 0x00004120
        public override void AI()
        {
            base.Projectile.rotation = base.Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            base.Projectile.spriteDirection = base.Projectile.direction;
        }

        // Token: 0x060000AB RID: 171 RVA: 0x00006E34 File Offset: 0x00005034
        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(10))
            {
                Item.NewItem(base.Projectile.GetSource_DropAsItem(null), (int)base.Projectile.position.X, (int)base.Projectile.position.Y, base.Projectile.width, base.Projectile.height, ModContent.ItemType<CrystalDagger>(), 1, false, 0, false, false);
                return;
            }
            SoundEngine.PlaySound(SoundID.Item27, new Vector2?(base.Projectile.position), null);
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, new Vector2?(base.Projectile.position), null);
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(base.Projectile.position + base.Projectile.velocity, base.Projectile.width, base.Projectile.height, 70, base.Projectile.oldVelocity.X * -0.3f, base.Projectile.oldVelocity.Y * -0.3f, 0, default(Color), 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.5f;
            }
            int RDust = Main.rand.Next(3, 8);
            for (int j = 0; j < RDust; j++)
            {
                int num = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 70, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].velocity *= 2f;
                Main.dust[num].scale = 1f;
                Main.dust[num].noGravity = false;
            }
            int RProj = Main.rand.Next(2, 4);
            for (int p = 0; p < RProj; p++)
            {
                Vector2 targetDir = ((float)Main.rand.Next(-180, 180)).ToRotationVector2();
                targetDir.Normalize();
                targetDir *= 3f;
                Projectile.NewProjectile(Terraria.Entity.InheritSource(base.Projectile), base.Projectile.Center, targetDir, 94, base.Projectile.damage / 2, base.Projectile.knockBack, base.Projectile.owner, 0f, 0f, 0f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new(texture.Width / 2, texture.Height / 2);
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                Color trailColor = lightColor * 0.4f * fade;
                float scale = Projectile.scale * (0.9f + 0.1f * fade);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.oldRot[i],
                    origin,
                    scale,
                    effects,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }
    }
}