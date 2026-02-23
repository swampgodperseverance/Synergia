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
    public class ValhalliteSwordRework : MaceTemplate
    {
        public override string Texture => "ValhallaMod/Items/Weapons/Melee/Swords/ValhalliteSword";
        public override string TrailTexture => "Synergia/Content/Projectiles/Reworks/AltUse/ValhalliteSwordRework";

        public override float ScaleMult => Projectile.ai[2] < 2 ? 1.3f : 1.1f;

        public override float MaxRotation => Projectile.ai[2] < 2 ? 4.2f : 3.0f;

        public override float SwingRadius => Projectile.ai[2] < 2 ? 90f : 70f;

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
                    DustID.IceTorch,
                    Vector2.Normalize(offsetFromHand * dirMod).RotatedBy(MathHelper.PiOver2 * Owner.direction) * speedMultiplier * 2.5f,
                    Scale: 0.9f,
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
            target.AddBuff(BuffID.Frostburn, 60);
        }
    }
}