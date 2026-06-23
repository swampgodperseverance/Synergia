using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroSkull : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Main.projFrames[Projectile.type] = 13;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.Center = new Vector2(player.Center.X - 3f, player.Center.Y - 60f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
                Projectile.frame++;

            if (Projectile.frame >= 13)
            {
                int count = 5;
                float spacing = 40f;
                Vector2 startPos = Projectile.Center + new Vector2(-(count - 1) * spacing / 2f, 750f);

                for (int i = 0; i < count; i++)
                {
                    Vector2 spawnPos = startPos + new Vector2(i * spacing, 0f);
                    Vector2 velocity = new Vector2(0f, -14f);

                    int knife = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<NecroKnife>(),
                        20,
                        0f,
                        Projectile.owner
                    );

                    Main.projectile[knife].ai[0] = 1;
                }

                for (int d = 0; d < 25; d++)
                {
                    int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.PurpleTorch,
                        Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                        150, new Color(200, 100, 255), 1.5f);
                    Main.dust[dust].noGravity = true;
                }

                SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 0.2f }, Projectile.Center);
                Projectile.Kill();
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.2f, 0.8f));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, 13, 0, Projectile.frame);
            Vector2 origin = frame.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color outlineColor = new Color(80, 20, 120, 60);

            for (int i = 3; i >= 1; i--)
            {
                Color layerColor = outlineColor * (0.4f / i);
                for (int x = -i; x <= i; x++)
                {
                    for (int y = -i; y <= i; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (Math.Abs(x) == i || Math.Abs(y) == i)
                        {
                            Vector2 offset = new Vector2(x, y);
                            Main.EntitySpriteDraw(
                                texture,
                                drawPos + offset,
                                frame,
                                layerColor,
                                Projectile.rotation,
                                origin,
                                Projectile.scale,
                                SpriteEffects.None,
                                0
                            );
                        }
                    }
                }
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                frame,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}