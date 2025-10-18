using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Dusts;
using Terraria.Audio;
using System;

namespace Synergia.Content.Projectiles.Friendly
{
    public class JadeGlaiveProj : ModProjectile
    {
        private bool hasDashed = false;
        private NPC target = null;
        public static readonly SoundStyle Impulse = new("Synergia/Assets/Sounds/Impulse");

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 260;
            Projectile.height = 40;
            Projectile.width = 40;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // JadeShardFriendly
            if (owner.whoAmI == Main.myPlayer && Main.mouseRight && Projectile.timeLeft > 10)
            {
                CreateExplosionDust();
                SoundEngine.PlaySound(Impulse, Projectile.Center);

                var s = Projectile.GetSource_FromThis();
                Projectile.NewProjectile(
                    s,
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<JadeShardFriendly>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                Projectile.Kill();
                return;
            }

            if (Projectile.ai[0] == 0)
            {
                float baseRotSpeed = 0.15f;
                float dynamicRot = MathHelper.Lerp(0.05f, 0.4f, 1f - Projectile.velocity.Length() / 10f);
                Projectile.rotation += baseRotSpeed + dynamicRot;
                Projectile.velocity *= 0.97f;

                if (Projectile.timeLeft == 200)
                {
                    target = FindNearestEnemy(Projectile.Center, 700f);
                    if (target != null)
                    {
                        hasDashed = true;
                        Vector2 dashDir = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                        Projectile.velocity = dashDir * 25f;

                        for (int i = 0; i < 20; i++)
                        {
                            Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                            d.noGravity = true;
                            d.velocity = dashDir.RotatedByRandom(0.5f) * Main.rand.NextFloat(2f, 6f);
                            d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                        }

                        SoundEngine.PlaySound(SoundID.Item74 with { Pitch = 0.5f, Volume = 1f }, Projectile.Center);
                    }
                }

                if (hasDashed)
                {
                    Projectile.rotation += 1.2f;
                    Projectile.velocity *= 0.985f;
                }

                if (Projectile.timeLeft == 1)
                {
                    CreateExplosionDust();
                    SoundEngine.PlaySound(SoundID.Item29 with { Pitch = 0.6f, Volume = 0.8f }, Projectile.Center);
                }
            }

            Lighting.AddLight(Projectile.Center, 0.1f, 0.8f, 0.3f);
        }

        private void CreateExplosionDust()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<JadeDust>());
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.4f, 2.2f);
                d.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
            }

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.9f, Pitch = 0.3f }, Projectile.Center);
        }

        private NPC FindNearestEnemy(Vector2 center, float maxDistance)
        {
            NPC nearest = null;
            float minDist = maxDistance;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float dist = Vector2.Distance(center, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = npc;
                    }
                }
            }
            return nearest;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
            var s = Projectile.GetSource_FromThis();

            // 20% шанс на спавн орбов
            if (Main.rand.NextFloat() <= 0.2f)
            {
                for (int i = 0; i < 16; i++)
                {
                    double angle = i * (Math.PI / 8);
                    Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) 
                                       * (5f + (i % 2 == 0 ? 1.5f : -1f));
                    Projectile.NewProjectile(s, Projectile.Center, velocity, ProjectileID.ChlorophyteOrb, Projectile.damage / 2, 1f, Projectile.owner);
                }
            }

            CreateExplosionDust();
            SoundEngine.PlaySound(SoundID.Item29 with { Volume = 1.2f, Pitch = 0.4f }, Projectile.Center);
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            CreateExplosionDust();
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1f, Pitch = 0.3f }, Projectile.Center);
            Projectile.Kill();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(tex.Width / 2, tex.Height / 2);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
                float progress = (float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length;
                Color color = new Color(100, 255, 150, 150) * (0.5f * progress);
                Main.spriteBatch.Draw(tex, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
