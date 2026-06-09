using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ValhallaMod.Projectiles.Magic.Spells
{
    public class GoldenBombRework : ModProjectile
    {
        private bool hasGlowEffect = false;
        private int glowTimer = 0;

        public override string Texture
        {
            get
            {
                return "ValhallaMod/Projectiles/Magic/Spells/GoldenBomb";
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -10;
        }

        public override void AI()
        {
            if (Projectile.timeLeft <= 30 && !hasGlowEffect)
            {
                hasGlowEffect = true;

                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        DustID.GoldCoin, 0f, 0f, 100, Color.Gold, 1.5f);
                    dust.velocity = Vector2.Normalize(new Vector2(Main.rand.NextFloat(-1f, 1f),
                        Main.rand.NextFloat(-1f, 1f))) * Main.rand.NextFloat(2f, 5f);
                    dust.noGravity = true;
                }
            }
            if (hasGlowEffect)
            {
                glowTimer++;
                if (glowTimer % 3 == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                            DustID.GoldFlame, 0f, 0f, 50, Color.Goldenrod, 1.2f);
                        dust.velocity = Projectile.velocity * 0.5f + new Vector2(Main.rand.NextFloat(-2f, 2f),
                            Main.rand.NextFloat(-2f, 2f));
                        dust.noGravity = true;
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Эффект взрыва с золотым свечением
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.position);

            // Золотая вспышка
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.GoldCoin, 0f, 0f, 150, Color.Gold, 2f);
                dust.velocity = new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f));
                dust.noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.YellowTorch, 0f, 0f, 100, Color.Orange, 1.8f);
                dust.velocity = new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
                dust.noGravity = true;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                int num = Main.rand.Next(10, 15);
                for (int i = 0; i < num; i++)
                {
                    Vector2 vector = new Vector2(Main.rand.Next(100, 201) * 0.1f, 0f);
                    vector = vector.RotatedBy((float)i * 6.2831855f / (float)num);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, vector,
                        ModContent.ProjectileType<GoldenBombCoin>(), (int)(Projectile.damage * 0.5f),
                        Projectile.knockBack, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Color outlineColor = Color.Gold;
            float outlineSize = 2f;
            if (hasGlowEffect)
            {
                outlineSize = 3f + (float)Math.Sin(glowTimer * 0.3f) * 0.5f;
                outlineColor = Color.Orange;
            }
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        Main.spriteBatch.Draw(texture, drawPosition + new Vector2(x, y) * outlineSize,
                            null, outlineColor, Projectile.rotation, drawOrigin, Projectile.scale,
                            spriteEffects, 0f);
                    }
                }
            }

            Main.spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation,
                drawOrigin, Projectile.scale, spriteEffects, 0f);
            if (hasGlowEffect)
            {
                float alpha = 0.5f + (float)Math.Sin(glowTimer * 0.5f) * 0.3f;
                Main.spriteBatch.Draw(texture, drawPosition, null, Color.Gold * alpha,
                    Projectile.rotation, drawOrigin, Projectile.scale * 1.1f, spriteEffects, 0f);
            }

            return false; 
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                    DustID.GoldCoin, 0f, 0f, 80, Color.Gold, 1.2f);
                dust.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
            }
        }
    }
}