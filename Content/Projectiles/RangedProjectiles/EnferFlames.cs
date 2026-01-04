using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class EnferFlames : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4; 
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60; 
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.light = 0.6f;
            Projectile.alpha = 50;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.velocity *= 0.96f;
            Projectile.velocity.Y -= 0.02f; 
            Lighting.AddLight(Projectile.Center, 1.1f, 0.4f, 0.1f);
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.4f);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 0.9f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 120);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position); 

            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.6f;
                Main.dust[dust].scale *= 1.2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle source = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(
                texture,
                drawPos,
                source,
                lightColor,
                Projectile.rotation,
                new Vector2(texture.Width / 2f, frameHeight / 2f),
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}
