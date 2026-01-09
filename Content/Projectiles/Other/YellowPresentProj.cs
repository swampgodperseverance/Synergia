using Avalon.Buffs.Debuffs;
using Bismuth.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Other
{
    public class YellowPresentProj : BasePresentProj {
        public override void Explode() {
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.45f, Pitch = -0.3f }, Projectile.Center);

            for (int i = 0; i < 30; i++) {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch);
                d.velocity *= 2.7f;
                d.noGravity = true;
                d.scale = 1.5f;
            }
            if (Main.myPlayer == Projectile.owner) {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(2f, 2f), ModContent.ProjectileType<YellowBuffer>(), 0, 0f, Projectile.owner);
            }

            Projectile.Kill();
        }
    }
    public class YellowBuffer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 360;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 toPlayer = player.Center - Projectile.Center;

            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                toPlayer.SafeNormalize(Vector2.Zero) * 10f,
                0.25f
            );

            Projectile.rotation += 0.25f;

            Lighting.AddLight(Projectile.Center, 0.9f, 0.8f, 0.2f);

            Dust d = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.YellowTorch
            );
            d.noGravity = true;
            d.scale = 1.1f;
            d.velocity *= 0.1f;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                ApplyRandomEffects(player);
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
                Projectile.Kill();
            }
        }

        private void ApplyRandomEffects(Player player)
        {
            int[] buffs =
            {
                BuffID.Regeneration,
                BuffID.Swiftness,
                BuffID.Ironskin,
                BuffID.Wrath,
                BuffID.ManaRegeneration,
                ModContent.BuffType<FlowOfWind>(),
                ModContent.BuffType<Glaciation>()
            };

            int[] debuffs =
            {
                BuffID.Poisoned,
                BuffID.Slow,
                BuffID.Weak,
                BuffID.BrokenArmor,
                BuffID.Darkness,
                ModContent.BuffType<BrokenWeaponry>()

            };

            for (int i = 0; i < 2; i++)
            {
                bool debuff = Main.rand.NextBool();

                if (debuff)
                    player.AddBuff(Main.rand.Next(debuffs), Main.rand.Next(180, 360));
                else
                    player.AddBuff(Main.rand.Next(buffs), Main.rand.Next(240, 480));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (1f - i / (float)Projectile.oldPos.Length) * 0.6f;

                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    Color.Yellow * alpha,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }
    }
}
