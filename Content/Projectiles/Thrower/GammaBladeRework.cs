using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewHorizons.Content.Projectiles.Throwing;
using Synergia.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class GammaBladeProjRework : ModProjectile
    {
        private float timer;
        private float acceleration;

        public override string Texture
        {
            get
            {
                return "NewHorizons/Content/Projectiles/Throwing/GammaBladeProj";
            }
        }

        public override void SetDefaults()
        {
            base.Projectile.width = 42;
            base.Projectile.height = 42;
            base.Projectile.friendly = true;
            base.Projectile.penetrate = -1;
            base.Projectile.DamageType = DamageClass.Throwing;
            base.Projectile.extraUpdates = 1;
            base.Projectile.tileCollide = true;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 20;
            base.Projectile.ignoreWater = false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == base.Projectile.owner)
            {
                float theta = (float)Main.rand.NextDouble() * 3.14f * 2f;
                for (int I = 0; I < 1; I++)
                {
                    theta = (float)Main.rand.NextDouble() * 3.14f * 2f;
                    float mag = 240f;
                    Projectile.NewProjectile(Terraria.Entity.InheritSource(base.Projectile), target.Center.X + (float)((int)((double)mag * Math.Cos((double)theta))), target.Center.Y + (float)((int)((double)mag * Math.Sin((double)theta))), -8f * (float)Math.Cos((double)theta), -8f * (float)Math.Sin((double)theta), ModContent.ProjectileType<GammaBladePortal>(), base.Projectile.damage, 0f, Main.myPlayer, 0f, 0f, 0f);
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[base.Projectile.owner];
            base.Projectile.spriteDirection = base.Projectile.direction;
            base.Projectile.rotation += 0.2f * (float)base.Projectile.direction;

            timer += 0.05f;

            if (Main.myPlayer == base.Projectile.owner)
            {
                if (player.channel)
                {
                    float maxDistance = 18f;
                    Vector2 vectorToCursor = Main.MouseWorld - base.Projectile.Center;
                    float distanceToCursor = vectorToCursor.Length();
                    if (distanceToCursor > maxDistance)
                    {
                        distanceToCursor = maxDistance / distanceToCursor;
                        vectorToCursor *= distanceToCursor;
                        this.acceleration = MathHelper.Lerp(this.acceleration, 3f, 0.025f);
                    }
                    int num = (int)(vectorToCursor.X * 1000f);
                    int oldVelocityXBy1000 = (int)(base.Projectile.velocity.X * 1000f);
                    int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
                    int oldVelocityYBy1000 = (int)(base.Projectile.velocity.Y * 1000f);
                    if (num != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
                    {
                        base.Projectile.netUpdate = true;
                    }
                    base.Projectile.velocity = vectorToCursor * this.acceleration;
                    this.acceleration = MathHelper.Lerp(this.acceleration, 0f, 0.025f);
                }
                else
                {
                    base.Projectile.tileCollide = false;
                    Vector2 Vec = Vector2.Normalize(player.Center - base.Projectile.Center) * 20f;
                    base.Projectile.velocity = (base.Projectile.velocity * 10f + Vec) / 11f;
                    if (base.Projectile.Hitbox.Intersects(player.Hitbox) && Main.myPlayer == base.Projectile.owner)
                    {
                        base.Projectile.Kill();
                    }
                }
                if (player.noItems || player.CCed || player.dead || !player.active)
                {
                    base.Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float rot = Projectile.rotation;
            float scale = Projectile.scale;
            Vector2 origin = tex.Size() / 2f;

            float pulse = (float)Math.Sin(timer * 2f) * 0.3f + 0.7f;

            Color outlineColor = new Color(80, 180, 255);
            Color glowColor = new Color(120, 220, 255);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            for (int i = 0; i < 8; i++)
            {
                float offset = 3f + i * 0.8f + pulse * 1.5f;
                float alpha = (0.7f - i * 0.075f) * pulse;
                float rotationOffset = i * 0.2f + timer * 0.5f;

                for (int k = 0; k < 6; k++)
                {
                    Vector2 offsetVec = new Vector2(offset, 0).RotatedBy(k * (MathHelper.Pi / 3f) + rotationOffset);
                    sb.Draw(tex, drawPos + offsetVec, null, outlineColor * alpha,
                        rot, origin, scale * 1.1f, SpriteEffects.None, 0f);
                }
            }

            for (int i = 0; i < 12; i++)
            {
                float offset = 5f + i * 1.2f + pulse * 2f;
                float alpha = (0.4f - i * 0.03f) * pulse * 0.5f;
                float rotationOffset = i * 0.15f + timer * 0.3f;

                for (int k = 0; k < 4; k++)
                {
                    Vector2 offsetVec = new Vector2(offset, 0).RotatedBy(k * MathHelper.PiOver2 + rotationOffset);
                    sb.Draw(tex, drawPos + offsetVec, null, glowColor * alpha,
                        rot, origin, scale * 1.2f, SpriteEffects.None, 0f);
                }
            }

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            sb.Draw(tex, drawPos, null, lightColor, rot, origin, scale, SpriteEffects.None, 0f);

            Color coreGlow = new Color(150, 230, 255) * (0.4f + pulse * 0.3f);
            sb.Draw(tex, drawPos, null, coreGlow, rot, origin, scale * 0.85f, SpriteEffects.None, 0f);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }
    }
}