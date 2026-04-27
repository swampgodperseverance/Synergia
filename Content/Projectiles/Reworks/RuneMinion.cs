using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Reworks;
using System;

namespace Synergia.Content.Projectiles.Reworks
{
    public class RuneMinion : ModProjectile
    {
        private const int TotalFrames = 8;
        private const int IdleFrames = 4;
        private const int AttackReadyFrames = 4;

        private int attackTimer;
        private NPC targetNPC;
        private bool justSpawned = true;
        private const int FadeInTime = 20;
        private const int FadeOutTime = 20;

        private float glowIntensity = 0f;
        private bool isAttacking = false;
        private int attackGlowTimer = 0;
        private const int AttackGlowDuration = 10;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = TotalFrames;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.RunicLegacy>());
                return;
            }

            if (!player.HasBuff(ModContent.BuffType<Buffs.RunicLegacy>()))
            {
                Projectile.alpha += (int)(255f / FadeOutTime);
                if (Projectile.alpha >= 255) Projectile.Kill();
                return;
            }
            else if (Projectile.timeLeft < 18000)
            {
                Projectile.timeLeft = 18000;
            }

            if (justSpawned)
            {
                SpawnDusts(Color.Cyan);
                justSpawned = false;
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= (int)(255f / FadeInTime);
                if (Projectile.alpha < 0) Projectile.alpha = 0;
            }

            if (attackGlowTimer > 0)
            {
                attackGlowTimer--;
                glowIntensity = MathHelper.Lerp(glowIntensity, 1f, 0.3f);
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
                glowIntensity = MathHelper.Lerp(glowIntensity, 0.5f, 0.05f);
            }

            FindTarget();

            if (targetNPC != null && targetNPC.active && !targetNPC.friendly)
            {
                float distanceToTarget = Vector2.Distance(Projectile.Center, targetNPC.Center);
                if (distanceToTarget > 400f)
                    MoveIdle(player);
                else
                    AttackTarget(targetNPC);
            }
            else
            {
                MoveIdle(player);
            }

            AnimateMinion();

            Projectile.position += new Vector2(
                (float)Math.Sin(Main.GameUpdateCount / 20f + Projectile.whoAmI) * 0.5f,
                (float)Math.Cos(Main.GameUpdateCount / 30f + Projectile.whoAmI) * 0.4f
            );

            if (Projectile.velocity.X > 0.5f)
                Projectile.spriteDirection = -1;
            else if (Projectile.velocity.X < -0.5f)
                Projectile.spriteDirection = 1;

            Lighting.AddLight(Projectile.Center, 0.2f + glowIntensity * 0.3f, 0.4f + glowIntensity * 0.6f, 0.6f + glowIntensity * 0.4f);
        }

        private void MoveIdle(Player player)
        {
            int index = Projectile.minionPos;
            Vector2 idleOffset = new Vector2((index - 1.5f) * 50f, -50f);
            Vector2 idlePosition = player.Center + idleOffset;

            Vector2 toIdle = idlePosition - Projectile.Center;
            float distance = toIdle.Length();
            float speed = 10f;

            if (distance > 200f)
                speed *= 1.5f;

            if (distance > 20f)
                Projectile.velocity = (Projectile.velocity * 20f + toIdle.SafeNormalize(Vector2.Zero) * speed) / 21f;
            else
                Projectile.velocity *= 0.9f;

            attackTimer = 0;
            targetNPC = null;
        }

        private void FindTarget()
        {
            targetNPC = null;
            float closest = 600f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < closest)
                    {
                        closest = distance;
                        targetNPC = npc;
                    }
                }
            }
        }

        private void AttackTarget(NPC target)
        {
            attackTimer++;

            Vector2 hoverPos = target.Center + new Vector2((float)Math.Sin(Main.GameUpdateCount / 15f + Projectile.whoAmI) * 60f, -60f);
            Vector2 toHover = hoverPos - Projectile.Center;
            Projectile.velocity = (Projectile.velocity * 20f + toHover.SafeNormalize(Vector2.Zero) * 10f) / 21f;

            if (attackTimer % 40 == 0 && Main.myPlayer == Projectile.owner)
            {
                attackGlowTimer = AttackGlowDuration;

                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                int proj = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    direction * 10f,
                    ModContent.ProjectileType<Bismuth.Content.Projectiles.MoonlightStaffP>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                for (int i = 0; i < 15; i++)
                {
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.BlueTorch,
                        direction * Main.rand.NextFloat(2f, 8f) + Main.rand.NextVector2Circular(2f, 2f),
                        100,
                        Color.Cyan,
                        1.5f
                    );
                    d.noGravity = true;
                }
            }
        }

        private void AnimateMinion()
        {
            int frameSpeed = 9;

            if (targetNPC == null)
            {
                if (++Projectile.frameCounter >= frameSpeed)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame >= IdleFrames) Projectile.frame = 0;
                }
            }
            else
            {
                float distanceToTarget = Vector2.Distance(Projectile.Center, targetNPC.Center);

                if (distanceToTarget > 200f)
                {
                    if (++Projectile.frameCounter >= frameSpeed)
                    {
                        Projectile.frameCounter = 0;
                        Projectile.frame++;
                        if (Projectile.frame < IdleFrames) Projectile.frame = IdleFrames;
                        if (Projectile.frame >= TotalFrames - AttackReadyFrames) Projectile.frame = IdleFrames;
                    }
                }
                else
                {
                    if (++Projectile.frameCounter >= frameSpeed)
                    {
                        Projectile.frameCounter = 0;
                        Projectile.frame++;
                        if (Projectile.frame < TotalFrames - AttackReadyFrames) Projectile.frame = TotalFrames - AttackReadyFrames;
                        if (Projectile.frame >= TotalFrames) Projectile.frame = TotalFrames - AttackReadyFrames;
                    }
                }
            }
        }

        private void SpawnDusts(Color color)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 offset = Main.rand.NextVector2CircularEdge(Projectile.width, Projectile.height);
                Dust d = Dust.NewDustPerfect(Projectile.Center + offset, DustID.BlueTorch, offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 3f), 150, color, 1.2f);
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / TotalFrames;
            Rectangle source = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = source.Size() / 2f;
            SpriteEffects fx = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float alpha = 1f - Projectile.alpha / 255f;
            Color outlineColor;
            if (isAttacking)
            {
                outlineColor = Color.Lerp(Color.Cyan, Color.White, 0.5f);
            }
            else
            {
                outlineColor = Color.Lerp(Color.Blue, Color.Cyan, glowIntensity);
            }
            int outlineThickness = 3;
            Vector2[] offsets = new Vector2[]
            {
                new Vector2(-outlineThickness, 0),
                new Vector2(outlineThickness, 0),
                new Vector2(0, -outlineThickness),
                new Vector2(0, outlineThickness)
            };
            for (int layer = 0; layer < 2; layer++)
            {
                float layerAlpha = (layer == 0 ? 0.6f : 0.3f) * alpha * (isAttacking ? 1.2f : glowIntensity + 0.3f);
                Color layerColor = outlineColor * layerAlpha;

                foreach (Vector2 offset in offsets)
                {
                    Vector2 drawOffset = offset * (layer + 1);
                    Main.EntitySpriteDraw(
                        texture,
                        Projectile.Center - Main.screenPosition + drawOffset,
                        source,
                        layerColor,
                        Projectile.rotation,
                        origin,
                        1f,
                        fx,
                        0
                    );
                }
            }
            if (isAttacking)
            {
                Color attackGlowColor = Color.Cyan * 0.4f * alpha;
                for (int i = 0; i < 8; i++)
                {
                    Vector2 offset = new Vector2(
                        (float)Math.Cos(Main.GameUpdateCount * 0.1f + i) * 5f,
                        (float)Math.Sin(Main.GameUpdateCount * 0.1f + i) * 5f
                    );
                    Main.EntitySpriteDraw(
                        texture,
                        Projectile.Center - Main.screenPosition + offset,
                        source,
                        attackGlowColor,
                        Projectile.rotation,
                        origin,
                        1f,
                        fx,
                        0
                    );
                }
            }
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                source,
                lightColor * alpha,
                Projectile.rotation,
                origin,
                1f,
                fx,
                0
            );

            return false;
        }
    }
}