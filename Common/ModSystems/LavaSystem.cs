using Synergia.Common.GlobalPlayer;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Helpers;
using System;
using Terraria;
using Terraria.ID;

namespace Synergia.Common.ModSystems {
    public class HellLavaDistortionSystem : ModSystem
    {
        public override void PostUpdateWorld()
        {
            foreach (Player player in Main.player)
            {
                if (!player.active || player.dead || !player.ZoneUnderworldHeight)
                    continue;

                TrySpawn(player);
            }
        }

        private static void TrySpawn(Player player)
        {
            if (!Main.rand.NextBool(player.GetModPlayer<BiomePlayer>().lakeBiome ? 5 : 15)) { return; }

            int radius = 600;
            Vector2 randomPos = player.Center + new Vector2(Main.rand.Next(-radius, radius), Main.rand.Next(-radius, radius));
            Point tilePos = randomPos.ToTileCoordinates();

            if (!WorldGen.InWorld(tilePos.X, tilePos.Y)) { return; }
            Tile tile = Main.tile[tilePos.X, tilePos.Y];

            if (tile != null && tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Lava)
            {
                Vector2 spawn = new(tilePos.X * 16 + 8, tilePos.Y * 16 - 8);
                Projectile.NewProjectile(player.GetSource_FromThis(), spawn, new Vector2(0f, -2f), ProjectileType<LavaInfernalGlow>(), 25, 2f, Main.myPlayer);
            }
        }
    }

    public class LavaInfernalGlow : ModProjectile
    {
        // Aeris fuck ur mum
        private float pulse;
        private float distortionTimer;
        bool lakeSpawn;

        public override string Texture => "Synergia/Assets/Textures/Glow";

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            bool lake = WorldHelper.CheckBiomeTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, 215, 119, SynergiaGenVars.HellLakeX - 236, SynergiaGenVars.HellLakeY - 119);
            if (lake && !lakeSpawn) { Projectile.timeLeft += 240; lakeSpawn = true; }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            Projectile.velocity.Y = -1.5f;

            pulse += 0.15f;
            distortionTimer += 0.2f;

            float wave = 0.6f + (float)Math.Sin(pulse) * 0.1f;

            if (Projectile.timeLeft < 40)
            {
                Projectile.alpha += 8;
                Projectile.scale *= 0.98f;
            }
            else
            {
                Projectile.scale = wave * 0.7f;
            }

            Projectile.rotation += 0.03f;

            float lightIntensity = 1.2f * (1f - Projectile.alpha / 255f);
            Lighting.AddLight(Projectile.Center, lightIntensity, lightIntensity * 0.4f, lightIntensity * 0.1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(
                    target.position,
                    target.width,
                    target.height,
                    DustID.Torch,
                    Main.rand.NextFloat(-2f, 2f),
                    Main.rand.NextFloat(-2f, 2f)
                );
            }

            Projectile.timeLeft = Math.Min(Projectile.timeLeft, 40);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-1f, 0f)
                );
            }

            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Lava,
                    Main.rand.NextFloat(-1.5f, 1.5f),
                    Main.rand.NextFloat(-1.5f, 0.5f)
                );
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow").Value;
            Texture2D ringTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            Texture2D coreTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/CoreGlow").Value;

            float opacity = 1f - Projectile.alpha / 255f;
            float shimmer = (float)Math.Sin(distortionTimer) * 0.2f;

            Color mainColor = new Color(255, 80 + (int)(30 * shimmer), 30) * opacity;
            Color ringColor = new Color(255, 130, 30) * opacity * 0.5f;
            Color coreColor = new Color(255, 180, 30) * opacity * 0.6f;

            Vector2 position = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(
                ringTexture,
                position,
                null,
                ringColor,
                Projectile.rotation,
                ringTexture.Size() / 2f,
                Projectile.scale * 1.2f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.Draw(
                glowTexture,
                position,
                null,
                mainColor,
                -Projectile.rotation * 0.6f,
                glowTexture.Size() / 2f,
                Projectile.scale * 0.9f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.Draw(
                coreTexture,
                position,
                null,
                coreColor,
                Projectile.rotation * 0.4f,
                coreTexture.Size() / 2f,
                Projectile.scale * 0.5f,
                SpriteEffects.None,
                0f
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}