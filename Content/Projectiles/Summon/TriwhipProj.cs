using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Avalon.Dusts;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Synergia.Content.Dusts;

namespace Synergia.Content.Projectiles.Summon
{
    public class TriwhipProjTriple : ModProjectile
    {
        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.WhipSettings.Segments = 17;
            Projectile.WhipSettings.RangeMultiplier = 1.1f;
        }

        public override bool? CanCutTiles() => null;

        public override void AI()
        {
            Timer++;

            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.MatchItemTimeToItemAnimation();

            if (player.itemAnimation < player.itemAnimationMax * 0.333f)
                player.bodyFrame.Y = player.bodyFrame.Height * 3;
            else if (player.itemAnimation < player.itemAnimationMax * 0.666f)
                player.bodyFrame.Y = player.bodyFrame.Height * 2;
            else
                player.bodyFrame.Y = player.bodyFrame.Height;

            // Dust
            float swingTime = player.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Timer / swingTime;

            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, true)
                * Utils.GetLerpValue(0.9f, 0.7f, swingProgress, true) > 0.8f
                && !Main.rand.NextBool(4))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;
                points.Clear();
                Projectile.FillWhipControlPoints(Projectile, points);
                int index = Main.rand.Next(points.Count - 10, points.Count);
                Vector2 dir = (points[index] - points[index - 1]).SafeNormalize(Vector2.Zero);

                Dust d = Dust.NewDustPerfect(points[index] + dir * 40f,
                    ModContent.DustType<StarstoneDust>(),
                    dir.RotatedBy(MathHelper.PiOver2 * player.direction) * 2f,
                    100,
                    Color.White,
                    1.2f);
                d.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.immortal) return;

            Player player = Main.player[Projectile.owner];
            player.MinionAttackTargetNPC = target.whoAmI;

            Projectile.damage = (int)(Projectile.damage * 0.6f);
        }

        private void DrawLine(List<Vector2> points)
        {
            Texture2D tex = TextureAssets.FishingLine.Value;
            Rectangle frame = tex.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            for (int i = 0; i < points.Count - 2; i++)
            {
                Vector2 p0 = points[i];
                Vector2 p1 = points[i + 1];
                float rotation = (p1 - p0).ToRotation() - MathHelper.PiOver2;

                Main.EntitySpriteDraw(
                    tex,
                    p0 - Main.screenPosition,
                    frame,
                    Lighting.GetColor(p0.ToTileCoordinates()),
                    rotation,
                    origin,
                    new Vector2(1, (p1 - p0).Length() / frame.Height),
                    SpriteEffects.None,
                    0
                );
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);
            DrawLine(points);

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 p0 = points[i];
                Vector2 p1 = points[i + 1];
                Vector2 diff = p1 - p0;
                Rectangle frame;

                if (i == 0)
                    frame = new Rectangle(0, 0, 14, 22); // handle
                else if (i == points.Count - 2)
                    frame = new Rectangle(0, 100, 14, 32); // tip
                else if (i > 10)
                    frame = new Rectangle(0, 74, 14, 26); // segment3
                else if (i > 5)
                    frame = new Rectangle(0, 48, 14, 26); // segment2
                else
                    frame = new Rectangle(0, 22, 14, 26); // segment1

                float rot = diff.ToRotation() - MathHelper.PiOver2;

                Main.EntitySpriteDraw(
                    tex,
                    p0 - Main.screenPosition,
                    frame,
                    Lighting.GetColor(p0.ToTileCoordinates()),
                    rot,
                    new Vector2(frame.Width / 2f, 2f),
                    1f,
                    flip,
                    0
                );
            }

            return false;
        }
    }
}
