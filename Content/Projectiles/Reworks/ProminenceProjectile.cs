using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Reworks
{
    public class ProminenceProjectile : ModProjectile
    {
        private float passiveRotationSpeed = 0.042f;
        private float activeRotationSpeed = 0.24f;
        private float chargeRotationSpeed = 0.35f;

        private int dashTimer = 0;
        private int dashCooldown = 48;
        private int maxDashTime = 22;

        private Vector2 dashTarget;
        private NPC targetNPC;
        private int chargeTimer = 0;
        private const int CHARGE_TIME = 26;

        private int deathTimer = 0;
        private const int DEATH_TIME = 38;

        private enum ProjectileState
        {
            Idle,
            Charging,
            Dashing,
            Returning,
            Dying
        }
        private ProjectileState currentState = ProjectileState.Idle;

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 1;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => currentState == ProjectileState.Dashing;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!CheckActive(player))
                return;

            int minionIndex = GetMinionIndex(player);
            targetNPC = FindTarget(player);

            if (currentState == ProjectileState.Dying)
            {
                HandleDyingState();
                return;
            }

            switch (currentState)
            {
                case ProjectileState.Idle:
                    HandleIdleState(player, minionIndex);
                    if (targetNPC != null && dashTimer <= 0)
                    {
                        currentState = ProjectileState.Charging;
                        chargeTimer = CHARGE_TIME;
                    }
                    if (dashTimer > 0) dashTimer--;
                    break;

                case ProjectileState.Charging:
                    HandleChargingState(player, minionIndex);
                    chargeTimer--;
                    if (chargeTimer <= 0)
                    {
                        currentState = ProjectileState.Dashing;
                        dashTimer = maxDashTime;
                        dashTarget = targetNPC?.Center ?? Projectile.Center;
                        SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
                        SpawnChargeEffects();
                    }
                    break;

                case ProjectileState.Dashing:
                    HandleDashingState();
                    dashTimer--;
                    if (dashTimer <= 0 || targetNPC == null || !targetNPC.active)
                        currentState = ProjectileState.Returning;
                    break;

                case ProjectileState.Returning:
                    HandleReturningState(player, minionIndex);
                    if (Vector2.Distance(Projectile.Center, GetIdlePosition(player, minionIndex)) < 55f)
                    {
                        currentState = ProjectileState.Idle;
                        dashTimer = dashCooldown;
                    }
                    break;
            }

            Projectile.rotation += GetCurrentRotationSpeed();
            Lighting.AddLight(Projectile.Center, 1.15f, 0.65f, 0.25f);
        }

        private void HandleIdleState(Player player, int minionIndex)
        {
            Vector2 idlePos = GetIdlePosition(player, minionIndex);
            Vector2 toIdle = idlePos - Projectile.Center;
            float speed = toIdle.Length() * 0.085f;
            if (speed > 11f) speed = 11f;

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(toIdle) * speed, 0.22f);

            Projectile.scale = 1f + (float)Math.Sin(Main.timeForVisualEffects * 0.13f + minionIndex) * 0.035f;

            if (Main.rand.NextBool(7))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 80, default, 1.1f);
                d.noGravity = true;
                d.velocity *= 0.6f;
            }
        }

        private void HandleChargingState(Player player, int minionIndex)
        {
            if (targetNPC == null || !targetNPC.active)
            {
                currentState = ProjectileState.Returning;
                return;
            }

            Vector2 dir = targetNPC.Center - Projectile.Center;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, dir * 0.04f, 0.15f);
            Projectile.scale = 1f + (float)Math.Sin(chargeTimer * 0.7f) * 0.14f;

            if (Main.rand.NextBool(2))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0, 0, 100, default, 1.5f);
            }
        }

        private void HandleDashingState()
        {
            if (targetNPC != null && targetNPC.active)
                dashTarget = targetNPC.Center;

            Vector2 dir = dashTarget - Projectile.Center;
            if (dir.Length() > 14f)
            {
                dir.Normalize();
                Projectile.velocity = dir * 28f;
            }

            for (int i = 0; i < 3; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, -Projectile.velocity.X * 0.55f, -Projectile.velocity.Y * 0.55f, 70, default, 1.7f);
                d.noGravity = true;
            }
            if (Main.rand.NextBool(4))
                Dust.NewDustDirect(Projectile.Center, 4, 4, DustID.Torch, -Projectile.velocity.X * 0.35f, -Projectile.velocity.Y * 0.35f, 80, default, 1.4f);
        }

        private void HandleReturningState(Player player, int minionIndex)
        {
            Vector2 idlePos = GetIdlePosition(player, minionIndex);
            Projectile.velocity = (idlePos - Projectile.Center) * 0.21f;

            if (Main.rand.NextBool(4))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0, 0, 100, default, 1.1f);
        }

        private void HandleDyingState()
        {
            deathTimer--;
            float progress = deathTimer / (float)DEATH_TIME;

            Projectile.scale = progress * 1.25f;
            Projectile.alpha = (int)(255 * (1f - progress));

            Projectile.velocity *= 0.93f;
            Projectile.rotation += 0.42f;

            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.OrangeTorch, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 90, default, 1.7f);
                d.noGravity = true;
            }

            if (deathTimer <= 0)
                Projectile.Kill();
        }

        private float GetCurrentRotationSpeed()
        {
            return currentState switch
            {
                ProjectileState.Charging => chargeRotationSpeed,
                ProjectileState.Dashing => activeRotationSpeed * 3.5f,
                ProjectileState.Returning => activeRotationSpeed * 1.5f,
                ProjectileState.Dying => 0.45f,
                _ => passiveRotationSpeed
            };
        }

        private Vector2 GetIdlePosition(Player player, int minionIndex)
        {
            int total = GetTotalMinions(player);
            if (total == 0) total = 1;

            float baseAngle = Main.GameUpdateCount * 0.026f + minionIndex * 0.7f;
            float wave1 = (float)Math.Sin(Main.GameUpdateCount * 0.033f + minionIndex) * 0.18f;
            float wave2 = (float)Math.Sin(Main.GameUpdateCount * 0.071f + minionIndex * 1.3f) * 0.09f;

            float angle = baseAngle + wave1 + wave2;

            float radius = 68f + (minionIndex % 3) * 13f + (float)Math.Sin(Main.GameUpdateCount * 0.04f + minionIndex) * 6f;

            return player.Center + new Vector2(
                (float)Math.Cos(angle) * radius,
                (float)Math.Sin(angle) * radius * 0.64f - 57f
            );
        }

        private int GetMinionIndex(Player player)
        {
            int index = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI && p.type == Projectile.type)
                {
                    if (p.whoAmI == Projectile.whoAmI) return index;
                    index++;
                }
            }
            return 0;
        }

        private int GetTotalMinions(Player player)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
                if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == Projectile.type)
                    count++;
            return count;
        }

        private NPC FindTarget(Player player)
        {
            NPC best = null;
            float bestDist = 640f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.CanBeChasedBy(this) && !npc.friendly)
                {
                    float d = Vector2.Distance(Projectile.Center, npc.Center);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        best = npc;
                    }
                }
            }
            return best;
        }

        private bool CheckActive(Player player)
        {
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.ProminenceBlessing>());
                StartDying();
                return true;
            }

            if (!player.HasBuff(ModContent.BuffType<Buffs.ProminenceBlessing>()))
            {
                StartDying();
                return true;
            }

            Projectile.timeLeft = 2;
            return true;
        }

        private void StartDying()
        {
            if (currentState != ProjectileState.Dying)
            {
                currentState = ProjectileState.Dying;
                deathTimer = DEATH_TIME;
                Projectile.velocity *= 0.35f;
            }
        }

        private void SpawnChargeEffects()
        {
            for (int i = 0; i < 18; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.OrangeTorch, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 80, default, 1.75f);
                d.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 28; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center, 4, 4, DustID.OrangeTorch,
                    Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f), 60, default, 2.1f);
                d.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = tex.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color glow = new Color(255, 145, 35) * 0.85f;
            glow.A = 0;

            float alphaMult = 1f - Projectile.alpha / 255f;

            if (currentState != ProjectileState.Idle && currentState != ProjectileState.Dying)
            {
                for (int i = 0; i < 4; i++)
                {
                    Main.EntitySpriteDraw(tex, drawPos, null, glow * (0.7f - i * 0.18f),
                        Projectile.rotation, origin, Projectile.scale * (1.2f + i * 0.15f), SpriteEffects.None, 0);
                }
            }
            else if (currentState == ProjectileState.Idle)
            {
                Main.EntitySpriteDraw(tex, drawPos, null, glow * 0.35f,
                    Projectile.rotation, origin, Projectile.scale * 1.15f, SpriteEffects.None, 0);
            }

            if (currentState == ProjectileState.Dashing)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    float prog = 1f - (float)i / Projectile.oldPos.Length;
                    Color trailCol = glow * prog * 0.65f;
                    float scale = Projectile.scale * (0.75f + prog * 0.45f);

                    Main.EntitySpriteDraw(tex, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null,
                        trailCol, Projectile.oldRot[i], origin, scale, SpriteEffects.None, 0);
                }
            }

            Main.EntitySpriteDraw(tex, drawPos, null, lightColor * alphaMult,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}