using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{ //ngl roa inspired //bookworms
    public class AnacondaProj : ModProjectile
    {
        private int _direction;
        private float _length;
        private float _waveTimer;
        private bool _hasHit;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(12, 18);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(_direction);
            writer.Write(_length);
            writer.Write(_waveTimer);
            writer.Write(_hasHit);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            _direction = reader.ReadInt32();
            _length = reader.ReadSingle();
            _waveTimer = reader.ReadSingle();
            _hasHit = reader.ReadBoolean();
        }

        private bool IsHead => Projectile.ai[0] == 0f;
        private bool IsTail => Projectile.ai[0] == 3f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (IsHead && Projectile.localAI[2] == 0f)
            {
                Projectile.localAI[2] = 1f;

                if (Projectile.owner == Main.myPlayer)
                {
                    _direction = (player.Center.X - Main.MouseWorld.X) > 0 ? -1 : 1;
                    _length = Main.rand.NextFloat(0.2f, 0.35f);
                    _waveTimer = 0f;

                    float speedBonus = 1f + (0.35f - _length) * 3f;
                    Projectile.velocity *= speedBonus;

                    Projectile.netUpdate = true;
                }
                Projectile.direction = Projectile.spriteDirection = _direction;
                SoundEngine.PlaySound(SoundID.Item20 with { PitchVariance = 0.1f, Pitch = 0.3f }, Projectile.Center);
            }

            if (IsHead && Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;

                int latest = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);
                if (Projectile.owner == Main.myPlayer)
                {
                    int count = Main.rand.Next(6, 12);
                    for (int i = 1; i <= count; i++)
                    {
                        int oldLatest = latest;
                        Vector2 offset = -Projectile.velocity.SafeNormalize(Vector2.Zero) * 14f * i;

                        latest = Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            Projectile.Center + offset,
                            Vector2.Zero,
                            Type,
                            i == count ? (int)(Projectile.damage * 0.7f) : 0,
                            Projectile.knockBack,
                            Projectile.owner
                        );

                        int me = Projectile.GetByUUID(Projectile.owner, latest);
                        Main.projectile[me].ai[0] = i == count ? 3f : 2f;
                        Main.projectile[me].ai[1] = oldLatest;
                        Main.projectile[me].ai[2] = i;

                        Projectile.netUpdate = true;
                        Main.projectile[me].netUpdate = true;
                    }
                }
            }

            if (IsHead)
            {
                _waveTimer += 0.15f;
                float waveAmplitude = 8f;
                float waveFrequency = 0.35f;
                float offsetY = (float)Math.Sin(_waveTimer * waveFrequency) * waveAmplitude;

                Projectile.velocity.Y = Projectile.velocity.Y * 0.98f + offsetY * 0.1f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                return;
            }

            int byUUID = Projectile.GetByUUID(Projectile.owner, (int)Projectile.ai[1]);
            if (byUUID == -1) return;

            if (Main.projectile.IndexInRange(byUUID))
            {
                Projectile following = Main.projectile[byUUID];
                Vector2 dif = following.Center - Projectile.Center;

                if (dif.LengthSquared() < 100f)
                {
                    Projectile.Opacity = 0f;
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(
                            Projectile.position - Projectile.Size / 4f,
                            Projectile.width,
                            Projectile.height,
                            DustID.GemEmerald,
                            Projectile.velocity.X,
                            Projectile.velocity.Y,
                            0,
                            default,
                            1.1f
                        );
                        dust.noGravity = true;
                    }
                }

                dif = dif.SafeNormalize(Vector2.Zero);

                float distance = 14f * (Projectile.ai[0] == 3f ? 0.8f : Projectile.ai[2] <= 1 ? 1f : 0.7f);

                Projectile.rotation = dif.ToRotation() + MathHelper.PiOver2;
                Projectile.Center = following.Center - dif * distance;

                if (following.ai[0] == 0f && following.identity > Projectile.identity)
                {
                    Projectile.Center += dif * distance;
                }
            }

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GemEmerald,
                    0f, 0f, 100,
                    default,
                    0.7f
                );
                dust.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!IsHead)
            {
                modifiers.FinalDamage *= 0f;
                return;
            }

            if (_hasHit)
            {
                modifiers.FinalDamage *= 0f;
                return;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsHead && !_hasHit)
            {
                _hasHit = true;

                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        target.position,
                        target.width,
                        target.height,
                        DustID.GemEmerald,
                        0f, 0f, 120,
                        default,
                        1.1f
                    );
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    dust.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int variant = IsHead ? 1 : (IsTail ? 3 : 2);
            Texture2D texture = ModContent.Request<Texture2D>(Texture + (variant == 1 ? "_Head" : variant == 2 ? "_Body" : "_Tail")).Value;

            Vector2 position = Projectile.position - Main.screenPosition;
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(6, 9);

            Color glowColor = new Color(0, 255, 100, 40);
            for (int i = 0; i < 3; i++)
            {
                Main.EntitySpriteDraw(
                    texture,
                    position + origin + new Vector2(i, i),
                    null,
                    glowColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    effects,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                position + origin,
                null,
                color * Projectile.Opacity,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }

        public override bool ShouldUpdatePosition() => IsHead;

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GemEmerald,
                    Projectile.velocity.X * 0.5f,
                    Projectile.velocity.Y * 0.5f,
                    100,
                    default,
                    1.2f
                );
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.NPCHit18 with { Volume = 0.1f, Pitch = -0.3f }, Projectile.Center);
        }
    }
}