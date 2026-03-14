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
        private float passiveRotationSpeed = 0.05f;
        private float activeRotationSpeed = 0.2f;
        private float chargeRotationSpeed = 0.3f;
        private float idleBobAmount = 0.1f;
        private float idleBobSpeed = 0.1f;
        private int dashTimer = 0;
        private int dashCooldown = 45;
        private int maxDashTime = 20;
        private Vector2 dashTarget;
        private NPC targetNPC;
        private bool isCharging = false;
        private int chargeTimer = 0;
        private const int CHARGE_TIME = 25;
        private enum ProjectileState
        {
            Idle,
            Charging,
            Dashing,
            Returning
        }
        private ProjectileState currentState = ProjectileState.Idle;
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override bool MinionContactDamage()
        {
            return currentState == ProjectileState.Dashing;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!CheckActive(player))
                return;
            int minionIndex = GetMinionIndex(player);
            targetNPC = FindTarget(player);
            switch (currentState)
            {
                case ProjectileState.Idle:
                    HandleIdleState(player, minionIndex);
                    if (targetNPC != null && dashTimer <= 0)
                    {
                        currentState = ProjectileState.Charging;
                        chargeTimer = CHARGE_TIME;
                        isCharging = true;
                    }
                    dashTimer--;
                    break;
                case ProjectileState.Charging:
                    HandleChargingState(player, minionIndex);
                    chargeTimer--;
                    if (chargeTimer <= 0)
                    {
                        currentState = ProjectileState.Dashing;
                        dashTimer = maxDashTime;
                        dashTarget = targetNPC.Center;
                        SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
                        SpawnChargeEffects();
                    }
                    break;
                case ProjectileState.Dashing:
                    HandleDashingState();
                    dashTimer--;
                    if (dashTimer <= 0 || targetNPC == null || !targetNPC.active)
                    {
                        currentState = ProjectileState.Returning;
                    }
                    break;
                case ProjectileState.Returning:
                    HandleReturningState(player, minionIndex);
                    if (Vector2.Distance(Projectile.Center, GetIdlePosition(player, minionIndex)) < 50f)
                    {
                        currentState = ProjectileState.Idle;
                        dashTimer = dashCooldown;
                    }
                    break;
            }
            Projectile.rotation += GetCurrentRotationSpeed();
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.8f);
        }
        private void HandleIdleState(Player player, int minionIndex)
        {
            Vector2 idlePosition = GetIdlePosition(player, minionIndex);
            idlePosition += new Vector2(
            (float)Math.Sin(Main.timeForVisualEffects * idleBobSpeed + minionIndex) * idleBobAmount * 20,
            (float)Math.Cos(Main.timeForVisualEffects * idleBobSpeed + minionIndex) * idleBobAmount * 20
            );
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (idlePosition - Projectile.Center) * 0.05f, 0.1f);
            if (Projectile.velocity.Length() > 5f)
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 5f;
        }
        private void HandleChargingState(Player player, int minionIndex)
        {
            Vector2 idlePosition = GetIdlePosition(player, minionIndex);
            if (targetNPC != null)
            {
                Vector2 directionToTarget = targetNPC.Center - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, directionToTarget * 0.02f, 0.05f);
                Projectile.scale = 1f + (float)Math.Sin(chargeTimer * 0.5f) * 0.1f;
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.OrangeTorch, 0f, 0f, 100, default, 1.2f);
                }
            }
            else
            {
                currentState = ProjectileState.Returning;
            }
        }
        private void HandleDashingState()
        {
            if (targetNPC != null && targetNPC.active)
            {
                dashTarget = targetNPC.Center;
            }
            Vector2 direction = dashTarget - Projectile.Center;
            if (direction.Length() > 10f)
            {
                direction.Normalize();
                Projectile.velocity = direction * 25f;
            }
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                DustID.Torch, -Projectile.velocity.X * 0.5f, -Projectile.velocity.Y * 0.5f, 100, default, 1.5f);
            }
        }
        private void HandleReturningState(Player player, int minionIndex)
        {
            Vector2 idlePosition = GetIdlePosition(player, minionIndex);
            Projectile.velocity = (idlePosition - Projectile.Center) * 0.15f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                DustID.OrangeTorch, 0f, 0f, 100, default, 1f);
            }
        }
        private float GetCurrentRotationSpeed()
        {
            switch (currentState)
            {
                case ProjectileState.Charging:
                    return chargeRotationSpeed;
                case ProjectileState.Dashing:
                    return activeRotationSpeed * 3f;
                case ProjectileState.Returning:
                    return activeRotationSpeed;
                default:
                    return passiveRotationSpeed;
            }
        }
        private Vector2 GetIdlePosition(Player player, int minionIndex)
        {
            float offsetX = (minionIndex - GetTotalMinions(player) / 2f) * 40f;
            float offsetY = -50f - (float)Math.Sin(Main.timeForVisualEffects * 0.02f + minionIndex) * 10f;
            return player.Center + new Vector2(offsetX, offsetY);
        }
        private int GetMinionIndex(Player player)
        {
            int index = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == Projectile.type)
                {
                    if (proj.whoAmI == Projectile.whoAmI)
                        return index;
                    index++;
                }
            }
            return 0;
        }
        private int GetTotalMinions(Player player)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == Projectile.type)
                {
                    count++;
                }
            }
            return count;
        }
        private NPC FindTarget(Player player)
        {
            NPC target = null;
            float maxDistance = 600f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.CanBeChasedBy(this) && !npc.friendly)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < maxDistance)
                    {
                        maxDistance = distance;
                        target = npc;
                    }
                }
            }
            return target;
        }
        private bool CheckActive(Player player)
        {
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<Buffs.ProminenceBlessing>());
                return false;
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.ProminenceBlessing>()))
            {
                Projectile.timeLeft = 2;
            }
            return true;
        }
        private void SpawnChargeEffects()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                DustID.OrangeTorch, 0f, 0f, 100, default, 1.5f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Color glowColor = Color.Orange * 0.5f;
            glowColor.A = 0;
            if (currentState != ProjectileState.Idle)
            {
                Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + origin,
                null, glowColor, Projectile.rotation, origin, Projectile.scale * 1.2f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition + origin,
            null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}