using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.ModSystems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Armor
{
    public class AshenCopy : ModProjectile
    {
        private int timer;
        private float distortionIntensity = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 800;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.aiStyle = -1;
            Projectile.hide = true;
        }

        public override void AI()
        {
            timer++;
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            if (timer == 1)
            {
                Vector2 spawnPos = owner.Center;
                spawnPos.Y += 16f;
                int tileX = (int)(spawnPos.X / 16f);
                int tileY = (int)(spawnPos.Y / 16f);
                while (tileY < Main.maxTilesY && (Main.tile[tileX, tileY] == null || !Main.tile[tileX, tileY].HasTile))
                {
                    tileY++;
                }
                spawnPos.Y = tileY * 16f - Projectile.height / 2;
                Projectile.Center = spawnPos;
                Projectile.spriteDirection = owner.direction;
            }

            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = 0f;

            if (timer < 30)
                Projectile.alpha = (int)MathHelper.Lerp(255, 40, timer / 30f);
            else if (Projectile.timeLeft < 60)
            {
                float t = Projectile.timeLeft / 60f;
                Projectile.alpha = (int)MathHelper.Lerp(40, 255, 1f - t);
            }

            SpawnAshDust();
            TauntEnemies();
            UpdateDistortion();
        }

        private void SpawnAshDust()
        {
            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(
                    Main.rand.NextFloat(-Projectile.width * 0.55f, Projectile.width * 0.55f),
                    Main.rand.NextFloat(-Projectile.height * 0.65f, Projectile.height * 0.35f)
                );

                Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    ModContent.DustType<Dusts.Steam>(),
                    new Vector2(Main.rand.NextFloat(-0.45f, 0.45f), Main.rand.NextFloat(-1.2f, -0.25f)),
                    100,
                    new Color(255, 255, 255),
                    Main.rand.NextFloat(0.65f, 0.95f)
                );
            }
        }

        private void TauntEnemies()
        {
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy())
                    continue;
                float dist = Vector2.Distance(npc.Center, Projectile.Center);
                if (dist > 900f)
                    continue;
                npc.target = Projectile.owner;
                Vector2 dir = Projectile.Center - npc.Center;
                npc.velocity += dir.SafeNormalize(Vector2.Zero) * 0.05f;
            }
        }

        private void UpdateDistortion()
        {
            float target = 0f;
            if (Projectile.alpha < 200)
                target = 0.065f;
            if (Projectile.timeLeft < 60)
                target *= Projectile.timeLeft / 60f;
            distortionIntensity = MathHelper.Lerp(distortionIntensity, target, 0.12f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (ShaderLoader.AshenDistortionEffect == null || distortionIntensity <= 0.01f)
                return true;

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = sourceRect.Size() / 2f;
            Color color = GetAlpha(lightColor) ?? lightColor;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();

            ShaderLoader.AshenDistortionEffect.Parameters["uTime"]?.SetValue((float)Main.GameUpdateCount * 0.05f);
            ShaderLoader.AshenDistortionEffect.Parameters["uIntensity"]?.SetValue(distortionIntensity);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, ShaderLoader.AshenDistortionEffect, Main.Transform);

            spriteBatch.Draw(texture, drawPosition, sourceRect, color, Projectile.rotation, origin, Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool? CanDamage() => false;

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 210, 200, 255 - Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 offset = new Vector2(
                    Main.rand.NextFloat(-Projectile.width * 0.55f, Projectile.width * 0.55f),
                    Main.rand.NextFloat(-Projectile.height * 0.65f, Projectile.height * 0.35f)
                );

                Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    ModContent.DustType<Dusts.Steam>(),
                    Main.rand.NextVector2Circular(0.9f, 1.8f),
                    100,
                    new Color(255, 255, 255),
                    0.9f
                );
            }
        }
    }
}