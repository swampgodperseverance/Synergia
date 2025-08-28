using Avalon.Common;
using Avalon.Common.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class WoodenClubRework : MaceTemplate
    {
        public override float MaxRotation => MathF.PI + MathF.PI / 6f;
        public override float? StartRotationLimit => MathHelper.PiOver2;
        public override float SwingRadius => 75f;
        public override float ScaleMult => 1.1f;
        public override float EndScaleTime => 0.45f;
        public override Func<float, float> EasingFunc => rot => Easings.PowOut(rot, 2f);
        public override int TrailLength => 4;
        public override Color? TrailColor => Color.Black;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailLength;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 40;
            Projectile.height = 40;
        }

        public override void EmitDust(Vector2 handPosition, float swingRadius, float rotationProgress, float easedRotationProgress)
        {
            Vector2 offsetFromHand = Projectile.Center - handPosition;
            float dirMod = SwingDirection * Owner.gravDir;
            float progressMult = 2f - (rotationProgress * 2f);

            if (Main.rand.NextBool(3))
            {
                Dust woodDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.WoodFurniture);
                woodDust.velocity = Vector2.Normalize(offsetFromHand * dirMod)
                                    .RotatedBy(MathHelper.PiOver2 * Owner.direction)
                                    * 2.5f * progressMult;
                woodDust.scale = Main.rand.NextFloat(0.9f, 1.3f);
                woodDust.noGravity = false;
            }

            if (Main.rand.NextBool(5))
            {
                Dust dirtDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt);
                dirtDust.velocity = Vector2.Normalize(offsetFromHand * dirMod)
                                    .RotatedBy(MathHelper.PiOver2 * Owner.direction)
                                    * 2f * progressMult;
                dirtDust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                dirtDust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Confused, Main.rand.Next(120, 240));
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP && Main.rand.NextBool(4))
                target.AddBuff(BuffID.Confused, Main.rand.Next(120, 240));
        }
    }
}