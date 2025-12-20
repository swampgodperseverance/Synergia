using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class ThunderjabProj : ModProjectile
    {
        private enum AttackType
        {
            Swing,
            Spin
        }

        private enum AttackStage
        {
            Prepare,
            Execute,
            Unwind
        }

        private bool spawnedProjectiles;

        private AttackType CurrentAttack
        {
            get => (AttackType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private AttackStage CurrentStage
        {
            get => (AttackStage)Projectile.localAI[0];
            set
            {
                Projectile.localAI[0] = (float)value;
                Timer = 0f;
            }
        }

        private float InitialAngle
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        private float Timer
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        private float Progress
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        private float Size
        {
            get => Projectile.localAI[2];
            set => Projectile.localAI[2] = value;
        }

        private Player Owner => Main.player[Projectile.owner];

        private float PrepTime =>
            30f / Owner.GetTotalAttackSpeed(Projectile.DamageType) *
            (CurrentAttack == AttackType.Spin ? 2f : 1f);

        private float ExecTime =>
            12f / Owner.GetTotalAttackSpeed(Projectile.DamageType) *
            (CurrentAttack == AttackType.Spin ? 1.5f : 1f);

        private float HideTime =>
            12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);

        public override string Texture =>
            "Synergia/Content/Items/Weapons/Melee/Thunderjab";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 10000;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void OnSpawn(IEntitySource source)
        {
            spawnedProjectiles = false;

            Projectile.spriteDirection =
                Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;

            float angle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();

            if (Projectile.spriteDirection == 1)
                angle = MathHelper.Clamp(angle, -1.05f, 0.52f);
            else
            {
                if (angle < 0f)
                    angle += MathHelper.TwoPi;

                angle = MathHelper.Clamp(angle, 2.62f, 4.19f);
            }

            InitialAngle = angle - 2.36f * Projectile.spriteDirection;
            CurrentStage = AttackStage.Prepare;
        }

        public override void AI()
        {
            if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }

            Owner.itemTime = 2;
            Owner.itemAnimation = 2;

            switch (CurrentStage)
            {
                case AttackStage.Prepare:
                    PrepareStrike();
                    break;

                case AttackStage.Execute:
                    ExecuteStrike();
                    break;

                case AttackStage.Unwind:
                    UnwindStrike();
                    break;
            }

            UpdatePosition();
            Timer++;
        }

        // =========================
        // ATTACK LOGIC
        // =========================

        private void PrepareStrike()
        {
            Progress = MathHelper.Lerp(0.8f, 0f, Timer / PrepTime);
            Size = MathHelper.SmoothStep(0f, 1f, Timer / PrepTime);

            SpawnTopazDust();

            if (Timer >= PrepTime)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                CurrentStage = AttackStage.Execute;
            }
        }

        private void ExecuteStrike()
        {
            if (CurrentAttack == AttackType.Swing)
            {
                Progress = MathHelper.SmoothStep(0f, 5.25f, Timer / ExecTime);

                if (!spawnedProjectiles && Timer >= ExecTime * 0.4f)
                    SpawnFanProjectiles();

                SpawnTopazDust();

                if (Timer >= ExecTime)
                    CurrentStage = AttackStage.Unwind;
            }
            else
            {
                Progress = MathHelper.SmoothStep(0f, 15.7f, Timer / (ExecTime * 5f));

                if (!spawnedProjectiles && Timer >= ExecTime * 2f)
                    SpawnFanProjectiles();

                SpawnTopazDust();

                if (Timer >= ExecTime * 5f)
                    CurrentStage = AttackStage.Unwind;
            }
        }

        private void UnwindStrike()
        {
            Size = 1f - MathHelper.SmoothStep(0f, 1f, Timer / HideTime);

            if (Timer >= HideTime)
                Projectile.Kill();
        }

        // =========================
        // VISUALS
        // =========================

        private void SpawnTopazDust()
        {
            if (Main.rand.NextBool(2))
            {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2Circular(20f, 20f);

                Dust dust = Dust.NewDustPerfect(
                    pos,
                    DustID.GemTopaz,
                    Projectile.rotation.ToRotationVector2() * 0.5f,
                    100,
                    default,
                    1.2f
                );

                dust.noGravity = true;
            }
        }

        // =========================
        // PROJECTILES
        // =========================

        private void SpawnFanProjectiles()
        {
            if (spawnedProjectiles)
                return;

            spawnedProjectiles = true;

            int count = CurrentAttack == AttackType.Spin ? 5 : 3;
            float spread = MathHelper.ToRadians(CurrentAttack == AttackType.Spin ? 40f : 25f);

            // позиция — КОНЧИК МЕЧА
            Vector2 bladeDir = Projectile.rotation.ToRotationVector2();
            Vector2 spawnPos = Projectile.Center + bladeDir * 50f;

            // НАПРАВЛЕНИЕ НА КУРСОР
            Vector2 cursorDir = Main.MouseWorld - spawnPos;
            if (cursorDir.LengthSquared() < 0.001f)
                cursorDir = bladeDir;

            cursorDir.Normalize();

            float speed = 11f;

            for (int i = 0; i < count; i++)
            {
                float t = count == 1 ? 0f : i / (float)(count - 1);
                float rot = MathHelper.Lerp(-spread, spread, t);

                Vector2 velocity = cursorDir.RotatedBy(rot) * speed;

                int proj = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    velocity,
                    ModContent.ProjectileType<ThunderSpike>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Owner.whoAmI
                );

                // увеличенный хитбокс
                Projectile p = Main.projectile[proj];
                p.width += 8;
                p.height += 8;
                p.Center = spawnPos;
            }

            SoundEngine.PlaySound(SoundID.Item122, spawnPos);
        }


        // =========================
        // POSITION / DRAW
        // =========================

        private void UpdatePosition()
        {
            Projectile.rotation =
                InitialAngle + Projectile.spriteDirection * Progress;

            Owner.SetCompositeArmFront(
                true,
                Player.CompositeArmStretchAmount.Full,
                Projectile.rotation - MathHelper.PiOver2
            );

            Vector2 handPos =
                Owner.GetFrontHandPosition(
                    Player.CompositeArmStretchAmount.Full,
                    Projectile.rotation - MathHelper.PiOver2
                );

            handPos.Y += Owner.gfxOffY;

            Projectile.Center = handPos;
            Projectile.scale = Size * Owner.GetAdjustedItemScale(Owner.HeldItem);
            Owner.heldProj = Projectile.whoAmI;
        }

        public override bool? CanDamage() =>
            CurrentStage != AttackStage.Prepare;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 origin = Projectile.spriteDirection == 1
                ? new Vector2(0, Projectile.height)
                : new Vector2(Projectile.width, Projectile.height);

            float rotOffset =
                Projectile.spriteDirection == 1
                ? MathHelper.ToRadians(45)
                : MathHelper.ToRadians(135);

            SpriteEffects fx =
                Projectile.spriteDirection == 1
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + rotOffset,
                origin,
                Projectile.scale,
                fx,
                0f
            );

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * 90f;
            float point = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                18f,
                ref point
            );
        }
    }
}
