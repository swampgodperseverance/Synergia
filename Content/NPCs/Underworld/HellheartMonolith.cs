using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using System;
using Synergia.Common.GlobalPlayer;
using Terraria.Audio;

namespace Synergia.Content.NPCs
{
    public class HellheartMonolith : ModNPC
    {
        private float breathTimer;
        private float strongBreathTimer;
        private float screenFade;
        private float targetScreenFade;
        private bool dying;
        private float deathTimer;
        private float originalMusicVolume;
        private bool musicSaved;
        private float musicFactor = 1f;

        private float pulseIntensity = 1f;
        private float rayRotation;
        private float heartbeatPulse;

        private float shakeIntensity;
        private Vector2 shakeOffset;
        private bool bossSpawned;

        private static Asset<Texture2D> rayTexture;
        private static Asset<Texture2D> glowTexture;
        private static Asset<Texture2D> ringTexture;
        private static Asset<Texture2D> coreTexture;

        public override void Load()
        {
            rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
            glowTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow");
            ringTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring");
            coreTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/CoreGlow");
        }

        public override void Unload()
        {
            rayTexture = null;
            glowTexture = null;
            ringTexture = null;
            coreTexture = null;
        }

        public override void SetDefaults()
        {
            NPC.width = 95;
            NPC.height = 170;
            NPC.lifeMax = 25000;
            NPC.damage = 0;
            NPC.defense = 18;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.chaseable = false;
        }

        public override void AI()
        {
            float lifeRatio = (float)NPC.life / NPC.lifeMax;

            breathTimer += 0.015f;
            strongBreathTimer += 0.005f;
            rayRotation += 0.0015f;

            heartbeatPulse = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.2f + 0.8f;

            if (lifeRatio <= 0.5f && !dying)
            {
                float shakeFactor = MathHelper.Clamp(1f - (lifeRatio / 0.5f), 0f, 1f);
                shakeIntensity = shakeFactor * 1.5f;

                shakeOffset = new Vector2(
                    Main.rand.NextFloat(-shakeIntensity, shakeIntensity),
                    Main.rand.NextFloat(-shakeIntensity, shakeIntensity)
                );

                NPC.position += shakeOffset;

                if (lifeRatio <= 0.1f && !bossSpawned)
                {
                    BossDeathSequence();
                }
            }
            else
            {
                shakeOffset = Vector2.Zero;
            }

            float basePulse = (float)Math.Sin(breathTimer) * 0.02f;
            float strongPulse = (float)Math.Sin(strongBreathTimer * 0.5f) * 0.06f;
            pulseIntensity = 1f + (float)Math.Sin(strongBreathTimer * 1.5f) * 0.05f;
            NPC.scale = 1f + basePulse + strongPulse;

            Player player = Main.LocalPlayer;
            float triggerDistance = 86f * 16f;
            float fadeDistance = 100f * 16f;

            float distance = Vector2.Distance(player.Center, NPC.Center);

            float baseScreenFade = 0f;
            if (!dying)
            {
                if (distance <= triggerDistance)
                    baseScreenFade = 1f;
                else if (distance >= fadeDistance)
                    baseScreenFade = 0f;
                else
                    baseScreenFade = 1f - ((distance - triggerDistance) / (fadeDistance - triggerDistance));
            }

            float healthFadeFactor = lifeRatio >= 0.5f ? 1f : (lifeRatio * 2f);
            targetScreenFade = baseScreenFade * healthFadeFactor;

            float fadeSpeed = 0.02f;
            screenFade = MathHelper.Lerp(screenFade, targetScreenFade, fadeSpeed);

            if (dying)
            {
                deathTimer += 0.01f;
                float progress = MathHelper.Clamp(deathTimer, 0f, 1f);

                screenFade = MathHelper.Lerp(1f, 0f, progress * 1.5f);

                if (progress >= 0.66f && !musicSaved)
                {
                    RestoreMusic();
                }

                if (progress >= 1f)
                {
                    NPC.active = false;
                }
            }

            HandleMusic(distance);

            float zoomFactor = MathHelper.Lerp(1f, 0.95f, screenFade * heartbeatPulse * 0.5f);
            Main.GameZoomTarget = MathHelper.Lerp(Main.GameZoomTarget, zoomFactor, 0.03f);

            float lightIntensity = 2.2f * (1f + strongPulse * 0.5f) * (1f + screenFade * 0.3f) * (1f + shakeIntensity * 0.1f);
            Color lightColor = new Color(1.6f, 0.6f, 0.2f) * lightIntensity;
            Lighting.AddLight(NPC.Center, lightColor.ToVector3());

            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = new Vector2(0, -20 * i).RotatedBy(Main.GlobalTimeWrappedHourly + i);
                Lighting.AddLight(NPC.Center + offset, lightColor.ToVector3() * 0.2f);
            }
        }

        private void BossDeathSequence()
        {
            bossSpawned = true;
            dying = true;
            deathTimer = 0f;

            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-14f, 14f),
                    Main.rand.NextFloat(-14f, 14f)
                );
                Dust.NewDust(NPC.position, NPC.width, NPC.height,
                    DustID.Torch, velocity.X, velocity.Y, 120, Color.OrangeRed, 3f);
            }

            SoundEngine.PlaySound(SoundID.NPCDeath14, NPC.Center);
            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

            var shakePlayer = Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>();
            if (shakePlayer != null)
            {
                shakePlayer.TriggerShake(80, 2f);
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int sinlord = NPC.NewNPC(
                    NPC.GetSource_Death(),
                    (int)NPC.Center.X,
                    (int)NPC.Center.Y - 100,
                    ModContent.NPCType<NPCs.Boss.SinlordWyrm.Sinlord>()
                );

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, sinlord);
                }
            }
        }

        private void HandleMusic(float distance)
        {
            if (!musicSaved)
            {
                originalMusicVolume = Main.musicVolume;
                musicSaved = true;
            }

            float targetMusicFactor = 1f - screenFade;

            if (distance > 120f * 16f)
                targetMusicFactor = 1f;

            float musicLerpSpeed = dying ? 0.1f : 0.03f;
            musicFactor = MathHelper.Lerp(musicFactor, targetMusicFactor, musicLerpSpeed);

            if (!dying)
            {
                Main.musicVolume = originalMusicVolume * musicFactor;
            }
        }

        private void RestoreMusic()
        {
            if (!musicSaved) return;

            Main.musicVolume = originalMusicVolume;
            musicSaved = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawGlowEffects(spriteBatch, screenPos);
            return false;
        }

        private void DrawGlowEffects(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            if (rayTexture == null || !rayTexture.IsLoaded) return;

            float deathProgress = dying ? MathHelper.Clamp(deathTimer, 0f, 1f) : 0f;
            float fadeOutAlpha = 1f - deathProgress * 0.8f;
            float scaleShrink = 1f - deathProgress * 0.3f;

            Vector2 pos = NPC.Center - screenPos + shakeOffset;
            float alpha = MathHelper.Clamp(screenFade * 1.3f, 0.2f, 0.9f) * fadeOutAlpha;

            float lifeRatio = (float)NPC.life / NPC.lifeMax;
            float glowMultiplier = 1f + (1f - lifeRatio) * 0.5f;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,
                SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

            DrawRays(spriteBatch, pos, alpha * glowMultiplier, deathProgress);
            DrawOuterGlow(spriteBatch, pos, alpha * glowMultiplier, deathProgress);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Color npcColor = NPC.GetAlpha(Lighting.GetColor((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16))) * fadeOutAlpha;

            Main.EntitySpriteDraw(
                TextureAssets.Npc[NPC.type].Value,
                NPC.Center - Main.screenPosition + shakeOffset,
                NPC.frame,
                npcColor,
                NPC.rotation,
                NPC.frame.Size() / 2f,
                NPC.scale * scaleShrink,
                SpriteEffects.None,
                0f
            );

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            DrawCoreGlow(spriteBatch, pos, alpha * glowMultiplier, deathProgress);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }

        private void DrawRays(SpriteBatch spriteBatch, Vector2 pos, float alpha, float deathProgress)
        {
            if (rayTexture == null) return;

            Texture2D rayTex = rayTexture.Value;
            Vector2 origin = new Vector2(rayTex.Width / 2f, rayTex.Height);

            float rotation = rayRotation;
            float pulseScale = 0.7f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.03f;

            float lifeRatio = (float)NPC.life / NPC.lifeMax;
            float shakeEffect = 1f + (1f - lifeRatio) * 0.3f;

            float fadeScale = 1f - deathProgress * 0.4f;

            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i + rotation;
                float distanceFactor = 1f + (float)Math.Sin(angle + Main.GlobalTimeWrappedHourly) * 0.05f;

                Color rayColor = new Color(255, 130, 70) * (0.4f * alpha * heartbeatPulse * shakeEffect);

                spriteBatch.Draw(
                    rayTex,
                    pos,
                    null,
                    rayColor,
                    angle,
                    origin,
                    new Vector2(pulseScale * distanceFactor * fadeScale, 1.5f * pulseIntensity * NPC.scale * shakeEffect * fadeScale),
                    SpriteEffects.None,
                    0f
                );
            }
        }

        private void DrawOuterGlow(SpriteBatch spriteBatch, Vector2 pos, float alpha, float deathProgress)
        {
            if (glowTexture == null) return;

            Texture2D glowTex = glowTexture.Value;
            float glowScale = 2.2f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 0.1f;

            float lifeRatio = (float)NPC.life / NPC.lifeMax;
            float shakeEffect = 1f + (1f - lifeRatio) * 0.4f;

            float fadeScale = 1f - deathProgress * 0.5f;

            Color glowColor = new Color(255, 90, 40) * (0.2f * alpha * heartbeatPulse * shakeEffect);

            spriteBatch.Draw(
                glowTex,
                pos,
                null,
                glowColor,
                0f,
                glowTex.Size() / 2f,
                glowScale * NPC.scale * shakeEffect * fadeScale,
                SpriteEffects.None,
                0f
            );
        }

        private void DrawCoreGlow(SpriteBatch spriteBatch, Vector2 pos, float alpha, float deathProgress)
        {
            if (coreTexture == null) return;

            Texture2D coreTex = coreTexture.Value;
            float coreScale = 0.9f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * 0.05f;

            float lifeRatio = (float)NPC.life / NPC.lifeMax;
            float shakeEffect = 1f + (1f - lifeRatio) * 0.5f;

            float fadeScale = 1f - deathProgress * 0.6f;

            Color coreColor = new Color(255, 150, 90) * (0.6f * alpha * shakeEffect);

            spriteBatch.Draw(
                coreTex,
                pos,
                null,
                coreColor,
                0f,
                coreTex.Size() / 2f,
                coreScale * NPC.scale * 1.1f * shakeEffect * fadeScale,
                SpriteEffects.None,
                0f
            );
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (screenFade <= 0.01f) return;

            Texture2D pixel = TextureAssets.MagicPixel.Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            float vignetteStrength = 0.9f * screenFade;
            Color fadeColor = Color.Black * vignetteStrength;

            spriteBatch.Draw(
                pixel,
                new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                fadeColor
            );

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            if (ringTexture != null && ringTexture.IsLoaded)
            {
                Texture2D ring = ringTexture.Value;
                float ringPulse = 1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.05f;
                float ringAlpha = screenFade * (0.6f + heartbeatPulse * 0.2f);

                float lifeRatio = (float)NPC.life / NPC.lifeMax;
                float shakeEffect = 1f + (1f - lifeRatio) * 0.3f;

                float deathProgress = dying ? MathHelper.Clamp(deathTimer, 0f, 1f) : 0f;
                float fadeScale = 1f - deathProgress * 0.7f;

                Color ringColor = new Color(255, 130, 70) * ringAlpha * shakeEffect;

                spriteBatch.Draw(
                    ring,
                    NPC.Center - screenPos + shakeOffset,
                    null,
                    ringColor,
                    Main.GlobalTimeWrappedHourly * 0.4f,
                    ring.Size() / 2f,
                    4.5f * ringPulse * shakeEffect * fadeScale,
                    SpriteEffects.None,
                    0f
                );

                spriteBatch.Draw(
                    ring,
                    NPC.Center - screenPos + shakeOffset,
                    null,
                    new Color(200, 70, 30) * (ringAlpha * 0.4f * shakeEffect),
                    -Main.GlobalTimeWrappedHourly * 0.2f,
                    ring.Size() / 2f,
                    5f * ringPulse * 0.7f * shakeEffect * fadeScale,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

        public override bool CheckActive() => false;

        public override bool CheckDead()
        {
            if (!dying && !bossSpawned)
            {
                BossDeathSequence();
                return false;
            }
            return true;
        }
    }
}