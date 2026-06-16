using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public class InfamousFlameProjectile : ModProjectile
    {
        private float pulseCounter = 0f;

        public override string Texture
        {
            get
            {
                return "Synergia/Content/Items/Weapons/Throwing/InfamousFlame";
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 3;
            AIType = ProjectileID.PaladinsHammerFriendly;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 50;
            Projectile.light = 0.6f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            pulseCounter += 0.05f;
            if (pulseCounter > MathHelper.TwoPi)
                pulseCounter -= MathHelper.TwoPi;

            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.1f);
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
            }

            ProjectileTrailEffect();
        }

        private void ProjectileTrailEffect()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 trailPosition = Projectile.position - Projectile.velocity * i * 0.2f;
                int dustIndex = Dust.NewDust(trailPosition, Projectile.width, Projectile.height, DustID.FlameBurst);
                Main.dust[dustIndex].scale = 1.1f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].fadeIn = 1.2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float pulse = 0.5f + (float)(Math.Sin(pulseCounter) * 0.25f);
            float scale = Projectile.scale;

            Color lavaColor1 = new Color(255, 80, 0, 0);   
            Color lavaColor2 = new Color(255, 40, 0, 0);      
            Color lavaColor3 = new Color(255, 150, 0, 0);     

            for (int i = 0; i < 8; i++)
            {
                float angle = i * MathHelper.TwoPi / 8f;
                Vector2 offset = angle.ToRotationVector2() * (3f * (0.5f + pulse * 0.3f));

                Main.EntitySpriteDraw(texture, drawPos + offset, null, lavaColor1 * (0.4f + pulse * 0.2f),
                    Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
            }

            for (int i = 0; i < 8; i++)
            {
                float angle = i * MathHelper.TwoPi / 8f;
                Vector2 offset = angle.ToRotationVector2() * (5f * (0.5f + pulse * 0.3f));

                Main.EntitySpriteDraw(texture, drawPos + offset, null, lavaColor2 * (0.25f + pulse * 0.15f),
                    Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
            }

            for (int i = 0; i < 8; i++)
            {
                float angle = i * MathHelper.TwoPi / 8f;
                Vector2 offset = angle.ToRotationVector2() * (7f * (0.5f + pulse * 0.25f));

                Main.EntitySpriteDraw(texture, drawPos + offset, null, lavaColor3 * (0.15f + pulse * 0.1f),
                    Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, null, lightColor,
                Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240);
        }
    }
}