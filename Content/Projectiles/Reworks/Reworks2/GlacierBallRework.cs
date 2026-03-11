using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class GlacierBallRework : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(14);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 9999;
            Projectile.scale = 1.2f;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;

            DrawOffsetX = -7;
            DrawOriginOffsetY = -7;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 4)
            {
                for (int i = 0; i < Main.rand.Next(3) + 15; i++)
                {
                    int dust = Dust.NewDust(
                        Projectile.position,
                        Projectile.height,
                        Projectile.width,
                        DustID.Frost,
                        Projectile.velocity.X * 0.5f,
                        Projectile.velocity.Y * 0.5f,
                        0,
                        default,
                        1.2f
                    );

                    Main.dust[dust].noGravity = true;
                }
            }

            if (Projectile.ai[0] > 10)
            {
                Projectile.velocity.Y += 0.3f;
                Projectile.velocity.X *= 0.99f;
            }

            Projectile.rotation += Projectile.velocity.Length() * 0.05f * Projectile.direction;

            if (Main.rand.NextBool(3))
            {
                int dust1 = Dust.NewDust(
                    Projectile.position + new Vector2(DrawOffsetX, DrawOriginOffsetY),
                    Projectile.height * 2,
                    Projectile.width * 2,
                    DustID.Frost,
                    0,
                    0,
                    0,
                    default,
                    1.2f
                );

                Main.dust[dust1].noGravity = true;
                Main.dust[dust1].velocity *= 0.5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.frame++;
            Projectile.penetrate--;

            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;

            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;

            Projectile.velocity.Y *= 0.75f;

            SoundEngine.PlaySound(SoundID.Item50, Projectile.position);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GlacierFlash>(),
                0,
                0,
                Projectile.owner
            );

            Mod avalon = ModLoader.GetMod("Avalon");

            if (avalon != null)
            {
                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Main.rand.NextVector2Circular(3, 3) + Projectile.velocity * 0.5f,
                    avalon.Find<ModGore>("GlacierShard" + Main.rand.Next(1, 4)).Type,
                    Main.rand.NextFloat(0.75f, 1f)
                );

                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Main.rand.NextVector2Circular(3, 3) + Projectile.velocity * 0.5f,
                    avalon.Find<ModGore>("GlacierShard" + Main.rand.Next(1, 4)).Type,
                    Main.rand.NextFloat(0.75f, 1f)
                );
            }

            for (int i = 0; i < Main.rand.Next(3) + 5; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.height,
                    Projectile.width,
                    DustID.IceRod,
                    0f,
                    0f,
                    0,
                    default,
                    1.25f
                );

                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].noGravity = true;

                if (Main.rand.NextBool(3))
                    Main.dust[dust].velocity *= 3f;
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            Mod avalon = ModLoader.GetMod("Avalon");

            if (avalon != null)
            {
                int randChunk = Main.rand.Next(1, 4);

                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Projectile.oldVelocity * 0.5f,
                    avalon.Find<ModGore>("GlacierChunk" + randChunk).Type,
                    1f
                );

                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Projectile.oldVelocity * 0.5f,
                    avalon.Find<ModGore>("GlacierChunk" +
                    (randChunk == 3 ? randChunk - Main.rand.Next(1, 3) : randChunk + 1)).Type,
                    1f
                );
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X)
                Projectile.velocity.X = -Projectile.oldVelocity.X;

            if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;

            Projectile.velocity.Y *= 0.75f;

            Projectile.NewProjectile(
                Projectile.GetSource_OnHit(target),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GlacierFlash>(),
                0,
                0,
                Projectile.owner
            );

            Mod avalon = ModLoader.GetMod("Avalon");

            if (avalon != null)
            {
                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Main.rand.NextVector2Circular(3, 3) + Projectile.velocity * 0.5f,
                    avalon.Find<ModGore>("GlacierShard" + Main.rand.Next(1, 4)).Type,
                    Main.rand.NextFloat(1f, 1.5f)
                );

                Gore.NewGore(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Main.rand.NextVector2Circular(3, 3) + Projectile.velocity * 0.5f,
                    avalon.Find<ModGore>("GlacierShard" + Main.rand.Next(1, 4)).Type,
                    Main.rand.NextFloat(1f, 1.5f)
                );
            }

            Projectile.frame++;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.ai[0] > 3)
                return new Color(255, 255, 255, 200);
            else
                return new Color(0, 0, 0, 0);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int size = 10;
            hitbox.X -= size;
            hitbox.Y -= size;
            hitbox.Width += size * 2;
            hitbox.Height += size * 2;
        }
    }

    public class GlacierFlash : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/Star";

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.timeLeft = 8;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            Projectile.scale += 0.15f;
            Projectile.alpha += 30;

            Lighting.AddLight(Projectile.Center, 0.2f, 0.45f, 0.9f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            Color c = new Color(120, 210, 255, 0) * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                c,
                0f,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}