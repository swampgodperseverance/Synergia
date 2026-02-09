using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using NewHorizons.Content.Projectiles.Throwing;

namespace Synergia.Content.Projectiles.Thrower
{
    public class Shroomerang2 : ModProjectile
    {
        private const int MinSpawnDelay = 10;
        private const int MaxSpawnDelay = 20;

        private int spawnTimer;
        private int nextSpawnDelay;

        private Texture2D AfterimageTexture =>
            ModContent.Request<Texture2D>(
                "Synergia/Content/Projectiles/Thrower/Shroomerang2",
                ReLogic.Content.AssetRequestMode.ImmediateLoad
            ).Value;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 14;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

            ResetSpawnDelay();
        }

        public override void AI()
        {
            HandleMushroomSpawning();
        }

        private void HandleMushroomSpawning()
        {
            spawnTimer++;

            if (spawnTimer < nextSpawnDelay)
                return;

            spawnTimer = 0;
            ResetSpawnDelay();

            if (Main.myPlayer != Projectile.owner)
                return;

            Vector2 velocity = Projectile.velocity.RotatedByRandom(0.25f) * 0.6f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ProjectileID.Mushroom,
                Projectile.damage / 2,
                Projectile.knockBack * 0.5f,
                Projectile.owner
            );
        }

        private void ResetSpawnDelay()
        {
            nextSpawnDelay = Main.rand.Next(MinSpawnDelay, MaxSpawnDelay + 1);
        }


        

        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 5; i++)
            {
                float progress = i * 0.2f;

                Vector2 position = Vector2.Lerp(
                    Projectile.Center,
                    Projectile.oldPos[3],
                    progress
                );

                float rotation = MathHelper.Lerp(
                    Projectile.rotation,
                    Projectile.oldRot[3],
                    progress
                );

                Color color = Projectile.GetAlpha(lightColor) * MathHelper.Lerp(0.3f, 0f, progress);

                Main.EntitySpriteDraw(
                    AfterimageTexture,
                    position - Main.screenPosition,
                    null,
                    color,
                    rotation,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }
    }
}
