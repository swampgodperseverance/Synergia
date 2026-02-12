using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class Tree : ModProjectile
    {
        private const int FullHeight = 216;
        private const int Width = 38;
        private const int RiseTime = 15;
        private const int StayTime = 100;
        private Vector2 groundPosition;

        public override void SetDefaults()
        {
            Projectile.width = Width;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = RiseTime + StayTime + 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            groundPosition = FindGroundPosition(Projectile.Center);

            Projectile.Bottom = groundPosition;

            Projectile.localAI[1] = 1f;
            Projectile.localAI[0] = Main.rand.NextFloat(-0.14f, 0.14f);

            SpawnGroundDust();

            SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
            Projectile.netUpdate = true;
        }
        private Vector2 FindGroundPosition(Vector2 startPos)
        {
            int x = (int)(startPos.X / 16f);
            int startY = (int)(startPos.Y / 16f);

            for (int y = startY; y < Main.maxTilesY - 5; y++)
            {
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    return new Vector2(x * 16f + 8f, y * 16f);
                }

                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    if (xOffset == 0) continue;

                    Tile neighbor = Framing.GetTileSafely(x + xOffset, y);
                    if (neighbor.HasTile && Main.tileSolid[neighbor.TileType] && !Main.tileSolidTop[neighbor.TileType])
                    {
                        return new Vector2((x + xOffset) * 16f + 8f, y * 16f);
                    }
                }
            }
            return new Vector2(startPos.X, startY * 16f + 80f);
        }

        private void SpawnGroundDust()
        {
            int tileX = (int)(groundPosition.X / 16f);
            int tileY = (int)(groundPosition.Y / 16f);

            Tile tile = Framing.GetTileSafely(tileX, tileY);
            int dustType = DustID.Dirt;

            if (tile.HasTile)
            {
                switch (tile.TileType)
                {
                    case TileID.Dirt:
                    case TileID.Mud:
                        dustType = DustID.Dirt;
                        break;
                    case TileID.Stone:
                    case TileID.GrayBrick:
                        dustType = DustID.Stone;
                        break;
                    case TileID.Sand:
                    case TileID.Pearlsand:
                    case TileID.Crimsand:
                    case TileID.Ebonsand:
                        dustType = DustID.Sand;
                        break;
                    case TileID.SnowBlock:
                    case TileID.IceBlock:
                        dustType = DustID.Snow;
                        break;
                    case TileID.Grass:
                    case TileID.CorruptGrass:
                    case TileID.CrimsonGrass:
                    case TileID.HallowedGrass:
                        dustType = DustID.Grass;
                        break;
                    default:
                        dustType = DustID.Dirt;
                        break;
                }
            }












            for (int k = 0; k < 12; k++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-16f, 16f), Main.rand.NextFloat(-8f, 4f));
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-4f, -1.5f));

                Dust dust = Dust.NewDustPerfect(groundPosition + offset, dustType, velocity, 0, default, Main.rand.NextFloat(0.8f, 1.6f));
                dust.noGravity = false;
            }
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            float progress = MathHelper.Clamp(Projectile.ai[0] / RiseTime, 0f, 1f);
            int targetHeight = (int)(FullHeight * progress);

            int deltaH = targetHeight - Projectile.height;
            Projectile.height = targetHeight;
            Projectile.position.Y -= deltaH;

            float baseRotation = Projectile.localAI[0];

            if (progress < 1f)
            {
                Projectile.rotation = baseRotation;

                if (Projectile.ai[0] < RiseTime && Main.rand.NextBool(2))
                {
                    Vector2 dustPos = groundPosition + new Vector2(Main.rand.NextFloat(-Width * 0.4f, Width * 0.4f), Main.rand.NextFloat(-8f, 2f));
                    int dustType = DustID.Dirt;

                    int tileX = (int)(groundPosition.X / 16f);
                    int tileY = (int)(groundPosition.Y / 16f);

                    Tile tile = Framing.GetTileSafely(tileX, tileY);
                    if (tile.HasTile)
                    {
                        switch (tile.TileType)
                        {
                            case TileID.Dirt:
                            case TileID.Mud:
                                dustType = DustID.Dirt;
                                break;
                            case TileID.Stone:
                                dustType = DustID.Stone;
                                break;
                            case TileID.Sand:
                            case TileID.Pearlsand:
                                dustType = DustID.Sand;
                                break;
                            case TileID.SnowBlock:
                                dustType = DustID.Snow;
                                break;
                            default:
                                dustType = DustID.Dirt;
                                break;
                        }
                    }

                    Vector2 dustVel = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-3f, -1f));
                    Dust dust = Dust.NewDustPerfect(dustPos, dustType, dustVel, Scale: Main.rand.NextFloat(1.1f, 1.8f));
                    dust.noGravity = false;
                }
            }
            else
            {
                Projectile.rotation = baseRotation + MathF.Sin(Projectile.ai[0] * 0.08f) * 0.02f;

                if (Main.rand.NextBool(25))
                {
                    Vector2 leafPos = Projectile.Top + new Vector2(Main.rand.NextFloat(-Width / 2f - 5f, Width / 2f + 5f), Main.rand.NextFloat(-15f, 0f));
                    Vector2 leafVel = new Vector2(Main.rand.NextFloat(-0.8f, 0.8f), Main.rand.NextFloat(-1.2f, -0.3f));
                    Dust leaf = Dust.NewDustPerfect(leafPos, DustID.Grass, leafVel, Scale: Main.rand.NextFloat(0.9f, 1.4f));
                    leaf.fadeIn = 0.5f;
                    leaf.noGravity = true;
                }
            }

            if (Projectile.ai[0] > RiseTime + StayTime)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                    return;
                }

                if (Main.rand.NextBool(8))
                {
                    Vector2 offset = Main.rand.NextVector2Circular(Projectile.width / 2f, Projectile.height / 2f);
                    Vector2 fadePos = Projectile.Center + offset;
                    Vector2 fadeVel = new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 0.5f));
                    Dust.NewDust(fadePos, 0, 0, DustID.Dirt, fadeVel.X, fadeVel.Y, Scale: 0.8f + Main.rand.NextFloat());
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.height < 108 || Projectile.alpha > 0) return false;

            Vector2 hitTopLeft = new Vector2(Projectile.Center.X - 7f, Projectile.position.Y);
            Vector2 hitSize = new Vector2(14f, 100f);

            return Collision.CheckAABBvAABBCollision(hitTopLeft, hitSize, targetHitbox.Location.ToVector2(), targetHitbox.Size());
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int drawH = Math.Min(Projectile.height, texture.Height);
            Rectangle sourceRect = new Rectangle(0, texture.Height - drawH, texture.Width, drawH);
            Vector2 origin = new Vector2(texture.Width * 0.5f, drawH);
            Vector2 drawPos = Projectile.Bottom - Main.screenPosition;
            Color color = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)) * (1f - Projectile.alpha / 255f);
            Main.EntitySpriteDraw(texture, drawPos, sourceRect, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}