using Synergia.Content.Dusts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Hostile
{
    public class SparkyTrail : ModProjectile
    {
        public ref float PreviousIdentity => ref Projectile.ai[0];
        public ref float NextIdentity => ref Projectile.ai[1];
        public ref float TrailWidthMultiplier => ref Projectile.ai[2];
        public ref float NewPatternTimer => ref Projectile.localAI[0];

        private const int PointsPerLine = 8;
        private const float PointMaxOffset = 16f;
        private const float CollisionWidth = 4f;

        private List<Vector2> lightningPoint = new List<Vector2>();
        private List<Vector2> prevLightningPoint = new List<Vector2>();
        private Projectile previousPoint;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.netImportant = true;
            Projectile.hide = true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (previousPoint == null)
                return false;
            float point = 1f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, previousPoint.Center, CollisionWidth, ref point);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Electrified, 300, false);
            base.OnHitPlayer(target, info);
        }
        public override void AI()
        {
            previousPoint = GetPrevious();
            if(previousPoint == null)
                return;
            NewPatternTimer++;
            if(NewPatternTimer >= 3)
            {
                NewPatternTimer = 0;
                if(Main.netMode != NetmodeID.Server)
                    CastDust();
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    CreateNewLightningPoints();
                    Projectile.netUpdate = true;
                }
            }
        }
        private Projectile GetPrevious()
        {
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].identity == PreviousIdentity && (NextIdentity == -1 || Main.projectile[i].ai[1] == Projectile.identity))
                    return Main.projectile[i];
            }
            return null;
        }
        private void CreateNewLightningPoints()
        {
            prevLightningPoint = new List<Vector2>(lightningPoint);
            lightningPoint.Clear();

            lightningPoint.Add(Projectile.Center);

            Vector2 point, offset;
            for(int i = 1; i < PointsPerLine - 1; i++)
            {
                point = Vector2.Lerp(Projectile.Center, previousPoint.Center, (float)i / (PointsPerLine - 1));
                offset = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0f, PointMaxOffset);
                point += offset;
                lightningPoint.Add(point);
            }

            lightningPoint.Add(previousPoint.Center);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(lightningPoint.Count);
            foreach (Vector2 v in lightningPoint)
                writer.WriteVector2(v);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            prevLightningPoint = new List<Vector2>(lightningPoint);
            lightningPoint.Clear();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                lightningPoint.Add(reader.ReadVector2());
        }
        private void CastDust()
        {
            Vector2 point;
            Vector2 spread = new Vector2(24, 24);
            for(int i = 0; i < 10; i++)
            {
                if (!Main.rand.NextBool(16))
                    continue;
                point = Vector2.Lerp(Projectile.Center - spread / 2, previousPoint.Center - spread / 2, (float)i / 10);
                Dust.NewDust(point, (int)spread.X, (int)spread.Y, ModContent.DustType<ElectricityDust>(), 0, 0);
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (previousPoint == null || lightningPoint.Count == 0)
                return false;

            float lifetimeMultiplier = previousPoint.timeLeft > 30 ? 1f : previousPoint.timeLeft / 30f;
            float totalMultiplier = TrailWidthMultiplier * lifetimeMultiplier;

            if (prevLightningPoint.Count != 0)
                for (int i = 1; i < PointsPerLine; i++)
                    DrawLine(prevLightningPoint[i - 1], prevLightningPoint[i], new Color(246, 206, 54, 40), 1.5f * totalMultiplier);
            for (int i = 1; i < PointsPerLine; i++)
                DrawLine(lightningPoint[i - 1], lightningPoint[i], new Color(255, 246, 147, 60), 2.25f * totalMultiplier);

            return false;
        }
        private void DrawLine(Vector2 point1, Vector2 point2, Color? color = null, float width = 1f)
        {
            if (color == null)
                color = Color.White;
            Texture2D texture = TextureAssets.FishingLine.Value;

            Vector2 direction = point2 - point1;
            float rotation = direction.ToRotation() - MathHelper.PiOver2;
            float scale = direction.Length() / texture.Height;
            Vector2 position = (point2 + point1) / 2 - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Main.EntitySpriteDraw(
                texture,
                position,
                null,
                (Color)color,
                rotation,
                origin,
                new Vector2(width, scale),
                SpriteEffects.None,
                0
            );
        }
    }
}
