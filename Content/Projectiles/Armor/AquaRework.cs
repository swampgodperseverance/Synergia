using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Armor
{
    public class AquaRework : ModProjectile
    {
        private const float OrbitDistance = 52f;
        private const float LerpSpeed = 0.16f;
        private const float PulseSpeed = 0.08f;
        private const int MaxShields = 4;
        private const float DamageReduction = 0.15f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            if (Projectile.localAI[0] < 0f)
            {
                Projectile.localAI[0] = FindFreeSlot();
            }

            int slot = (int)Projectile.localAI[0];
            Vector2 targetOffset = GetSlotOffset(slot);
            Vector2 targetPos = player.Center + targetOffset;

            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, LerpSpeed);
            Projectile.velocity *= 0.92f;
            Projectile.rotation = targetOffset.ToRotation() + MathHelper.PiOver2;

            Projectile.localAI[1] += 0.02f;
            float pulse = (float)System.Math.Sin(Projectile.localAI[1] * PulseSpeed * 2f) * 0.15f;
            Projectile.scale = 1f + pulse;

            if (Main.rand.NextBool(20))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.WaterCandle,
                    Vector2.UnitY * -1f, Scale: Projectile.scale * 1.2f);
                d.noGravity = true;
                d.velocity *= 0.2f;
            }

            ReduceHostileDamage(player);
        }

        private int FindFreeSlot()
        {
            bool[] occupied = new bool[MaxShields];

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.type == Type && other.owner == Projectile.owner)
                {
                    if (other.localAI[0] >= 0 && other.localAI[0] < MaxShields)
                    {
                        occupied[(int)other.localAI[0]] = true;
                    }
                }
            }

            for (int slot = 0; slot < MaxShields; slot++)
            {
                if (!occupied[slot])
                    return slot;
            }

            return Main.rand.Next(MaxShields);
        }

        private Vector2 GetSlotOffset(int slot)
        {
            return slot switch
            {
                0 => new Vector2(0, -OrbitDistance),
                1 => new Vector2(OrbitDistance, 0),
                2 => new Vector2(0, OrbitDistance),
                3 => new Vector2(-OrbitDistance, 0),
                _ => Vector2.Zero
            };
        }

        private void ReduceHostileDamage(Player player)
        {
            Rectangle shieldHitbox = Projectile.Hitbox;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (!proj.active || proj.damage <= 0 || proj.friendly)
                    continue;

                if (proj.owner == player.whoAmI || proj.owner == 255)
                    continue;

                if (Collision.CheckAABBvAABBCollision(shieldHitbox.TopLeft(), shieldHitbox.Size(),
                    proj.Hitbox.TopLeft(), proj.Hitbox.Size()))
                {
                    if (proj.localAI[1] != Projectile.whoAmI + 1)
                    {
                        proj.localAI[1] = Projectile.whoAmI + 1;
                        proj.damage = (int)(proj.damage * (1f - DamageReduction));

                        for (int d = 0; d < 6; d++)
                        {
                            Dust dust = Dust.NewDustPerfect(proj.Center, DustID.WaterCandle,
                                Main.rand.NextVector2Circular(3f, 3f), Scale: 1.2f);
                            dust.noGravity = true;
                        }

                        SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float time = Main.GlobalTimeWrappedHourly * 2f;
            float pulse1 = (float)System.Math.Sin(time + Projectile.whoAmI) * 0.5f + 0.5f;
            float pulse2 = (float)System.Math.Cos(time * 0.7f + Projectile.whoAmI) * 0.4f + 0.6f;

            Color glow1 = new Color(0.2f, 0.5f + pulse1 * 0.3f, 1f, 0f) * 0.6f;
            Main.EntitySpriteDraw(texture, drawPos, null, glow1, Projectile.rotation, origin,
                Projectile.scale * 1.2f, SpriteEffects.None);

            Color glow2 = new Color(0.3f, 0.8f, 1f, 0f) * (0.3f + pulse2 * 0.3f);
            Main.EntitySpriteDraw(texture, drawPos, null, glow2, Projectile.rotation, origin,
                Projectile.scale * 1.1f, SpriteEffects.None);

            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor),
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
        }
    }
}