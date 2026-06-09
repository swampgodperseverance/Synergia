using Consolaria.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class GalvornBladeRework : ModProjectile
    {
        public override void SetStaticDefaults()
           => Main.projFrames[Type] = 4;

        public override void SetDefaults()
        {
            int width = 16;
            int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 190;

            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.noEnchantmentVisuals = true;

            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.localAI[0]++;
            float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1];
            float scaleMulti = 0.9f;
            float scaleAdder = 1.3f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = scaleAdder + percentageOfLife * scaleMulti;

            float offset = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 position = Projectile.Center + offset.ToRotationVector2() * 48f * Projectile.scale;
            Vector2 velocity = (offset + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();

            if (Main.rand.NextFloat() < Projectile.Opacity)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset.ToRotationVector2() * (Main.rand.NextFloat() * 45f * Projectile.scale + 15f * Projectile.scale), DustID.Shadowflame, velocity * 1f, 50, new Color(20, 20, 20), 0.5f);
                dust.fadeIn = 0.3f + Main.rand.NextFloat() * 0.15f;
                dust.noGravity = true;
            }

            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
            {
                Dust dust2 = Dust.NewDustPerfect(position, DustID.Shadowflame, velocity * 1.5f, 100, new Color(15, 15, 15) * Projectile.Opacity, Projectile.Opacity);
                dust2.noGravity = true;
            }

            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextFloat() < 0.3f)
                {
                    Dust blackDust = Dust.NewDustDirect(
                        Projectile.Center + Main.rand.NextVector2Circular(40f * Projectile.scale, 40f * Projectile.scale),
                        0, 0,
                        DustID.Shadowflame,
                        Main.rand.NextFloat(-2.5f, 2.5f),
                        Main.rand.NextFloat(-2.5f, 2.5f),
                        80,
                        new Color(30, 30, 30),
                        0.9f
                    );
                    blackDust.noGravity = true;
                    blackDust.fadeIn = 0.2f;
                }
            }

            if (Projectile.localAI[0] >= Projectile.ai[1])
                Projectile.Kill();

            Projectile.scale *= Projectile.ai[2];

            for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2)
            {
                Rectangle rectangle = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 40f * Projectile.scale, new Vector2(35f * Projectile.scale, 35f * Projectile.scale));
                Projectile.EmitEnchantmentVisualsAt(rectangle.TopLeft(), rectangle.Width, rectangle.Height);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust blackDust = Dust.NewDustDirect(
                    target.Center,
                    20, 20,
                    DustID.Shadowflame,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100,
                    new Color(20, 20, 20),
                    1.3f
                );
                blackDust.noGravity = true;
            }

            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust blackDust = Dust.NewDustDirect(
                    target.Center,
                    20, 20,
                    DustID.Shadowflame,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100,
                    new Color(20, 20, 20),
                    1.3f
                );
                blackDust.noGravity = true;
            }

            info.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }

        private void DrawLikeExcalibur(SpriteBatch spriteBatch)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type]);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float projectileScale = Projectile.scale * 0.65f;
            SpriteEffects effects = (!(Projectile.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1];
            float lerpTime = Utils.Remap(percentageOfLife, 0f, 0.5f, 0f, 1f) * Utils.Remap(percentageOfLife, 0.5f, 1f, 1f, 0f);
            float lightningValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            lightningValue = Utils.Remap(lightningValue, 0.2f, 1f, 0f, 1f);

            Color value = Color.Lerp(new Color(30, 30, 30, 220), new Color(60, 60, 60, 220), lerpTime);
            spriteBatch.Draw(texture, position, sourceRectangle, value * lightningValue * lerpTime, Projectile.rotation + Projectile.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - percentageOfLife), origin, projectileScale, effects, 0f);

            Color value2 = Color.Lerp(new Color(40, 40, 40, 220), new Color(80, 80, 80, 220), lerpTime) * 1.25f;
            Color color = Color.Lerp(new Color(30, 30, 30, 220), new Color(90, 90, 90, 220), lerpTime) * 1.25f;

            Color value3 = Color.White * lerpTime * 0.4f;
            value3.A = (byte)(value3.A * (1f - lightningValue));
            Color value4 = value3 * lightningValue * 0.4f;

            spriteBatch.Draw(texture, position, sourceRectangle, value4 * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, projectileScale, effects, 0f);
            spriteBatch.Draw(texture, position, sourceRectangle, color * lightningValue * lerpTime * 0.3f, Projectile.rotation, origin, projectileScale, effects, 0f);
            spriteBatch.Draw(texture, position, sourceRectangle, value2 * lightningValue * lerpTime * 0.5f, Projectile.rotation, origin, projectileScale * 0.975f, effects, 0f);
            spriteBatch.Draw(texture, position, texture.Frame(1, 4, 0, 3), new Color(50, 50, 50) * 0.7f * lerpTime, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, projectileScale, effects, 0f);
            spriteBatch.Draw(texture, position, texture.Frame(1, 4, 0, 3), new Color(40, 40, 40) * 0.6f * lerpTime, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, projectileScale * 0.8f, effects, 0f);
            spriteBatch.Draw(texture, position, texture.Frame(1, 4, 0, 3), new Color(30, 30, 30) * 0.5f * lerpTime, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, projectileScale * 0.6f, effects, 0f);

            float scaleFactor = projectileScale * 0.85f;
            for (float i = 0f; i < 12f; i += 1f)
            {
                float edgeRotation = Projectile.rotation + Projectile.ai[0] * i * ((float)Math.PI * -2f) * 0.025f + Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 0.9f) * Projectile.ai[0];
                Vector2 drawPos = position + edgeRotation.ToRotationVector2() * (texture.Width * 0.55f - 6f) * projectileScale;
                float scale = i / 12f;
                DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos, Color.Lerp(new Color(80, 80, 80, 0), new Color(50, 50, 50, 0), percentageOfLife) * lerpTime * 2f * scale, color, percentageOfLife, 0f, 0.5f, 0.5f, 1f, edgeRotation, new Vector2(0f, Utils.Remap(percentageOfLife, 0f, 1f, 3f, 0f)) * scaleFactor, Vector2.One * scaleFactor);
            }

            Vector2 drawpos2 = position + (Projectile.rotation + Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 0.9f) * Projectile.ai[0]).ToRotationVector2() * (texture.Width * 0.55f - 4f) * projectileScale;
            DrawHelper.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos2, Color.Lerp(new Color(80, 80, 80, 0), new Color(50, 50, 50, 0), percentageOfLife) * lerpTime * 1.5f, color, percentageOfLife, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(percentageOfLife, 0f, 1f, 4f, 1f)) * scaleFactor, Vector2.One * scaleFactor);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            DrawLikeExcalibur(spriteBatch);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust blackDust = Dust.NewDustDirect(
                    Projectile.Center,
                    30, 30,
                    DustID.Shadowflame,
                    Main.rand.NextFloat(-6f, 6f),
                    Main.rand.NextFloat(-6f, 6f),
                    100,
                    new Color(15, 15, 15),
                    1.5f
                );
                blackDust.noGravity = true;
            }
        }
    }
}