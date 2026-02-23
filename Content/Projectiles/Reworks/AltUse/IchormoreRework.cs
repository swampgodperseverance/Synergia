using Avalon.Common;
using Avalon.Common.Extensions; 
using Avalon.Common.Templates;
using Avalon.Data.Sets;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class IchormoreRework : MaceTemplate
    {
        public override string Texture => "ValhallaMod/Items/Weapons/Melee/Swords/Ichormore";
        public override string TrailTexture => "Synergia/Content/Projectiles/Reworks/AltUse/IchormoreRework";

        public override float ScaleMult
        {
            get
            {
                float progress = Projectile.localAI[0];
                float pop = Utils.GetLerpValue(0.3f, 0.6f, progress, true);

                float baseScale = Projectile.ai[2] < 2 ? 0.95f : 0.9f;
                float maxScale  = Projectile.ai[2] < 2 ? 1.35f : 1.15f;

                return MathHelper.Lerp(baseScale, maxScale, pop);
            }
        }


        public override float MaxRotation => Projectile.ai[2] < 2 ? 5.4f : 4.2f;

        public override float SwingRadius
        {
            get
            {
                float progress = Projectile.localAI[0]; // rotation progress 0–1
                float pop = Utils.GetLerpValue(0.25f, 0.6f, progress, true);

                float baseRadius = Projectile.ai[2] < 2 ? 85f : 70f;
                float maxRadius  = Projectile.ai[2] < 2 ? 120f : 100f;

                return MathHelper.Lerp(baseRadius, maxRadius, pop);
            }
        }



        public override float StartScaleTime => 0.5f;
        public override float StartScaleMult => 0.7f;

        public override float EndScaleTime => 0.35f;
        public override float EndScaleMult => 0.6f;

        public override Color? TrailColor => new Color(1f, 0.9f, 0.3f, 0f);
        public override Func<float, float> EasingFunc => rot => Easings.PowInOut(rot, 4f);
        public override int TrailLength => 6;

        public override void EmitDust(Vector2 handPosition, float swingRadius, float rotationProgress, float easedRotationProgress)
        {
            if (Projectile.localAI[2] != 1 && easedRotationProgress > 0.1f)
            {
                Projectile.localAI[2] = 1;
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.9f, Pitch = Projectile.ai[2] < 2 ? 0f : 0.3f }, Projectile.position);
            }

            float speedMultiplier = Math.Clamp(Math.Abs(Projectile.oldRot[0] - Projectile.rotation), 0f, 1f);
            if (speedMultiplier > 0.15f)
            {
                Vector2 offsetFromHand = Projectile.Center - handPosition;
                float dirMod = SwingDirection * Owner.gravDir;
                Dust d = Dust.NewDustPerfect(
                    Vector2.Lerp(Projectile.Center, handPosition, Main.rand.NextFloat(0.2f, 0.5f)),
                    DustID.Ichor,
                    Vector2.Normalize(offsetFromHand * dirMod).RotatedBy(MathHelper.PiOver2 * Owner.direction) * speedMultiplier * 2.5f,
                    Scale: 0.7f,
                    Alpha: 100
                );
                d.noGravity = true;
                d.fadeIn = 1.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                hit.Knockback *= 1.2f;
            }
        }
    }
}