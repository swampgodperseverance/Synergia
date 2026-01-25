using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using NewHorizons.Globals;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Thrower
{
    public class AdamantiteDagger2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            int frameHeight = texture.Height / 3;
            Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            Vector2 origin = frame.Size() / 2f;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor,
                Projectile.rotation, origin, Projectile.scale, effects, 0);

            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, frame, Color.White,
                Projectile.rotation, origin, Projectile.scale, effects, 0);

            return false;
        }

        public override void AI()
        {

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Vector2 targetPos = Vector2.Zero;
            float distance = 600f;
            bool hasTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float d = Vector2.Distance(npc.Center, Projectile.Center);
                    if (d < distance)
                    {
                        distance = d;
                        targetPos = npc.Center;
                        hasTarget = true;
                    }
                }
            }

            if (hasTarget)
            {
                Projectile.Move(targetPos, Projectile.timeLeft > 50 ? 30f : 50f, 50f);
            }

            if (Main.myPlayer == Projectile.owner && Main.rand.NextBool(120))
            {
                int count = Main.rand.Next(2, 4);

                for (int i = 0; i < count; i++)
                {
                    float xOffset = Main.rand.NextFloat(-40f, 40f);
                    float speed = Main.rand.NextFloat(2.5f, 4.2f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center + new Vector2(xOffset, -10f),
                        new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), speed),
                        ModContent.ProjectileType<AdamantiteRocket>(),
                        Projectile.damage / 2,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            for (int i = 0; i < 15; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Adamantite, 0f, 0f, 0, default, 1.4f);

                Main.dust[d].velocity *= 4f;
                Main.dust[d].noGravity = true;
            }
        }
    }


    public class AdamantiteRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
            }

            Projectile.velocity.Y += 0.25f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 25; i++)
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Adamantite,
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f),
                    0,
                    default,
                    1.6f
                );

                Main.dust[d].noGravity = true;
            }

            Projectile.Damage();
        }
    }
}
